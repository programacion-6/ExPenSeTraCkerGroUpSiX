# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# restore
COPY ["src/Ibn/Ibn.csproj", "Ibn/"]
RUN dotnet restore 'Ibn/Ibn.csproj'

# build
COPY ["src/Ibn", "Ibn/"]
WORKDIR /src/Ibn
RUN dotnet build 'Ibn.csproj' -c Release -o /app/build

# Stage 2: Publish Stage
FROM build as publish
RUN dotnet publish 'Ibn.csproj' -c Release -o /app/publish

# Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ibn.dll"]

