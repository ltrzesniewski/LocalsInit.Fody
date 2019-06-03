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

            var methodDefaults = new Dictionary<MethodDefinition, bool>();

            foreach (var typeDefinition in ModuleDefinition.GetTypes())
            {
                var typeValue = ConsumeAttribute(typeDefinition) ?? moduleValue;

                methodDefaults.Clear();

                if (typeDefinition.HasProperties)
                {
                    foreach (var propertyDefinition in typeDefinition.Properties)
                    {
                        var propValue = ConsumeAttribute(propertyDefinition);
                        if (propValue == null)
                            continue;

                        if (propertyDefinition.GetMethod != null)
                            methodDefaults[propertyDefinition.GetMethod] = propValue.GetValueOrDefault();

                        if (propertyDefinition.SetMethod != null)
                            methodDefaults[propertyDefinition.SetMethod] = propValue.GetValueOrDefault();

                        if (propertyDefinition.HasOtherMethods)
                        {
                            foreach (var methodDefinition in propertyDefinition.OtherMethods)
                                methodDefaults[methodDefinition] = propValue.GetValueOrDefault();
                        }
                    }
                }

                foreach (var methodDefinition in typeDefinition.Methods)
                {
                    if (!methodDefinition.HasBody)
                        continue;

                    var methodValue = ConsumeAttribute(methodDefinition);

                    if (methodValue == null && methodDefaults.TryGetValue(methodDefinition, out var methodDefault))
                        methodValue = methodDefault;

                    if (methodValue == null)
                        methodValue = typeValue;

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
