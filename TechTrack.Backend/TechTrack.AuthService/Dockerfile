FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy both projects
COPY TechTrack.Backend/TechTrack.Shared/ ./TechTrack.Shared/
COPY TechTrack.Backend/TechTrack.AuthService/ ./TechTrack.AuthService/

# Build with correct paths
RUN dotnet build "TechTrack.AuthService/TechTrack.AuthService.csproj" -c http
RUN dotnet publish "TechTrack.AuthService/TechTrack.AuthService.csproj" -c http -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechTrack.AuthService.dll"]
