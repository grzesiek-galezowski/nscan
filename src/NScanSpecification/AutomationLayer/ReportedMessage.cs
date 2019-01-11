using System.Linq;
using TddXt.NScan.Domain;
using static System.Environment;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class ReportedMessage
  {
    private readonly string _returnValue;

    public override string ToString()
    {
      return _returnValue;
    }

    public ReportedMessage(string returnValue)
    {
      _returnValue = returnValue;
    }

    public ReportedMessage Error()
    {
      return new ReportedMessage(ToString() + "[ERROR]");
    }

    public ReportedMessage Ok()
    {
      return new ReportedMessage(ToString() + "[OK]");
    }

    public static ReportedMessage HasCorrectNamespaces(string projectGlob)
    {
      return new ReportedMessage($"{projectGlob} hasCorrectNamespaces: ");
    }

    public static ReportedMessage HasNoCircularUsings(string projectGlob)
    {
      return new ReportedMessage($"{projectGlob} hasNoCircularUsings: ");
    }

    public ReportedMessage ButFoundIncorrectNamespaceFor(string fileName, string actualNamespace)
    {
      return new ReportedMessage(ToString() +
                                 $" but the file {fileName} has incorrect namespace {actualNamespace}");
    }

    public ReportedMessage ButNoNamespaceDeclaredIn(string fileName)
    {
      return new ReportedMessage(ToString() + $" but the file {fileName} has no namespace declared");
    }

    public ReportedMessage ButHasMultipleNamespaces(string fileName, params string[] namespaces)
    {
      return new ReportedMessage(ToString() + $" but the file {fileName} declares multiple namespaces: {string.Join(", ", namespaces)}");
    }


    public ReportedMessage ExpectedNamespace(string projectName, string rootNamespace)
    {
      return new ReportedMessage(ToString() + $"{NewLine}" + $"{projectName} has root namespace {rootNamespace}");
    }

    public static ReportedMessage ProjectIndependentOfAssembly(string projectName, string packageName)
    {
      return new ReportedMessage(
        $"[{projectName}] independentOf [assembly:{packageName}]: ");
    }

    public static ReportedMessage ProjectIndependentOfPackage(string projectName, string packageName)
    {
      return new ReportedMessage($"[{projectName}] independentOf [package:{packageName}]: ");
    }

    public static ReportedMessage ProjectIndependentOfProject(string assemblyNamePattern, string projectAssemblyNamePattern)
    {
      return new ReportedMessage($"[{assemblyNamePattern}] independentOf [project:{projectAssemblyNamePattern}]: ");
    }

    public ReportedMessage ViolationPath(params string[] path)
    {
      return new ReportedMessage(this
                                 + NewLine +
                                 "PathViolation in path: "+ string.Join("->", 
                                   path.Select(c => $"[{c}]")));
    }

    public ReportedMessage CycleFound(string projectName, params string[] cyclePath)
    {
      return new ReportedMessage(this + NewLine + 
                                $"Discovered cycle(s) in project {projectName}:{NewLine}" +
                                Format(cyclePath));
    }

    private static string Format(string[] cyclePath)
    {
      var result = $"Cycle 1:{NewLine}";
      for (var i = 0; i < cyclePath.Length; ++i)
      {
        result += ((i + 1) * 2).Spaces() + cyclePath[i] + NewLine;
      }

      return result;
    }
  }
}