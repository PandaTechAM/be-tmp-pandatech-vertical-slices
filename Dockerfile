FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY ["./publish/", "./"]
ENTRYPOINT ["dotnet", "PandaWebApi.dll"]
