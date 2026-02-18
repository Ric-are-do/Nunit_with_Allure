FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet build
RUN pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps

ENTRYPOINT ["dotnet", "test", "--logger", "trx;LogFileName=test_results.trx", "--results-directory", "TestReports"]
