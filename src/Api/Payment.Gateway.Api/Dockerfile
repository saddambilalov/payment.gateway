#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Api/Payment.Gateway.Api/Payment.Gateway.Api.csproj", "src/Api/Payment.Gateway.Api/"]
COPY ["src/Payment.Gateway.Infrastructure/Payment.Gateway.Infrastructure.csproj", "src/Payment.Gateway.Infrastructure/"]
COPY ["src/Payment.Gateway.Domain/Payment.Gateway.Domain.csproj", "src/Payment.Gateway.Domain/"]
COPY ["src/Api/Payment.Gateway.Api.Abstractions/Payment.Gateway.Api.Abstractions.csproj", "src/Api/Payment.Gateway.Api.Abstractions/"]
RUN dotnet restore "src/Api/Payment.Gateway.Api/Payment.Gateway.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/Payment.Gateway.Api"
RUN dotnet build "Payment.Gateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.Gateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Gateway.Api.dll"]