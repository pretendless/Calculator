using System;

namespace Task4
{
    public abstract class Operation
    {
        public static Operation Resolve(string s)
        {
            if(s.Contains('(') && s.Contains(','))
                return new DoubleArgFunction(s);

            if(s.Contains('('))
                return new Function(s);

            return new Expression(s);
        }

        public abstract double Solve();
        public abstract string Display();
    }

    public class Function : Operation
    {
         public double x;
         public string name;
         public Function(string Name, double X)
         {
             x = X;
             name = Name;
         }

         public Function(string s)
         {
             Function fn = Parse(s);
             x = fn.x;
             name = fn.name;
         }

         public Function Parse(string s)
         {
            s = s.Replace(" ", "");
            double sq;
            string nm;
            int i = FindOper(s);
            try
            {
                nm = s.Substring(0, i);
                sq = Convert.ToDouble(s.Substring(i+1, s.Length - i - 2));
            }
            catch
            {
                throw new FormatException("Not suitable types");
            }
            return new Function(nm, sq);
        }

         public int FindOper(string s)
        {
            int i = 0;
            i = s.IndexOf('('); 
            
            if(i == -1)
                throw new FormatException("Not recognised operation");
            
            return i;
        }
         
         public override double Solve()
         {
            if(name.ToUpper() == "SQR")
                return SQR();

            throw new Exception("No solution was found");
         }

         public override string Display()
         {
             string s;
             s = name + " " + x;
             return s;
         }

         double SQR()
         {
             return x*x;
         }

    }

    public class DoubleArgFunction : Operation
    {
        public double y;
        public double x;
        public string name;
        
        public DoubleArgFunction(string Name, double X, double Y)
        {
            x = X;
            y = Y;
            name = Name;
        }

        public DoubleArgFunction(string s)
        {
            DoubleArgFunction DAFn = Parse(s);
            y = DAFn.y;
            x = DAFn.x;
            name = DAFn.name;
        }

        public DoubleArgFunction Parse(string s)
        {
            s = s.Replace(" ", "");
            double fr, sc;
            string nm;
            int i, j;
            if(s.Contains(',') && s.Contains('('))
            {
                i = s.IndexOf('(');
                j = s.IndexOf(',');
            }
            else
                throw new FormatException("Not recognized operation");

            try
            {
                nm = s.Substring(0, i);
                fr = Convert.ToDouble(s.Substring(i+1, j - (i+1)));
                sc = Convert.ToDouble(s.Substring(j+1, s.Length - j - 2));
            }
            catch
            {
                throw new FormatException("Not suitable types");
            }
            return new DoubleArgFunction(nm, fr, sc);
        }

        public override double Solve()
        {
            if(name.ToUpper() == "PWR")
                return PWR();

            if(name.ToUpper() == "ROOT")
                return ROOT();

            throw new Exception("No solution was found");
        }

        public override string Display()
        {
            string s;
            s = name + " " + x + " " + y;
            return s;
        }

        double PWR()
        {
            double ans = 1, x_ = x, y_ = y;
            if(y_ < 0)
            {
                x_ = 1 / x_;
                y_ = Math.Abs(y_);
            }
                
            for(int i = 0; i < y_; i++)
                ans = ans * x_;
            
            return ans;
        }

        double ROOT()
        {
            if(y <= 0)
                throw new Exception("Cant solve");
            
            return Math.Pow(x, 1/y);
        }

    }

    public class Expression : Operation
    {
        public double x, y;
        public char op;

        public Expression(double x_, char o_, double y_)
        {
            x = x_;
            y = y_;
            op = o_;
        }

        public Expression(string s)
        {
            Expression exp = Parse(s);
            x = exp.x;
            y = exp.y;
            op = exp.op;
        }

        public Expression Parse(string s)
        {
            s = s.Replace(" ", "");
            double fr, sc;
            char opp;
            int i = FindOper(s);
            try
            {
                fr = Convert.ToDouble(s.Substring(0, i));
                opp = s[i];
                sc = Convert.ToDouble(s.Substring(i+1));
            }
            catch
            {
                throw new FormatException("Not suitable types");
            }
            return new Expression(fr, opp, sc);
        }

        public int FindOper(string s)
        {
            int i = 0;
            while(i < s.Length && ((s[i] >= '0' && s[i] <= '9') || (s[i] == '.')))
                i++; 
            
            if(i == s.Length)
                throw new FormatException("Operation not found");
            
            return i;
        }

        public override double Solve()
        {
            if(op == '-')
                return Subtruct();
            
            if(op == '+')
                return Sum();

            if(op == '*')
                return Multiply();

            if(op == '/')
                return Divide();
             
            throw new Exception("No solution was found");
        }

        public override string Display()
        {
            string s;
            s = x + " " + op + " " + y;
            return s;
        }
        /// <summary>
        /// Вычитание
        /// </summary>

        double Subtruct()
        {
            return x - y;
        }

        /// <summary>
        /// Сложение
        /// </summary>
        /// <returns></returns>

        double Sum()
        {
            return x + y;
        }
        /// <summary>
        /// Умножение
        /// </summary>
        /// <returns></returns>

        double Multiply()
        {
            return x * y;
        }

        /// <summary>
        /// Деление
        /// </summary>
        /// 
        double Divide()
        {
            return x / y;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("input the expression: ");
            string s;
            s = Console.ReadLine();
            Operation operation = Operation.Resolve(s);

            Console.WriteLine(operation.Solve());
        }
    }
}