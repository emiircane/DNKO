# DNKO - Windows Form Uygulaması

DNKO, .NET 8.0 kullanılarak geliştirilen Windows Forms tabanlı bir masaüstü uygulamasıdır. Bu uygulama, kullanıcı yönetimi ve posta işlemleri gibi temel işlevsellikler sunar.

## Özellikler

- **Kullanıcı Yönetimi**: Kayıt olma, giriş yapma ve şifre yenileme
- **Yönetici Paneli**: Kullanıcı verilerini görüntüleme ve düzenleme
- **Mail Sistemi**: Kullanıcılara mail gönderme ve log kayıtlarını görüntüleme
- **Doğrulama Sistemi**: Mail doğrulama işlemleri

## Teknik Detaylar

- **Framework**: .NET 8.0 Windows Forms
- **Veritabanı**: Microsoft SQL Server
- **Dil**: C#
- **Paketler**:
  - Microsoft.Data.SqlClient 6.0.1
  - System.Data.SqlClient 4.9.0

## Proje Yapısı

Proje aşağıdaki temel bileşenlerden oluşmaktadır:

- **Admin**: Yönetici paneli ve kullanıcı yönetimi
- **Kayıt_Giriş**: Kullanıcı giriş ekranı
- **Kayıt_Ol**: Yeni kullanıcı kaydı
- **MailGonderici**: E-posta gönderme işlemleri
- **MailDogrulamaForm**: E-posta doğrulama arayüzü
- **SifreYenilemeEkrani**: Şifre yenileme işlemleri
- **Sifremi_Unuttum**: Şifre hatırlatma işlemleri
- **Birinci_Bolum**: Oyun veya uygulama bölümü

## Kurulum

1. Projeyi yerel bilgisayarınıza klonlayın
2. Visual Studio 2022 veya daha yeni bir sürümü kullanarak DNKO.sln dosyasını açın
3. Gerekli NuGet paketlerinin yüklenmesini bekleyin
4. Veritabanı bağlantı ayarlarını güncelleyin (Admin.cs içinde SqlConnection)
5. Uygulamayı derleyin ve çalıştırın

## Veritabanı Yapılandırması

Uygulama, aşağıdaki tablolara sahip bir SQL Server veritabanı gerektirir:
- `kayıt`: Kullanıcı bilgileri (ad, soyad, oyuncuadi, sifre, mail)
- `mail_logs`: E-posta gönderim kayıtları

## Yönetici Erişimi

Varsayılan yönetici bilgileri:
- Kullanıcı adı: admin
- Şifre: 12345

## Geliştirici

DNKO projesi [Emir Can] tarafından geliştirilmiştir.

## Lisans

Bu proje özel kullanım içindir ve açık kaynak değildir.
