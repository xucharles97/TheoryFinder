using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TheoryFinder
{
    public class PUTCollector : CSharpSyntaxWalker
    {
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            //base.VisitMethodDeclaration(node);
            SyntaxList<AttributeListSyntax> attributes = node.AttributeLists;
            
            foreach( AttributeListSyntax attList in attributes)
            {
                
                //Console.WriteLine(attList);
                //att has AttributeSyntax type
                var targetAtt  = attList.Attributes.Where( att => att.Name.ToString().Equals("Fact"));
                foreach (var a in targetAtt)
                {
                    Console.WriteLine(a);
                }
            }
            
        
        }
    }
}
