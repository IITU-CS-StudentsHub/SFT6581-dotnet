FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore as distinct layers
COPY TaskTracker.sln ./
COPY TaskTracker.Core/TaskTracker.Core.csproj TaskTracker.Core/
COPY TaskTracker.Web/TaskTracker.Web.csproj TaskTracker.Web/
RUN dotnet restore

# Copy everything else and build an app
COPY . ./
WORKDIR /src/TaskTracker.Web
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image optimized for size
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/publish .

# Expose port and configure ASP.NET Core URL
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "TaskTracker.Web.dll"]
