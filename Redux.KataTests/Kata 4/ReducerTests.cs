using Redux.Kata.Kata_3;

namespace Redux.Kata.Kata_4;

public class ReducerTests
{
    [Fact]
    public void ReducerIncrementAction()
    {
        var sut = new ReduceCounterSlice();
        var expected = new CounterSlice()
        {
            Value = 2
        };
        CounterSlice slice = sut.Reduce(new CounterSlice(), new CounterSliceAction(2));
        Assert.Equal(expected, slice);
    }

    [Fact]
    public void ReduceNameAction()
    {
        var sut = new ReduceName();
        var expected = new NameSlice()
        {
            Name = "Toto"
        };

        NameSlice slice = sut.Reduce(new NameSlice(), new NameAction("Toto"));

        Assert.Equal(expected, slice);
    }
}

public record NameAction(string Name) : IAction;

public record ReduceName() : Reducer<NameSlice, NameAction>
{
    public override NameSlice Reduce(NameSlice slice, NameAction action)
    {
        return slice with { Name = action.Name };
    }
}

public record NameSlice : ISlice
{
    public string Name { get; init; }
}

public record CounterSliceAction(int Value) : IAction;

public record ReduceCounterSlice : Reducer<CounterSlice, CounterSliceAction>
{
    public override CounterSlice Reduce(CounterSlice slice, CounterSliceAction action)
    {
        return slice with { Value = slice.Value + action.Value };
    }
}

public record CounterSlice : ISlice
{
    public int Value { get; init; }
}