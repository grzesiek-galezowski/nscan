#tool "nuget:?package=ILRepack&version=2.0.16"
#addin nuget:?package=Cake.SemVer&loaddependencies=true
#tool "nuget:?package=GitVersion.CommandLine"
#addin nuget:?package=Cake.NScan&loaddependencies=true

//////////////////////////////////////////////////////////////////////
// VERSION
//////////////////////////////////////////////////////////////////////

GitVersion nugetVersion = null; 

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"");
var netstandard20 = new Framework("netstandard2.0");
var netcoreapp21 = new Framework("netcoreapp2.1");
var solutionName = "NScan.sln";
var mainDll = "TddXt.NScan.dll";

var nscanConsoleTitle = "NScan.Console";
var nscanConsoleVersion = "0.8.0";
var nscanConsoleReleaseNotes = "Added hasNoCircularUsings for checking if there is a cycle between namespaces in a project";

var nscanTitle = "NScan";
var nscanVersion = "0.8.0";
var nscanReleaseNotes = nscanConsoleReleaseNotes;

var cakeNscanTitle = "Cake.NScan";
var cakeNscanVersion = "0.8.0";
var cakeNScanReleaseNotes = nscanConsoleReleaseNotes;


//////////////////////////////////////////////////////////////////////
// DEPENDENCIES
//////////////////////////////////////////////////////////////////////

var buildalyzer         = new[] { "Buildalyzer"                        , "2.2.0"      };
var glob                = new[] { "Glob"                               , "1.1.1"      };
var sprache             = new[] { "Sprache"                            , "2.2.0"      };
var functionalMaybe     = new[] { "Functional.Maybe"                   , "2.0.10"     };
var functionalMaybeJust = new[] { "Functional.Maybe.Just"              , "1.0.0"      };
var fluentCommandline   = new[] { "FluentCommandLineParser-netstandard", "1.4.3.13"   };
var cakeCore            = new[] { "Cake.Core"                          , "0.32.1"     };
var nscanDependency     = new[] { nscanTitle                           , nscanVersion };
var roslyn              = new[] { "Microsoft.CodeAnalysis.CSharp"      , "2.10.0"};
var atmaFileSystem      = new[] { "AtmaFileSystem"                     , "2.0.0"};

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var nscan = "nscan";
var nscanConsole = "nscan.console";
var cakeNscan = "cake.nscan";

// Define directories.
var buildDir = Directory("./build") + Directory(configuration);
var publishDir = Directory("./publish");
var srcDir = Directory("./src");
var specificationDir = Directory("./specification") + Directory(configuration);
var buildNScanDir = buildDir + Directory(nscan) + Directory("netstandard2.0");
var buildNScanConsoleDir = buildDir + Directory(nscanConsole) + Directory("netcoreapp2.1");
var buildCakeNScanDir = buildDir + Directory(cakeNscan) + Directory("netstandard2.0");
var publishNScanDir = publishDir + Directory(nscan) + Directory("netstandard2.0");
var publishNScanConsoleDir = publishDir + Directory(nscanConsole) + Directory("netcoreapp2.1");
var publishCakeNScanDir = publishDir + Directory(cakeNscan) + Directory("netstandard2.0");
var srcNetStandardDir = srcDir; //TODO inline
var slnNetStandard = srcNetStandardDir + File(solutionName);
var specificationNetStandardDir = specificationDir + Directory("netcoreapp2.1");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(publishDir);
    CleanDirectory("./nuget");
});

Task("RunPreviousNScan").Does(() =>
{
  NScanAnalyze(slnNetStandard, @".\nscan.config");
});

Task("BuildNScan")
    .IsDependentOn("GitVersion")
    .IsDependentOn("RunPreviousNScan")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("NScan"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildNScanDir,
        ArgumentCustomization = args => args.Append("/property:Version=" + nscanVersion)
    });

});

Task("BuildNScanConsole")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("NScan.Console"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildNScanConsoleDir,
        ArgumentCustomization = args => args.Append("/property:Version=" + nscanConsoleVersion)
    });
});

Task("BuildCakeNScan")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("Cake.NScan"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildCakeNScanDir,
        ArgumentCustomization = args => args.Append("/property:Version=" + cakeNscanVersion)
    });


});

Task("RunNScanUnitTests")
    .IsDependentOn("BuildNScan")
    .Does(() =>
{
    var projectFiles = GetFiles(srcNetStandardDir.ToString() + "/**/*Specification.csproj");
    foreach(var file in projectFiles)
    {
        DotNetCoreTest(file.FullPath, new DotNetCoreTestSettings           
        {
           Configuration = configuration,
           Logger = "trx"
        });
    }
});

Task("GitVersion")
    .Does(() =>
{
    nugetVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
    });
});

