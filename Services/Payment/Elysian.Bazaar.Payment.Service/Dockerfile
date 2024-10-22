# Etapa de bază pentru runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiem fișierul .sln și fișierele .csproj ale fiecărui proiect pentru restaurarea dependențelor
COPY ["Elysian.Bazaar.Payment.Service.sln", "."]
COPY ["Bootstrapper/Bootstrapper.csproj", "Bootstrapper/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Configurations/Configurations.csproj", "Configurations/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["Api/Api.csproj", "Api/"]

# Verificăm locația fișierului .sln
RUN ls -la /src

# Restaurăm dependențele pentru întreaga soluție
RUN dotnet restore "Elysian.Bazaar.Payment.Service.sln"

# Copiem restul fișierelor sursă ale soluției
COPY . .

# Setăm directorul de lucru pentru construcție, la nivelul proiectului Bootstrapper
WORKDIR "/src/Bootstrapper"

# Construim întreaga soluție (sau doar proiectul Bootstrapper dacă dorești)
RUN dotnet build "../Elysian.Bazaar.Payment.Service.sln" -c Release -o /app/build

# Etapa de publicare
FROM build AS publish
RUN dotnet publish "Bootstrapper.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa finală
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bootstrapper.dll"]
