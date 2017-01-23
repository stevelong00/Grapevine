#tool "nuget:?package=xunit.runner.console&version=2.1.0"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"

private const string TestAssembly = "./**/Grapevine.Tests";
private const string AssemblyUnderTest = "+[Grapevine]*";

private const string OutputFolderName = "code-coverage";
private string OutputDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), OutputFolderName);

private string UnitTestCoverageResults = System.IO.Path.Combine(OutputDirectory, "unit-test-coverage.xml");
private string UnitTestCoverageReport = System.IO.Path.Combine(OutputDirectory, "unit-test-coverage");
private string UnitTestCoverageIndex =  System.IO.Path.Combine(UnitTestCoverageReport, "index.htm");

Task("unit-test-coverage")
.Does(() =>
{
    OpenCover(
        tool => { tool.XUnit2(TestAssembly, GetXunitSettings()); },
        new FilePath(UnitTestCoverageResults),
        GetOpenCoverSettings()
    );
});

Task("unit-test-coverage-report")
.IsDependentOn("unit-test-coverage")
.Does(() =>
{
    ReportGenerator(
        new FilePath(UnitTestCoverageResults),
        new DirectoryPath(UnitTestCoverageReport)
    );
});

Task("launch-coverage-report")
.IsDependentOn("unit-test-coverage-report")
.Does(() =>
{
    if (IsRunningOnWindows())
    {
        StartProcess("explorer.exe", UnitTestCoverageIndex);
    }
    else
    {
        Information("Unable to launch report on non-Windows OS.");
        Information("Coverage report can be found at: " + UnitTestCoverageIndex);
    }
});

private XUnit2Settings GetXunitSettings ()
{
    return new XUnit2Settings{ShadowCopy = false};
}

private OpenCoverSettings GetOpenCoverSettings ()
{
    return new OpenCoverSettings().WithFilter(AssemblyUnderTest);
}