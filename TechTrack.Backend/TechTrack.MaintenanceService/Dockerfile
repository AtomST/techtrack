FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy both projects
COPY TechTrack.Backend/TechTrack.Shared/ ./TechTrack.Shared/
COPY TechTrack.Backend/TechTrack.MaintenanceService/ ./TechTrack.MaintenanceService/

# Build with correct paths
RUN dotnet build "TechTrack.MaintenanceService/TechTrack.MaintenanceService.csproj" -c http
RUN dotnet publish "TechTrack.MaintenanceService/TechTrack.MaintenanceService.csproj" -c http -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechTrack.MaintenanceService.dll"]