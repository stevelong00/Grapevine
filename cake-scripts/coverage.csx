#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=xunit.runner.console&version=2.1.0"

using System;

//private const string TestAssembly = "./**/Grapevine.Tests";
//private const string AssemblyUnderTest = "+[Grapevine]*";

private const string OutputFolderName = "code-coverage";
private string OutputDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), OutputFolderName);

System.IO.Directory.CreateDirectory(OutputDirectory);

private string UnitTestCoverageResults = System.IO.Path.Combine(OutputDirectory, "unit-test-coverage.xml");
private string UnitTestCoverageReport = System.IO.Path.Combine(OutputDirectory, "unit-test-coverage");
private string UnitTestCoverageIndex =  System.IO.Path.Combine(UnitTestCoverageReport, "index.htm");

// private XUnit2Settings xuSettings = new XUnit2Settings {ShadowCopy = false};
// private OpenCoverSettings ocSettings = new OpenCoverSettings().WithFilter("+[Grapevine]*");

Task("unit-test-coverage")
.Does(() =>
{
    var dirpath = new DirectoryPath(OutputDirectory);
    var filepath = new FilePath(UnitTestCoverageResults);
    var xuSettings = new XUnit2Settings {ShadowCopy = false};
    var ocSettings = new OpenCoverSettings().WithFilter("+[Grapevine]*");

    Console.WriteLine("File Path          : " + filepath.FullPath);
    Console.WriteLine("XUnit Settings     : " + (xuSettings != null));
    Console.WriteLine("OpenCover Settings : " + (ocSettings != null));

    OpenCover(tool => { tool.XUnit2("./**/Grapevine.Tests.dll", xuSettings); }, filepath, ocSettings);

    // OpenCover(
    //     tool => { tool.XUnit2("./**/Grapevine.Tests", xuSettings); },
    //     new FilePath(UnitTestCoverageResults),
    //     ocSettings
    // );
});

Task("unit-test-coverage-report")
.IsDependentOn("unit-test-coverage")
.Does(() =>
{
    Console.WriteLine("Executing Unit Test Coverage Report");
    // ReportGenerator(
    //     new FilePath(UnitTestCoverageResults),
    //     new DirectoryPath(UnitTestCoverageReport)
    // );
});

Task("launch-coverage-report")
.IsDependentOn("unit-test-coverage-report")
.Does(() =>
{
    Console.WriteLine("Executing Launch Unit Test Coverage Report");
    // if (IsRunningOnWindows())
    // {
    //     StartProcess("explorer.exe", UnitTestCoverageIndex);
    // }
    // else
    // {
    //     Information("Unable to launch report on non-Windows OS.");
    //     Information("Coverage report can be found at: " + UnitTestCoverageIndex);
    // }
});

// private XUnit2Settings GetXunitSettings ()
// {
//     return new XUnit2Settings{ShadowCopy = false};
// }

// private OpenCoverSettings GetOpenCoverSettings ()
// {
//     return new OpenCoverSettings().WithFilter(AssemblyUnderTest);
// }