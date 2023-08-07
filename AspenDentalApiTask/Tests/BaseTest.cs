using AspenDentalApiTask.Services;
using AspenDentalApiTask.Setup;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace AspenDentalApiTask.Tests
{
    public abstract class BaseTest
    {
        protected ExtentReports extent;
        protected ExtentTest test;

        protected Variables _variables;

        [OneTimeSetUp]
        public void SetupReporting()
        {
            extent = new ExtentReports();
            var htmlReporter = new ExtentHtmlReporter(_variables.ReportPath);
            extent.AttachReporter(htmlReporter);
        }

        [SetUp]
        public void BeforeEachTest()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        // This method gets called after each test has finished.
        [TearDown]
        public async Task Teardown()
        {
            // Determines the result status of the currently executed test.
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            // Fetches the stack trace of a failed test or provides an empty string 
            // if there's no failure or if the stack trace is null.
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                                ? ""
                                : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);

            Status logstatus;

            // Determine the test's execution status and log it.
            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;

                    // Logs the failure status and reason for the failure (stack trace).
                    test.Log(logstatus, "Test ended with " + logstatus + " – " + stacktrace);
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;

                    // Logs the skip status.
                    test.Log(logstatus, "Test ended with " + logstatus);
                    break;
                default:
                    logstatus = Status.Pass;

                    // Logs the success status.
                    test.Log(logstatus, "Test ended with " + logstatus);
                    break;
            }
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            extent.Flush();
        }
    }
}
