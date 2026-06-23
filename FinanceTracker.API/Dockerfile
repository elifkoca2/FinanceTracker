# ── Aşama 1: Build ──────────────────────────────────────────
# SDK image'ı kullan — hem build hem publish için gerekli araçlar burada
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Önce sadece .csproj dosyalarını kopyala ve restore et
COPY ["FinanceTracker.API/FinanceTracker.API.csproj", "FinanceTracker.API/"]
RUN dotnet restore "FinanceTracker.API/FinanceTracker.API.csproj"

# Geri kalan tüm dosyaları kopyala ve build et
COPY . .
WORKDIR "/src/FinanceTracker.API"
RUN dotnet build "FinanceTracker.API.csproj" -c Release -o /app/build

# ── Aşama 2: Publish ─────────────────────────────────────────
FROM build AS publish
RUN dotnet publish "FinanceTracker.API.csproj" -c Release -o /app/publish

# ── Aşama 3: Runtime ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Uygulamanın dinleyeceği port
EXPOSE 8080

ENTRYPOINT ["dotnet", "FinanceTracker.API.dll"]