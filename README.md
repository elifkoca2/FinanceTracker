# Finance Tracker

Kullanıcıların hisse senetlerini takip edebildiği, fiyat alarmları kurabildiği 
ve gerçek zamanlı bildirimler alabileceği full-stack bir finans takip uygulaması.

## Teknolojiler

### Backend
- **.NET 8 Web API** — Clean Architecture (5 katman)
- **ASP.NET Identity + JWT** — Authentication
- **Entity Framework Core + SQLite** — Veritabanı
- **SignalR** — Gerçek zamanlı WebSocket iletişimi
- **Background Services** — Otomatik fiyat güncelleme ve alert kontrolü
- **Docker** — Containerization

### Frontend
- **React + Vite** — UI
- **Tailwind CSS** — Styling
- **Axios** — HTTP istekleri
- **@microsoft/signalr** — Gerçek zamanlı bağlantı
- **React Router** — Sayfa yönlendirme

## Mimari
- FinanceTracker.Core            → Modeller, DTO'lar, Interface'ler 
- FinanceTracker.Application     → İş mantığı / Services 
- FinanceTracker.Infrastructure  → Veritabanı, Repositories 
- FinanceTracker.Mock            → Fiyat simülasyon servisi 
- FinanceTracker.API             → HTTP katmanı, Controllers, Hubs 

## Özellikler

### Kullanıcı Yönetimi
- Kayıt (ad, soyad, e-mail, şifre)
- Giriş (JWT token ile)
- Her kullanıcı sadece kendi verilerini görür

### Hisse Takibi
- Hisse ekleme (sembol + hedef fiyat + yön)
- Hedef fiyat ve yön güncelleme
- Hisse silme
- Manuel fiyat yenileme

### Otomatik Sistem
- Her 30 saniyede bir fiyat otomatik güncellenir
- Her 35 saniyede bir alert kontrolü yapılır
- Hedef aşılınca veritabanına alert kaydedilir

### Gerçek Zamanlı Bildirimler (SignalR)
- Fiyat değişince anlık güncelleme (refresh'e gerek yok)
- Hedef fiyata ulaşınca anlık bildirim
- Fiyat arınca yeşil, düşünce kırmızı renk animasyonu

### Diğer
- Rate limiting (dakikada 60 istek)
- Health check endpoint (/health)
- Global exception handling
- Input validation
- Docker desteği

## Kurulum

### Gereksinimler
- .NET 8 SDK
- Node.js 20+
- Docker Desktop (opsiyonel)

### 1. Lokal Çalıştırma

**Backend:**
```bash
# Veritabanını oluştur
cd FinanceTracker.Infrastructure
dotnet ef database update --startup-project ../FinanceTracker.API/FinanceTracker.API.csproj

# API'yi çalıştır
cd ../FinanceTracker.API
dotnet run
# Swagger: http://localhost:5181/swagger
```

**Frontend:**
```bash
cd finance-tracker-client
npm install
npm run dev
# Uygulama: http://localhost:5173
```

### 2. Docker ile Çalıştırma (Sadece Backend)
```bash
docker-compose up --build
# Swagger: http://localhost:8080/swagger
```

## API Endpoints

### Auth
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | /api/auth/register | Kayıt ol |
| POST | /api/auth/login | Giriş yap, JWT al |

### Watchlist (🔒 JWT gerekli)
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /api/watchlist | Takip listesi |
| POST | /api/watchlist | Hisse ekle |
| GET | /api/watchlist/{id} | Hisse detayı |
| PUT | /api/watchlist/{id} | Hedef güncelle |
| PUT | /api/watchlist/{id}/refresh | Fiyat yenile |
| DELETE | /api/watchlist/{id} | Hisse sil |

### Alert (🔒 JWT gerekli)
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /api/alert | Bildirimler |
| GET | /api/alert/unread-count | Okunmamış sayısı |
| PUT | /api/alert/read-all | Tümünü okundu yap |

### Sistem
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /health | Sistem sağlığı |

## SignalR

Bağlantı: `/hubs/price` (JWT token gerekli)

| Event | Açıklama | Veri |
|-------|----------|------|
| PriceUpdated | Fiyat güncellendi | itemId, symbol, currentPrice |
| AlertTriggered | Hedefe ulaşıldı | itemId, symbol, currentPrice, targetPrice |

## Background Services

| Servis | Interval | Görev |
|--------|----------|-------|
| PriceUpdateBackgroundService | 30 sn | Fiyat günceller + SignalR bildirir |
| AlertCheckBackgroundService | 35 sn | Alert kontrol eder + SignalR bildirir |

## 📁 Proje Yapısı

```text
FinanceTracker/
├── FinanceTracker.API/              # Web API katmanı
│   ├── Controllers/
│   ├── Hubs/
│   ├── Middleware/
│   └── Services/                    # Background Services
├── FinanceTracker.Core/             # Domain modelleri ve interface'ler
│   ├── Models/
│   ├── DTOs/
│   └── Interfaces/
├── FinanceTracker.Application/      # Business logic
│   └── Services/
├── FinanceTracker.Infrastructure/   # Veritabanı ve repository katmanı
│   ├── Data/
│   ├── Repositories/
│   └── Migrations/
├── FinanceTracker.Mock/             # Mock fiyat servisi
│   └── MockPriceService.cs
├── finance-tracker-client/          # React frontend
│   └── src/
│       ├── pages/
│       ├── components/
│       ├── hooks/
│       ├── context/
│       └── api/
├── docker-compose.yml               # Docker Compose yapılandırması
└── Dockerfile                       # Docker image tanımı
```
