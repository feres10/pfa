# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

# 👉 CHANGE THIS to your real project path
WORKDIR /src/E_santeFrontend

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# 👉 Use explicit DLL name (CRITICAL)
ENTRYPOINT ["dotnet", "E_santeFrontend.dll"]
