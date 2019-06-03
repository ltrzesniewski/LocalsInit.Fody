using System.Linq;
using LocalsInit.Tests.Support;
using Xunit;

namespace LocalsInit.Tests
{
    public class AssemblyToProcessTests : AssemblyTestsBase
    {
        public AssemblyToProcessTests()
            : base(AssemblyFixture.AssemblyToProcess)
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

        [Theory]
        [InlineData("TestTypeFalse", "PropertyTrueSetFalse", false, true)]
        [InlineData("TestTypeFalse", "PropertyTrueSetFalse", true, false)]
        [InlineData("TestTypeFalse", "PropertyDefaultSetTrue", false, false)]
        [InlineData("TestTypeFalse", "PropertyDefaultSetTrue", true, true)]
        public void should_set_flag_on_property_accessors(string className, string propName, bool setter, bool expectedValue)
        {
            var propertyDefinition = GetTypeDefinition(className).Properties.Single(i => i.Name == propName);
            var method = setter ? propertyDefinition.SetMethod : propertyDefinition.GetMethod;
            method.Body.InitLocals.ShouldEqual(expectedValue);
        }
    }
}
