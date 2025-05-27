FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY MiniAPI/*.csproj ./MiniAPI/
COPY MiniAPI.Data/*.csproj ./MiniAPI.Data/
COPY MiniAPI.Models/*.csproj ./MiniAPI.Models/
COPY MiniAPI.Services/*.csproj ./MiniAPI.Services/
COPY MiniAPI.Tests/*.csproj ./MiniAPI.Tests/
RUN dotnet restore

COPY . .
WORKDIR /src/MiniAPI
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /src


COPY --from=build /src/MiniAPI/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "MiniAPI.dll"]