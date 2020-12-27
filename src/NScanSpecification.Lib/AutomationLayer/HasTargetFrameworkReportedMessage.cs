using static System.Environment;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class HasTargetFrameworkReportedMessage : GenericReportedMessage<HasTargetFrameworkReportedMessage>
  {
    public HasTargetFrameworkReportedMessage(string returnValue) : base(returnValue)
    {
    }

    protected override HasTargetFrameworkReportedMessage NewInstance(string str)
    {
      return new(str);
    }

    public static HasTargetFrameworkReportedMessage HasFramework(string projectAssemblyNamePattern, string frameworkId)
    {
      return new(TestRuleFormats.FormatHasTargetFrameworkRule(projectAssemblyNamePattern, frameworkId));
    }

    public HasTargetFrameworkReportedMessage ProjectHasAnotherTargetFramework(string projectName, string actualTargetFramework)
    {
      return NewInstance(this + NewLine + $"Project {projectName} has target framework {actualTargetFramework}");
    }
  }
}
