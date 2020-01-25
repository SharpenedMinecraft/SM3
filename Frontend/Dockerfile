FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY Directory.Build.targets .
COPY BannedSymbols.txt .
COPY Frontend/ Frontend/
COPY Data/ Data/
WORKDIR /source/Frontend
RUN dotnet restore
RUN dotnet build -c release --no-restore

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test
WORKDIR /source/tests
COPY Frontend.Tests/ .
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build AS publish
RUN dotnet publish -c release --no-build -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=publish /app .
RUN apt-get update
RUN apt-get install tree 
RUN tree
ENTRYPOINT ["./Frontend"]
