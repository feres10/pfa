# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore using the actual project file
RUN dotnet restore $(find . -name "*.csproj" | head -n 1)

# Publish using the same project
RUN dotnet publish $(find . -name "*.csproj" | head -n 1) -c Release -o /app/publish

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# Run the generated DLL dynamically
ENTRYPOINT ["sh", "-c", "dotnet $(ls *.dll | head -n 1)"]
