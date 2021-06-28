/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            Expression<Func<int, int>> expressionToIncDec = a => a + 1 + a + (a + 1) * (a - 1);
            Console.WriteLine("Source expression:");
            Console.WriteLine(expressionToIncDec.ToString());
            Console.WriteLine();

            var incDecVisitor = new IncDecExpressionVisitor();

            var result = incDecVisitor.Visit(expressionToIncDec);
            Console.WriteLine("Result expression:");
            Console.WriteLine(result);
            Console.WriteLine();

            var paramReplacer = new ParamReplacerVisitor();

            Expression targetExpression;

            Console.WriteLine("Expression Visitor for transformation of lambda Expressions");

            Expression<Func<int, int>> sourceExpression = a => a + 1 + a;
            Console.WriteLine("Source expression:");
            Console.WriteLine(sourceExpression.ToString());
            Console.WriteLine();

            targetExpression = sourceExpression;

            Dictionary<string, int> listOfValues = new Dictionary<string, int>();

            foreach (var parameter in sourceExpression.Parameters)
            {
                Console.WriteLine("Enter value for parameter " + parameter.Name + ": ");
                var value = Convert.ToInt32(Console.ReadLine());
                listOfValues.Add(parameter.Name, value);
            }

            targetExpression = Expression.Block(targetExpression, Expression.Constant(listOfValues));
            result = paramReplacer.Visit(targetExpression);

            Console.WriteLine("Result expression:");
            Console.WriteLine(result.ToString());

            Console.ReadLine();
        }
    }
}
