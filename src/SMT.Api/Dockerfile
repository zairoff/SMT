#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/SMT.Api/SMT.Api.csproj", "src/SMT.Api/"]
COPY ["src/SMT.Services/SMT.Services.csproj", "src/SMT.Services/"]
COPY ["src/SMT.Access/SMT.Access.csproj", "src/SMT.Access/"]
COPY ["src/SMT.Domain/SMT.Domain.csproj", "src/SMT.Domain/"]
COPY ["src/SMT.ViewModel/SMT.ViewModel.csproj", "src/SMT.ViewModel/"]
COPY ["src/SMT.Notification/SMT.Notification.csproj", "src/SMT.Notification/"]
COPY ["src/SMT.Security/SMT.Security.csproj", "src/SMT.Security/"]
RUN dotnet restore "src/SMT.Api/SMT.Api.csproj"
COPY . .
WORKDIR "/src/src/SMT.Api"
RUN dotnet build "SMT.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SMT.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMT.Api.dll"]