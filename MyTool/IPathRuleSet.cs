using MyTool.App;

namespace MyTool
{
  public interface IPathRuleSet
  {
    void AddDirectIndependentOfProjectRule(ProjectId depending, ProjectId dependent);
  }
}