﻿using FluentAssertions;
using System.Linq;
using System.Reactive.Linq;
using Xunit;

namespace MorleyDev.Reactive.Monad.SampleTests
{
	public class EitherTests
	{
		[Fact]
		public void ValueTest()
		{
			Either<int, double> left = Either.Left(10);
			left.Lhs().ToArray().Should().BeEquivalentTo(new[] { 10 });
			left.Rhs().ToArray().Should().BeEmpty();

			left.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(25.0);

			Either<int, double> right = Either.Right(25.0);
			right.Rhs().ToArray().Should().BeEquivalentTo(new[] { 25.0 });
			right.Lhs().ToArray().Should().BeEmpty();
			right.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(50.0);
		}

		[Fact]
		public void FuncConversionTest()
		{
			Either<int, double> left = Either.Left(() => 10);
			left.Lhs().ToArray().Should().BeEquivalentTo(new[] { 10 });
			left.Rhs().ToArray().Should().BeEmpty();
			left.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(25.0);

			Either<int, double> right = Either.Right(() => 25.0);
			right.Rhs().ToArray().Should().BeEquivalentTo(new[] { 25.0 });
			right.Lhs().ToArray().Should().BeEmpty();
			right.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(50.0);
		}

		[Fact]
		public void LazyValueConversionTest()
		{
			Either<int, double> left = Either.Left(LazyValue.From(10));
			left.Lhs().ToArray().Should().BeEquivalentTo(new[] { 10 });
			left.Rhs().ToArray().Should().BeEmpty();

			left.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(25.0);

			Either<int, double> right = Either.Right(LazyValue.From(25.0));
			right.Rhs().ToArray().Should().BeEquivalentTo(new[] { 25.0 });
			right.Lhs().ToArray().Should().BeEmpty();
			right.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(50.0);
		}

		[Fact]
		public void MaybeMatchTest()
		{
			Either<int, double> left = Either.Left(LazyValue.From(10));
			left.Match(f => Maybe.Just(f * 10), r => Maybe.Just(50)).Single().Should().Be(100);
			left.Match(f => Maybe.None, r => Maybe.Just(50)).Count().Should().Be(0);

			Either<int, double> right = Either.Right(LazyValue.From(25.0));
			right.Match(f => Maybe.Just(0.0), r => Maybe.Just(r * 2)).Single().Should().Be(50.0);
			right.Match(f => Maybe.Just(20), r => Maybe.None).Count().Should().Be(0);
		}
	}
}
