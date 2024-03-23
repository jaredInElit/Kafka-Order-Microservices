#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Kafka Ordering API/Kafka Ordering API.csproj", "Kafka Ordering API/"]
RUN dotnet restore "Kafka Ordering API/Kafka Ordering API.csproj"
COPY . .
WORKDIR "/src/Kafka Ordering API"
RUN dotnet build "Kafka Ordering API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kafka Ordering API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kafka Ordering API.dll"]