Task("PackNScan")
    .IsDependentOn("BuildNScan")
    .Does(() => 
    {
        CopyDirectory(buildNScanDir, publishNScanDir);
        PrepareNuget
        (
          title: nscanTitle,
          version: nscanVersion,
          summary: "A utility for enforcing project dependency conventions.",
          releaseNotes: nscanReleaseNotes,
          files: new [] 
            {
                new NuSpecContent {Source = @".\publish\nscan\netstandard2.0\*NScan*", Exclude=@"**\*.json;**\*.config", Target = @"lib\netstandard2.0"},
            },
          dependencies: new [] 
            {
                netstandard20.Dependency(buildalyzer),
                netstandard20.Dependency(glob),
                netstandard20.Dependency(sprache),
                netstandard20.Dependency(functionalMaybe),
                netstandard20.Dependency(functionalMaybeJust),
                netstandard20.Dependency(roslyn),
                netstandard20.Dependency(atmaFileSystem),
            }
        );
    });

Task("PackNScanConsole")
    .IsDependentOn("BuildNScanConsole")
    .Does(() => 
    {
        CopyDirectory(buildNScanConsoleDir, publishNScanConsoleDir);
        PrepareNuget
        (
          title: nscanConsoleTitle,
          version: nscanConsoleVersion,
          summary: "A utility for enforcing project dependency conventions - console runner.",
          releaseNotes: nscanConsoleReleaseNotes,
          files: new [] 
            {
                new NuSpecContent {Source = @".\publish\nscan.console\netcoreapp2.1\*NScan*", Exclude=@"**\*.json;**\*.config", Target = @"lib\netcoreapp2.1"},
            },
          dependencies: new [] 
            {
                netcoreapp21.Dependency(buildalyzer),
                netcoreapp21.Dependency(glob),
                netcoreapp21.Dependency(sprache),
                netcoreapp21.Dependency(functionalMaybe),
                netcoreapp21.Dependency(functionalMaybeJust),
                netcoreapp21.Dependency(roslyn),
                netcoreapp21.Dependency(fluentCommandline),
                netcoreapp21.Dependency(atmaFileSystem),
            }
        );
    });

    Task("PackCakeNScan")
    .IsDependentOn("BuildCakeNScan")
    .Does(() => 
    {
        CopyDirectory(buildCakeNScanDir, publishCakeNScanDir);
        PrepareNuget
        (
          title: cakeNscanTitle,
          version: cakeNscanVersion,
          summary: "A utility for enforcing project dependency conventions - cake plugin.",
          releaseNotes: cakeNScanReleaseNotes,
          files: new [] 
            {
                new NuSpecContent {Source = @".\publish\cake.nscan\netstandard2.0\*NScan*", Exclude=@"**\*.json;**\*.config", Target = @"lib\netstandard2.0"},
            },
          dependencies: new [] 
            {
                netstandard20.Dependency(buildalyzer),
                netstandard20.Dependency(glob),
                netstandard20.Dependency(sprache),
                netstandard20.Dependency(functionalMaybe),
                netstandard20.Dependency(functionalMaybeJust),
                netstandard20.Dependency(roslyn),
                netstandard20.Dependency(cakeCore),
                netstandard20.Dependency(atmaFileSystem),
            }
        );
    });

    public void PrepareNuget(
     string summary, 
     string title, 
     string releaseNotes, 
     string version,
     NuSpecContent[] files,
     NuSpecDependency[] dependencies)
    {
        NuGetPack("./NScan.nuspec", new NuGetPackSettings()
        {
            Id = title,
            Title = title,
            Owners = new [] { "Grzegorz Galezowski" },
            Authors = new [] { "Grzegorz Galezowski" },
            Summary = summary,
            Description = summary,
            Language = "en-US",
            ReleaseNotes = new[] {releaseNotes},
            ProjectUrl = new Uri("https://github.com/grzesiek-galezowski/nscan"),
            IconUrl = new Uri("https://github.com/grzesiek-galezowski/nscan/raw/master/NScan.png"),
            LicenseUrl = new Uri("https://github.com/grzesiek-galezowski/nscan/blob/master/LICENSE"),
            OutputDirectory = "./nuget",
            Version = version,
            Files = files,
            Dependencies = dependencies
        });  
    }

    public class Framework
    {
        string _name;

        public Framework(string name)
        {
            _name = name;
        }

        public NuSpecDependency Dependency(params string[] idAndVersion)
        {
            return new NuSpecDependency { Id = idAndVersion[0], Version = idAndVersion[1], TargetFramework = _name };
        }
    }



//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("GitVersion")
    .IsDependentOn("BuildNScan")
    .IsDependentOn("BuildNScanConsole")
    .IsDependentOn("BuildCakeNScan")
    .IsDependentOn("RunNScanUnitTests")
    .IsDependentOn("PackNScan")
    .IsDependentOn("PackNScanConsole")
    .IsDependentOn("PackCakeNScan");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

//#r "C:\\Users\\grzes\\Documents\\GitHub\\nscan\\CakePlugin\\bin\\Debug\\netstandard2.0\\CakePlugin.dll"

//var target = Argument("target", "Default");
//Task("Default")
//  .Does(() =>
//{
//  Information("Hello World!");
//  Analyze(
//    @"C:\Users\grzes\Documents\GitHub\nscan\MyTool.sln", 
//    @"C:\Users\grzes\Documents\GitHub\nscan\NScan.Console\nscan.config");
//});
//RunTarget(target);