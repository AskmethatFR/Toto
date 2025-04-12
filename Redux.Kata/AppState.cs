using System.Text.Json;

namespace Redux.Kata;

public record AppState
{
    private Dictionary<Type, object> _slice = [];

    public T? GetSlice<T>() where T : class
    {
        if (!_slice.TryGetValue(typeof(T), out object? first))
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(first))!;
    }

    public void AddSlice(params object[] slice)
    {
        _slice = slice.ToDictionary(s => s.GetType(), s => s);
    }
}