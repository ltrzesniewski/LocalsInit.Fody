using System.Linq;
using LocalsInit.Fody;
using LocalsInit.Tests.Support;
using Xunit;

namespace LocalsInit.Tests
{
    public class AssemblyToProcessDefaultFalseTests : AssemblyTestsBase
    {
        public AssemblyToProcessDefaultFalseTests()
            : base(AssemblyFixture.AssemblyToProcessDefaultFalse)
        {
        }

        [Theory]
        [InlineData("TestTypeDefault", "MethodDefault", false)]
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

        [Fact]
        public void should_consume_attribute_from_assembly_and_module()
        {
            _fixture.ResultModule.CustomAttributes.Any(i => i.AttributeType.FullName == ModuleWeaver.AttributeFullName).ShouldBeFalse();
            _fixture.ResultModule.Assembly.CustomAttributes.Any(i => i.AttributeType.FullName == ModuleWeaver.AttributeFullName).ShouldBeFalse();
        }
    }
}
