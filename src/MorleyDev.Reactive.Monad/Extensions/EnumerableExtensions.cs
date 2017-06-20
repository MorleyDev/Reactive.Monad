using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

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
	}
}
