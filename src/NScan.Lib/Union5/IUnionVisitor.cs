namespace NScan.Lib.Union5
{
  public interface IUnionVisitor<in T1, in T2, in T3, in T4, in T5>
  {
    void Visit(T1 arg);
    void Visit(T2 dto);
    void Visit(T3 dto);
    void Visit(T4 dto);
    void Visit(T5 dto);
  }
}