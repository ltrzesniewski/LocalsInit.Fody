using Xunit;

namespace LocalsInit.Tests.Support
{
    internal static class AssertionExtensions
    {
        public static void ShouldEqual<T>(this T actual, T expected)
            => Assert.Equal(expected, actual);
        
        public static void ShouldBeTrue(this bool actual)
            => Assert.True(actual);

        public static void ShouldBeFalse(this bool actual)
            => Assert.False(actual);
    }
}
