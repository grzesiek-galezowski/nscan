namespace NScan.Lib.Union1;

public interface IUnionVisitor<in T1>
{
  void Visit(T1 arg);
}