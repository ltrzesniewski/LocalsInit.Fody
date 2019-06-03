using System.Diagnostics.CodeAnalysis;

namespace LocalsInit.Tests.AssemblyToProcess
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class TestTypeDefault : TestTypeBase
    {
        public void MethodDefault()
            => EnsureLocal(out _);

        [LocalsInit(true)]
        public void MethodTrue()
            => EnsureLocal(out _);

        [LocalsInit(false)]
        public void MethodFalse()
            => EnsureLocal(out _);
    }
}
