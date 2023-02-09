namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectDefinition
{

  public ProjectDefinition(string projectName)
  {
    ProjectName = projectName;
    TargetFramework = "net6.0";
  }

  public string ProjectName { get; }
  public string TargetFramework { get; set; }
}
