using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class MaybeExtensions
	{
		public static Maybe<T> Or<T>(this Maybe<T> self, Maybe<T> rhs) => Maybe<T>.Or(self, rhs);

		public static MaybeIO<T> Or<T>(this Maybe<T> self, MaybeIO<T> rhs) => MaybeIO<T>.Or(self, rhs);

		public static MaybeIO<T> Or<T>(this Maybe<T> self, IO<T> rhs) => MaybeIO<T>.Or(self, rhs);

		public static MaybeIO<T> ToMaybeIO<T>(this Maybe<IO<T>> self) => self.ToObservable().SelectMany(io => io.AsObservable()).ToMaybeIO();
	}
}
