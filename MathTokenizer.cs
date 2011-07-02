using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Math_Interpreter
{
	class MathTokenizer
	{
		/// <summary>
		/// Holds the result of a NextToken call.
		/// Token is the next token in the string.
		/// Skipped is an index into the original string the
		/// token was found in. It points to the character
		/// immediately after the token (whitespace or EOL).
		/// </summary>
		public class Result<T>
		{
			public T Token { get; private set; }
			public int Skip { get; private set; }

			public Result(T newToken, int newSkip)
			{
				Token = newToken;
				Skip = newSkip;
			}
		}

		/// <summary>
		/// Returns next character sequence that is surrounded by
		/// whitespace (or starts at index and/or ends with an EOL).
		/// If no such sequence is found, the Result's Skip field equals -1.
		/// </summary>
		/// <param name="str">Haystack string</param>
		/// <param name="index">Index to begin looking at</param>
		/// <returns>The next token or a Result.Skip of -1 if no token found.</returns>
		public static Result<string> NextToken(string str, int index)
		{
			string token = "";
			for (; index < str.Length && str[index] == ' '; ++index); // skip whitespace

			// add characters until whitespace or EOL
			for (; ; ++index)
			{
				if (index >= str.Length || str[index] == ' ')
				{
					break;
				}

				token += str[index];
			}

			if (String.IsNullOrEmpty(token))
			{
				return new Result<string>("", -1);
			}

			Result<string> result = new Result<string>(token, index);
			return result;
		}

		/// <summary>
		/// Returns the next token in the string interpreted as a double.
		/// If there are no more tokens after index, returns a Result with a Skip of -1.
		/// If the next token is not a number, throws an InvalidOperationException.
		/// </summary>
		/// <param name="str">Haystack string</param>
		/// <param name="index">Index to begin looking at</param>
		/// <returns>The next token or a Result with Skip of -1 if no token found.</returns>
		public static Result<double> NextNumber(string str, int index)
		{
			Result<string> stringResult = NextToken(str, index);
			if (stringResult.Skip == -1)
			{
				return new Result<double>(0, -1);
			}

			string tok = stringResult.Token;

			double num;
			if (double.TryParse(tok, out num))
			{
				return new Result<double>(num, stringResult.Skip);
			}
			else
			{
				throw new InvalidOperationException(tok + " is not a number.");
			}
		}


		/// <summary>
		/// Returns the next token in the string interpreted as a double.
		/// If there are no more tokens after index, returns a Result with a Skip of -1.
		/// If the next token is not a number, throws an InvalidOperationException.
		/// </summary>
		/// <param name="str">Haystack string</param>
		/// <param name="index">Index to begin looking at</param>
		public static Result<MathSymbol.Operator> NextOperator(string str, int index)
		{
			Result<string> stringResult = NextToken(str, index);
			if (stringResult == null || stringResult.Token.Length != 1)
			{
				return new Result<MathSymbol.Operator>(MathSymbol.Operator.Add, -1);
			}

			MathSymbol.Operator op = MathSymbol.TranslateOperator(stringResult.Token[0]);
			return new Result<MathSymbol.Operator>(op, stringResult.Skip);
		}
	}
}