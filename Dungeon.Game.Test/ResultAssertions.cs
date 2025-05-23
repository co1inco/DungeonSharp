using CSharpFunctionalExtensions;
using Shouldly;

namespace Dungeon.Game.Test;

[ShouldlyMethods]
public static class ResultAssertions
{

    public static void ShouldSucceed(this Result result)
    {
        result.IsSuccess.ShouldBeTrue();
    }

    public static void ShouldSucceedWith<T>(this Result<T> result, T expected)
    {
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expected);
    }
    
    public static void ShouldSucceedWith<T>(this Result<T> result, Func<T, bool> predicate)
    {
        result.IsSuccess.ShouldBeTrue();
        predicate(result.Value).ShouldBeTrue();
    }
    
    public static void ShouldFail(this Result result)
    {
        result.IsFailure.ShouldBeTrue();
    }
    
    public static void ShouldFailWith(this Result result, string message)
    {
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(message);
    }
    
    public static void ShouldFail<T>(this Result<T> result)
    {
        result.IsFailure.ShouldBeTrue();
    }
    
    public static void ShouldFailWith<T>(this Result<T> result, string message)
    {
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(message);
    }


    public static void ShouldHaveSomeValue<T>(this Maybe<T> maybe)
    {
        maybe.HasValue.ShouldBeTrue();
    }
    
    public static void ShouldHaveValue<T>(this Maybe<T> maybe, T expected)
    {
        maybe.HasValue.ShouldBeTrue();
        maybe.Value.ShouldBe(expected);
    }
    
    public static void ShouldHaveValue<T>(this Maybe<T> maybe, Func<T, bool> predicate)
    {
        maybe.HasValue.ShouldBeTrue();
        predicate(maybe.Value).ShouldBeTrue();
    }
    
    public static void ShouldHaveNoValue<T>(this Maybe<T> maybe)
    {
        maybe.HasValue.ShouldBeFalse();
    }
    
}