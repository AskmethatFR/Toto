using System.Text.Json;

namespace Toto;

/// <summary>
///
/// Store => Gerer (modifier, récupérer, écouter ) le State global
/// State Global => Gère les slice créer par l'utilisateur du package
/// Slice => Sous-Partie du State créer par l'utilisateur
/// Action => Demande de modification de State ( Via un Slice )
/// Selector => Demande de lecture d'une partie du state ( la slice ) 
///
/// Mécanique interne
///
/// Reducer => Modifie un slice et retourne le slice modifier via le State ( retour le state )
/// Dispatcher => Gère les demande d'action ( l'utilisateur l'appel aka mediator )
/// Observable => Ecoute les changement du State 
/// Tu peux avoir une version asynchrone des 2
///  
/// </summary>
public class AppStateTests
{
    private readonly AppState _sut = new AppState();

    [Fact]
    public void AppStateBeAbleToGetASlice()
    {
        var slice = new Slice()
        {
            Value = 40
        };
        _sut.AddSlice(slice);

        Verify(slice);
    }

    [Fact]
    public void AppStateBeAbleToGetASlice2()
    {
        var slice = new Slice2()
        {
            Value = true,
            Texts = ["toto", "titi"]
        };
        _sut.AddSlice(slice);

        Verify(slice);
    }

    [Fact]
    public void AppStateBeAbleToGetASlice3()
    {
        var slice = new Slice3()
        {
            Value = new Slice()
            {
                Value = 3
            },
            Texts = ["toto", "titi"]
        };
        _sut.AddSlice(slice);

        Verify(slice);
    }

    [Fact]
    public void AppStateNotBeAbleToAddSliceTwice()
    {
        try
        {
            var slice = new Slice();
            _sut.AddSlice(slice, slice);

            Verify(slice);
            Assert.Fail("Adding same slice multiple time is prohibited");
        }
        catch (ArgumentException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void AppStateNotBeAbleToAddSliceTwice2()
    {
        try
        {
            var slice = new Slice();
            var slice2 = new Slice();
            _sut.AddSlice(slice, slice2);

            Verify(slice);
            Assert.Fail("Adding same slice multiple time is prohibited");
        }
        catch (ArgumentException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void AppStateBeAbleToGetSecondSlice()
    {
        var slice = new Slice();
        var slice2 = new Slice2();
        _sut.AddSlice(slice, slice2);

        Verify(slice2);
    }


    [Fact]
    public void NullSlice()
    {
        try
        {
            Slice slice = null!;
            _sut.AddSlice(slice);

            Verify(slice);
            Assert.Fail("Adding null slice is prohibited");
        }
        catch (NullReferenceException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void UnknownSlice()
    {
        var slice = new Slice();
        _sut.AddSlice(slice);

        Slice2? slice2 = null;
        Verify(slice2);
    }


    private void Verify<T>(T? slice) where T : class
    {
        T? actual = _sut.GetSlice<T>();

        Assert.Equivalent(slice, actual);
        if (actual is not null)
        {
            Assert.NotSame(slice, actual);
        }
    }
}

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

public record Slice
{
    public int Value { get; init; }
}

public record Slice2
{
    public bool Value { get; init; }
    public List<string> Texts { get; init; }
}

public class Slice3
{
    public Slice Value { get; init; }
    public List<string> Texts { get; init; }
}