FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["E_santeFrontend.csproj", "./"]
RUN dotnet restore "./E_santeFrontend.csproj"

# Copy everything else and publish
COPY . .
RUN dotnet publish "./E_santeFrontend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "E_santeFrontend.dll"]
