using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LocalsInit.Tests.AssemblyToProcess
{
    public abstract class TestTypeBase
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected static int EnsureLocal(out int i)
            => i = 0;
    }

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
    }

    [LocalsInit(false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ValueParameterNotUsed")]
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public class TestTypeFalse : TestTypeBase
    {
        public void MethodDefault()
            => EnsureLocal(out _);

        [LocalsInit(true)]
        public void MethodTrue()
            => EnsureLocal(out _);

        [LocalsInit(false)]
        public void MethodFalse()
            => EnsureLocal(out _);

        public int PropertyDefaultSetTrue
        {
            get => EnsureLocal(out _);
            [LocalsInit(true)] set => EnsureLocal(out _);
        }

        [LocalsInit(true)]
        public int PropertyTrueSetFalse
        {
            get => EnsureLocal(out _);
            [LocalsInit(false)] set => EnsureLocal(out _);
        }

        public event Action EventDefaultRemoveTrue
        {
            add => EnsureLocal(out _);
            [LocalsInit(true)] remove => EnsureLocal(out _);
        }

        [LocalsInit(true)]
        public event Action EventTrueRemoveFalse
        {
            add => EnsureLocal(out _);
            [LocalsInit(false)] remove => EnsureLocal(out _);
        }
    }
}
