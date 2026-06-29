FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FinanceTracker.API/FinanceTracker.API.csproj", "FinanceTracker.API/"]
COPY ["FinanceTracker.Core/FinanceTracker.Core.csproj", "FinanceTracker.Core/"]
COPY ["FinanceTracker.Application/FinanceTracker.Application.csproj", "FinanceTracker.Application/"]
COPY ["FinanceTracker.Infrastructure/FinanceTracker.Infrastructure.csproj", "FinanceTracker.Infrastructure/"]
COPY ["FinanceTracker.Mock/FinanceTracker.Mock.csproj", "FinanceTracker.Mock/"]

RUN dotnet restore "FinanceTracker.API/FinanceTracker.API.csproj"

COPY . .
WORKDIR "/src/FinanceTracker.API"
RUN dotnet build "FinanceTracker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceTracker.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinanceTracker.API.dll"]