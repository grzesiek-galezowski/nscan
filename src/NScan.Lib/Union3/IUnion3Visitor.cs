namespace NScan.Lib.Union3
{
  public interface IUnion2Visitor<in T1, in T2, in T3>
  {
    void Visit(T1 arg);
    void Visit(T2 dto);
    void Visit(T3 dto);
  }
}