FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Reservei.Api/Reservei.Api.csproj", "Reservei.Api/"]
COPY ["Reservei.ServiceDefaults/Reservei.ServiceDefaults.csproj", "Reservei.ServiceDefaults/"]

RUN dotnet restore "Reservei.Api/Reservei.Api.csproj"

COPY . .
WORKDIR /src/Reservei.Api
RUN dotnet build "Reservei.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reservei.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Reservei.Api.dll"]
