# ���������� SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# �������� � ��������������� �����������
COPY BlazorApp.csproj ./
COPY ./BlazorApp.Server/Shared/BlazorApp.Shared.csproj Shared/
RUN dotnet restore 
# �������� ����� ������� � �������� ���������� �����
COPY . .
RUN dotnet publish -c Release -o /app/build

# ���������� Nginx ��� ������� ������
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/build/wwwroot ./

# ��������� ���� ��� Nginx
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
