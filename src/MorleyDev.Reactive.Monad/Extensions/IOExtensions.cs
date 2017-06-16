using System.Collections.Generic;
using System.Reactive.Linq;
using MorleyDev.Reactive.Monad.Extensions;
using System;

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
		public static ManyIO<T> Merge<T>(this IO<IEnumerable<T>> self) => self.AsObservable().Select(s => s.ToObservable()).Merge().ToManyIO();

		/// <summary>
		/// Merge an IO of an Enumerable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Concat<T>(this IO<IEnumerable<T>> self) => self.AsObservable().Select(s => s.ToObservable()).Concat().ToManyIO();
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
		public static ManyIO<T> Merge<T>(this IO<IObservable<T>> self) => self.SelectMany(s => s).ToManyIO();

		/// <summary>
		/// Merge an IO of an Enumerable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Concat<T>(this IO<IObservable<T>> self) => self.AsObservable().Select(s => (IObservable<T>)s).Concat().ToManyIO();
	}
}
