using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Math_Interpreter
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Example: ");
			MathTree tree = new MathTree(new MathTreeNode(new MathSymbol(MathSymbol.Operator.Add)));
			tree.root.right = new MathTreeNode(new MathSymbol(MathSymbol.Operator.Add));
			tree.root.right.right = new MathTreeNode(new MathSymbol(MathSymbol.Operator.Multiply));
			tree.root.right.right.right = new MathTreeNode(new MathSymbol(4));
			tree.root.right.right.left = new MathTreeNode(new MathSymbol(3));
			tree.root.right.left = new MathTreeNode(new MathSymbol(2));
			tree.root.left = new MathTreeNode(new MathSymbol(1));

			Console.WriteLine(": " + String.Join(" ", tree.ToPreOrderString().ToCharArray()));
			Console.WriteLine("=> " + tree.Evaluate());
			while (true)
			{
				Console.Write(": ");
				string input = Console.ReadLine();
				try
				{
					tree = MathTree.FromString(input);
					Console.WriteLine("=> " + tree.Evaluate());
				}
				catch (InvalidOperationException e)
				{
					Console.WriteLine("Error: " + e.Message);
				}
			}
		}
	}
}
