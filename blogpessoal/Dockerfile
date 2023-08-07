FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV ASPNETCORE_URLS http://*:80
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["blogpessoal.csproj", "."]
RUN dotnet restore "./blogpessoal.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "blogpessoal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blogpessoal.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blogpessoal.dll"]