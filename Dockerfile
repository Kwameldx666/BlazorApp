# ���������� SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# �������� ����� �������� �������� ��� �����������
COPY BlazorApp/BlazorApp.csproj BlazorApp/
COPY BlazorApp.Server/Shared/BlazorApp.Shared.csproj BlazorApp.Server/Shared/

# ��������������� �����������
WORKDIR /app/BlazorApp
RUN dotnet restore

# �������� ���� ��� � ��������
COPY . .
RUN dotnet publish -c Release -o /app/build

# ���������� Nginx ��� �������� WASM
FROM nginx:alpine AS final
COPY --from=build /app/build/wwwroot /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
