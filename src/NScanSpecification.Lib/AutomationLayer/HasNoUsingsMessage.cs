using System;

namespace NScanSpecification.Lib.AutomationLayer;

public class HasNoUsingsMessage : GenericReportedMessage<HasNoUsingsMessage>
{
  public static HasNoUsingsMessage HasNoUsings(string project, string from, string to)
  {
    return new HasNoUsingsMessage(TestRuleFormats.FormatNoUsingsRule(project, from, to));
  }

  public HasNoUsingsMessage(string text) : base(text)
  {
  }

  protected override HasNoUsingsMessage NewInstance(string str)
  {
    return new HasNoUsingsMessage(str);
  }

  public HasNoUsingsMessage UsingsPathFound(string projectAssemblyName, params string[] path)
  {
    return NewInstance(
      $"{this}{Environment.NewLine}" +
      $"Discovered violation(s) in project {projectAssemblyName}:{Environment.NewLine}" +
      NamespacePath(path));
  }

  private string NamespacePath(string[] path)
  {
    return PathFormat.For("Violation 1", path);
  }
}