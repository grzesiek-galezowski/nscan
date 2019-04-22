using System;
using System.Linq;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class ProjectIndependentOfMessage : GenericReportedMessage<ProjectIndependentOfMessage>
  {
    public ProjectIndependentOfMessage(string returnValue) : base(returnValue)
    {
    }

    public ProjectIndependentOfMessage ViolationPath(params string[] path)
    {
      return NewInstance(this
                         + Environment.NewLine +
                         "Violating path: " + String.Join("->",
                           path.Select(c => $"[{c}]")));
    }

    protected override ProjectIndependentOfMessage NewInstance(string str)
    {
      return new ProjectIndependentOfMessage(str);
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfAssembly(string projectName, string packageName)
    {
      return new ProjectIndependentOfMessage(
        $"[{projectName}] independentOf [assembly:{packageName}]: ");
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfPackage(string projectName, string packageName)
    {
      return new ProjectIndependentOfMessage($"[{projectName}] independentOf [package:{packageName}]: ");
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfProject(string assemblyNamePattern, string projectAssemblyNamePattern)
    {
      return new ProjectIndependentOfMessage($"[{assemblyNamePattern}] independentOf [project:{projectAssemblyNamePattern}]: ");
    }
  }
}