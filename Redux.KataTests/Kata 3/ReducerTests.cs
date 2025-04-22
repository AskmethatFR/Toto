namespace Redux.Kata.Kata_3;

public class ReducerTests
{
    [Fact]
    public void ReducerIncrementAction()
    {
        var sut = new ActionReducer();
        var expected = new CounterSlice()
        {
            Value = 2
        };
        CounterSlice slice = sut.Reduce(new CounterSlice(), new CounterSliceAction()
        {
            Value = 2
        });

        Assert.Equal(expected, slice);
    }

    [Fact]
    public void ReduceNameAction()
    {
        var sut = new ActionReducer();
        var expected = new NameSlice()
        {
            Name = "Toto"
        };

        NameSlice slice = sut.Reduce(new NameSlice(), new NameAction()
        {
            Value = 2
        });

        Assert.Equal(expected, slice);
    }
}

public record NameAction : CounterSliceAction
{
}

public record CounterSliceAction : ActionReducer
{
}


public record NameSlice
{
    public string Name { get; init; }
}

public abstract class ActionReducer
{
    public CounterSlice Reduce(CounterSlice counterSlice, CounterSliceAction action)
    {
        return counterSlice with
        {
            Value = counterSlice.Value + action.Value
        };
    }
}

public record CounterSliceAction
{
    public int Value { get; init; }
}

public record CounterSlice : ISlice
{
    public int Value { get; init; }
}