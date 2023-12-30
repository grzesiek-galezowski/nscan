using LanguageExt;

namespace NScan.DependencyPathBasedRules;

public interface IDependencyPathReportFragmentsFormat
{
  string ApplyToPath(Seq<IDependencyPathBasedRuleTarget> violationPath);
}
