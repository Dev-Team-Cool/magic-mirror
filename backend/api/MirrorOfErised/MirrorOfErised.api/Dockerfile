FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster-arm32v7 AS build
WORKDIR /src
COPY ["MirrorOfErised.api/MirrorOfErised.api.csproj", "MirrorOfErised.api/"]
RUN dotnet restore "MirrorOfErised.api/MirrorOfErised.api.csproj"
COPY . .
WORKDIR "/src/MirrorOfErised.api"
RUN dotnet build "MirrorOfErised.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MirrorOfErised.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MirrorOfErised.api.dll"]