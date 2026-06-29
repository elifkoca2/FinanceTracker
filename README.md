# Finance Tracker API

Kullanıcıların hisse senetlerini takip edebildiği, fiyat alarmları kurabildiği ve gerçek zamanlı bildirimler alabileceği bir finans takip uygulaması.

## Teknolojiler

- **.NET 8** — Web API
- **Clean Architecture** — Core, Application, Infrastructure, API katmanları
- **Entity Framework Core** — ORM
- **SQLite** — Veritabanı
- **ASP.NET Identity** — Kullanıcı yönetimi
- **JWT Bearer** — Authentication
- **SignalR** — Gerçek zamanlı bildirimler
- **Health Checks** — Uygulama sağlık durumu kontrolü
- **Rate Limiting** — API istek sınırlandırma
- **Docker** — Containerization

## Mimari
- FinanceTracker.Core → Domain models, DTOs, abstractions (interfaces)
- FinanceTracker.Application → Business logic (services, use cases)
- FinanceTracker.Infrastructure → Data access (EF Core, repositories)
- FinanceTracker.API → Presentation layer (controllers, hubs, middleware)

## Özellikler

- Kullanıcı kaydı ve girişi (JWT token)
- Hisse senedi takip listesi oluşturma
- Aynı hissenin tekrar eklenmesini engelleme
- Hedef fiyat belirleme (yukarı/aşağı yön)
- Otomatik fiyat güncelleme (30 saniyede bir)
- Hedef fiyata ulaşınca otomatik bildirim
- Gerçek zamanlı fiyat ve bildirim akışı (SignalR)
- Bildirimleri toplu okundu işaretleme
- Okunmamış bildirim sayısını görüntüleme
- Health Check endpoint'i
- API Rate Limiting koruması
- Docker desteği

## Kurulum

### Gereksinimler

- .NET 8 SDK
- Docker Desktop (opsiyonel)

### Lokal Çalıştırma

```bash
# Projeyi klonla
git clone https://github.com/elifkoca2/FinanceTracker.git
cd FinanceTracker

# Veritabanını oluştur
cd FinanceTracker.Infrastructure
dotnet ef database update --startup-project ../FinanceTracker.API/FinanceTracker.API.csproj

# API'yi çalıştır
cd ../FinanceTracker.API
dotnet run
```

Swagger arayüzü: `https://localhost:7103/swagger`

### Docker ile Çalıştırma

```bash
docker-compose up --build
```

Swagger arayüzü: `http://localhost:8080/swagger`

## API Endpoints

### Auth
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | /api/auth/register | Yeni kullanıcı kaydı |
| POST | /api/auth/login | Giriş yap, JWT token al |

### Watchlist
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /api/watchlist | Takip listesini getir |
| POST | /api/watchlist | Yeni hisse ekle |
| GET | /api/watchlist/{id} | Hisse detayı |
| PUT | /api/watchlist/{id}/refresh | Fiyatı güncelle |
| DELETE | /api/watchlist/{id} | Hisseyi sil |

### Alert
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /api/alert | Bildirimleri listele |
| PUT | /api/alert/read-all | Okundu işaretle |
| GET | /api/alert/unread-count | Okunmamış bildirim sayısı |

## SignalR

Bağlantı endpoint'i: `/hubs/price`

| Event | Açıklama |
|-------|----------|
| PriceUpdated | Fiyat güncellendiğinde tetiklenir |
| AlertTriggered | Hedef fiyata ulaşıldığında tetiklenir |

## Background Services

| Servis | Interval | Görev |
|--------|----------|-------|
| PriceUpdateBackgroundService | 30 saniye | Fiyatları günceller, SignalR ile bildirir |
| AlertCheckBackgroundService | 35 saniye | Alert kontrolü yapar, tetiklenince bildirir |   

## Health Check

| Endpoint  | Açıklama                         |
| --------- | -------------------------------- |
| `/health` | API sağlık durumunu kontrol eder |

### Örnek Yanıt

```json
{
  "status": "Healthy"
}
```

---

## Rate Limiting

API istekleri rate limiting ile korunmaktadır.

* Belirlenen süre içerisinde maksimum istek sayısı sınırlandırılmıştır.
* Limit aşıldığında API aşağıdaki yanıtı döndürür:

### Örnek Yanıt

```http
HTTP/1.1 429 Too Many Requests
Content-Type: application/json

{
  "message": "Too many requests. Please try again later."
}
```

