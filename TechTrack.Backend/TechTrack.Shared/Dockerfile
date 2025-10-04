FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build "TechTrack.Shared/TechTrack.Shared.csproj" -c Release -o /shared-output

# Export as build artifact
FROM scratch AS export
COPY --from=build /shared-output/ /