FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./BlazorApp.Server/Shared/BlazorApp.Shared.csproj ./BlazorApp.Server/Shared/
COPY ./BlazorApp.Server/BlazorApp.Server.csproj ./BlazorApp.Server/
COPY ./BlazorApp/BlazorApp.csproj ./BlazorApp/

RUN dotnet restore ./BlazorApp.Server/BlazorApp.Server.csproj

COPY . .

RUN dotnet build ./BlazorApp.Server/BlazorApp.Server.csproj -c Release -o /app/build

RUN dotnet publish ./BlazorApp.Server/BlazorApp.Server.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "BlazorApp.Server.dll"]