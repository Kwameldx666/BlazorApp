# ���������� ����������� ����� .NET SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# �������� ����� �������� �������� ��� �����������
COPY Server/BlazorApp.Server.csproj Server/
COPY Shared/BlazorApp.Shared.csproj Shared/

# ��������������� �����������
RUN dotnet restore Server/BlazorApp.Server.csproj

# �������� ���� �������� ���
COPY . .

# ������ � ����������
RUN dotnet publish Server/BlazorApp.Server.csproj -c Release -o /app/publish

# ���������� ����������� ����� ASP.NET Core ��� �������
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# �������� �������������� �����
COPY --from=build /app/publish .

# ������ ����������
ENTRYPOINT ["dotnet", "BlazorApp.Server.dll"]
