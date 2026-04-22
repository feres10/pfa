# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish E_santeFrontend.csproj -c Release -o /app --no-restore

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
# Security: Create non-root user (optional, uncomment if needed)
# RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
# USER appuser
ENTRYPOINT ["dotnet", "E_santeFrontend.dll"]
