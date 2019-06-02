using System.Linq;

namespace LocalsInit.Tests
{
    public abstract class AssemblyTestsBase
    {
        private readonly AssemblyFixture _fixture;

        protected AssemblyTestsBase(AssemblyFixture fixture)
        {
            _fixture = fixture;
        }

        protected bool GetFlagValue(string className, string methodName)
            => _fixture.ResultModule
                       .GetType($"{_fixture.OriginalModule.Assembly.Name.Name}.{className}")
                       .Methods
                       .Single(m => m.Name == methodName)
                       .Body
                       .InitLocals;
    }
}
