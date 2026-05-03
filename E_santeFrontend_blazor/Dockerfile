FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["E_santeFrontend.csproj", "./"]
RUN dotnet restore "./E_santeFrontend.csproj"

COPY . .
RUN dotnet publish "./E_santeFrontend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# List files during build to confirm dll name (remove after fix)
RUN ls -la

ENTRYPOINT ["dotnet", "E_santeFrontend_blazor.dll"]
