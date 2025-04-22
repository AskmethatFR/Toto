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