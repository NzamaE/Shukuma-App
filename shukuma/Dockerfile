# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project files
COPY ["shukuma/shukuma.csproj", "shukuma/"]
COPY ["shukuma.domain/shukuma.domain.csproj", "shukuma.domain/"]
COPY ["shukuma.persistence.firebase/shukuma.persistence.firebase.csproj", "shukuma.persistence.firebase/"]

# Restore dependencies
RUN dotnet restore "shukuma/shukuma.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/shukuma"
RUN dotnet build "shukuma.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "shukuma.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "shukuma.dll"]
