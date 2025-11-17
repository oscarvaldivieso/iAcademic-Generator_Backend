# ==========================
# Etapa de build
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copiar la solución y los proyectos .csproj para restaurar dependencias
COPY *.sln ./
COPY iAcademicGenerator.API/*.csproj ./iAcademicGenerator.API/
COPY iAcademicGenerator.BusinessLogic/*.csproj ./iAcademicGenerator.BusinessLogic/
COPY iAcademicGenerator.DataAccess/*.csproj ./iAcademicGenerator.DataAccess/
COPY iAcademicGenerator.Models/*.csproj ./iAcademicGenerator.Models/

# Restaurar solo la API principal (y sus proyectos referenciados)
RUN dotnet restore "iAcademicGenerator.API/iAcademicGenerator.API.csproj"

# Copiar todo el código fuente
COPY . .

# Publicar la API en modo Release
RUN dotnet publish iAcademicGenerator.API/iAcademicGenerator.API.csproj -c Release -o /app/out

# ==========================
# Etapa runtime
# ==========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar los archivos publicados desde la etapa de build
COPY --from=build-env /app/out .

# Configurar el entrypoint
ENTRYPOINT ["dotnet", "iAcademicGenerator.API.dll"]
