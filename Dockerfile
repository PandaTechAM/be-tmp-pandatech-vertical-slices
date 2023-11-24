FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY ["./publish/", "./"]
ENTRYPOINT ["dotnet", "PandaWebApi.dll"]
