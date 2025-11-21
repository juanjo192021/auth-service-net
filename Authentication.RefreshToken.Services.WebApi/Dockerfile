FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev

WORKDIR /src

# Copiar archivos de proyecto (para evitar restaurar en cada cambio)
#COPY Authentication.RefreshToken.Services.WebApi/*.csproj ./

#RUN dotnet restore

# Variables de entorno (8080 - opcional)
ENV ASPNETCORE_URLS=http://+:7234
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 7234

# Permitir hot reload (dotnet watch ya está incluido en SDK .NET 9)
ENTRYPOINT ["dotnet", "watch", "run", "--project", "Authentication.RefreshToken.Services.WebApi/Authentication.RefreshToken.Services.WebApi.csproj", "--urls", "http://0.0.0.0:7234"]