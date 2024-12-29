# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/skd:8.0 AS build
USER $APP_UID

# Copy the main project and library project files
COPY ["ApiToDo/ToDo.Api.csproj", "ApiToDo/"]
COPY ["ToDo.Application/ToDo.Application.csproj", "ToDo.Application/"]
COPY ["ToDo.Domain/ToDo.Domain.csproj", "ToDo.Domain/"]
COPY ["ToDo.Infrastructure/ToDo.Infrastructure.csproj", "ToDo.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "ApiToDo/ToDo.Api.csproj"

# Copy all source files
COPY . .

# Build the solution
WORKDIR "/src/ApiToDo"
RUN dotnet build "ToDo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ToDo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage for running the application
FROM base AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS="http://0.0.0.0:8080;https://0.0.0.0:8081"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo.Api.dll"]