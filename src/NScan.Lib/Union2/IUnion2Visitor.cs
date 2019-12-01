namespace NScan.Lib.Union2
{
  public interface IUnion2Visitor<in T1, in T2>
  {
    void Visit(T1 arg);
    void Visit(T2 dto);
  }
}