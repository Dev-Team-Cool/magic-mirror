FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MirrorOfErised/MirrorOfErised.csproj", "MirrorOfErised/"]
RUN dotnet restore "MirrorOfErised/MirrorOfErised.csproj"
COPY . .
WORKDIR "/src/MirrorOfErised"
RUN dotnet build "MirrorOfErised.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MirrorOfErised.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MirrorOfErised.dll"]