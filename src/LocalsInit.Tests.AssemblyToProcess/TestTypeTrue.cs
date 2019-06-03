using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LocalsInit.Tests.AssemblyToProcess
{
    [LocalsInit(true)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class TestTypeTrue : TestTypeBase
    {
        [LocalsInit(false)]
        static TestTypeTrue()
            => EnsureLocal(out _);

        [LocalsInit(false)]
        public TestTypeTrue()
            => EnsureLocal(out _);

        public void MethodDefault()
            => EnsureLocal(out _);

        [LocalsInit(true)]
        public void MethodTrue()
            => EnsureLocal(out _);

        [LocalsInit(false)]
        public void MethodFalse()
            => EnsureLocal(out _);

        public void MethodWithoutLocals()
        {
            // No need to set localsinit on a method without locals or stackalloc.
        }

        public unsafe void MethodWithStackalloc()
        {
            // This test case is only relevant in Release mode,
            // as a local variable will be created in Debug.
            var data = stackalloc byte[12];
            Consume(data);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void Consume(void* _)
        {
        }
    }
}
