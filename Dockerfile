# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
ENV ASPNETCORE_HTTP_PORTS=5000
RUN apt-get update && apt install curl -y && apt install pgcli -y
USER $APP_UID
WORKDIR /app



# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebMarket.csproj", "."]
RUN dotnet restore "./WebMarket.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./WebMarket.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebMarket.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebMarket.dll"]
HEALTHCHECK --interval=5m --timeout=15s  CMD curl -f http://localhost:$ASPNETCORE_HTTP_PORTS