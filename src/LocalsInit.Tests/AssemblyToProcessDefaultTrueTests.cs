using LocalsInit.Tests.Support;
using Xunit;

namespace LocalsInit.Tests
{
    public class AssemblyToProcessDefaultTrueTests : AssemblyTestsBase
    {
        public AssemblyToProcessDefaultTrueTests()
            : base(AssemblyFixture.AssemblyToProcessDefaultTrue)
        {
        }

        [Theory]
        [InlineData("TestTypeDefault", "MethodDefault", true)]
        [InlineData("TestTypeDefault", "MethodTrue", true)]
        [InlineData("TestTypeDefault", "MethodFalse", false)]
        [InlineData("TestTypeTrue", "MethodDefault", true)]
        [InlineData("TestTypeTrue", "MethodTrue", true)]
        [InlineData("TestTypeTrue", "MethodFalse", false)]
        [InlineData("TestTypeFalse", "MethodDefault", false)]
        [InlineData("TestTypeFalse", "MethodTrue", true)]
        [InlineData("TestTypeFalse", "MethodFalse", false)]
        public void should_set_flag(string className, string methodName, bool expectedValue)
            => GetFlagValue(className, methodName).ShouldEqual(expectedValue);
    }
}
