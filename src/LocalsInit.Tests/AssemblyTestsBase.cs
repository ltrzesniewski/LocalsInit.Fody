using System.Linq;
using Mono.Cecil;

namespace LocalsInit.Tests
{
    public abstract class AssemblyTestsBase
    {
        protected readonly AssemblyFixture _fixture;

        protected AssemblyTestsBase(AssemblyFixture fixture)
        {
            _fixture = fixture;
        }

        protected bool GetFlagValue(string className, string methodName)
            => GetTypeDefinition(className).Methods.Single(m => m.Name == methodName).Body.InitLocals;

        protected TypeDefinition GetTypeDefinition(string className)
            => _fixture.ResultModule.GetType($"{_fixture.OriginalModule.Assembly.Name.Name}.{className}");
    }
}
