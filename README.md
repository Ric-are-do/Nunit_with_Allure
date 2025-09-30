# NUnit + Allure Integration Project

This project demonstrates how to integrate **Allure reporting** with **NUnit** tests in **Visual Studio Code**. It provides a simple setup to run tests using Playwright and generate rich, interactive test reports with Allure.  

*Insert an image here showing Allure workflow or architecture.*

Allure collects results from your test framework (NUnit) and generates an interactive HTML report. The report can include test steps, attachments (screenshots, logs), links to issues or test management systems, and environment information.  

The project requires the following tools: **NUnit** ([Docs](https://docs.nunit.org/articles/nunit/intro.html)), **Allure** ([Docs](https://allurereport.org/docs/)), **Playwright** ([.NET Docs](https://playwright.dev/dotnet/docs/intro)), and **Java** (required for report generation) ([Installation](https://docs.qameta.io/allure/#_installing_a_commandline)).  

To install Allure, you need Java 8.0 or higher and the Allure NUnit NuGet package.  

To install Playwright, run the following shell commands:

```powershell
# Create a new NUnit project
dotnet new nunit -n PlaywrightTests

# Enter the project directory
cd PlaywrightTests

# Add the Playwright NUnit package
dotnet add package Microsoft.Playwright.NUnit

# Build the project
dotnet build

# Install Playwright browsers
pwsh bin/Debug/net8.0/playwright.ps1 install
```

Once your project is set up, write your NUnit + Playwright tests and run them via:

```powershell
dotnet test
```

To generate an Allure report and open it in your browser:

```powershell
allure serve allure-results
```

> **Note:** Since this repository is mainly for running tests and generating Allure reports, detailed project settings will not be configured.  

To run tests in headed mode (with a visible browser), use the following commands:

```powershell
# Enable headed mode
$env:HEADED="1"

# Run the tests
dotnet test
```

For more information on setting up tests and generating Allure reports in NUnit, see the [Allure NUnit Docs](https://allurereport.org/docs/nunit/).


By default, Allure will store the test results in: bin\Debug\net8.0

This location can be changed with a configuration file.  

To run the Allure report, navigate to the location of the results and run the following command:

```powershell
allure serve allure-results


---
Using the allure workflow 
- Once you download the artifact from the test 
- Unzip the folder 
- open CMD and rune the following powershell command 

```powershell
allure serve <allure-results>

This will retunr the folder details adn the allure report associated wit the  project

Notes
For more information on re-using state and authentication see https://www.youtube.com/watch?v=ew1kV8gQeJQ