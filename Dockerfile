#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClinicBookingSystem/ClinicBookingSystem_API.csproj", "ClinicBookingSystem/"]
COPY ["ClinicBookingSystem_Service/ClinicBookingSystem_Service.csproj", "ClinicBookingSystem_Service/"]
COPY ["ClinicBookingSystem_Repository/ClinicBookingSystem_Repository.csproj", "ClinicBookingSystem_Repository/"]
COPY ["ClinicBookingSystem_DataAccessObject/ClinicBookingSystem_DataAccessObject.csproj", "ClinicBookingSystem_DataAccessObject/"]
COPY ["ClinicBookingSystem_BusinessObject/ClinicBookingSystem_BusinessObject.csproj", "ClinicBookingSystem_BusinessObject/"]
RUN dotnet restore "./ClinicBookingSystem/ClinicBookingSystem_API.csproj"
COPY . .
WORKDIR "/src/ClinicBookingSystem"
RUN dotnet build "./ClinicBookingSystem_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClinicBookingSystem_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME /app/TemplateView
ENV TZ=Asia/Bangkok
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENTRYPOINT ["dotnet", "ClinicBookingSystem_API.dll"]