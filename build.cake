#addin nuget:?package=Cake.NScan&loaddependencies=true
#tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"");
var solutionName = "NScan.sln";

/////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var nscan = "nscan";
var nscanConsole = "nscan.console";
var cakeNscan = "cake.nscan";

// Define directories. (TODO: clean this up even more)
var buildDir = Directory("./build") + Directory(configuration);
var publishDir = Directory("./publish");
var srcDir = Directory("./src");
var buildNScanDir = buildDir + Directory(nscan) + Directory("netstandard2.1");
var buildNScanConsoleDir = buildDir + Directory(nscanConsole) + Directory("netcoreapp3.1"); //bug!
var buildCakeNScanDir = buildDir + Directory(cakeNscan) + Directory("netstandard2.1");
var srcNetStandardDir = srcDir; //TODO inline
var slnNetStandard = srcNetStandardDir + File(solutionName);
var version="0.61.1";
Func<ProcessArgumentBuilder, ProcessArgumentBuilder> versionCustomization = args => args.Append("-p:VersionPrefix=" + version); 

var defaultNugetPackSettings = new DotNetCorePackSettings 
{
	IncludeSymbols = true,
	Configuration = "Release",
	OutputDirectory = "./nuget",
	ArgumentCustomization = args => args.Append("--include-symbols -p:SymbolPackageFormat=snupkg -p:VersionPrefix=" + version)
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory("./nuget");

});

Task("RunPreviousNScan").Does(() =>
{
  NScanAnalyze(slnNetStandard, @"./nscan.config");
});

Task("DupFinder")
    .Description("Find duplicates in the code")
    .Does(() =>
{
    var settings = new DupFinderSettings() {
        ShowStats = true,
        ShowText = true,
        OutputFile = $"dupfinder.xml",
        ThrowExceptionOnFindingDuplicates = true,
		ExcludePattern = new string[] { "**/*Specification/**/*Specification.cs" }
    };
    DupFinder(slnNetStandard, settings);
});

Task("BuildNScan")
    .IsDependentOn("RunPreviousNScan")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("NScan.Main"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildNScanDir,
	    ArgumentCustomization = versionCustomization
    });

});

Task("BuildNScanConsole")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("NScan.Console"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildNScanConsoleDir,
	    ArgumentCustomization = versionCustomization
    });
});

Task("BuildCakeNScan")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("Cake.NScan"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildCakeNScanDir,
	    ArgumentCustomization = versionCustomization
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
    DotNetCoreTest(srcNetStandardDir.ToString() +"/NScanSpecification.Component/NScanSpecification.Component.csproj", new DotNetCoreTestSettings           
    {
        Configuration = configuration,
        Logger = "trx"
    });
});

Task("RunE2ETests")
    .IsDependentOn("BuildNScanConsole", "RunNScanUnitTests")
    .Does(() =>
    {
        DotNetCoreTest(srcNetStandardDir.ToString() + "/NScanSpecification.E2E/NScanSpecification.E2E.csproj", new DotNetCoreTestSettings           
        {
            Configuration = configuration,
            Logger = "trx"
        });
    })

Task("PackNScan")
    .IsDependentOn("BuildNScan", "RunE2ETests")
    .Does(() => 
    {
		DotNetCorePack(srcNetStandardDir + File("NScan.Main"), defaultNugetPackSettings);
    });

Task("PackNScanConsole")
    .IsDependentOn("BuildNScanConsole", "RunE2ETests")
    .Does(() => 
    {
		DotNetCorePack(srcNetStandardDir + File("NScan.Console"), defaultNugetPackSettings);
    });

Task("PackCakeNScan")
	.IsDependentOn("BuildCakeNScan", "RunE2ETests")
    .Does(() => 
    {
		DotNetCorePack(srcNetStandardDir + File("Cake.NScan"), defaultNugetPackSettings);
    });

Task("Push")
    .IsDependentOn("Clean")
    .IsDependentOn("PackNScan")
    .IsDependentOn("PackNScanConsole")
    .IsDependentOn("PackCakeNScan")
	.Does(() =>
	{
	    var projectFiles = GetFiles("./nuget/*.nupkg");
		foreach(var file in projectFiles)
		{
			DotNetCoreNuGetPush(file.FullPath, new DotNetCoreNuGetPushSettings
			{
				Source = "https://api.nuget.org/v3/index.json",
			});
		}
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
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

//#r "C:\\Users\\grzes\\Documents\\GitHub\\nscan\\CakePlugin\\bin\\Debug\\netstandard2.1\\CakePlugin.dll"

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