# NUnit + Allure Integration Project

## Project Goal
This project demonstrates how to integrate **Allure reporting** with **NUnit** tests in **Visual Studio Code**. It provides a simple setup to run tests using Playwright and generate rich, interactive test reports with Allure.

---

## How Allure Works
*Insert an image here showing Allure workflow or architecture.*

Allure collects results from your test framework (NUnit) and generates an interactive HTML report. The report can include:

- Test steps
- Attachments (screenshots, logs)
<img width="940" height="256" alt="image" src="https://github.com/user-attachments/assets/e80ff0fe-d4f4-4cb0-9bd3-69d259dd0b4a" />


---

## Requirements

- **NUnit** – [NUnit Docs](https://docs.nunit.org/articles/nunit/intro.html)  
- **Allure** – [Allure Docs](https://allurereport.org/docs/)  
- **Playwright** – [Playwright .NET Docs](https://playwright.dev/dotnet/docs/intro)  
- **Java** (required for report generation) – [Allure Report Docs – Installation](https://docs.qameta.io/allure/#_installing_a_commandline)

---

## Installing Allure

**Requirements:**

- Java 8.0 or higher  
- Allure NUnit NuGet package

---

## Installing Playwright

Run the following shell commands:

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

## Notes

> **Note:** Since this repository is mainly for running tests and generating Allure reports, I will not be setting up detailed project settings.  

---

### Running Tests in Headed Mode

To run a test with a visible browser (headed mode), use the following commands in PowerShell:

```powershell
# Enable headed mode
$env:HEADED="1"

# Run the tests
dotnet test

