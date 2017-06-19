using System;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad
{
	public static class ManyIO
	{
		/// <summary>
		/// Wraps an 0-N observable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static ManyIO<T> From<T>(IObservable<T> source) => ManyIO<T>.From(source);

		/// <summary>
		/// Wraps an 0-N observable into a ManyIO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static ManyIO<T> From<T>(Func<IObservable<T>> source) => ManyIO<T>.From(Observable.Defer(source));
	}

	/// <summary>
	/// ManyIO encapsulates an IO operation that can return 0-N results.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ManyIO<T> : IObservable<T>
	{
		private readonly IObservable<T> _observableStream;

		private ManyIO(IObservable<T> observableStream)
		{
			_observableStream = observableStream;
		}

		public static ManyIO<T> Empty => new ManyIO<T>(Observable.Empty<T>());

		public IObservable<T> AsObservable() => _observableStream;

		public static ManyIO<T> From(IObservable<T> source) => new ManyIO<T>(source);

		public IDisposable Subscribe(IObserver<T> observer) => _observableStream.Subscribe(observer);
	}
}