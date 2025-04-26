FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RO.DevTest.WebApi/RO.DevTest.WebApi.csproj", "RO.DevTest.WebApi/"]
RUN dotnet restore "RO.DevTest.WebApi/RO.DevTest.WebApi.csproj"
COPY . .
WORKDIR "/src/RO.DevTest.WebApi"
RUN dotnet build "RO.DevTest.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RO.DevTest.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RO.DevTest.WebApi.dll"]
