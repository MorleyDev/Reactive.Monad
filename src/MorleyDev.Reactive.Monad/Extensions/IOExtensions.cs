using System.Collections.Generic;
using System.Reactive.Linq;
using MorleyDev.Reactive.Monad.Extensions;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class IOExtensionsNestedEnumerable
	{
		/// <summary>
		/// Merge an IO of an Enumerable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Merge<T, U>(this IO<U> self) where U : IEnumerable<T> => self.Select(s => s.ToObservable()).Merge().ToManyIO();

		/// <summary>
		/// Merge an IO of an Enumerable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Concat<T, U>(this IO<U> self) where U : IEnumerable<T> => self.Select(s => s.ToObservable()).Concat().ToManyIO();
	}

	public static class IOExtensionsNestedObservable
	{

		/// <summary>
		/// Merge an IO of an Enumerable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Merge<T, U>(this IO<U> self) where U : IObservable<T> => self.SelectMany(s => s).ToManyIO();

		/// <summary>
		/// Merge an IO of an Enumerable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Concat<T, U>(this IO<U> self) where U : IObservable<T> => self.Select(s => (IObservable<T>)s).Concat().ToManyIO();
	}
}
