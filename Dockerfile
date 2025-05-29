# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src
COPY . .
RUN dotnet restore ./MiniAPI/MiniAPI.csproj
RUN dotnet publish ./MiniAPI/MiniAPI.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:latest AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80

# expose a container port 
EXPOSE 80
ENTRYPOINT ["dotnet", "MiniAPI.dll"]