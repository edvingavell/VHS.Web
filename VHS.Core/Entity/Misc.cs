using System;
using System.Globalization;

namespace VHS.Core.Entity
{
    public class Misc
    {
        public static bool CheckRegNo(string x)
        {
            if (String.IsNullOrEmpty(x)) { return false; }
            if (x.Length != 6) { return false; }

            int countChars = 0, countNumbers = 0;
            x = x.ToUpper();
            for (int i = 0; i < x.Length; i++)
            {
                char c = x[i];
                if (Char.IsDigit(c))
                {
                    countNumbers++;
                }
                else if (Char.IsLetter(c))
                {
                    countChars++;
                }
            }
            if (countNumbers != 3 || countChars != 3)
            {
                return false;
            }
            return true;
        }

        public static bool CheckTirePressures(double tire1, double tire2, double tire3, double tire4)
        {
            static bool ControlTirePressure(double x)
            {
                var max_value_tirePressure = 10;
                var min_value_tirePressure = 0;
                if (x < min_value_tirePressure || x > max_value_tirePressure)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            var result = true;

            if (!ControlTirePressure(tire1) || !ControlTirePressure(tire2) || !ControlTirePressure(tire3) || !ControlTirePressure(tire4)) {
                result = false;
            }

            return result;
        }

    }
}
