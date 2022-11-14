#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["customerSdmodule/customerSdmodule.csproj", "customerSdmodule/"]
COPY ["Taskmanager/Tokenmanager.csproj", "Taskmanager/"]
COPY ["RedisCacheDemo/RedisCacheDemo.csproj", "RedisCacheDemo/"]
RUN dotnet restore "customerSdmodule/customerSdmodule.csproj"
COPY . .
WORKDIR "/src/customerSdmodule"
RUN dotnet build "customerSdmodule.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "customerSdmodule.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "customerSdmodule.dll"]