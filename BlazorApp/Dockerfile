# Используем SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем и восстанавливаем зависимости
COPY BlazorApp.csproj ./
COPY ./BlazorApp.Server/Shared/BlazorApp.Shared.csproj Shared/
RUN dotnet restore 
# Копируем файлы проекта и собираем клиентскую часть
COPY . .
RUN dotnet publish -c Release -o /app/build

# Используем Nginx для раздачи файлов
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/build/wwwroot ./

# Открываем порт для Nginx
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
