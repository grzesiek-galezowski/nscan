#tool "nuget:?package=ILRepack"
#addin nuget:?package=Cake.SemVer
#addin nuget:?package=semver&version=2.0.4
#tool "nuget:?package=GitVersion.CommandLine"

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
var solutionName = "NScan.sln";
var mainDll = "TddXt.NScan.dll";


//////////////////////////////////////////////////////////////////////
// DEPENDENCIES
//////////////////////////////////////////////////////////////////////

var buildalyzer = new[] {"Buildalyzer", "2.1.0"};
var glob        = new[] {"Glob"     , "0.4.0"};
var sprache     = new[] {"Sprache"   , "2.2.0"};

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./build") + Directory(configuration);
var publishDir = Directory("./publish");
var srcDir = Directory("./src");
var specificationDir = Directory("./specification") + Directory(configuration);
var buildNetStandardDir = buildDir + Directory("netstandard2.0");
var publishNetStandardDir = publishDir + Directory("netstandard2.0");
var srcNetStandardDir = srcDir + Directory("netstandard2.0");
var slnNetStandard = srcNetStandardDir + File(solutionName);
var specificationNetStandardDir = specificationDir + Directory("netcoreapp2.0");

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

Task("Build")
    .IsDependentOn("GitVersion")
    .Does(() =>
{
    DotNetCoreBuild("./src/netstandard2.0/Root", new DotNetCoreBuildSettings
     {
         Configuration = configuration,
         OutputDirectory = buildNetStandardDir,
         ArgumentCustomization = args => args.Append("/property:Version=" + nugetVersion.NuGetVersionV2)
     });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projectFiles = GetFiles(srcNetStandardDir.ToString() + "/**/*Specification.csproj");
    foreach(var file in projectFiles)
    {
        DotNetCoreTest(file.FullPath, new DotNetCoreTestSettings           
        {
           Configuration = configuration
        });
    }
});

public void BundleDependencies(DirectoryPath specificVersionPublishDir, string rootDllName)
{
    var fullRootDllFilePath = specificVersionPublishDir + "/" + rootDllName;
    var assemblyPaths = GetFiles(specificVersionPublishDir + "/TddXt.XNSubstitute.*.dll");
    var mainAssemblyPath = new FilePath(fullRootDllFilePath).MakeAbsolute(Context.Environment);
    assemblyPaths.Remove(mainAssemblyPath);
    foreach(var path in assemblyPaths)
    {
        Console.WriteLine(path);
    }
    ILRepack(fullRootDllFilePath, fullRootDllFilePath, assemblyPaths);
    DeleteFiles(assemblyPaths);
}

Task("GitVersion")
    .Does(() =>
{
    nugetVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
    });
});


Task("Pack")
    .IsDependentOn("Build")
    .Does(() => 
    {
        CopyDirectory(buildDir, publishDir);
        BundleDependencies(publishNetStandardDir, mainDll);
        //BundleDependencies(publishNet45Dir, mainDll);
        NuGetPack("./XNSubstitute.nuspec", new NuGetPackSettings()
        {
            Id = "XNSubstitute",
            Title = "XNSubstitute",
            Owners = new [] { "Grzegorz Galezowski" },
            Authors = new [] { "Grzegorz Galezowski" },
            Summary = "A set of NSubstitute extensions.",
            Description = "A set of NSubstitute extensions.",
            Language = "en-US",
            ReleaseNotes = new[] {"Fixed missing dependency"},
            ProjectUrl = new Uri("https://github.com/grzesiek-galezowski/xnsubstitute"),
            OutputDirectory = "./nuget",
            Version = nugetVersion.NuGetVersionV2,
            Files = new [] 
            {
                new NuSpecContent {Source = @".\publish\netstandard2.0\TddXt.XNSubstitute.*", Exclude=@"**\*.json", Target = @"lib\netstandard2.0"},
            },

            Dependencies = new [] 
            {
                netstandard20.Dependency(nSubstitute),
                netstandard20.Dependency(fluentAssertions),
                netstandard20.Dependency(xFluentAssert),
            }

        });  
    });

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
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack");

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