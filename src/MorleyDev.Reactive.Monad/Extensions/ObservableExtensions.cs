﻿using System;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class ObservableExtensions
	{
		public static IO<T> ToIO<T>(this IObservable<T> self) => IO<T>.From(self);

		public static ManyIO<T> ToManyIO<T>(this IObservable<T> self) => ManyIO<T>.From(self);
	}
}