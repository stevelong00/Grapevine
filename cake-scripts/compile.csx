private Dictionary<string,MSBuildToolVersion> toolVersions = new Dictionary<string,MSBuildToolVersion>
{
    {"4.0", MSBuildToolVersion.NET40},
    {"4.5", MSBuildToolVersion.NET45},
    {"4.6", MSBuildToolVersion.NET46}
};

Task("compile-debug")
    .IsDependentOn("clean")
    .IsDependentOn("nuget-restore")
    .Does(() => {
        MSBuild(solutionPath, settings =>
        {
            settings.SetConfiguration("Debug")
            .SetVerbosity(Verbosity.Minimal)
            .SetMaxCpuCount(1)
            .SetNodeReuse(false)
            .UseToolVersion(toolVersions[toolVersion]);
        });
    });