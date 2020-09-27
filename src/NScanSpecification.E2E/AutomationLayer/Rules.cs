using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtmaFileSystem;
using NScanSpecification.Lib.AutomationLayer;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class Rules
  {
    private readonly List<RuleUnionDto> _rules = new List<RuleUnionDto>();

    public Task SaveIn(AbsoluteFilePath fullRulesPath)
    {
      var lines = _rules.Select(Name).ToList();
      return File.WriteAllLinesAsync(fullRulesPath.ToString(), lines);
    }

    public void Add(RuleUnionDto ruleDto)
    {
      _rules.Add(ruleDto);
    }

    private static string Name(RuleUnionDto dto)
    {
      return dto.Accept(new RuleToStringVisitor());
    }
  }


}