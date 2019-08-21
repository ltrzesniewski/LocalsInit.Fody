using Fody;
using LocalsInit.Fody;
using LocalsInit.Tests.AssemblyToProcess;
using LocalsInit.Tests.AssemblyToProcess.DefaultFalse;
using LocalsInit.Tests.AssemblyToProcess.DefaultTrue;
using Mono.Cecil;

#pragma warning disable 618

namespace LocalsInit.Tests
{
    public class AssemblyFixture
    {
        public static AssemblyFixture AssemblyToProcess { get; } = Create<AssemblyToProcessReference>();
        public static AssemblyFixture AssemblyToProcessDefaultTrue { get; } = Create<AssemblyToProcessDefaultTrueReference>();
        public static AssemblyFixture AssemblyToProcessDefaultFalse { get; } = Create<AssemblyToProcessDefaultFalseReference>();

        public TestResult TestResult { get; }

        public ModuleDefinition OriginalModule { get; }
        public ModuleDefinition ResultModule { get; }

        private AssemblyFixture(string assemblyPath)
        {
            var weavingTask = new ModuleWeaver();

            TestResult = weavingTask.ExecuteTestRun(
                assemblyPath,
                false
            );

            using (var assemblyResolver = new TestAssemblyResolver())
            {
                var readerParams = new ReaderParameters(ReadingMode.Immediate)
                {
                    AssemblyResolver = assemblyResolver
                };

                OriginalModule = ModuleDefinition.ReadModule(assemblyPath, readerParams);
                ResultModule = ModuleDefinition.ReadModule(TestResult.AssemblyPath, readerParams);
            }
        }

        private static AssemblyFixture Create<T>() => new AssemblyFixture(FixtureHelper.IsolateAssembly<T>());
    }
}
