using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyMetadata("ImplicitNullability.AppliesTo", "InputParameters, RefParameters, OutParametersAndResult, Fields")]
[assembly: AssemblyMetadata("ImplicitNullability.Fields", "RestrictToReadonly")]
[assembly: AssemblyMetadata("ImplicitNullability.GeneratedCode", "Exclude")]

[assembly: InternalsVisibleTo("Cvdm.Scheduler.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

#if !FEATURE_ASSEMBLY_METADATA_ATTRIBUTE
namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AssemblyMetadataAttribute : Attribute
    {
        public AssemblyMetadataAttribute(string key, string value) { }
    }
}
#endif