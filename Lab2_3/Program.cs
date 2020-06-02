using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            IEEE754_Multiply floating_multiplier = new IEEE754_Multiply();
            float firstNumber = 0, secondNumber = 0;
            string first = "", second = ""; ;
            Console.WriteLine("Multiplication of numbers");
            do
            {
                try
                {
                    Console.Write("Enter first value: ");
                    first = Console.ReadLine();
                }
                catch (FormatException exc)
                {
                    Console.WriteLine("Wrong format: " + exc);
                }
            } while (!float.TryParse(first, out firstNumber));
            do
            {
                try
                {
                    Console.Write("Enter second value: ");
                    second = Console.ReadLine();
                }
                catch (FormatException exc)
                {
                    Console.WriteLine("Wrong format: " + exc);
                }
            } while (!float.TryParse(second, out secondNumber));

            float result = floating_multiplier.FloatingNumbersMultiplication(firstNumber, secondNumber);

            Console.ReadKey();
        }
    }
    class IEEE754_Multiply
    {
        public IEEE754_Multiply() { }

        public string ConvertToPrint(string number, int bits_amount)
        {
            int zeros_amount = bits_amount - number.Length;
            string zeros = "";
            for (int i = 0; i < zeros_amount; i++)
            {
                zeros += "0";
            }
            return zeros + number;
        }

        public int FloatingBitsMask(float number)
        {
            byte[] bits = BitConverter.GetBytes(number);
            int floatingResult = 0;
            floatingResult |= bits[0]; 
            floatingResult |= bits[1] << 8; 
            floatingResult |= bits[2] << 16;
            floatingResult |= bits[3] << 24; 
            return floatingResult;
        }

        public float FloatingNumbersMultiplication(float first, float second)
        {
            long firstBits = FloatingBitsMask(first);
            long secondBits = FloatingBitsMask(second);

            int sign_1 = (int)((firstBits >> 31) & 1);
            int sign_2 = (int)((secondBits >> 31) & 1);
            int resultSign = 1 & (sign_1 + sign_2);
            Console.WriteLine("\nDefinition of the sign:");
            Console.WriteLine(sign_1 + " XOR " + sign_2 + " = " + resultSign);

            Console.WriteLine("\nMultiplication :");
            long mantissa_1 = firstBits & ((int)Math.Pow(2, 23) - 1);
            Console.WriteLine("Mantissa of the first number:\n " + ConvertToPrint(Convert.ToString(mantissa_1, 2), 24)
                + "\nХ (multiply by)");
            long mantissa_2 = secondBits & ((int)Math.Pow(2, 23) - 1);
            Console.WriteLine("Mantissa of the second number:\n " + ConvertToPrint(Convert.ToString(mantissa_2, 2), 24)
                + "\n= (is equal to)");
            long multiplicatedMantissa = ((1 << 23) | mantissa_1) * ((1 << 23) | mantissa_2);
            Console.WriteLine("Mantissa resulting : " + ConvertToPrint(Convert.ToString(multiplicatedMantissa, 2), 48));

            multiplicatedMantissa >>= 23;
            int normalizer = ((1 << 24) & multiplicatedMantissa) > 0 ? 1 : 0;

            Console.WriteLine();
            if (normalizer > 0)
            {
                Console.WriteLine("Normalization");
                multiplicatedMantissa >>= 1;
                multiplicatedMantissa &= ~(1 << 23);
            }
            else
            {
                multiplicatedMantissa &= ~(3 << 23);
            }
            Console.WriteLine("The result: ");
            Console.WriteLine(ConvertToPrint(Convert.ToString(multiplicatedMantissa, 2), 23));
            Console.WriteLine();

            int exponent_1 = (int)((firstBits >> 23) & 255),
                exponent_2 = (int)((secondBits >> 23) & 255);
            int resultExponent = exponent_1 + exponent_2 - 127 + normalizer;

            Console.WriteLine();
            Console.WriteLine("Calculation of the exponent of the result: ");
            Console.WriteLine("The exponent of the first number: " + ConvertToPrint(Convert.ToString(exponent_1, 2), 8)
                + "\nthat in decimal form: " + exponent_1);
            Console.WriteLine("The exponent of the second number : " + ConvertToPrint(Convert.ToString(exponent_2, 2), 8)
                + "\nthat in decimal form: " + exponent_2);
            Console.WriteLine("The resulting exponent: ");
            Console.Write(exponent_1 + " " + exponent_2 + " - 127 + " + normalizer + " = " + resultExponent
                + "\nthat in binary representation: " + ConvertToPrint(Convert.ToString(resultExponent, 2), 8));

            int resultFloatingMask = (int)multiplicatedMantissa;
            resultFloatingMask |= resultExponent << 23;
            resultFloatingMask |= resultSign << 31;

            byte[] floatingBits = new byte[4];
            floatingBits[0] = (byte)(resultFloatingMask & 255);
            floatingBits[1] = (byte)((resultFloatingMask >> 8) & 255);
            floatingBits[2] = (byte)((resultFloatingMask >> 16) & 255);
            floatingBits[3] = (byte)((resultFloatingMask >> 24) & 255);

            float floatingResult = BitConverter.ToSingle(floatingBits, 0);
            Console.WriteLine("\n\nAnswer:\nBinary:\n"
                + ConvertToPrint(Convert.ToString(resultFloatingMask, 2), 32)
                + "\nDecimal: " + floatingResult);
            return floatingResult;
        }
    }
}