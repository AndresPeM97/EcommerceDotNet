# Usar imagen base de .NET para construir la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos y restaurar dependencias
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Usar imagen base más ligera para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Especificar el comando de inicio
CMD ["dotnet", "Ecommerce.dll"]
