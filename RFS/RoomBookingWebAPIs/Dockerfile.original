FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["RoomBookingWebAPIs/RoomBookingWebAPIs.csproj", "RoomBookingWebAPIs/"]
RUN dotnet restore "RoomBookingWebAPIs/RoomBookingWebAPIs.csproj"
COPY . .
WORKDIR "/src/RoomBookingWebAPIs"
RUN dotnet build "RoomBookingWebAPIs.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RoomBookingWebAPIs.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RoomBookingWebAPIs.dll"]