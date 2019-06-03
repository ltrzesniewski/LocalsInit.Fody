using System.Linq;
using LocalsInit.Fody;
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
        [InlineData("TestTypeDefault", ".ctor", false)]
        [InlineData("TestTypeDefault", ".cctor", false)]
        [InlineData("TestTypeTrue", "MethodDefault", true)]
        [InlineData("TestTypeTrue", "MethodTrue", true)]
        [InlineData("TestTypeTrue", "MethodFalse", false)]
        [InlineData("TestTypeTrue", "MethodWithoutLocals", false)]
        [InlineData("TestTypeTrue", "MethodWithStackalloc", true)]
        [InlineData("TestTypeTrue", ".ctor", false)]
        [InlineData("TestTypeTrue", ".cctor", false)]
        [InlineData("TestTypeFalse", "MethodDefault", false)]
        [InlineData("TestTypeFalse", "MethodTrue", true)]
        [InlineData("TestTypeFalse", "MethodFalse", false)]
        public void should_set_flag(string className, string methodName, bool expectedValue)
            => GetFlagValue(className, methodName).ShouldEqual(expectedValue);

        [Theory]
        [InlineData("TestTypeFalse", "PropertyDefaultSetTrue", false, false)]
        [InlineData("TestTypeFalse", "PropertyDefaultSetTrue", true, true)]
        [InlineData("TestTypeFalse", "PropertyTrueSetFalse", false, true)]
        [InlineData("TestTypeFalse", "PropertyTrueSetFalse", true, false)]
        public void should_set_flag_on_property_accessors(string className, string propName, bool setter, bool expectedValue)
        {
            var propertyDefinition = GetTypeDefinition(className).Properties.Single(i => i.Name == propName);
            var method = setter ? propertyDefinition.SetMethod : propertyDefinition.GetMethod;
            method.Body.InitLocals.ShouldEqual(expectedValue);
        }

        [Theory]
        [InlineData("TestTypeFalse", "EventDefaultRemoveTrue", false, false)]
        [InlineData("TestTypeFalse", "EventDefaultRemoveTrue", true, true)]
        [InlineData("TestTypeFalse", "EventTrueRemoveFalse", false, true)]
        [InlineData("TestTypeFalse", "EventTrueRemoveFalse", true, false)]
        public void should_set_flag_on_event_modifiers(string className, string propName, bool remove, bool expectedValue)
        {
            var eventDefinition = GetTypeDefinition(className).Events.Single(i => i.Name == propName);
            var method = remove ? eventDefinition.RemoveMethod : eventDefinition.AddMethod;
            method.Body.InitLocals.ShouldEqual(expectedValue);
        }

        [Fact]
        public void should_consume_attribute_on_all_methods()
        {
            foreach (var methodDefinition in GetTypeDefinition("TestTypeDefault").Methods)
                methodDefinition.CustomAttributes.Any(i => i.AttributeType.FullName == ModuleWeaver.AttributeFullName).ShouldBeFalse();
        }
    }
}
