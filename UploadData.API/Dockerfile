FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY UploadData.API/*.csproj ./UploadData.API/
RUN dotnet restore UploadData.API/UploadData.API.csproj
COPY . .
RUN dotnet publish UploadData.API/UploadData.API.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "UploadData.API.dll"]
