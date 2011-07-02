using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Math_Interpreter
{
	class MathTreeNode
	{
		public MathSymbol Value { get; set; }
		public MathTreeNode left { get; set; }
		public MathTreeNode right { get; set; }

		public MathTreeNode()
		{
			left = null;
			right = null;
		}

		public MathTreeNode(MathSymbol newValue)
		{
			Value = newValue;
		}

		public String ToPreOrderString()
		{
			return ToPreOrderString("");
		}

		private String ToPreOrderString(string acc)
		{
			if (left != null)
			{
				acc = left.ToPreOrderString(acc);
			}

			acc += Value;

			if (right != null)
			{
				acc = right.ToPreOrderString(acc);
			}

			return acc;
		}

		public double Evaluate()
		{
			return Evaluate(0).Value;
		}

		public MathSymbol Evaluate(double acc) {
			if (!Value.IsOperator)
			{
				throw new InvalidOperationException("Numeric value evaluated as mathematical operator.");
			}
			MathSymbol.Operator thisOperator = (MathSymbol.Operator)Value.Value;

			// if it's an operator, we'll immediately overwrite below:
			MathSymbol leftValue = left.Value;
			if (leftValue.IsOperator)
			{
				leftValue = ((MathTreeNode)left).Evaluate(acc);
			}

			MathSymbol rightValue = right.Value;
			if (rightValue.IsOperator)
			{
				rightValue = ((MathTreeNode)right).Evaluate(acc);
			}

			return MathSymbol.ApplyOperator(leftValue, thisOperator, rightValue);
		}
	}
}
