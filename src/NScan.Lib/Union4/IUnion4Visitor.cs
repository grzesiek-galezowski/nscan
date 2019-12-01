namespace NScan.Lib.Union4
{
  public interface IUnion3Visitor<in T1, in T2, in T3, in T4>
  {
    void Visit(T1 arg);
    void Visit(T2 dto);
    void Visit(T3 dto);
    void Visit(T4 dto);
  }
}