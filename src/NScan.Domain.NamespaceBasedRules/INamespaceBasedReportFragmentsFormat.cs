using LanguageExt;

namespace NScan.NamespaceBasedRules;

public interface INamespaceBasedReportFragmentsFormat
{
  string ApplyTo(Seq<NamespaceDependencyPath> paths, string header);
}
