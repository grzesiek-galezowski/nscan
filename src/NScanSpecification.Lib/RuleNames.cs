using System;
using NScan.SharedKernel;

namespace NScanSpecification.Lib
{
  public static class RuleNames
  {
    public static T Switch<T>(string ruleName,
      Func<T> independentOfValueFactory,
      Func<T> correctNamespacesValueFactory, 
      Func<T> noCircularUsingsValueFactory, 
      Func<T> noUsingsValueFactory, 
      Func<T> isDecoratedWithAttributeValueFactory,
      Func<T> hasTargetFrameworkValueFactory)
    {
      if (ruleName == IndependentRuleMetadata.IndependentOf)
      {
        return independentOfValueFactory();
      }
      else if (ruleName == HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces)
      {
        return correctNamespacesValueFactory();
      }
      else if(ruleName == HasNoCircularUsingsRuleMetadata.HasNoCircularUsings)
      {
        return noCircularUsingsValueFactory();
      }
      else if(ruleName == "hasNoUsings") //bug
      {
        return noUsingsValueFactory();
      }
      else if(ruleName == HasAttributesOnRuleMetadata.HasAttributesOn)
      {
        return isDecoratedWithAttributeValueFactory();
      }
      else if(ruleName == HasTargetFrameworkRuleMetadata.HasTargetFramework)
      {
        return hasTargetFrameworkValueFactory();
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {ruleName}");
      }
    }
  }
  
  public static class ProjectScopedRuleNames
  {
    public static T Switch<T>(string ruleName,
      Func<T> correctNamespacesValueFactory, 
      Func<T> isDecoratedWithAttributeValueFactory,
      Func<T> hasTargetFrameworkValueFactory)
    {
      if (ruleName == HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces)
      {
        return correctNamespacesValueFactory();
      }
      else if(ruleName == HasAttributesOnRuleMetadata.HasAttributesOn)
      {
        return isDecoratedWithAttributeValueFactory();
      }
      else if(ruleName == HasTargetFrameworkRuleMetadata.HasTargetFramework)
      {
        return hasTargetFrameworkValueFactory();
      }
      else
      {
        throw new InvalidOperationException($"Not a project scoped rule name {ruleName}");
      }
    }
  }
}