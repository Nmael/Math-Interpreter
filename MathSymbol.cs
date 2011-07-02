using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Math_Interpreter
{
	/// <summary>
	/// Represents any mathematical symbol - either a number or an operator.
	/// If representing a number, MathSymbol.Value is set to that number.
	/// If representing an operator, MathSymbol.Value is set to the value of the matching
	/// MathSymbol.Operator and IsOperator is set to true.
	/// </summary>
	class MathSymbol
	{
		// The following Operators must be in order of increasing precedence:
		public enum Operator { Add, Subtract, Multiply, Divide, Exponent };
		public double Value { get; private set; }
		public bool IsOperator { get; private set; }

		/// <summary>
		/// Translates a character into an Operator.
		/// </summary>
		/// <param name="cOp">The character version of the operator.</param>
		static public Operator TranslateOperator(char cOp) {
			switch(cOp) {
				case '+':
					return Operator.Add;
				case '-':
					return Operator.Subtract;
				case '*':
					return Operator.Multiply;
				case '/':
					return Operator.Divide;
				case '^':
					return Operator.Exponent;
				default:
					throw new InvalidOperationException("Unknown operator \"" + cOp + "\".");
			}
		}

		/// <summary>
		/// Applies the mathematical operation specified by 'op' to the two given
		/// MathSymbols. Returns a new MathSymbol representing the result.
		/// </summary>
		/// <param name="sym1">A value-holding MathSymbol</param>
		/// <param name="op">The operator to apply to sym1 and sym2</param>
		/// <param name="sym2">A value-holding MathSymbol</param>
		/// <returns></returns>
		static public MathSymbol ApplyOperator(MathSymbol sym1, Operator op, MathSymbol sym2) {
			if(sym1.IsOperator || sym2.IsOperator)
			{
				throw new InvalidOperationException("Mathematic operation passed as numeric value.");
			}

			MathSymbol newSym = new MathSymbol((double)0);
			switch (op)
			{
				case Operator.Add:
					newSym.Value = sym1.Value + sym2.Value;
					break;
				case Operator.Subtract:
					newSym.Value = sym1.Value - sym2.Value;
					break;
				case Operator.Multiply:
					newSym.Value = sym1.Value * sym2.Value;
					break;
				case Operator.Divide:
					newSym.Value = sym1.Value / sym2.Value;
					break;
				case Operator.Exponent:
					newSym.Value = Math.Pow(sym1.Value, sym2.Value);
					break;
				default:
					throw new InvalidOperationException("Operator \"" + op + "\" unknown.");
			}

			return newSym;
		}

		public MathSymbol(double newValue)
		{
			Value = newValue;
			IsOperator = false;
		}

		public MathSymbol(Operator newOperator)
		{
			Value = (double)newOperator;
			IsOperator = true;
		}

		public override string ToString()
		{
			if (!IsOperator)
			{
				return Value.ToString();
			}
			else
			{
				MathSymbol.Operator CastedValue = (MathSymbol.Operator)Value;
				switch (CastedValue)
				{
					case Operator.Add:
						return "+";
					case Operator.Subtract:
						return "-";
					case Operator.Multiply:
						return "*";
					case Operator.Divide:
						return "/";
					case Operator.Exponent:
						return "^";
					default:
						throw new InvalidOperationException("Operator \"" + Value + "\" unknown.");
				}
			}
		}
	}
}
