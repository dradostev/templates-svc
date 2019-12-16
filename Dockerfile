FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-alpine3.9 AS build-env
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-alpine3.9
WORKDIR /app
COPY --from=build-env /out .

EXPOSE 5000
ENTRYPOINT ["dotnet", "Upsaleslab.Projects.App.dll"]