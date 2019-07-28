using TddXt.NScan.Specification.EndToEnd.AutomationLayer;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class HasTargetFrameworkReportedMessage : GenericReportedMessage<HasTargetFrameworkReportedMessage>
  {
    public HasTargetFrameworkReportedMessage(string returnValue) : base(returnValue)
    {
    }

    protected override HasTargetFrameworkReportedMessage NewInstance(string str)
    {
      return new HasTargetFrameworkReportedMessage(str);
    }

    public static HasTargetFrameworkReportedMessage HasFramework(string projectAssemblyNamePattern, string frameworkId)
    {
      return new HasTargetFrameworkReportedMessage(TestRuleFormats.FormatHasTargetFrameworkRule(projectAssemblyNamePattern, frameworkId));
    }
  }
}