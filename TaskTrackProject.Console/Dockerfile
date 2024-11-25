FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env-console
WORKDIR /app

COPY TaskTrackProject.Console.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish TaskTrackProject.Console.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env-console /app/out .
ENTRYPOINT ["dotnet", "TaskTrackProject.Console.dll"]