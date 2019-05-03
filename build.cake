#addin nuget:?package=Cake.NScan&loaddependencies=true

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
var specificationDir = Directory("./specification") + Directory(configuration);
var buildNScanDir = buildDir + Directory(nscan) + Directory("netstandard2.0");
var buildNScanConsoleDir = buildDir + Directory(nscanConsole) + Directory("netcoreapp2.1");
var buildCakeNScanDir = buildDir + Directory(cakeNscan) + Directory("netstandard2.0");
var srcNetStandardDir = srcDir; //TODO inline
var slnNetStandard = srcNetStandardDir + File(solutionName);
var specificationNetStandardDir = specificationDir + Directory("netcoreapp2.1");

var defaultNugetPackSettings = new DotNetCorePackSettings 
{
	IncludeSymbols = true,
	Configuration = "Release",
	OutputDirectory = "./nuget"
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
  NScanAnalyze(slnNetStandard, @".\nscan.config");
});

Task("BuildNScan")
    .IsDependentOn("RunPreviousNScan")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("NScan"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildNScanDir,
    });

});

Task("BuildNScanConsole")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("NScan.Console"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildNScanConsoleDir,
    });
});

Task("BuildCakeNScan")
    .Does(() =>
{
    DotNetCoreBuild(srcDir + Directory("Cake.NScan"), new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        OutputDirectory = buildCakeNScanDir,
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

Task("PackNScan")
    .IsDependentOn("BuildNScan")
    .Does(() => 
    {
		DotNetCorePack(srcNetStandardDir + File("NScan"), defaultNugetPackSettings);
    });

Task("PackNScanConsole")
    .IsDependentOn("BuildNScanConsole")
    .Does(() => 
    {
		DotNetCorePack(srcNetStandardDir + File("NScan.Console"), defaultNugetPackSettings);
    });

    Task("PackCakeNScan")
    .IsDependentOn("BuildCakeNScan")
    .Does(() => 
    {
		DotNetCorePack(srcNetStandardDir + File("Cake.NScan"), defaultNugetPackSettings);
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