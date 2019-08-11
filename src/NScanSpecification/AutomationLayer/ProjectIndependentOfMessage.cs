﻿using System.Linq;
using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using static System.Environment;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class ProjectIndependentOfMessage : GenericReportedMessage<ProjectIndependentOfMessage>
  {
    public ProjectIndependentOfMessage(string returnValue) : base(returnValue)
    {
    }

    public ProjectIndependentOfMessage ViolationPath(params string[] path)
    {
      return NewInstance(this + NewLine + "Violating path: " + StringFrom(path));
    }

    private static string StringFrom(string[] path)
    {
      return string.Join("->",
        path.Select(c => $"[{c}]"));
    }

    protected override ProjectIndependentOfMessage NewInstance(string str)
    {
      return new ProjectIndependentOfMessage(str);
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfAssembly(string projectName, string packageName)
    {
      return new ProjectIndependentOfMessage(
        TestRuleFormats.FormatIndependentRule($"{projectName}", "assembly", $"{packageName}"));
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfPackage(string projectName, string packageName)
    {
      return new ProjectIndependentOfMessage(
        TestRuleFormats.FormatIndependentRule($"{projectName}", "package", $"{packageName}"));
    }

    public static ProjectIndependentOfMessage ProjectIndependentOfProject(string assemblyNamePattern, string projectAssemblyNamePattern)
    {
      return new ProjectIndependentOfMessage(
        TestRuleFormats.FormatIndependentRule($"{assemblyNamePattern}", "project", $"{projectAssemblyNamePattern}"));
    }
  }
}