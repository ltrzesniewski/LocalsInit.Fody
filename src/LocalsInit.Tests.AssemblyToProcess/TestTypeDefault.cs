using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace LocalsInit.Tests.AssemblyToProcess
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class TestTypeDefault : TestTypeBase
    {
        [LocalsInit(false)]
        static TestTypeDefault()
            => EnsureLocal(out _);

        [LocalsInit(false)]
        public TestTypeDefault()
            => EnsureLocal(out _);

        public void MethodDefault()
            => EnsureLocal(out _);

        [LocalsInit(true)]
        public void MethodTrue()
            => EnsureLocal(out _);

        [LocalsInit(false)]
        public void MethodFalse()
            => EnsureLocal(out _);

        [DllImport("dummy")]
        [LocalsInit(false)] // This is nonsensical, but make sure it is removed by the weaver
        public static extern void MethodWithoutBody();
    }
}
