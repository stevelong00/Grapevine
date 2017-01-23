//#load "./cake-scripts/compile.csx"
//#load "./cake-scripts/xunit.csx"
#load "./cake-scripts/clean.csx"
#load "./cake-scripts/nuget.csx"
//#load "./cake-scripts/coverage.csx"

var target = Argument("target", "Default");
var config = Argument("configuration", "Release");

var projectName = "Grapevine";
var projectPath = "./src/" + projectName;
var solutionPath = projectPath + ".sln";

Task("default")
    //.IsDependentOn("clean")
    .IsDependentOn("nuget-restore")
    .Does(() =>
    {
        Console.WriteLine("Project Name  : " + projectName);
        Console.WriteLine("Project Path  : " + projectPath);
        Console.WriteLine("Solution Path : " + solutionPath);
    });

//    .IsDependentOn("compile-debug");

//Task("cover")
//    .IsDependentOn("compile-debug")
//    .IsDependentOn("launch-coverage-report");

RunTarget(target);
