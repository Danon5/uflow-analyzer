using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace UFlowAnalyzer {
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class LogicPreserveAnalyzer : DiagnosticAnalyzer {
        private static readonly DiagnosticDescriptor s_descriptor = new DiagnosticDescriptor(
            AnalyzerIds.Warnings.LOGIC_PRESERVE,
            "Logic Not Preserved",
            "ILogic implementation missing [Preserve] attribute",
            AnalyzerCategories.USAGE,
            DiagnosticSeverity.Warning,
            true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_descriptor);

        public override void Initialize(AnalysisContext context) {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.StructDeclaration);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context) {
            var node = (TypeDeclarationSyntax)context.Node;
            var semanticModel = context.SemanticModel;
            var nodeSymbol = semanticModel.GetDeclaredSymbol(node);
            if (nodeSymbol == null) return;
            var logicInterfaceSymbol = semanticModel.Compilation.GetTypeByMetadataName("UFlow.Core.Runtime.ILogic`1");
            if (!AnalyzerUtilities.ImplementsInterface(nodeSymbol, logicInterfaceSymbol)) return;
            var preserveInterfaceSymbol = semanticModel.Compilation.GetTypeByMetadataName("UnityEngine.Scripting.PreserveAttribute");
            if (AnalyzerUtilities.HasAttribute(nodeSymbol, preserveInterfaceSymbol)) return;
            context.ReportDiagnostic(Diagnostic.Create(s_descriptor, node.GetLocation()));
        }
    }
}