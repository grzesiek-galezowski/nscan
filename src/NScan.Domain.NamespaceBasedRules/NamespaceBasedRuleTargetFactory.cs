using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleTargetFactory
{
  public List<INamespaceBasedRuleTarget> NamespaceBasedRuleTargets(IEnumerable<CsharpProjectDto> csharpProjectDtos)
  {
    return csharpProjectDtos.Select(dataAccess =>
        new NamespaceBasedRuleTarget(
          new AssemblyName(dataAccess.AssemblyName),
          SourceCodeFilesUsingNamespaces(dataAccess),
          new NamespacesDependenciesCache()))
      .ToList<INamespaceBasedRuleTarget>();
  }

  private Arr<ISourceCodeFileUsingNamespaces> SourceCodeFilesUsingNamespaces(CsharpProjectDto dataAccess)
  {
    return dataAccess.SourceCodeFiles.Select(scf =>
      new SourceCodeFileUsingNamespaces(
        scf.Usings.Select(n => new NamespaceName(n)),
        scf.DeclaredNamespaces.Select(n => new NamespaceName(n))) as ISourceCodeFileUsingNamespaces);
  }
}
