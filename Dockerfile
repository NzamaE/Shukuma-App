FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["shukuma/shukuma.csproj", "shukuma/"]
COPY ["shukuma.domain/shukuma.domain.csproj", "shukuma.domain/"]
COPY ["shukuma.persistence.firebase/shukuma.persistence.firebase.csproj", "shukuma.persistence.firebase/"]

RUN dotnet restore "shukuma/shukuma.csproj"

COPY . .

WORKDIR "/src/shukuma"
RUN dotnet build "shukuma.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "shukuma.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "shukuma.dll"]