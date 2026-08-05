FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy both projects
COPY TechTrack.Backend/TechTrack.Shared/ ./TechTrack.Shared/
COPY TechTrack.Backend/TechTrack.OrganizationService/ ./TechTrack.OrganizationService/

# Build with correct paths
RUN dotnet build "TechTrack.OrganizationService/TechTrack.OrganizationService.csproj" -c http
RUN dotnet publish "TechTrack.OrganizationService/TechTrack.OrganizationService.csproj" -c http -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechTrack.OrganizationService.dll"]
