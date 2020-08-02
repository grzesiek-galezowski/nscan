using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.NamespaceBasedRules
{
  public class NamespaceBasedRuleTargetFactory
  {
    public List<INamespaceBasedRuleTarget> NamespaceBasedRuleTargets(IEnumerable<CsharpProjectDto> csharpProjectDtos)
    {
      return csharpProjectDtos.Select(dataAccess =>
          new NamespaceBasedRuleTarget(
            dataAccess.AssemblyName,
            SourceCodeFilesUsingNamespaces(dataAccess),
            new NamespacesDependenciesCache()))
        .ToList<INamespaceBasedRuleTarget>();
    }

    private List<SourceCodeFileUsingNamespaces> SourceCodeFilesUsingNamespaces(CsharpProjectDto dataAccess)
    {
      return dataAccess.SourceCodeFiles.Select(scf => new SourceCodeFileUsingNamespaces(
          scf.Usings,
          scf.DeclaredNamespaces))
        .ToList();
    }
  }
}