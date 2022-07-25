#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SMT.Api/SMT.Api.csproj", "SMT.Api/"]
COPY ["SMT.Services/SMT.Services.csproj", "SMT.Services/"]
COPY ["SMT.Access/SMT.Access.csproj", "SMT.Access/"]
COPY ["SMT.Domain/SMT.Domain.csproj", "SMT.Domain/"]
COPY ["SMT.ViewModel/SMT.ViewModel.csproj", "SMT.ViewModel/"]
COPY ["SMT.Notification/SMT.Notification.csproj", "SMT.Notification/"]
COPY ["SMT.Security/SMT.Security.csproj", "SMT.Security/"]
RUN dotnet restore "SMT.Api/SMT.Api.csproj"
COPY . .
WORKDIR "/src/SMT.Api"
RUN dotnet build "SMT.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SMT.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMT.Api.dll"]