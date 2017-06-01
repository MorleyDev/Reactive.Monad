using System;
using System.Collections.Generic;
using System.Linq;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class EnumerableExtensions
	{
		public static Maybe<T> ToMaybe<T>(this IEnumerable<T> self)
		{
			return LazyValue<Maybe<T>>.From(self.Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single);
		}

		public static LazyValue<T> ToLazyValue<T>(this IEnumerable<T> self)
		{
			return LazyValue<T>.From(self.Single);
		}

		public static IEnumerable<T> Merge<T>(this IEnumerable<IEnumerable<T>> self)
		{
			return self.SelectMany(inner => inner);
		}

		public static void ForEach<T>(this IEnumerable<T> self, Action<T> actor)
		{
			foreach (var value in self) actor(value);
		}

		public static void ForEach<T>(this IEnumerable<T> self, Action<T, int> actor)
		{
			var i = 0;
			foreach (var value in self) actor(value, i++);
		}
	}
}