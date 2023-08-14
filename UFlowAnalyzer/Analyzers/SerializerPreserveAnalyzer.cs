using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace UFlowAnalyzer {
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SerializerPreserveAnalyzer : BaseInterfaceAttributeAnalyzer {
        private readonly DiagnosticDescriptor m_descriptor = new DiagnosticDescriptor(
            AnalyzerIds.Warnings.SERIALIZER_PRESERVE,
            "Serializer Not Preserved",
            "ISerializer implementation missing [Preserve] attribute",
            AnalyzerCategories.USAGE,
            DiagnosticSeverity.Warning,
            true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(m_descriptor);
        protected override DiagnosticDescriptor Descriptor => m_descriptor;
        protected override string InterfaceMetadataName => "UFlow.Addon.Serialization.Core.Runtime.ISerializer`1";
        protected override string AttributeMetadataName => "UnityEngine.Scripting.PreserveAttribute";
    }
}