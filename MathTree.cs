using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Math_Interpreter
{
	class MathTree
	{
		public MathTreeNode root { get; set; }

		public MathTree()
		{
			root = null;
		}

		public MathTree(MathTreeNode newRoot)
		{
			root = newRoot;
		}

		public String ToPreOrderString() {
			return (root == null) ? "" : root.ToPreOrderString();
		}

		/// <summary>
		/// Interprets a string into a MathTree.
		/// Main loop evaluates the next number and operator. If the operator is of
		/// lower precedence than the current root, the operator becomes the new root
		/// and the old root becomes the operator's left child. If the operator is
		/// of higher precedence, it is added as the right child of the last added
		/// operator.
		/// </summary>
		/// <param name="str">String to convert into a MathTree.</param>
		/// <returns></returns>
		public static MathTree FromString(String str) {
			if (str.Length <= 0)
			{
				return new MathTree();
			}

			int index = 0;
			ArrayList symbols = new ArrayList();
			while (true)
			{
				MathTokenizer.Result<double> numResult = MathTokenizer.NextNumber(str, index);
				if (numResult.Skip < 0)
				{
					Console.WriteLine("Expected number after \"" + str.Substring(0, index - 1) + "\"");
					break;
				}
				else
				{
					symbols.Add(new MathSymbol(numResult.Token));
					index = numResult.Skip + 1;
					MathTokenizer.Result<MathSymbol.Operator> opResult = MathTokenizer.NextOperator(str, index);
					if (opResult.Skip < 0)
					{
						break;
					} else {
						symbols.Add(new MathSymbol(opResult.Token));
						index = opResult.Skip + 1;
					}
				}
			}

			MathTree tree = TreeFromArrayList(symbols);
			return tree;
		}

		/// <summary>
		/// Generates a MathTree given an array of alternating numbers and operators.
		/// </summary>
		/// <param name="symbols">Guarunteed to contain any number of MathSymbols,
		/// alternating numbers and operators.</param>
		/// <returns>A MathTree representing the ArrayList of symbols.</returns>
		private static MathTree TreeFromArrayList(ArrayList symbols)
		{
			MathTree tree = new MathTree();

			foreach (MathSymbol thisSymbol in symbols)
			{
				MathTreeNode thisNode = tree.root;
				if (thisNode == null)
				{
					tree.root = new MathTreeNode(thisSymbol);
				}
				else
				{
					if (thisSymbol.IsOperator)
					{
						// if thisNode is a number or is an operator of higher precedence,
						// make thisSymbol the new root and thisNode the left child:
						if (!thisNode.Value.IsOperator ||
							 thisNode.Value.IsOperator && thisSymbol.Value <= thisNode.Value.Value)
						{
							tree.root = new MathTreeNode(thisSymbol);
							tree.root.left = thisNode;
						} // else, if thisNode is an operator of lower precedence, scroll to the
						// right until we've found a number or an operator of higher precedence;
						// replace with thisSymbol and make replaced symbol thisSymbol's left child:
						else if (thisNode.Value.IsOperator && thisSymbol.Value > thisNode.Value.Value)
						{
							while (thisNode.right.Value.IsOperator &&
										thisSymbol.Value > thisNode.right.Value.Value)
							{ // staying one step above
								thisNode = thisNode.right; // note that this cannot be null if symbols alternate between operators and numbers
							}

							MathTreeNode temp = thisNode.right;
							thisNode.right = new MathTreeNode(thisSymbol);
							thisNode.right.left = temp;
						}
					}
					else // thisSymbol is a number
					{
						// add it as far right as we can
						while (thisNode.right != null)
						{
							thisNode = thisNode.right;
						}

						thisNode.right = new MathTreeNode(thisSymbol);
					}
				}
			}

			return tree;
		}

		/// <summary>
		/// Solves the MathTree. Returns the evaulated number.
		/// </summary>
		public double Evaluate()
		{
			if (root == null)
				return 0;
			else
				return ((MathTreeNode)root).Evaluate();
		}

		public override string ToString()
		{
			return ToPreOrderString();
		}
	}
}
