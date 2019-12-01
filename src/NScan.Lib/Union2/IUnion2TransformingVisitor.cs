namespace NScan.Lib.Union2
{
  public interface IUnion2TransformingVisitor<in T1, in T2, out TReturn>
  {
    TReturn Visit(T1 arg);
    TReturn Visit(T2 dto);
  }
}