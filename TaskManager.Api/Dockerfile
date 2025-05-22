FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskManager.Api/TaskManager.Api.csproj", "TaskManager.Api/"]
COPY ["TaskManager.Domain/TaskManager.Domain.csproj", "TaskManager.Domain/"]
COPY ["TaskManager.Infra/TaskManager.Infra.csproj", "TaskManager.Infra/"]
COPY ["TaskManager.Interface/TaskManager.Interface.csproj", "TaskManager.Interface/"]

RUN dotnet restore "TaskManager.Api/TaskManager.Api.csproj"
COPY . .
WORKDIR "/src/TaskManager.Api"
RUN dotnet build "TaskManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]
