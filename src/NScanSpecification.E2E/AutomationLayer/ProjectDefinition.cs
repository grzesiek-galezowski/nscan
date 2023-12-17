namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectDefinition(string projectName)
{
  public string ProjectName { get; } = projectName;
  public string TargetFramework { get; set; } = AutomationLayer.TargetFramework.RecentDotNet;
}
