using System;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class ObservableExtensions
	{
		public static IO<T> ToIO<T>(this IObservable<T> self) => IO<T>.From(self);

		public static MaybeIO<T> ToMaybeIO<T>(this IObservable<T> self) => MaybeIO.From(self);
		
		public static MaybeIO<T> ToMaybeIO<T>(this IObservable<Maybe<T>> self) => MaybeIO.From(self.SelectMany(s => s));
	}
}