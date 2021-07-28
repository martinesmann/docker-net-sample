# 
# BASE
#
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
EXPOSE 80
EXPOSE 443

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
WORKDIR /app
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# 
# BUILD
#
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs

WORKDIR /src
COPY ["frontend.csproj", "./"]
RUN dotnet restore "frontend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "frontend.csproj" -c Release -o /app/build

# 
# PUBLISH
#
FROM build AS publish
RUN dotnet publish "frontend.csproj" -c Release -o /app/publish


# 
# FINAL
#
FROM base AS final
WORKDIR /https
COPY --from=publish /src/https .
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "frontend.dll"]
