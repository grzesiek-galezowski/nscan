using LanguageExt;

namespace NScan.NamespaceBasedRules;

public interface INamespaceBasedReportFragmentsFormat
{
  string ApplyTo(Arr<NamespaceDependencyPath> paths, string header);
}
