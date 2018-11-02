using System.Collections.Generic;
using System.IO;
using GlobExpressions;
using Sprache;
using TddXt.NScan.App;
using TddXt.NScan.Xml;

namespace TddXt.NScan.CompositionRoot
{
  public static class NScanMain
  {
    /// <summary>
    /// Entry point
    /// </summary>
    /// <param name="inputArguments">arguments</param>
    /// <param name="output">output for report</param>
    /// <param name="support">logging stuff</param>
    /// <returns></returns>
    public static int Run(
      InputArgumentsDto inputArguments, 
      INScanOutput output, 
      INScanSupport support)
    {
      var ruleDtos = ReadRules(inputArguments);
      var xmlProjects = ReadXmlProjects(inputArguments, support);
      var analysis = Analysis.PrepareFor(xmlProjects, support);

      analysis.AddRules(ruleDtos);

      analysis.Run();
      output.Write(analysis.Report);
      return analysis.ReturnCode;
    }

    private static List<XmlProject> ReadXmlProjects(InputArgumentsDto cliOptions, INScanSupport support)
    {
      var paths = ProjectPaths.From(
        cliOptions.SolutionPath,
        support);
      var xmlProjects = paths.LoadXmlProjects();
      return xmlProjects;
    }

    private static IEnumerable<RuleDto> ReadRules(InputArgumentsDto cliOptions)
    {
      var rulesString = File.ReadAllText(cliOptions.RulesFilePath);
      var ruleDtos = SingleLine().Many().Parse(rulesString);
      return ruleDtos;
    }

    public static Parser<RuleDto> SingleLine()
    {
      return from depending in Parse.AnyChar.Until(Parse.WhiteSpace).Text()
        from ruleName in Parse.AnyChar.Until(Parse.WhiteSpace).Text()
        from dependencyType in Parse.AnyChar.Until(Parse.Char(':')).Text()
        from dependency in Parse.AnyChar.Until(Parse.LineEnd).Text()
        select new RuleDto
        {
          DependingPattern = new Glob(depending),
          RuleName = ruleName,
          DependencyPattern = new Glob(dependency),
          DependencyType = dependencyType
        };
    }
  }
}