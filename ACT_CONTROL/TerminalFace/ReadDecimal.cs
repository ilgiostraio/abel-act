using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using EasyConsole;

namespace TerminalFace
{
    static class Input
    {
        public static decimal ReadDecimal(string prompt, decimal min, decimal max)
        {
            Output.DisplayPrompt(prompt);
            return ReadDecimal(min, max);
        }

        public static decimal ReadDecimal(decimal min, decimal max)
        {
            decimal value = ReadDecimal();

            while (value < min || value > max)
            {
                Output.DisplayPrompt("Please enter an decimal between {0} and {1} (inclusive)", min, max);
                value = ReadDecimal();
            }

            return value;
        }

        public static decimal ReadDecimal()
        {
            string input = Console.ReadLine();
            decimal value;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
            while (!decimal.TryParse(input, NumberStyles.AllowDecimalPoint, culture, out value))
            {
                Output.DisplayPrompt("Please enter an decimal");
                input = Console.ReadLine();
            }

            return value;
        }
    }
}
