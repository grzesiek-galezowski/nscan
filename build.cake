#r "C:\\Users\\grzes\\Documents\\GitHub\\nscan\\CakePlugin\\bin\\Debug\\netstandard2.0\\CakePlugin.dll"

var target = Argument("target", "Default");

Task("Default")
  .Does(() =>
{
  Information("Hello World!");
  Analyze("a", "b");
});

RunTarget(target);