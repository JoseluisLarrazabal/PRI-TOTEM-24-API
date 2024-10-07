# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Cambiar la ruta de acuerdo a donde esté tu archivo .csproj
COPY Totem_API/*.csproj ./Totem_API/
RUN dotnet restore ./Totem_API/*.csproj

# Copiar el resto del código fuente y publicar la aplicación
COPY . .
RUN dotnet publish ./Totem_API -c Release -o out

# Etapa de ejecución (más ligera, solo con el runtime de .NET)
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Comando de inicio
ENTRYPOINT ["dotnet", "Totem_API.dll", "--urls", "http://*:5001"]
