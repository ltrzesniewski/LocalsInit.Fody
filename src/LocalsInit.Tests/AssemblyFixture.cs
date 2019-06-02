using Fody;
using LocalsInit.Fody;
using Mono.Cecil;

#pragma warning disable 618

namespace LocalsInit.Tests
{
    public class AssemblyFixture
    {
        public static AssemblyFixture AssemblyToProcess { get; } = new AssemblyFixture("LocalsInit.Tests.AssemblyToProcess.dll");

        public TestResult TestResult { get; }

        public ModuleDefinition OriginalModule { get; }
        public ModuleDefinition ResultModule { get; }

        private AssemblyFixture(string assemblyName)
        {
            var assemblyPath = FixtureHelper.IsolateAssembly(assemblyName);

            var weavingTask = new ModuleWeaver();

            TestResult = weavingTask.ExecuteTestRun(
                assemblyPath,
                false
            );

            var readerParams = new ReaderParameters(ReadingMode.Immediate)
            {
                ReadSymbols = true
            };

            OriginalModule = ModuleDefinition.ReadModule(assemblyPath, readerParams);
            ResultModule = ModuleDefinition.ReadModule(TestResult.AssemblyPath, readerParams);
        }
    }
}
