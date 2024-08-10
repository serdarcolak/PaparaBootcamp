
## DigitalSalesPlatform API

### Proje Hakkında

Proje, dijital ürünler ve ürün lisansları satan bir platform geliştirilmesiniamaçlamaktadır. Bu platform, Android, iOS ve Web olmak üzere üç farklıkanal üzerinden satış yapacaktır. Kullanıcılar, sisteme kayıt olarak dijital ürün ve lisansları satın alabilirler.
Platform, kullanıcıların alışveriş yaptıkça puan kazandığı bir sadakatsistemi ile çalışmaktadır. Kullanıcılar kazandıkları puanları bir sonrakialışverişlerinde indirim olarak kullanabilirler. Ayrıca, kupon sistemi sayesinde hediye kuponları ile sepet tutarını düşürebilirler. Admin kullanıcıları tüm yetkilere sahiptir. Ürün, Kategori, Kupon, Sipariş, Kullanıcı oluşturma, güncelleme, silme, görüntüleme işlemlerini yapabilirler.

### Kullanıcı İşlemleri
- Kullanıcılar: normal kullanıcı ve admin.
- Kayıt ve login işlemleri JWT token ile yapılmaktadır..
- Dijital cüzdan kullanılarak anlık ödeme işlemleri yapılmaktadır.
- Admin kullanıcılar kullanıcı ekleme, silme ve güncelleme işlemleri yapabilir.

### Ürün İşlemleri
- Admin kullanıcılar tarafından ürün ekleme, silme, güncelleme işlemleri yapılmaktadır.
- Ürünler kategori bazlı listelenebilmektedir.
- Ürünler üzerinde puan kazandırma yüzdesi ve max puan tutarı vardır.

### Kupon
- Admin kullanıcılar kupon oluşturabilir, listeleyebilir ve silebilir.
- Kullanıcılar kupon kodlarını kullanarak indirim yapabilir.
- Kuponlar tek kullanımlık ve geçerlilik tarihi ile sınırlıdır.

### Raporlama
- Sipariş ve sipariş detay tabloları üzerinden raporlanmaktadır.

### Veritabanı Modelleri
- Kullanıcı: ad, soyad, email, role, şifre, statü, dijital cüzdan bilgileri, puan bakiyesi.
- Kategori: adı, url, tags, statü.
- Ürün: kategori, adı, özellikleri, fiyat, statü, stok, puan kazandırma yüzdesi, max puan tutarı.
- Ürün Kategori Map: kategoriId, ürünId (Many-To-Many ilişki).
- Sipariş: kullanıcıId, sipariş numarası, sepet tutarı, dijital cüzdan, kupon tutarı, kupon kodu, puan tutarı, sipariş tarihi.
- Sipariş Detay: siparişId, ürünId, miktar, fiyat, statü.

### API İşlemleri
- Kullanıcı işlemleri: kayıt olma, login, güncelleme, silme, JWT token ile yetkilendirme.
- Ürün işlemleri: ekleme, güncelleme, silme, listeleme.
- Kategori işlemleri: ekleme, güncelleme, silme (kategoriye bağlı ürün varsa silinemez), listeleme.
- Kupon işlemleri: oluşturma, listeleme, silme.
- Sipariş işlemleri: oluşturma, aktif siparişler, geçmiş siparişler.
- Puan sorgulama ve kullanma.

### Kullanılan Teknolojiler
- Veri tabanı: PostgreSQL, MSSQL
- JWT token (Yetkilendirme)
- EF-Core Repository Pattern ve UnitOfWork
- Swagger (API dokümantasyonu)
- RabbitMQ (Mesaj Kuyruğu)
- Redis (Uzak Veri Depolama)


*******************************************************************************************************

### Projeyi Ayağa Kaldırma

#### Docker
Docker üzerinden kolayca Redis ve RabbitMQ kurulumu yapabilirsiniz.

#### RabbitMQ CMD Komutu 
    docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=your_username -e RABBITMQ_DEFAULT_PASS=your_password rabbitmq:3.13-management

