using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace UFlowAnalyzer {
    public abstract class BaseInterfaceAttributeAnalyzer : DiagnosticAnalyzer {
        protected abstract DiagnosticDescriptor Descriptor { get; }
        protected abstract string InterfaceMetadataName { get; }
        protected abstract string AttributeMetadataName { get; }
        
        public override void Initialize(AnalysisContext context) {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.StructDeclaration);
        }
        
        protected virtual void Analyze(SyntaxNodeAnalysisContext context) {
            var node = (TypeDeclarationSyntax)context.Node;
            var semanticModel = context.SemanticModel;
            var nodeSymbol = semanticModel.GetDeclaredSymbol(node);
            if (nodeSymbol == null) return;
            var interfaceSymbol = semanticModel.Compilation.GetTypeByMetadataName(InterfaceMetadataName);
            if (!AnalyzerUtilities.ImplementsInterface(nodeSymbol, interfaceSymbol)) return;
            var attributeSymbol = semanticModel.Compilation.GetTypeByMetadataName(AttributeMetadataName);
            if (AnalyzerUtilities.HasAttribute(nodeSymbol, attributeSymbol)) return;
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation()));
        }
    }
}