

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS src
WORKDIR /src
COPY /Tests .
COPY /src .
RUN dotnet build MiniBank.Web -c Release
RUN dotnet test MiniBank.Core.Tests --no-build
RUN dotnet publish MiniBank.Web -c Release --no-build -o /dist
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as final
WORKDIR /app
COPY --from=src /dist .
ENV ASPNETCORE_URLS=http://localhost:5001;http://localhost:5000
EXPOSE 5000 5001
ENTRYPOINT ["dotnet", "MiniBank.Web.dll"]