#### Redis CMD Komutu
    docker run --rm -p 6379:6379 --name rediscontainer -d redis

#### appsettigs.json Dosyası Ayarları 

"ConnectionStrings" => {MsSqlConnection, PostgresSqlConnection, HangfireConnection}  => DB(MsSqlConnection, PostgresSqlConnection) ve (Hangfire) için "ConnectionStrings" ayarları yapılması gerekmektedir.

"JwtConfig" => Burası JwtToken için eklenmesi gerekmektedir.

"RabbitMQ" ve "Smtp" => RabbitMQ çalışması ve Smtp ile mail gönderimi yapmak için ayarların yapılması gerekmektedir. Projede bu ayarların yapılması önemlidir. Sebebi ise USER oluştururken problem yaşayabilirsiniz. ADMIN kullanıcısında mail gönderme işlemi yapılmamaktadır.

"Smtp" => Email alanına gmail hesabı girilmesi gerekmektedir.

"Redis" => localhost olarak kalabilir. Lütfen projeyi çalıştırmadan önce Redisi çalıştırın.

Google Hesabı => appsettings.json dosyasında bulunan Smtp email şifresi için Google hesabınızdan "Uygulama Şifreleri" bölümünden smtp için oluşturabilirsiniz.
https://support.google.com/accounts/answer/185833?sjid=3211176039208314485-EU bu linkten uygulama şifrenizi oluşturabilirsiniz.


*******************************************************************************************************

#### Database ve Tabloları Oluşturma

Database Adı => DigitalPlatformDb

Domain, Context ve Configurationları oluşturduktan sonra MS SQL Server veya PostgreSQL' de "DigitalPlatformDb" isimli Database oluşturuyoruz.

Aşağıdaki migration komutlarını çalıştırıyoruz.

#### 1 => Create Migration
     Data katmanına gitmeniz gerekiyor => cd DigitalSalesPlatform.Data
     
     Create Migration MSSQL Server
     dotnet ef migrations add InitialCreate -s ../DigitalSalesPlatform.Api/ --context DigitalSalesPlatformDbContext

     Create Migration PostgreSQL Server
     dotnet ef migrations add InitialCreatePostgre -s ../DigitalSalesPlatform.Api/ --context DigitalSalesPlatformDbContext
        
  
#### 2=> DB Guncelleme 
     DigitalSalesPlatform.Api gitmeniz gerekiyor => cd.. 

     DB guncelleme MSSQL Server
     dotnet ef database update --project "./DigitalSalesPlatform.Api" --startup-project "DigitalSalesPlatform.Api/" --context DigitalSalesPlatformDbContext

     DB guncelleme PostgreSQL Server
     dotnet ef database update --project "./DigitalSalesPlatform.Api" --startup-project "DigitalSalesPlatform.Api/" --context DigitalSalesPlatformDbContext


*******************************************************************************************************

### Swagger

![run01](https://github.com/user-attachments/assets/822f0174-65f0-48bd-9fa4-c167dd8668fa)

![run02](https://github.com/user-attachments/assets/9ec447ac-59b6-4500-b858-04882c81796e)

![run03](https://github.com/user-attachments/assets/d5eb1088-ee88-49ac-8845-37a5037c1e6f)

*******************************************************************************************************

### Database

![mssqldb](https://github.com/user-attachments/assets/24e65020-742e-4d41-9fa3-61bcb11d6770)

*******************************************************************************************************

### RabbitMQ

![rabbitmq](https://github.com/user-attachments/assets/668a3eeb-af73-4a33-9f28-c5b28a3dc74c)

*******************************************************************************************************

### Kullanıcı register olduğunda gönderilen email

![email-foto](https://github.com/user-attachments/assets/a10af55c-37b3-4b5e-8298-fe45a624f315)

*******************************************************************************************************

### Redis

![redis](https://github.com/user-attachments/assets/e3ec2dd0-7453-477f-9e38-9cf34a98cdbc)





     
