# WorkerDemoApp

E-posta doğrulamalı kayıt akışı ve arka planda çalışan bir .NET Worker Service ile e-posta hatırlatmalarını gösteren demo proje.

## Neyi gösteriyor

- **Kayıt ve doğrulama:** kullanıcı kaydı, e-postayla gönderilen doğrulama kodu, giriş (MVC arayüzü)
- **Background Worker:** `EmailReminderWorker`, web uygulamasından bağımsız bir `BackgroundService` olarak periyodik e-posta hatırlatmaları gönderir
- **SMTP entegrasyonu:** `IOptions<SmtpSettings>` ile yapılandırılan mail servisi
- **Katmanlı mimari:** Repository + Unit of Work, servis katmanı, Mapster

## Projeler

| Proje | Görev |
|---|---|
| `WorkerDemoApp.WebUI` | Kayıt, doğrulama ve giriş ekranları (ASP.NET Core MVC) |
| `WorkerDemoApp.Worker` | Arka plan e-posta hatırlatma servisi |
| `WorkerDemoApp.API` | Web API iskeleti |
| `WorkerDemoApp.Services` / `DAL` / `Entity` / `Mapping` / `Core` | Paylaşılan katmanlar |

## Teknolojiler

.NET 9, ASP.NET Core MVC, Worker Service, EF Core + SQL Server, ASP.NET Core Identity, Mapster.

## Çalıştırma

```bash
git clone https://github.com/Oguzhancilesiz/WorkerDemoApp.git
cd WorkerDemoApp
dotnet user-secrets --project WorkerDemoApp.WebUI set "Smtp:Password" "<smtp-sifresi>"
dotnet user-secrets --project WorkerDemoApp.Worker set "Smtp:Password" "<smtp-sifresi>"
dotnet run --project WorkerDemoApp.WebUI
dotnet run --project WorkerDemoApp.Worker
```
