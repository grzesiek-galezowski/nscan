using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtmaFileSystem;
using NScanSpecification.Lib.AutomationLayer;
using NSubstitute;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class Rules
  {
    private readonly List<ITestedRuleDefinition> _rules = new();

    public Task SaveIn(AbsoluteFilePath fullRulesPath)
    {
      var lines = _rules.Select(r => r.Name()).ToList();
      return File.WriteAllLinesAsync(fullRulesPath.ToString(), lines);
    }

    public void Add(ITestedRuleDefinition definition)
    {
      _rules.Add(definition);
    }
  }


}
