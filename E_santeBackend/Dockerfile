# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln ./
COPY E_santeBackend/*.csproj ./E_santeBackend/
RUN dotnet restore ./E_santeBackend/E_santeBackend.csproj
COPY . .
RUN dotnet publish ./E_santeBackend/E_santeBackend.csproj -c Release -o /app --no-restore

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
# Security: Create non-root user (optional, uncomment if needed)
# RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
# USER appuser
ENTRYPOINT ["dotnet", "E_santeBackend.dll"]
