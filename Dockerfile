# Используем SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файлы проектов отдельно для кэширования
COPY BlazorApp/BlazorApp.csproj BlazorApp/
COPY BlazorApp.Server/Shared/BlazorApp.Shared.csproj BlazorApp.Server/Shared/

# Восстанавливаем зависимости
WORKDIR /app/BlazorApp
RUN dotnet restore

# Копируем весь код и собираем
COPY . .
RUN dotnet publish -c Release -o /app/build

# Используем Nginx для хостинга WASM
FROM nginx:alpine AS final
COPY --from=build /app/build/wwwroot /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
