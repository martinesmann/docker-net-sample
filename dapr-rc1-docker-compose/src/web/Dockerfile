###
# DEFINE HOST IMAAGE 
###
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5002
EXPOSE 5002

###
# DEFINE AND RUN BUILD 
###
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["web.csproj", "./"]
RUN dotnet restore "web.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "web.csproj" -c Release -o /app/build

###
# PUBLISH 
###
FROM build AS publish
RUN dotnet publish "web.csproj" -c Release -o /app/publish

###
# COPY DAPR components files/config 
###
FROM build AS dapr
WORKDIR /dapr
COPY /components/ .

###
# COPY REDIS config file 
###
FROM build AS redis
WORKDIR /redis
COPY /redis/ .

###
# RUN 
###
FROM base AS final
WORKDIR /dapr
COPY --from=dapr /dapr .
WORKDIR /redis
COPY --from=redis /redis .
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web.dll"]
