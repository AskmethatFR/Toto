using System.Text.Json;

namespace Redux.Kata.Kata_2;

internal record AppState
{
    private readonly Dictionary<Type, ISlice> _slices = [];

    public T? GetSlice<T>() where T : class, ISlice
    {
        if (!_slices.TryGetValue(typeof(T), out ISlice first))
        {
            return null;
        }

        return DeepCopy<T>((first as T)!);
    }

    public AppState AddSlice(ISlice slice)
    {
        _slices.Add(slice.GetType(), slice);
        return this;
    }

    public T UpdateSlice<T>(T value) where T : class, ISlice
    {
        if (_slices.ContainsKey(typeof(T)))
        {
            var deepCopy = DeepCopy(value);
            _slices[typeof(T)] = deepCopy;
            return deepCopy!;
        }
        else
        {
            throw new ArgumentNullException();
        }
    }

    private static T DeepCopy<T>(T first) where T : class, ISlice
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(first))!;
    }

    public static AppState InitialState()
    {
        return new AppState();
    }
}

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