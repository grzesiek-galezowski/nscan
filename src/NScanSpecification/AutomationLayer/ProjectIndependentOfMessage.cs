using System;
using System.Linq;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Adapters;
using TddXt.NScan.ReadingRules.Ports;

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
      //bug some rules have square brackets, some not. Adjust it!
      return new ProjectIndependentOfMessage(
        RuleFormats.FormatIndependentRule($"{projectName}", RuleNames.IndependentOf, "assembly", $"{packageName}" + ": "));
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfPackage(string projectName, string packageName)
    {
      return new ProjectIndependentOfMessage(
        RuleFormats.FormatIndependentRule($"{projectName}", RuleNames.IndependentOf, "package", $"{packageName}" + ": "));
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfProject(string assemblyNamePattern, string projectAssemblyNamePattern)
    {
      return new ProjectIndependentOfMessage(
        RuleFormats.FormatIndependentRule($"{assemblyNamePattern}", RuleNames.IndependentOf, "project", $"{projectAssemblyNamePattern}" + ": "));
    }
  }
}