﻿namespace NScan.Lib.Union7;

public interface IUnionTransformingVisitor<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TReturn>
{
  TReturn Visit(T1 arg);
  TReturn Visit(T2 dto);
  TReturn Visit(T3 dto);
  TReturn Visit(T4 dto);
  TReturn Visit(T5 dto);
  TReturn Visit(T6 dto);
  TReturn Visit(T7 dto);
}