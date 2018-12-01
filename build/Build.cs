using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("configuration")]
    public string Configuration { get; set; }

    [Parameter("framework")]
    public string Framework { get; set; }

    [Parameter("runtime")]
    public string Runtime { get; set; }

    [Parameter("version-suffix")]
    public string VersionSuffix { get; set; }

    [Solution("ThemeEditor.sln")]
    readonly Solution Solution;

    [GitRepository]
    readonly GitRepository GitRepository;

    AbsolutePath SourceDirectory => RootDirectory / "src";

    AbsolutePath TestsDirectory => RootDirectory / "tests";

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    protected override void OnBuildInitialized()
    {
        Configuration = Configuration ?? "Release";
        Framework = Framework ?? "netcoreapp2.1";
        Runtime = Runtime ?? "win7-x64";
        VersionSuffix = VersionSuffix ?? "";
        // TODO: https://github.com/nuke-build/nuke/issues/188
        if (!string.IsNullOrWhiteSpace(VersionSuffix))
        {
            VersionSuffix = "-" + VersionSuffix;
        }
    }

    Target Clean => _ => _
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj"));
            DeleteDirectories(GlobDirectories(TestsDirectory, "**/bin", "**/obj"));
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVersionSuffix(VersionSuffix)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore());
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetVersionSuffix(VersionSuffix)
                .SetOutputDirectory(ArtifactsDirectory / "NuGet")
                .EnableNoBuild()
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPublish(s => s
                .SetProject(Solution.GetProject("ThemeEditor"))
                .SetConfiguration(Configuration)
                .SetVersionSuffix(VersionSuffix)
                .SetFramework(Framework)
                .SetRuntime(Runtime)
                .SetOutput(ArtifactsDirectory / "Publish" / "ThemeEditor-" + Runtime));
        });
}
