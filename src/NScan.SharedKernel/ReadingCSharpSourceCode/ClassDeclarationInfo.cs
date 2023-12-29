using LanguageExt;

namespace NScan.SharedKernel.ReadingCSharpSourceCode;

public class ClassDeclarationInfo(string className, string @namespace)
{
  private Lst<MethodDeclarationInfo> _methods;
  public string FullName => NamespacePrefix() + Name;
  public string Namespace { get; } = @namespace;
  public string Name { get; } = className;

  public Lst<MethodDeclarationInfo> Methods => _methods;

  private string NamespacePrefix()
  {
    return Namespace != string.Empty ? Namespace + "." : "";
  }

  public void AddMethodDeclaration(MethodDeclarationInfo methodDeclarationInfo)
  {
    _methods = _methods.Add(methodDeclarationInfo);
  }

  public void AddMethodDeclarations(Seq<MethodDeclarationInfo> methodDeclarationInfos)
  {
    _methods = _methods.AddRange(methodDeclarationInfos);
  }
}
