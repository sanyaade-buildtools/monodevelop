// 
// RedundantPrivateInspector.cs
//  
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
// 
// Copyright (c) 2012 Xamarin <http://xamarin.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using ICSharpCode.NRefactory.PatternMatching;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.CSharp.Refactoring
{
	/// <summary>
	/// Finds redundant internal modifiers.
	/// </summary>
	public class RedundantPrivateInspector : IInspector
	{
		string title = "Remove redundant 'private' modifier";

		public string Title {
			get {
				return title;
			}
			set {
				title = value;
			}
		}

		public IEnumerable<InspectionIssue> Run (BaseRefactoringContext context)
		{
			var visitor = new GatherVisitor (context, this);
			context.RootNode.AcceptVisitor (visitor);
			return visitor.FoundIssues;
		}

		class GatherVisitor : GatherVisitorBase
		{
			readonly RedundantPrivateInspector inspector;
			
			public GatherVisitor (BaseRefactoringContext ctx, RedundantPrivateInspector inspector) : base (ctx)
			{
				this.inspector = inspector;
			}

			void CheckNode(EntityDeclaration node)
			{
				foreach (var token in node.ModifierTokens) {
					if (token.Modifier == Modifiers.Private) {
						AddIssue(token, inspector.Title, delegate {
							using (var script = ctx.StartScript ()) {
								script.Remove(token);
							}
						});
					}
				}
			}

			public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
			{
				base.VisitMethodDeclaration(methodDeclaration);
				CheckNode(methodDeclaration);
			}
			
			public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
			{
				base.VisitFieldDeclaration(fieldDeclaration);
				CheckNode(fieldDeclaration);
			}
			
			public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
			{
				base.VisitPropertyDeclaration(propertyDeclaration);
				CheckNode(propertyDeclaration);
			}

			public override void VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration)
			{
				base.VisitIndexerDeclaration(indexerDeclaration);
				CheckNode(indexerDeclaration);
			}

			public override void VisitEventDeclaration(EventDeclaration eventDeclaration)
			{
				base.VisitEventDeclaration(eventDeclaration);
				CheckNode(eventDeclaration);
			}
			
			public override void VisitCustomEventDeclaration(CustomEventDeclaration eventDeclaration)
			{
				base.VisitCustomEventDeclaration(eventDeclaration);
				CheckNode(eventDeclaration);
			}
			
			public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
			{
				base.VisitConstructorDeclaration(constructorDeclaration);
				CheckNode(constructorDeclaration);
			}

			public override void VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
			{
				base.VisitOperatorDeclaration(operatorDeclaration);
				CheckNode(operatorDeclaration);
			}

			public override void VisitFixedFieldDeclaration(FixedFieldDeclaration fixedFieldDeclaration)
			{
				base.VisitFixedFieldDeclaration(fixedFieldDeclaration);
				CheckNode(fixedFieldDeclaration);
			}

			public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
			{
				if (!(typeDeclaration.Parent is TypeDeclaration)) {
					CheckNode(typeDeclaration);
				}
				base.VisitTypeDeclaration(typeDeclaration);
			}
		}
	}
}