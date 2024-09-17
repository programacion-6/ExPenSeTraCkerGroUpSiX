# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# restore
COPY ["RestApi/RestApi.csproj", "RestApi/"]
RUN dotnet restore 'RestApi/RestApi.csproj'

# build
COPY ["RestApi", "RestApi/"]
WORKDIR /src/RestApi
RUN dotnet build 'RestApi.csproj' -c Release -o /app/build

# Stage 2: Publish Stage
FROM build as publish
RUN dotnet publish 'RestApi.csproj' -c Release -o /app/publish

# Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestApi.dll"]

