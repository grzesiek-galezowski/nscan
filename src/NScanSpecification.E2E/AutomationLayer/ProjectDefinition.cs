namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectDefinition
{

  public ProjectDefinition(string projectName)
  {
    ProjectName = projectName;
    TargetFramework = AutomationLayer.TargetFramework.RecentDotNet;
  }

  public string ProjectName { get; }
  public string TargetFramework { get; set; }
}
