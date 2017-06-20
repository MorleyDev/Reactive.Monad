using MorleyDev.Reactive.Monad;
using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Linq
{
	public static class LinqExtensions
	{
		public static Maybe<U> Select<U, T>(this Maybe<T> self, Func<T, U> mapper) => Maybe<U>.From(self.AsEnumerable().Select(mapper));
		public static Maybe<T> Where<T>(this Maybe<T> self, Func<T, bool> predicate) => Maybe<T>.From(self.AsEnumerable().Where(predicate));
		public static Maybe<U> SelectMany<U, T>(this Maybe<T> self, Func<T, LazyValue<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static Maybe<U> SelectMany<U, T>(this Maybe<T> self, Func<T, Maybe<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static LazyValue<T> DefaultIfEmpty<T>(this Maybe<T> self, T defaultValue = default(T)) => LazyValue<T>.From(self.AsEnumerable().DefaultIfEmpty(defaultValue).Single);

		public static LazyValue<U> Select<U, T>(this LazyValue<T> self, Func<T, U> mapper) => LazyValue<U>.From(self.AsEnumerable().Select(mapper).Single);
		public static Maybe<T> Where<T>(this LazyValue<T> self, Func<T, bool> predicate) => Maybe<T>.From(self.AsEnumerable().Where(predicate));
		public static LazyValue<U> SelectMany<U, T>(this LazyValue<T> self, Func<T, LazyValue<U>> mapper) => LazyValue<U>.From(self.AsEnumerable().SelectMany(mapper).Single);
		public static Maybe<U> SelectMany<U, T>(this LazyValue<T> self, Func<T, Maybe<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static LazyValue<U> DefaultIfEmpty<U, T>(this LazyValue<T> self, Func<T, Maybe<U>> mapper) => LazyValue<U>.From(self.AsEnumerable().SelectMany(mapper).Single);

		public static IO<U> Select<U, T>(this IO<T> self, Func<T, U> mapper) => IO<U>.From(self.AsObservable().Select(mapper));
		public static MaybeIO<T> Where<T>(this IO<T> self, Func<T, bool> predicate) => MaybeIO.From(self.AsObservable().Where(predicate));
		public static IO<U> SelectMany<U, T>(this IO<T> self, Func<T, IO<U>> mapper) => IO<U>.From(self.AsObservable().SelectMany(mapper));
		public static IO<U> SelectMany<U, T>(this IO<T> self, Func<T, LazyValue<U>> mapper) => IO<U>.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, Maybe<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(f => mapper(f)));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, MaybeIO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, IO<Maybe<U>>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(value => mapper(value).AsObservable().SelectMany(maybe => maybe)));
		public static IO<bool> IsEmpty<T>(this IO<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.Return(false));

		public static MaybeIO<U> Select<U, T>(this MaybeIO<T> self, Func<T, U> mapper) => MaybeIO.From(self.AsObservable().Select(mapper));
		public static MaybeIO<T> Where<T>(this MaybeIO<T> self, Func<T, bool> predicate) => MaybeIO.From(self.AsObservable().Where(predicate));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, IO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, Maybe<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, LazyValue<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, MaybeIO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, IO<Maybe<U>>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(value => mapper(value).AsObservable().SelectMany(maybe => maybe)));
		public static IO<bool> IsEmpty<T>(this MaybeIO<T> self) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().IsEmpty());
		public static IO<T> DefaultIfEmpty<T>(this MaybeIO<T> self, T defaultValue = default(T)) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().DefaultIfEmpty(defaultValue));

		public static IObservable<U> SelectMany<U, T>(this IObservable<T> self, Func<T, IObservable<Maybe<U>>> mapper) => Observable.AsObservable(self).SelectMany(value => mapper(value).SelectMany(maybe => maybe));
		public static MaybeIO<T> FirstOrNone<T>(this IObservable<T> self) => MaybeIO.From(Observable.AsObservable(self).Take(1));
		public static IO<T> SingleIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).SingleAsync());
		public static IO<bool> IsEmptyIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).IsEmpty());
		public static IO<T[]> ToArrayIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).ToArray());
		public static IO<IList<T>> ToListIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).ToList());

		public static IO<T> Do<T>(this IO<T> self, Action<T> actor) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().Do(actor));
		public static MaybeIO<T> Do<T>(this MaybeIO<T> self, Action<T> actor) => MaybeIO.From(self.AsObservable().Do(actor));

		public static MaybeIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, IO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static MaybeIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, MaybeIO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static IO<T> Catch<T, TException>(this IO<T> self, Func<TException, IO<T>> catcher) where TException : Exception => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static MaybeIO<T> Catch<T, TException>(this IO<T> self, Func<TException, MaybeIO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
	}
}