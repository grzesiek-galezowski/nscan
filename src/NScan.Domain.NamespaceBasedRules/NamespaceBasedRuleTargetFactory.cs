using LanguageExt;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleTargetFactory
{
  public Seq<INamespaceBasedRuleTarget> NamespaceBasedRuleTargets(Seq<CsharpProjectDto> csharpProjectDtos)
  {
    return csharpProjectDtos.Select(dataAccess =>
        new NamespaceBasedRuleTarget(
          new AssemblyName(dataAccess.AssemblyName),
          SourceCodeFilesUsingNamespaces(dataAccess),
          new NamespacesDependenciesCache()))
      .ToSeq<INamespaceBasedRuleTarget>();
  }

  private Seq<ISourceCodeFileUsingNamespaces> SourceCodeFilesUsingNamespaces(CsharpProjectDto dataAccess)
  {
    return dataAccess.SourceCodeFiles.Select(scf =>
      new SourceCodeFileUsingNamespaces(
        scf.Usings.Select(n => new NamespaceName(n)),
        scf.DeclaredNamespaces.Select(n => new NamespaceName(n))) as ISourceCodeFileUsingNamespaces);
  }
}
