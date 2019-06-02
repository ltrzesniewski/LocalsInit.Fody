using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;

namespace LocalsInit.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override IEnumerable<string> GetAssembliesForScanning()
            => Enumerable.Empty<string>();

        public override bool ShouldCleanReference => true;

        public override void Execute()
        {
            var moduleValue = ConsumeAttribute(ModuleDefinition)
                              ?? ConsumeAttribute(ModuleDefinition.Assembly);

            foreach (var typeDefinition in ModuleDefinition.GetTypes())
            {
                var typeValue = ConsumeAttribute(typeDefinition) ?? moduleValue;

                foreach (var methodDefinition in typeDefinition.Methods)
                {
                    if (!methodDefinition.HasBody)
                        continue;

                    var methodValue = ConsumeAttribute(methodDefinition) ?? typeValue;
                    if (methodValue != null)
                        methodDefinition.Body.InitLocals = methodValue.GetValueOrDefault();
                }
            }
        }

        private static bool? ConsumeAttribute(ICustomAttributeProvider item)
        {
            if (!item.HasCustomAttributes)
                return null;

            var attr = item.CustomAttributes.SingleOrDefault(i => i.AttributeType.FullName == "LocalsInit.LocalsInitAttribute");
            if (attr == null)
                return null;

            var value = (bool?)attr.ConstructorArguments.Single().Value;
            item.CustomAttributes.Remove(attr);

            return value;
        }
    }
}
