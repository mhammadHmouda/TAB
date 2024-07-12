namespace TAB.Domain.Core.Primitives.Maybe;

public static class MaybeExtensions
{
    public static TOut Match<TIn, TOut>(
        this Maybe<TIn> maybe,
        Func<TIn, TOut> onSuccess,
        Func<TOut> onFailure
    ) => maybe.HasValue ? onSuccess(maybe.Value) : onFailure();
}
