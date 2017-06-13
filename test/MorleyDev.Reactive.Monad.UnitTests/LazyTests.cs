using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class LazyTests
	{
		[Fact]
		public void Basic()
		{
			LazyValue<int> x = 10;
			x.Extract().Should().Be(10);
			x.Select(v => v * 2).First().Should().Be(20);
			x.Should().BeEquivalentTo(new[] { 10 });
			x.ToArray().Should().BeEquivalentTo(new[] { 10 });

			LazyValue<int> y = (Func<int>)(() => 10);
			y.Extract().Should().Be(10);
			y.Select(v => v * 2).First().Should().Be(20);
			y.Should().BeEquivalentTo(new[] { 10 });
			y.ToArray().Should().BeEquivalentTo(new[] { 10 });
		}
	}
}
