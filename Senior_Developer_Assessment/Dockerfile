# Use .NET SDK for development (hot reload)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy project files
COPY . ./

# Restore dependencies
RUN dotnet restore ./Senior_Developer_Assessment.Api.csproj

# Expose port 80 for Docker
EXPOSE 50

# Run the API with hot reload
ENTRYPOINT ["dotnet", "watch", "run", "--project", "Senior_Developer_Assessment.Api.csproj", "--urls", "http://+:80"]
