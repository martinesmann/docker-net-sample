###
# DEFINE HOST IMAAGE 
###
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

###
# DEFINE AND RUN BUILD 
###
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["api.csproj", "./"]
RUN dotnet restore "api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "api.csproj" -c Release -o /app/build

###
# PUBLISH 
###
FROM build AS publish
RUN dotnet publish "api.csproj" -c Release -o /app/publish

###
# COPY DAPR components files/config 
###
FROM build AS dapr
WORKDIR /dapr
COPY /components/ .

###
# RUN 
###
FROM base AS final
WORKDIR /dapr
COPY --from=dapr /dapr .
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]