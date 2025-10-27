FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy both projects
COPY TechTrack.Shared/ ./TechTrack.Shared/
COPY TechTrack.ApiGateway/ ./TechTrack.ApiGateway/

# Build with correct paths
RUN dotnet build "TechTrack.ApiGateway/TechTrack.ApiGateway.csproj" -c http
RUN dotnet publish "TechTrack.ApiGateway/TechTrack.ApiGateway.csproj" -c http -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechTrack.ApiGateway.dll"]