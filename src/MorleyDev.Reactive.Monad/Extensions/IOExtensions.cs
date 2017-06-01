using System.Collections.Generic;
using System.Reactive.Linq;
using MorleyDev.Reactive.Monad.Extensions;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class IOExtensions
	{

		/// <summary>
		/// Merge an IO of an Array into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Merge<T>(this IO<T[]> self) => self.Select(s => s.ToObservable()).Merge().ToManyIO();

		/// <summary>
		/// Merge an IO of an Enumerable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <returns></returns>
		public static ManyIO<T> Merge<T>(this IO<IEnumerable<T>> self) => self.Select(s => s.ToObservable()).Merge().ToManyIO();
	}
}