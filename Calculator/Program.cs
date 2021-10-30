using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class1
{
    public class Program
    {
        public static ArrayList expression = new ArrayList();
        public static ArrayList operators = new ArrayList();
        public static bool MainCheck = false;

        static void Main(string[] args)
        {
            MainCheck = true;
            operators.Add("/");
            operators.Add("*");
            operators.Add("-");
            operators.Add("+");
            string input;
            do
            {
                Console.WriteLine("Enter an expression to evaluate it\nEnter exit to leave the calculator");
                input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }
                ProcessCommand(input);
            } while (input != "exit");
        }

        public static string ProcessCommand(string input)
        {
            // Add each operator to ArrayList operators when ProcessCommand() is being directly called from Class1.Tests
            if (MainCheck == false)
            {
                operators.Add("/");
                operators.Add("*");
                operators.Add("-");
                operators.Add("+");
            }
            string output = "";
            var buildString = new StringBuilder();
            try
            {
                expression.Clear();
                expression = Extract(input);
                while (expression.Count > 1)
                {
                    expression = Evaluate(expression);
                    for (int j = 0; j < expression.Count; j++)
                    {
                        Console.Write(expression[j]);
                    }
                    Console.WriteLine();
                }
                // Use StringBuilder class to convert ArrayList to string
                for (int j = 0; j < expression.Count; j++)
                {
                    buildString.Append(expression[j]);
                }
                output = buildString.ToString();
                return output;
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e;
            }
        }

        public static ArrayList Extract(string expr)
        {
            if (expr.Contains("("))
            {
                expr = Extractforbracketts(expr);
            }
            string num = "";
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr.Substring(i, 1) == " ")
                {
                    continue;
                }
                int digit = 0;
                bool canConvert = int.TryParse(expr.Substring(i, 1), out digit);
                if (canConvert || expr.Substring(i, 1) == ".")
                {
                    num = num + expr.Substring(i, 1);
                }
                //extract negative numbers ... begin
                else if (expr.Substring(i, 1) == "-" && num == "")
                {
                    while (expr.Substring(i + 1, 1) == " ")
                    {
                        i++;
                    }
                    bool canConvertNext = int.TryParse(expr.Substring(i + 1, 1), out digit);
                    if (canConvertNext)
                    {
                        if (i == 0)
                        {
                            num = "-";
                        }
                        else
                        {
                            int j = i;
                            while (expr.Substring(j - 1, 1) == " ")
                            {
                                j--; ;
                            }
                            bool canConvertPrevious = int.TryParse(expr.Substring(j - 1, 1), out digit);
                            if (!canConvertPrevious)
                            {
                                num = "-";
                            }
                        }
                        continue;
                    }
                }
                //end
                else
                {
                    if (num != "")
                    {
                        expression.Add(double.Parse(num));
                    }
                    num = "";
                    expression.Add(expr.Substring(i, 1));
                }
            }
            if (num != "")
            {
                expression.Add(double.Parse(num));
            }
            return expression;
        }

        //to handle brackets
        public static string Extractforbracketts(string expr)
        {
            while (expr.Contains(")"))
            {
                for (int i = expr.IndexOf(")"); i >= 0; i--)
                {
                    if (expr.Substring(i, 1) == "(")
                    {
                        string section = expr.Substring(i, expr.IndexOf(")") + 1 - i);
                        ArrayList arr = new ArrayList();
                        arr = Extract(section.Substring(1, section.Length - 2));
                        arr = Evaluate(arr);
                        //add * to places where num and ( are together without operator
                        if (expr.Substring(i, 1) == "(" && i > 0)
                        {
                            int digit;
                            bool canConvertPrevious = int.TryParse(expr.Substring(i - 1, 1), out digit);
                            if (canConvertPrevious)
                            {
                                expr = expr.Insert(i, "*");
                            }
                        }
                        if (expr.IndexOf(")") < expr.Length - 1)
                        {
                            int digit;
                            bool canConvertNext = int.TryParse(expr.Substring(expr.IndexOf(")") + 1, 1), out digit);
                            if (canConvertNext)
                            {
                                expr = expr.Insert(expr.IndexOf(")") + 1, "*");
                            }
                        }
                        //end
                        expr = expr.Replace(section, arr[0].ToString());
                        Console.WriteLine(expr);
                        arr.Clear();
                        break;
                    }
                }
            }
            return expr;
        }

        public static ArrayList Evaluate(ArrayList expression)
        {
            double val = 0;
            double n1 = 0, n2 = 0;

            for (int op = 0; op < operators.Count; op++)
            {
                while (expression.Contains(operators[op]))
                {
                    int i = expression.IndexOf(operators[op]);
                    if (i >= 1)
                    {
                        if (expression[i - 1].GetType() == typeof(double))
                            n1 = double.Parse(expression[i - 1].ToString());
                        if (expression[i + 1].GetType() == typeof(double))
                            n2 = double.Parse(expression[i + 1].ToString());

                        if (operators[op].ToString() == "+")
                        {
                            val = n1 + n2;
                        }
                        else if (operators[op].ToString() == "-")
                        {
                            val = n1 - n2;
                        }
                        else if (operators[op].ToString() == "*")
                        {
                            val = n1 * n2;
                        }
                        else if (operators[op].ToString() == "/")
                        {
                            val = n1 / n2;
                        }

                        expression[i - 1] = val;
                        expression.RemoveRange(i, 2);

                    }
                }
            }
            return expression;
        }
    }
}






