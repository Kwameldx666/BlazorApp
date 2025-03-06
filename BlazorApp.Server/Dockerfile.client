# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файлы проектов отдельно для кеширования
COPY Server/BlazorApp.Server.csproj Server/
COPY Shared/BlazorApp.Shared.csproj Shared/

# Восстанавливаем зависимости
RUN dotnet restore Server/BlazorApp.Server.csproj

# Копируем весь исходный код
COPY . .

# Сборка и публикация
RUN dotnet publish Server/BlazorApp.Server.csproj -c Release -o /app/publish

# Используем официальный образ ASP.NET Core для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Копируем опубликованные файлы
COPY --from=build /app/publish .

# Запуск приложения
ENTRYPOINT ["dotnet", "BlazorApp.Server.dll"]
