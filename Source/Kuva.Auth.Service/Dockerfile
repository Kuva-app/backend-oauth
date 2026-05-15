FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Source ./Source
RUN dotnet restore Source/Kuva.Auth.sln
RUN dotnet publish Source/Kuva.Auth.Service/Kuva.Auth.Service.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build --chown=root:root --chmod=755 /app/publish .
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health/live || exit 1
USER app
ENTRYPOINT ["dotnet", "Kuva.Auth.Service.dll"]