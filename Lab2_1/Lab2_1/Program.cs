using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1
{
    class Program
    {
        static void Main(string[] args)
        {
            Mult data = new Mult();
            data.GetInputData();
            long result = data.Multiply();
            Console.WriteLine($"\nResult: {result}");

            Console.ReadKey();
        }
    }

    class Mult
    {
        public int Multiplicand { get; set; }
        public int Multiplier { get; set; }
        public List<int> BinaryMultiplicand { get; set; }
        public List<int> BinaryMultiplier { get; set; }

        public void GetInputData()
        {
            int temp;
            do
            {
                Console.Write("Enter multiplicand: ");
            }
            while (!Int32.TryParse(Console.ReadLine(), out temp));
            this.Multiplicand = temp;
            do
            {
                Console.Write("Enter multiplier: ");
            }
            while (!Int32.TryParse(Console.ReadLine(), out temp));
            this.Multiplier = temp;

            this.BinaryMultiplicand = GetNumbInBinary(this.Multiplicand);
            Supplement32Bits(this.BinaryMultiplicand, this.Multiplicand);
            this.BinaryMultiplier = GetNumbInBinary(this.Multiplier);
            Supplement32Bits(this.BinaryMultiplier, this.Multiplier);
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
        public void Supplement32Bits(List<int> binaryNumb, int number)
        {
            int numberForSupplement = 0;
            if (number < 0)
            {
                numberForSupplement = 1;
            }
            List<int> helper = new List<int>();
            int countToSupplement = 32 - binaryNumb.Count;
            for (int i = 0; i < countToSupplement; i++)
            {
                helper.Add(numberForSupplement);
            }
            helper.AddRange(binaryNumb);
            binaryNumb.Clear();
            binaryNumb.AddRange(helper);
        }
        public long Multiply()
        {
            long result = 0;
            List<int> binaryResult = new List<int>();
            FillListZeros(binaryResult);
            for (int i = 0; i < this.BinaryMultiplier.Count; i++)
            {
                Console.Write("\nMultiplicand: ");
                Output(BinaryMultiplicand);
                Console.Write("\tMultiplier: ");
                Output(BinaryMultiplier);
                Console.Write("\tResult:   ");
                Output(binaryResult);
                if (BinaryMultiplier[31] == 1)
                {
                    AddBinary(ref binaryResult, this.BinaryMultiplicand);
                }
                this.BinaryMultiplier.RemoveAt(31);
                this.BinaryMultiplier.Insert(0, 0);
                this.BinaryMultiplicand.RemoveAt(0);
                this.BinaryMultiplicand.Add(0);
            }
            if (binaryResult[0] == 0)
            {
                result = GetNumberFromBinary(binaryResult);
            }
            else
            {
                Inverse(ref binaryResult);
                result = GetNumberFromBinary(binaryResult);
            }
            if ((Multiplicand > 0 && Multiplier > 0) || (Multiplicand < 0 && Multiplier < 0))
            {
                return result;
            }
            else
            {
                return -result;
            }
        }
        public void AddBinary(ref List<int> binaryResult, List<int> multiplicand)
        {
            int mind = 0;
            for (int i = binaryResult.Count - 1; i >= 0; i--)
            {
                if ((binaryResult[i] == 0 && multiplicand[i] == 1 && mind == 0) || (binaryResult[i] == 1 && multiplicand[i] == 0 && mind == 0) || (binaryResult[i] == 0 && multiplicand[i] == 0 && mind == 1))
                {
                    binaryResult[i] = 1;
                    mind = 0;
                }
                else if ((binaryResult[i] == 0 && multiplicand[i] == 1 && mind == 1) || (binaryResult[i] == 1 && multiplicand[i] == 0 && mind == 1) || (binaryResult[i] == 1 && multiplicand[i] == 1 && mind == 0))
                {
                    binaryResult[i] = 0;
                    mind = 1;
                }
                else if (binaryResult[i] == 0 && multiplicand[i] == 0 && mind == 0)
                {
                    binaryResult[i] = 0;
                    mind = 0;
                }
                else
                {
                    binaryResult[i] = 1;
                    mind = 1;
                }
            }
        }
        public long GetNumberFromBinary(List<int> binaryNumb)
        {
            long result = 0;
            int power2 = 0;
            for (int i = binaryNumb.Count - 1; i >= 0; i--)
            {
                if (binaryNumb[i] == 1)
                {
                    result += (long)Math.Pow(2, power2);
                }
                power2++;
            }
            if (binaryNumb[0] == 1)
            {
                result = -result;
            }
            return result;
        }
        public void FillListZeros(List<int> list)
        {
            for (int i = 0; i < 32; i++)
            {
                list.Add(0);
            }
        }
        public void Output(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write(list[i]);
            }
            Console.Write("\t");
        }
    }
}
