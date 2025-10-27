FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy both projects
COPY TechTrack.Shared/ ./TechTrack.Shared/
COPY TechTrack.UserService/ ./TechTrack.UserService/

# Build with correct paths
RUN dotnet build "TechTrack.UserService/TechTrack.UserService.csproj" -c http
RUN dotnet publish "TechTrack.UserService/TechTrack.UserService.csproj" -c http -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechTrack.UserService.dll"]