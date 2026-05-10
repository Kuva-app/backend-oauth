FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Source ./Source
RUN dotnet restore Source/Kuva.Auth.sln
RUN dotnet publish Source/Kuva.Auth.Service/Kuva.Auth.Service.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
RUN adduser --disabled-password --home /app --gecos "" appuser && chown -R appuser /app
USER appuser
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Kuva.Auth.Service.dll"]
