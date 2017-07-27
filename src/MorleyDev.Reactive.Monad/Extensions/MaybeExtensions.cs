using System.Linq;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class MaybeExtensions
	{
		public static LazyValue<T> Or<T>(this Maybe<T> self, T rhs) => LazyValue.Defer(Maybe<T>.Or(self, Maybe.Just(rhs)).Single);

		public static LazyValue<T> Or<T>(this Maybe<T> self, LazyValue<T> rhs) => LazyValue.Defer(Maybe<T>.Or(self, Maybe.Defer(() => Maybe.Just(rhs.Single()))).Single);

		public static Maybe<T> Or<T>(this Maybe<T> self, Maybe<T> rhs) => Maybe<T>.Or(self, rhs);

		public static MaybeIO<T> Or<T>(this Maybe<T> self, MaybeIO<T> rhs) => MaybeIO<T>.Or(self, rhs);

		public static IO<T> Or<T>(this Maybe<T> self, IO<T> rhs) => MaybeIO<T>.Or(self, rhs);

		public static MaybeIO<T> ToMaybeIO<T>(this Maybe<T> self) => MaybeIO.From(self);
	}
}
