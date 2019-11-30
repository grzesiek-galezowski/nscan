namespace NScan.Lib.Union1
{
  public interface IUnion1TransformingVisitor<in T1, out TReturn>
  {
    TReturn Visit(T1 arg);
  }
}