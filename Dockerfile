FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY BlazorApp.sln ./
COPY BlazorApp.Server/Shared/BlazorApp.Shared.csproj BlazorApp.Server/Shared/
COPY BlazorApp.Server/Server/BlazorApp.Server.csproj BlazorApp.Server/Server/
COPY BlazorApp/BlazorApp.csproj BlazorApp/
RUN dotnet restore BlazorApp.Server/Server/BlazorApp.Server.csproj
COPY BlazorApp.Server/Shared/ BlazorApp.Server/Shared/
COPY BlazorApp.Server/Server/ BlazorApp.Server/Server/
COPY BlazorApp/ BlazorApp/
WORKDIR /app/BlazorApp.Server/Server
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080 
ENTRYPOINT ["dotnet", "BlazorApp.Server.dll"]