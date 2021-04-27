#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SmartHomeAPI/SmartHomeAPI.csproj", "SmartHomeAPI/"]
RUN dotnet restore "SmartHomeAPI/SmartHomeAPI.csproj"
COPY . .
WORKDIR "/src/SmartHomeAPI"
RUN dotnet build "SmartHomeAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHomeAPI.csproj" -c Release -o /app/publish

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "SmartHomeAPI.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet SmartHomeAPI.dll
