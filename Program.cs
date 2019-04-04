using System;
using System.Text;

namespace FloatingPointConverter
{
    public class Program
    {
        #region Helper Methods
        /// <summary>
        /// Object that represents the scientific notation of the entered number
        /// </summary>
        class NumberNotation
        {
            /// <summary>
            /// The leftside of the notation
            /// </summary>
            public double significand;

            /// <summary>
            /// The exponent of the right side of the notation
            /// </summary>
            public int exponent;

            /// <summary>
            /// Sign of the notation
            /// </summary>
            public int sign;

            /// <summary>
            /// Parameterized constructor
            /// </summary>
            /// <param name="sign">The sign</param>
            /// <param name="significand">The significand</param>
            /// <param name="exponent">The exponent</param>
            public NumberNotation(int sign, double significand, int exponent)
            {
                this.sign = sign;
                this.significand = significand;
                this.exponent = exponent;
            }
        }

        /// <summary>
        /// Reverses the contents of a string
        /// </summary>
        /// <param name="s">String to reverse</param>
        /// <returns>The reversed string</returns>
        static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        #endregion

        #region Converters
        /// <summary>
        /// Converter logic that the 32 and 64 bit floating point converters share
        /// </summary>
        /// <param name="input">Number to convert</param>
        /// <param name="exp">Exponent value to add</param>
        /// <param name="loops">Number of loops to do in the mantissa</param>
        /// <returns>The converted string</returns>
        static string BaseConverter(NumberNotation input, int exp, int count, int loops)
        {
            // Adds the sign bit
            StringBuilder output = new StringBuilder();
            output.Append(input.sign);

            // Calculates the exponetial bits
            int exponentField = input.exponent + exp;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(exponentField % 2);
                exponentField /= 2;
            }
            output.Append(Reverse(sb.ToString()));
            sb.Clear();

            // Calculates the mantissa bits
            double inputDecimal = input.significand - 1;
            for (int i = 0; i < loops; i++)
            {
                if (i == loops - 1 && input.significand * Math.Pow(2, input.exponent) < 1 && loops != 52)
                {
                    output.Append("1");
                    break;
                }
                inputDecimal *= 2;
                if (inputDecimal < 1)
                    output.Append("0");
                else
                {
                    output.Append("1");
                    inputDecimal -= 1;
                }
            }
            return output.ToString();
        }

        /// <summary>
        /// Converts a decimal to 32bit floating point and prints it out
        /// </summary>
        /// <param name="input">Decimal to convert</param>
        static void Convert32Bit(NumberNotation input)
        {
            Console.WriteLine("32 bit: " + BaseConverter(input, 127, 8, 23));
        }

        /// <summary>
        /// Converts a decimal to 64bit floating point and prints it out
        /// </summary>
        /// <param name="input">Decimal to convert</param>
        static void Convert64Bit(NumberNotation input)
        {
            Console.WriteLine("64 bit: " + BaseConverter(input, 1023, 11, 52));
        }

        /// <summary>
        /// Method that converts the demical input into scientific notation and calls both 
        /// converter methods.
        /// </summary>
        /// <param name="input">Decimal to convert</param>
        static void ConvertFloatingPoint(string input)
        {
            double number = Convert.ToDouble(input);

            // Calculates the sign bit
            int sign;
            if (number >= 0)
                sign = 0;
            else
                sign = 1;

            // Calculates the significand
            number = Math.Abs(number);
            int count = 0;
            if (number < 1 && number > 0)
            {
                while (number < 1)
                {
                    number *= 2;
                    count--;
                }
            }
            else
            {
                while (number >= 2)
                {
                    number /= 2;
                    count++;
                }
            }
            NumberNotation toConvert = new NumberNotation(sign, number, count);
            if (number.Equals(0))
                toConvert.exponent = -127;
            Convert32Bit(toConvert);
            if (number.Equals(0))
                toConvert.exponent = -1023;
            Convert64Bit(toConvert);
        }
        #endregion

        public static void Main(string[] args)
        {
            Console.Write("Enter a decimal to convert to floating point:");
            string input = Console.ReadLine();
            Console.WriteLine("------------------------------------------------------------------------------");
            ConvertFloatingPoint(input);
            Console.WriteLine("------------------------------------------------------------------------------");
        }
    }
}
