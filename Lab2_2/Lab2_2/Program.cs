using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2
{

    class Program
    {
        static void Main(string[] args)
        {
            Division division = new Division();
            division.GetInputData();
            division.Divide();
            Console.WriteLine($"Fraction: {division.Fraction}\nRemainder: {division.Remainder}");

            Console.ReadKey();
        }
    }
    class Division
    {
        public int Divided { get; set; }
        public int Divider { get; set; }
        public List<int> BinRemainder { get; set; }
        public List<int> BinDivisor { get; set; }
        public List<int> BinQuotient { get; set; }
        public int Fraction { get; set; }
        public int Remainder { get; set; }

        public void GetInputData()
        {
            BinQuotient = new List<int>();
            int temp;
            do
            {
                Console.Write("Enter divided: ");
            }
            while (!Int32.TryParse(Console.ReadLine(), out temp));
            this.Divided = temp;
            do
            {
                Console.Write("Enter divider: ");
            }
            while (!Int32.TryParse(Console.ReadLine(), out temp));
            this.Divider = temp;
            if (Divided < 0 && Divider < 0)
            {
                Divided = Math.Abs(Divided);
                Divider = Math.Abs(Divider);
            }

            this.BinRemainder = GetNumbInBinary(this.Divided);
            SupplementBits(this.BinRemainder, this.Divided, 64);
            this.BinDivisor = GetNumbInBinary(this.Divider);
            SupplementBits(this.BinDivisor, this.Divider, 32);
        }
        public List<int> GetNumbInBinary(int number)
        {
            int absNumb = Math.Abs(number);
            List<int> result = new List<int>();
            while (absNumb > 0)
            {
                result.Add(absNumb % 2);
                absNumb /= 2;
            }
            result.Reverse();
            if (number < 0)
            {
                Inverse(ref result);
            }
            return result;
        }
        public void Inverse(ref List<int> binaryNumb)
        {
            for (int i = 0; i < binaryNumb.Count; i++)
            {
                if (binaryNumb[i] == 0)
                {
                    binaryNumb[i] = 1;
                }
                else
                {
                    binaryNumb[i] = 0;
                }
            }
            binaryNumb = AddOne(binaryNumb);
        }
        public List<int> AddOne(List<int> binaryNumb)
        {
            int mind = 1;
            for (int i = binaryNumb.Count - 1; i >= 0; i--)
            {
                if (i == 0 && binaryNumb[i] == 1 && mind == 1)
                {
                    List<int> newBinaryNumb = new List<int>();
                    newBinaryNumb.AddRange(new List<int>() { 1, 1 });
                    newBinaryNumb.AddRange(binaryNumb);
                    return newBinaryNumb;
                }
                else if (binaryNumb[i] == 0 && mind == 1)
                {
                    binaryNumb[i] = 1;
                    mind = 0;
                }
                else if (binaryNumb[i] == 1 && mind == 1)
                {
                    binaryNumb[i] = 0;
                }
            }
            return binaryNumb;
        }
        public void SupplementBits(List<int> binaryNumb, int number, int forSupplement)
        {
            int numberForSupplement = 0;
            if (number < 0)
            {
                numberForSupplement = 1;
            }
            List<int> helper = new List<int>();
            int countToSupplement = forSupplement - binaryNumb.Count;
            for (int i = 0; i < countToSupplement; i++)
            {
                helper.Add(numberForSupplement);
            }
            helper.AddRange(binaryNumb);
            binaryNumb.Clear();
            binaryNumb.AddRange(helper);
        }
        public void Divide()
        {
            for (int i = 0; i < 32; i++)
            {
                ShiftLeft(BinRemainder);
                Output(BinRemainder, "\nDivided");
                Output(BinDivisor, "\nDivisor");
                Output(BinQuotient, "\nFraction");
                Subtract(BinRemainder, BinDivisor);
                Console.WriteLine();
                if (BinRemainder[0] == 0)
                {
                    BinQuotient.Add(1);
                }
                else
                {
                    BinQuotient.Add(0);
                    Add(BinRemainder, BinDivisor);
                }
            }
            Fraction = GetNumbFromBinary(BinQuotient);
            Remainder = GetNumbFromBinary(BinRemainder.Take(32).ToList());
        }
        public void Add(List<int> remainder, List<int> divisor)
        {
            int mind = 0;
            for (int i = 31; i >= 0; i--)
            {
                if ((remainder[i] == 0 && divisor[i] == 1 && mind == 0) || (remainder[i] == 1 && divisor[i] == 0 && mind == 0) || (remainder[i] == 0 && divisor[i] == 0 && mind == 1))
                {
                    remainder[i] = 1;
                    mind = 0;
                }
                else if ((remainder[i] == 0 && divisor[i] == 1 && mind == 1) || (remainder[i] == 1 && divisor[i] == 0 && mind == 1) || (remainder[i] == 1 && divisor[i] == 1 && mind == 0))
                {
                    remainder[i] = 0;
                    mind = 1;
                }
                else if (remainder[i] == 0 && divisor[i] == 0 && mind == 0)
                {
                    remainder[i] = 0;
                    mind = 0;
                }
                else
                {
                    remainder[i] = 1;
                    mind = 1;
                }
            }
        }
        public void Output(List<int> binNumb, string what)
        {
            Console.Write(what + ": ");
            for (int i = 0; i < binNumb.Count; i++)
            {
                Console.Write(binNumb[i]);
            }
            Console.Write("\t");
        }
        public int GetNumbFromBinary(List<int> binNumb)
        {
            int result = 0;
            int power2 = 0;
            for (int i = binNumb.Count - 1; i >= 0; i--)
            {
                if (binNumb[i] == 1)
                {
                    result += (int)Math.Pow(2, power2);
                }
                power2++;
            }
            return result;
        }
        public void ShiftLeft(List<int> remainder)
        {
            for (int i = 0; i < 63; i++)
            {
                remainder[i] = remainder[i + 1];
            }
            remainder[63] = 0;
        }
        public void Subtract(List<int> reduction, List<int> subtractor)
        {
            List<int> helperSubtract = new List<int>();
            helperSubtract.AddRange(subtractor);
            Inverse(ref helperSubtract);
            Add(reduction, helperSubtract);
        }
    }
}
