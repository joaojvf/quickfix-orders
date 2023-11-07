FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/QuickFixOrders.OrderAccumulator/QuickFixOrders.OrderAccumulator.csproj", "src/QuickFixOrders.OrderAccumulator/"]
RUN dotnet restore "src/QuickFixOrders.OrderAccumulator/QuickFixOrders.OrderAccumulator.csproj"
COPY . .
WORKDIR "/src/src/QuickFixOrders.OrderAccumulator"
RUN dotnet build "QuickFixOrders.OrderAccumulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickFixOrders.OrderAccumulator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickFixOrders.OrderAccumulator.dll"]