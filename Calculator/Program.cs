using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class1
{
    class Program
    {
        static ArrayList expression= new ArrayList();
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the expression to evaluate it");
            string input = Console.ReadLine();
            Extract(input);
            for(int i=0;i<expression.Count;i++)
            {
                Console.WriteLine(expression[i]);
            }
        }

        public static void Extract(string expr)
        {
            int num=0;
            for(int i=0;i<expr.Length;i++)
            {
                int digit=0;
                bool canConvert= int.TryParse(expr.Substring(i,1),out digit);
                if(canConvert)
                {
                    num=num*10+digit;
                    continue;
                }
                else
                {
                    if(num!=0)
                    {
                         expression.Add(num);
                    }
                    
                    num=0;
                    expression.Add(expr.Substring(i,1));
                }
            }
            
        }
    }
}
