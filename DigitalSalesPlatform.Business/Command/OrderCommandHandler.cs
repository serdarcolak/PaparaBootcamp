using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Business.Helper;
using DigitalSalesPlatform.Business.Service;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using DigitalSalesPlatform.Schema.Payment;
using MediatR;
using Newtonsoft.Json;

namespace DigitalSalesPlatform.Business.Command;

public class OrderCommandHandler :
        IRequestHandler<CreateCreditCardOrderCommand, ApiResponse<OrderResponse>>,
        IRequestHandler<CreateOrderDigitalWalletCommand, ApiResponse<OrderResponse>>,
        IRequestHandler<DeleteOrderCommand, ApiResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IOrderCalculationService _orderCalculationService;
        private readonly IRedisCacheService _redisCacheService;

        public OrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, 
            IOrderCalculationService orderCalculationService, IRedisCacheService redisCacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
            _orderCalculationService = orderCalculationService;
            _redisCacheService = redisCacheService;
        }

        public async Task<ApiResponse<OrderResponse>> Handle(CreateCreditCardOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.Request);
            order.OrderDate = DateTime.UtcNow;
            order.OrderNumber = OrderNumberGenerator.GenerateOrderNumber();

            await _orderCalculationService.CalculateOrderCreditCardDetailsAsync(order, request.Request);
            await _orderCalculationService.ApplyCouponAsync(order, request.Request.CouponCode);
            var user = await _unitOfWork.UserRepository.GetById(order.UserId);
            await _orderCalculationService.ApplyPointsAsync(order, user);

            var paymentRequest = new PaymentRequest { 
                UserId = request.Request.UserId, CardNumber = request.Request.CardNumber, 
                ExpiryDate = request.Request.ExpiryDate, CVC = request.Request.CVC, Amount = order.TotalAmount
            };

            var paymentResult = await _paymentService.ProcessPayment(paymentRequest);
            if (!paymentResult.IsSuccessful)
                return new ApiResponse<OrderResponse>("Payment Not Found");
            
            await _orderCalculationService.CalculatePointsEarnedAsync(order);
            user.PointsBalance += order.PointsEarned;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.OrderRepository.Insert(order);
            await _unitOfWork.Complete();

            var orderResponse = _mapper.Map<OrderResponse>(order);
            await _redisCacheService.SetValueAsync($"order_{order.Id}", JsonConvert.SerializeObject(orderResponse));
            return new ApiResponse<OrderResponse>(orderResponse);
        }
        
        public async Task<ApiResponse<OrderResponse>> Handle(CreateOrderDigitalWalletCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.Request);
            order.OrderDate = DateTime.UtcNow;
            order.OrderNumber = OrderNumberGenerator.GenerateOrderNumber();

            await _orderCalculationService.CalculateOrderDigitalWalletDetailsAsync(order, request.Request);
            await _orderCalculationService.ApplyCouponAsync(order, request.Request.CouponCode);

            var user = await _unitOfWork.UserRepository.GetById(order.UserId);
            if (user == null)
                return new ApiResponse<OrderResponse>("User not found");

            await _orderCalculationService.ApplyPointsAsync(order, user);
            await _orderCalculationService.ApplyWalletBalanceAsync(order, user);

            if (order.TotalAmount > 0)
                return new ApiResponse<OrderResponse>("Insufficient balance");

            await _orderCalculationService.CalculatePointsEarnedAsync(order);
            user.PointsBalance += order.PointsEarned;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.OrderRepository.Insert(order);
            await _unitOfWork.Complete();

            var orderResponse = _mapper.Map<OrderResponse>(order);
            await _redisCacheService.SetValueAsync($"order_{order.Id}", JsonConvert.SerializeObject(orderResponse));
            return new ApiResponse<OrderResponse>(orderResponse);
        }
                
        
        
        public async Task<ApiResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.OrderRepository.Delete(request.OrderId);
            await _unitOfWork.Complete();
            await _redisCacheService.Clear($"order_{request.OrderId}");
            return new ApiResponse("Order deleted successfully");
        }
    }
