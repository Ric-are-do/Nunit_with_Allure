using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // Code that runs once before any tests in the assembly
        Console.WriteLine("Global setup before any tests run.");
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {


        var timestamps = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var targetDir = Path.Combine($"Run_{timestamps}");
        var baseTestDirectory = "../../../TestReports";

        Directory.CreateDirectory("../../../TestReports"); 

        if (Directory.Exists("allure-results"))
        {
            Directory.Move("allure-results", $"{baseTestDirectory}/{targetDir}");
        }
        else
        {
            Console.WriteLine("Directory 'allure-results' not found.");
        }
    }
}

