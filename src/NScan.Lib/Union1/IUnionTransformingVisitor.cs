namespace NScan.Lib.Union1
{
  public interface IUnionTransformingVisitor<in T1, out TReturn>
  {
    TReturn Visit(T1 arg);
  }
}