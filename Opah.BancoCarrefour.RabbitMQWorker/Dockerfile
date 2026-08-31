FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Opah.BancoCarrefour.RabbitMQWorker/Opah.BancoCarrefour.RabbitMQWorker.csproj", "Opah.BancoCarrefour.RabbitMQWorker/"]
COPY ["Opah.BancoCarrefour.SqlServer.Repository/Opah.BancoCarrefour.SqlServer.Repository.csproj", "Opah.BancoCarrefour.SqlServer.Repository/"]
COPY ["Opah.BancoCarrefour.Domain/Opah.BancoCarrefour.Domain.csproj", "Opah.BancoCarrefour.Domain/"]
COPY ["Opah.BancoCarrefour.Messaging/Opah.BancoCarrefour.Messaging.csproj", "Opah.BancoCarrefour.Messaging/"]
RUN dotnet restore "Opah.BancoCarrefour.RabbitMQWorker/Opah.BancoCarrefour.RabbitMQWorker.csproj"

COPY . .
WORKDIR "/src/Opah.BancoCarrefour.RabbitMQWorker"
RUN dotnet build "Opah.BancoCarrefour.RabbitMQWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Opah.BancoCarrefour.RabbitMQWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Opah.BancoCarrefour.RabbitMQWorker.dll"]
