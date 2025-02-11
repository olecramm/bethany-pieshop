# Use official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY BethanyPieShop.InventoryManagement/*.csproj BethanyPieShop.InventoryManagement/
COPY BethanyPieShop.InventoryManagement.Tests/*.csproj BethanyPieShop.InventoryManagement.Tests/

# Restore dependencies
RUN dotnet restore

# Copy everything and build
COPY . ./
RUN dotnet publish -c Release -o /out

# Use official runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Set entry point
CMD ["sh", "-c", "dotnet BethanyPieShop.InventoryManagement.dll && tail -f /dev/null"]

