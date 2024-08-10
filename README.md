
DigitalSalesPlatform API

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
