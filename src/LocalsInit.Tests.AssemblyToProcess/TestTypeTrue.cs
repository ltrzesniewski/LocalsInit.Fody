using System.Diagnostics.CodeAnalysis;

namespace LocalsInit.Tests.AssemblyToProcess
{
    [LocalsInit(true)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class TestTypeTrue : TestTypeBase
    {
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
            var _ = stackalloc byte[12];
        }
    }
}
