namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class MaybeIOExtensions
	{
		public static MaybeIO<T> Or<T>(this MaybeIO<T> self, MaybeIO<T> rhs) => MaybeIO<T>.Or(self, rhs);

		public static MaybeIO<T> Or<T>(this MaybeIO<T> self, Maybe<T> rhs) => MaybeIO<T>.Or(self, rhs);

		public static IO<T> Or<T>(this MaybeIO<T> self, IO<T> rhs) => MaybeIO<T>.Or(self, rhs);

		public static IO<T> Or<T>(this MaybeIO<T> self, LazyValue<T> rhs) => MaybeIO<T>.Or(self, IO.From(rhs));
	}
}
