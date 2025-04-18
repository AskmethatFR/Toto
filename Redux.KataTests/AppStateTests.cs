using Toto.Kata_1.src;

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
        AddSlice(slice);
        Verify(slice);
    }

    [Fact]
    public void AppStateBeAbleToGetASlice2()
    {
        Slice2 slice = new Slice2()
        {
            Value = true,
            Texts = ["toto", "titi"]
        };
        AddSlice(slice);
        Verify(slice);
    }

    [Fact]
    public void AppStateBeAbleToGetASlice3()
    {
        Slice3 slice = new Slice3()
        {
            Value = new Slice()
            {
                Value = 3
            },
            Texts = ["toto", "titi"]
        };
        AddSlice(slice);

        Verify(slice);
    }


    [Fact]
    public void AppStateNotBeAbleToAddSliceTwice()
    {
        try
        {
            Slice slice = new Slice();
            AddSlice(slice, slice);

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
            Slice slice = new Slice();
            Slice slice2 = new Slice();
            AddSlice(slice, slice2);

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
        Slice slice = new Slice();
        Slice2 slice2 = new Slice2();
        AddSlice(slice, slice2);

        Verify(slice2);
    }


    [Fact]
    public void NullSlice()
    {
        try
        {
            Slice slice = null!;
            AddSlice(slice);

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
        Slice slice = new Slice();
        AddSlice(slice);

        Slice2? slice2 = null;
        Verify(slice2);
    }

    private void AddSlice(params ISlice[] slice)
    {
        _sut.AddSlice(slice);
    }

    private void Verify<T>(T? slice) where T : class, ISlice
    {
        T? actual = _sut.GetSlice<T>();

        Assert.Equivalent(slice, actual);
        if (actual is not null)
        {
            Assert.NotSame(slice, actual);
        }
    }
}

public record Slice : ISlice
{
    public int Value { get; init; }
}

public record Slice2: ISlice
{
    public bool Value { get; init; }
    public List<string> Texts { get; init; }
}

public class Slice3: ISlice
{
    public Slice Value { get; init; }
    public List<string> Texts { get; init; }
}