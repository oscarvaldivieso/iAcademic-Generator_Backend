# ========== Build stage ==========
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos todo el código de la ML API
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ========== Runtime stage ==========
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

#   Dependencia nativa que necesita LightGBM en Linux
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgomp1 \
    && rm -rf /var/lib/apt/lists/*

# Copiamos lo publicado desde el stage de build
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AcademicOfferPredictor.API.dll"]
