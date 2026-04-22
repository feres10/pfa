# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln ./
COPY E_santeFrontend_blazor/*.csproj ./E_santeFrontend_blazor/
RUN dotnet restore ./E_santeFrontend_blazor/E_santeFrontend.csproj
COPY . .
RUN dotnet publish ./E_santeFrontend_blazor/E_santeFrontend.csproj -c Release -o /app --no-restore

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
# Security: Create non-root user (optional, uncomment if needed)
# RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
# USER appuser
ENTRYPOINT ["dotnet", "E_santeFrontend.dll"]
