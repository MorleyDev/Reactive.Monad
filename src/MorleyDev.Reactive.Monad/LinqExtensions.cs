using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad
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
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, IO<Maybe<U>>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this IO<T> self, Func<T, ManyIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static IO<bool> IsEmpty<T>(this IO<T> self) => IO.From(Observable.Return(false));

		public static MaybeIO<U> Select<U, T>(this MaybeIO<T> self, Func<T, U> mapper) => MaybeIO.From(self.AsObservable().Select(mapper));
		public static MaybeIO<T> Where<T>(this MaybeIO<T> self, Func<T, bool> predicate) => MaybeIO.From(self.AsObservable().Where(predicate));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, IO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, Maybe<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, LazyValue<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, MaybeIO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, IO<Maybe<U>>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, ManyIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static IO<bool> IsEmpty<T>(this MaybeIO<T> self) => IO.From(self.AsObservable().IsEmpty());
		public static IO<T> DefaultIfEmpty<T>(this MaybeIO<T> self, T defaultValue = default(T)) => IO.From(self.AsObservable().DefaultIfEmpty(defaultValue));


		public static ManyIO<U> Select<U, T>(this ManyIO<T> self, Func<T, U> mapper) => ManyIO.From(self.AsObservable().Select(mapper));
		public static ManyIO<T> Where<T>(this ManyIO<T> self, Func<T, bool> predicate) => ManyIO.From(self.AsObservable().Where(predicate));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, IO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, ManyIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, Maybe<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, MaybeIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, IO<Maybe<U>>> mapper) => ManyIO.From(self.AsObservable().SelectMany(value => mapper(value).SelectMany(v => v)));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, LazyValue<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<T> FirstAsync<T>(this ManyIO<T> self) => MaybeIO.From(self.AsObservable().Take(1));
		public static IO<T> SingleAsync<T>(this ManyIO<T> self) => IO.From(self.AsObservable().SingleAsync());
		public static IO<bool> IsEmpty<T>(this ManyIO<T> self) => IO.From(self.AsObservable().IsEmpty());
		public static ManyIO<T> DefaultIfEmpty<T>(this ManyIO<T> self, T defaultValue = default(T)) => ManyIO.From(self.AsObservable().DefaultIfEmpty(defaultValue));
		public static IO<T[]> ToArray<T>(this ManyIO<T> self) => IO.From(self.AsObservable().ToArray());
		public static IO<IList<T>> ToList<T>(this ManyIO<T> self) => IO.From(self.AsObservable().ToList());

		public static IO<T> Do<T>(this IO<T> self, Action<T> actor) => IO.From(self.AsObservable().Do(actor));
		public static ManyIO<T> Do<T>(this ManyIO<T> self, Action<T> actor) => ManyIO.From(self.AsObservable().Do(actor));
		public static MaybeIO<T> Do<T>(this MaybeIO<T> self, Action<T> actor) => MaybeIO.From(self.AsObservable().Do(actor));

		public static MaybeIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, IO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static MaybeIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, MaybeIO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static ManyIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, ManyIO<T>> catcher) where TException : Exception => ManyIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static IO<T> Catch<T, TException>(this IO<T> self, Func<TException, IO<T>> catcher) where TException : Exception => IO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static MaybeIO<T> Catch<T, TException>(this IO<T> self, Func<TException, MaybeIO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static ManyIO<T> Catch<T, TException>(this IO<T> self, Func<TException, ManyIO<T>> catcher) where TException : Exception => ManyIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static ManyIO<T> Catch<T, TException>(this ManyIO<T> self, Func<TException, IO<T>> catcher) where TException : Exception => ManyIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static ManyIO<T> Catch<T, TException>(this ManyIO<T> self, Func<TException, MaybeIO<T>> catcher) where TException : Exception => ManyIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static ManyIO<T> Catch<T, TException>(this ManyIO<T> self, Func<TException, ManyIO<T>> catcher) where TException : Exception => ManyIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
	}
}