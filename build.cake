#addin "wk.StartProcess"
#addin "wk.ProjectParser"

using PS = StartProcess.Processor;
using ProjectParser;

var npi = EnvironmentVariable("npi");
var name = "NullLock.Console";

var currentDir = new DirectoryInfo(".").FullName;
var info = Parser.Parse($"src/{name}/{name}.csproj");

Task("Pack").Does(() => {
    CleanDirectory("publish");
    var settings = new DotNetCorePackSettings {
        OutputDirectory = "publish"
    };
    DotNetCorePack($"src/{name}", settings);
    DotNetCorePack($"src/NullLockAnalyzer", settings);
});

Task("Publish-Local")
    .IsDependentOn("Pack")
    .Does(() => {
        var nupkg = new DirectoryInfo("publish").GetFiles("wk.NullLockAnalyzer*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "http://bcircle2.asuscomm.com:7777/nuget",
            ApiKey = EnvironmentVariable("nuget")
        });
    });

Task("Publish-Analyzer")
    .IsDependentOn("Pack")
    .Does(() => {
        var nupkg = new DirectoryInfo("publish").GetFiles("*Analyzer*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
});

Task("Publish-Console")
    .IsDependentOn("Pack")
    .Does(() => {
        var nupkg = new DirectoryInfo("publish").GetFiles("*Console*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
});

Task("Install")
    .IsDependentOn("Pack")
    .Does(() => {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        PS.StartProcess($"dotnet tool uninstall -g {info.PackageId}");
        PS.StartProcess($"dotnet tool install   -g {info.PackageId}  --add-source {currentDir}/publish --version {info.Version}");
    });

var target = Argument("target", "Pack");
RunTarget(target);