namespace Redux.Kata.Kata_3;

public abstract record Reducer<TS, TA>
    where TS : class, ISlice
    where TA : class, IAction
{
    public abstract TS Reduce(TS slice, TA action);
}