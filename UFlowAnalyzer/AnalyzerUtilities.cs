using System.Linq;
using Microsoft.CodeAnalysis;

namespace UFlowAnalyzer {
    internal static class AnalyzerUtilities {
        public static bool ImplementsInterface(ITypeSymbol typeSymbol, INamedTypeSymbol interfaceSymbol) {
            if (typeSymbol == null || interfaceSymbol == null) return false;
            if (typeSymbol.AllInterfaces.Any(i => i.OriginalDefinition.Equals(interfaceSymbol, SymbolEqualityComparer.Default) && 
                i.TypeArguments.Length == interfaceSymbol.TypeArguments.Length)) return true;
            return typeSymbol.BaseType != null && ImplementsInterface(typeSymbol.BaseType, interfaceSymbol);
        }

        public static bool HasAttribute(ISymbol symbol, INamedTypeSymbol attributeSymbol) {
            if (symbol == null || attributeSymbol == null) return false;
            return symbol.GetAttributes().Any(a => a.AttributeClass != null && 
                a.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
        }
    }
}