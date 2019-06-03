using System;
using System.Diagnostics.CodeAnalysis;

namespace LocalsInit
{
    /// <summary>
    /// Set the value of the localsinit flag on methods under the scope this attribute is applied on.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property, Inherited = false)]
    public sealed class LocalsInitAttribute : Attribute
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="value">The value of the localsinit flag.</param>
        public LocalsInitAttribute(bool value)
        {
        }
    }
}
