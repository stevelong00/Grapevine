Task("nuget-restore")
.Does(() =>
{
    NuGetRestore(solutionPath);
});