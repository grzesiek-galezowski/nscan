using System;
using NScan.DependencyPathBasedRules;

namespace NScan.Domain
{
  public class ProjectShadowingException : Exception
  {
    public ProjectShadowingException(IDependencyPathBasedRuleTarget previousProject, IDependencyPathBasedRuleTarget newProject)
      : base(
        "Two distinct projects are being added with the same path. " +
        $"{previousProject} would be shadowed by {newProject}. " +
        "This typically indicates a programmer error.")
    {
      
    }
  }
}