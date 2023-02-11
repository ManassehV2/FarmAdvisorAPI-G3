# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY src/*.csproj .
RUN dotnet restore

# copy everything else and build app
COPY src/. .
RUN dotnet publish -c Release -o /app
RUN dotnet tool install -g dotnet-ef
RUN $HOME/.dotnet/tools/dotnet-ef migrations add CreateTables
ENTRYPOINT ["dotnet", "/app/FarmAdvisor.dll"]