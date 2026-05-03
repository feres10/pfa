FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore (path relative to context = E_santeFrontend_blazor/)
COPY ["E_santeFrontend_blazor.csproj", "./"]
RUN dotnet restore "./E_santeFrontend_blazor.csproj"

# Copy everything else and publish
COPY . .
RUN dotnet publish "./E_santeFrontend_blazor.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "E_santeFrontend_blazor.dll"]
