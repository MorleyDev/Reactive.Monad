using System;
using System.Linq;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad
{
	public static class LinqExtensions
	{
		public static Maybe<U> Select<U, T>(this Maybe<T> self, Func<T, U> mapper) => Maybe<U>.From(self.AsEnumerable().Select(mapper));
		public static Maybe<U> SelectMany<U, T>(this Maybe<T> self, Func<T, LazyValue<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static Maybe<U> SelectMany<U, T>(this Maybe<T> self, Func<T, Maybe<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static LazyValue<T> DefaultIfEmpty<T>(this Maybe<T> self, T defaultValue = default(T)) => LazyValue<T>.From(self.AsEnumerable().DefaultIfEmpty(defaultValue).Single);

		public static LazyValue<U> Select<U, T>(this LazyValue<T> self, Func<T, U> mapper) => LazyValue<U>.From(self.AsEnumerable().Select(mapper).Single);
		public static LazyValue<U> SelectMany<U, T>(this LazyValue<T> self, Func<T, LazyValue<U>> mapper) => LazyValue<U>.From(self.AsEnumerable().SelectMany(mapper).Single);
		public static Maybe<U> SelectMany<U, T>(this LazyValue<T> self, Func<T, Maybe<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static LazyValue<U> DefaultIfEmpty<U, T>(this LazyValue<T> self, Func<T, Maybe<U>> mapper) => LazyValue<U>.From(self.AsEnumerable().SelectMany(mapper).Single);

		public static IO<U> Select<U, T>(this IO<T> self, Func<T, U> mapper) => IO<U>.From(self.AsObservable().Select(mapper));
		public static IO<U> SelectMany<U, T>(this IO<T> self, Func<T, IO<U>> mapper) => IO<U>.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, Maybe<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(f => mapper(f)));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, MaybeIO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this IO<T> self, Func<T, ManyIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));

		public static MaybeIO<U> Select<U, T>(this MaybeIO<T> self, Func<T, U> mapper) => MaybeIO.From(self.AsObservable().Select(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, IO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, Maybe<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, LazyValue<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, MaybeIO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, ManyIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static IO<T> DefaultIfEmpty<T>(this MaybeIO<T> self, T defaultValue = default(T)) => IO.From(self.AsObservable().DefaultIfEmpty(defaultValue));

		public static ManyIO<U> Select<U, T>(this ManyIO<T> self, Func<T, U> mapper) => ManyIO.From(self.AsObservable().Select(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, IO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, ManyIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, Maybe<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, MaybeIO<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static ManyIO<U> SelectMany<U, T>(this ManyIO<T> self, Func<T, LazyValue<U>> mapper) => ManyIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<T> FirstAsync<T>(this ManyIO<T> self) => MaybeIO.From(self.AsObservable().Take(1));
		public static IO<T> SingleAsync<T>(this ManyIO<T> self) => IO.From(self.AsObservable().SingleAsync());
		public static ManyIO<T> DefaultIfEmpty<T>(this ManyIO<T> self, T defaultValue = default(T)) => ManyIO.From(self.AsObservable().DefaultIfEmpty(defaultValue));
	}
}