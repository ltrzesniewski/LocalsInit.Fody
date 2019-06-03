using System;
using System.Diagnostics.CodeAnalysis;

namespace LocalsInit.Tests.AssemblyToProcess
{
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
