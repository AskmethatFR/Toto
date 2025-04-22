using System.Text.Json;

namespace Redux.Kata.Kata_1;

public record AppState
{
    private Dictionary<Type, ISlice> _slice = [];

    public T? GetSlice<T>() where T : class, ISlice
    {
        if (!_slice.TryGetValue(typeof(T), out ISlice first))
        {
            return null;
        }

        return DeepCopy<T>((first as T)!);
    }

    public void AddSlice(params ISlice[] slice)
    {
        _slice = slice.ToDictionary(s => s.GetType(), s => s);
    }

    private static T DeepCopy<T>(T first) where T : class, ISlice
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(first))!;
    }
}