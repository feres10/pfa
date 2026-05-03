# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

# 👉 FIX PATH HERE
RUN dotnet restore ./E_santeFrontend/E_santeFrontend.csproj
RUN dotnet publish ./E_santeFrontend/E_santeFrontend.csproj -c Release -o /app/publish

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "E_santeFrontend.dll"]
