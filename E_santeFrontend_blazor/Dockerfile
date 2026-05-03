# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy full project (safer than *.csproj only)
COPY . .

# Restore + publish explicitly
RUN dotnet restore E_santeFrontend.csproj
RUN dotnet publish E_santeFrontend.csproj -c Release -o /app/publish

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Ensure correct binding (important for Docker)
ENV ASPNETCORE_URLS=http://0.0.0.0:80

ENTRYPOINT ["dotnet", "E_santeFrontend.dll"]
