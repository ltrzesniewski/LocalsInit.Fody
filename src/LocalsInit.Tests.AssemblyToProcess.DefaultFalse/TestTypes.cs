using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using LocalsInit;

[assembly: LocalsInit(false)]

namespace LocalsInit.Tests.AssemblyToProcess.DefaultFalse
{
    public abstract class TestTypeBase
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected static void EnsureLocal(out int i)
            => i = 0;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public class TestTypeDefault : TestTypeBase
    {
        public void MethodDefault()
            => EnsureLocal(out var i);

        [LocalsInit(true)]
        public void MethodTrue()
            => EnsureLocal(out var i);

        [LocalsInit(false)]
        public void MethodFalse()
            => EnsureLocal(out var i);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    [LocalsInit(true)]
    public class TestTypeTrue : TestTypeBase
    {
        public void MethodDefault()
            => EnsureLocal(out var i);

        [LocalsInit(true)]
        public void MethodTrue()
            => EnsureLocal(out var i);

        [LocalsInit(false)]
        public void MethodFalse()
            => EnsureLocal(out var i);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    [LocalsInit(false)]
    public class TestTypeFalse : TestTypeBase
    {
        public void MethodDefault()
            => EnsureLocal(out var i);

        [LocalsInit(true)]
        public void MethodTrue()
            => EnsureLocal(out var i);

        [LocalsInit(false)]
        public void MethodFalse()
            => EnsureLocal(out var i);
    }
}
