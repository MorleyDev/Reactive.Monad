MorleyDev Monad
===============

The MorleyDev Monad Library is built upon the idea that the .NET Standard provides two abstract representation of Monads:
* IEnumerable<T>: For synchronous pull-based monads
* IObservable<T>: For asynchronous push-based monads

These two monads allow for Synchronous and Asynchronous LINQ, allowing for pure functional code, but are not by themselves sufficient. Neither correctly encode in their names any additional restraints a developer needs.

* IEnumerable is a set of data from 0 - N
* IObservable is a set of data from 0 - N

Neither can therefore encode in the return type of a function whether they:
* Perform any IO
* Return a data set of size 1
* Return a data set of size 0 - 1

Instead, IO must be assumed and the size of the data set, without any internal knowledge of the function, must be assumed to be 0 - N.

This library provides classes to explicitly encode the extra information in the return type
* Synchronous
  * Maybe<T>: A data set of size 0 - 1
  * LazyValue<T>: Returns a data set of size 1
    * Mostly intended for usage internally (As synchronous single non-io values will typically just be returned without a wrapping Monad)

* Asynchronous
  * IO<T>: Performs IO and returns a data set of size 1
  * MaybeIO<T>: Performs IO and returns a data set of size 0 - 1

The intent is to use these in situations where a non-descriptive IEnumerable or IObservable would otherwise be returned (or, with Maybe, a null).
This allows IEnumerable and IObservable to always be treated as sets of data from 0 - N, whilst allowing for leaner code that leverages the LINQ/RX syntax for single/optional value results.

Contact-preserving LINQ implementations are also provided, allowing for IO/MaybeIO/Maybe/LazyValue to flow downwards through a LINQ statement.

This library also provides classes built on top of the guarantees provided by these primitives:
* Either<L, R>
