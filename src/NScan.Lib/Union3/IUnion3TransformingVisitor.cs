namespace NScan.Lib.Union3
{
  public interface IUnion2TransformingVisitor<in T1, in T2, in T3, out TReturn>
  {
    TReturn Visit(T1 arg);
    TReturn Visit(T2 dto);
    TReturn Visit(T3 dto);
  }
}