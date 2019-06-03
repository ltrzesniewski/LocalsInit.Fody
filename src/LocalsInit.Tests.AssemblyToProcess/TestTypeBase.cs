using System.Runtime.CompilerServices;

namespace LocalsInit.Tests.AssemblyToProcess
{
    public abstract class TestTypeBase
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected static int EnsureLocal(out int i)
            => i = 0;
    }
}
