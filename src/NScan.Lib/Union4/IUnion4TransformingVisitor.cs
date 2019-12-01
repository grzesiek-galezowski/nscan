namespace NScan.Lib.Union4
{
  public interface IUnion4TransformingVisitor<in T1, in T2, in T3, in T4, out TReturn>
  {
    TReturn Visit(T1 arg);
    TReturn Visit(T2 dto);
    TReturn Visit(T3 dto);
    TReturn Visit(T4 dto);
  }
}