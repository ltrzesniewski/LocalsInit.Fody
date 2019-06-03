using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace LocalsInit.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public const string AttributeFullName = "LocalsInit.LocalsInitAttribute";

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

                SetMethodDefaultsForType(typeDefinition, methodDefaults);

                foreach (var methodDefinition in typeDefinition.Methods)
                {
                    var methodValue = ConsumeAttribute(methodDefinition);

                    if (!methodDefinition.HasBody)
                        continue;

                    if (methodValue == null && methodDefaults.TryGetValue(methodDefinition, out var methodDefault))
                        methodValue = methodDefault;

                    if (methodValue == null)
                        methodValue = typeValue;

                    switch (methodValue)
                    {
                        case null:
                            break;

                        case false:
                            methodDefinition.Body.InitLocals = false;
                            break;

                        case true:
                            if (ShouldHaveInitLocals(methodDefinition))
                                methodDefinition.Body.InitLocals = true;
                            break;
                    }
                }
            }
        }

        private static void SetMethodDefaultsForType(TypeDefinition typeDefinition, Dictionary<MethodDefinition, bool> methodDefaults)
        {
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

            if (typeDefinition.HasEvents)
            {
                foreach (var eventDefinition in typeDefinition.Events)
                {
                    var eventValue = ConsumeAttribute(eventDefinition);
                    if (eventValue == null)
                        continue;

                    if (eventDefinition.AddMethod != null)
                        methodDefaults[eventDefinition.AddMethod] = eventValue.GetValueOrDefault();

                    if (eventDefinition.RemoveMethod != null)
                        methodDefaults[eventDefinition.RemoveMethod] = eventValue.GetValueOrDefault();

                    if (eventDefinition.InvokeMethod != null)
                        methodDefaults[eventDefinition.InvokeMethod] = eventValue.GetValueOrDefault();

                    if (eventDefinition.HasOtherMethods)
                    {
                        foreach (var methodDefinition in eventDefinition.OtherMethods)
                            methodDefaults[methodDefinition] = eventValue.GetValueOrDefault();
                    }
                }
            }
        }

        private static bool ShouldHaveInitLocals(MethodDefinition methodDefinition)
            => methodDefinition.Body.HasVariables
               || methodDefinition.Body.Instructions.Any(i => i.OpCode == OpCodes.Localloc);

        private static bool? ConsumeAttribute(ICustomAttributeProvider item)
        {
            if (!item.HasCustomAttributes)
                return null;

            var attr = item.CustomAttributes.SingleOrDefault(i => i.AttributeType.FullName == AttributeFullName);
            if (attr == null)
                return null;

            var value = (bool?)attr.ConstructorArguments.Single().Value;
            item.CustomAttributes.Remove(attr);

            return value;
        }
    }
}
