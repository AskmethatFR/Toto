namespace Redux.Kata.Kata_3;

public record Store
{
    private Store(ISlice[] slices)
    {
        foreach (var slice in slices)
        {
            _state = _state.AddSlice(slice);
        }
    }

    public static Store Init(params ISlice[] slices)
    {
        return new Store(slices);
    }


    private readonly AppState _state = AppState.InitialState();

    public TSlice? GetSlice<TSlice>() where TSlice : class, ISlice =>
        _state.GetSlice<TSlice>();

    public TSlice UpdateSlice<TSlice>(TSlice update) where TSlice : class, ISlice
    {
        return _state.UpdateSlice(update);
    }
}