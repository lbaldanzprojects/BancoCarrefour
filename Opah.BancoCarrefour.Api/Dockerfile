# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Opah.BancoCarrefour.Api/Opah.BancoCarrefour.Api.csproj", "Opah.BancoCarrefour.Api/"]
COPY ["Opah.BancoCarrefour.Application/Opah.BancoCarrefour.Application.csproj", "Opah.BancoCarrefour.Application/"]
COPY ["Opah.BancoCarrefour.Domain/Opah.BancoCarrefour.Domain.csproj", "Opah.BancoCarrefour.Domain/"]
COPY ["Opah.BancoCarrefour.IoC/Opah.BancoCarrefour.IoC.csproj", "Opah.BancoCarrefour.IoC/"]
COPY ["Opah.BancoCarrefour.SqlServer.Repository/Opah.BancoCarrefour.SqlServer.Repository.csproj", "Opah.BancoCarrefour.SqlServer.Repository/"]
RUN dotnet restore "./Opah.BancoCarrefour.Api/Opah.BancoCarrefour.Api.csproj"
COPY . .
WORKDIR "/src/Opah.BancoCarrefour.Api"
RUN dotnet build "./Opah.BancoCarrefour.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Opah.BancoCarrefour.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Opah.BancoCarrefour.Api.dll"]