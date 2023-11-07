FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/QuickFixOrders.OrderGenerator/QuickFixOrders.OrderGenerator.csproj", "src/QuickFixOrders.OrderGenerator/"]
RUN dotnet restore "src/QuickFixOrders.OrderGenerator/QuickFixOrders.OrderGenerator.csproj"
COPY . .
WORKDIR "/src/src/QuickFixOrders.OrderGenerator"
RUN dotnet build "QuickFixOrders.OrderGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickFixOrders.OrderGenerator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickFixOrders.OrderGenerator.dll"]
