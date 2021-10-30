﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Class1
{
    public class Program
    {
        static ArrayList expression = new ArrayList();
        static ArrayList operators = new ArrayList();

        static void Main(string[] args)
        {
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
            string output;
            try
            {
                expression.Clear();
                Extract(input);
                while (expression.Count > 1)
                {
                    expression = Evaluate(expression);
                    for (int j = 0; j < expression.Count; j++)
                    {
                        Console.Write(expression[j]);
                    }
                    Console.WriteLine();
                }
                output = Convert.ToString(expression);
                return output;
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e;
            }
        }

        public static void Extract(string expr)
        {
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
                    continue;
                }
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
        }

        public static ArrayList Evaluate(ArrayList expression)
        {
            double val = 0;
            double n1 = 0, n2 = 0;
            while (expression.Contains("("))
            {
                ArrayList section = new ArrayList();
                int sI = expression.IndexOf("(") + 1;
                while (sI < expression.LastIndexOf(")"))
                {
                    section.Add(expression[sI]);
                    sI++;
                }
                int secCount = section.Count;
                while (section.Count > 1)
                {
                    section = Evaluate(section);
                }
                if (expression.IndexOf("(") > 0)
                {
                    if (expression[expression.IndexOf("(") - 1].GetType() == typeof(double))
                    {
                        expression.Insert(expression.IndexOf("("), "*");
                    }
                }
                expression.Insert(expression.IndexOf("("), section[0]);
                expression.RemoveRange(expression.IndexOf("("), secCount + 2);
            }
            for (int op = 0; op < operators.Count; op++)
            {
                int i = expression.IndexOf(operators[op]);
                if (i != -1)
                {
                    if (i - 1 >= 0)
                    {
                        if (expression[i - 1].GetType() == typeof(double))
                            n1 = double.Parse(expression[i - 1].ToString());
                        if (expression[i + 1].GetType() == typeof(double))
                            n2 = double.Parse(expression[i + 1].ToString());

                        if (operators[op] == "+")
                        {
                            val = n1 + n2;
                        }
                        else if (operators[op] == "-")
                        {
                            if (expression[i + 1].ToString() == "-")
                            {
                                val = n1 + double.Parse(expression[i + 2].ToString());
                                expression.RemoveAt(i);
                            }
                            else
                            {
                                val = n1 - n2;
                            }
                        }
                        else if (operators[op] == "*")
                        {
                            val = n1 * n2;
                        }
                        else if (operators[op] == "/")
                        {
                            val = n1 / n2;
                        }

                        expression[i - 1] = val;
                        expression.RemoveAt(i);
                        expression.RemoveAt(i);
                    }
                    else
                    {
                        if (operators[op] == "-")
                        {
                            val = double.Parse(expression[i + 1].ToString());
                            val *= -1;
                            expression[i + 1] = val;
                            expression.RemoveAt(i);
                        }
                        else
                        {
                            throw new Exception("Incorrect syntax, please try again");
                        }
                    }
                }
            }
            return expression;
        }
    }
}






