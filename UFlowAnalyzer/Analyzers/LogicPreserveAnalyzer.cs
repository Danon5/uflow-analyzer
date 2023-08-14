using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace UFlowAnalyzer {
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class LogicPreserveAnalyzer : BaseInterfaceAttributeAnalyzer {
        private readonly DiagnosticDescriptor m_descriptor = new DiagnosticDescriptor(
            AnalyzerIds.Warnings.LOGIC_PRESERVE,
            "Logic Not Preserved",
            "ILogic implementation missing [Preserve] attribute",
            AnalyzerCategories.USAGE,
            DiagnosticSeverity.Warning,
            true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(m_descriptor);
        protected override DiagnosticDescriptor Descriptor => m_descriptor;
        protected override string InterfaceMetadataName => "UFlow.Core.Runtime.ILogic`1";
        protected override string AttributeMetadataName => "UnityEngine.Scripting.PreserveAttribute";
    }
}