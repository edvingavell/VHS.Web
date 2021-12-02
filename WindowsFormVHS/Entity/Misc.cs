using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WindowsFormVHS.Entity
{
    public static class Misc
    {
        public static bool CheckRegNo(string x) {
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
    
        public static bool CheckBatteryStatus(string x) 
        { 
            if (String.IsNullOrEmpty(x)) { return false; }
            int y = int.Parse(x);
            if (y <= 100 && y >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        public static bool CheckGPS(string x)
        {
            //NMEA?
            //$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*42


            return true;
        }

        public static bool CheckTripMeter(string x)
        {
            if (String.IsNullOrEmpty (x)) { return false; }
            double y = double.Parse(x);
            if (y <= Double.MaxValue && y >= 0)
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public static bool CheckLockStatus(string x)
        {
            if (String.IsNullOrEmpty(x)) { return false; }
            int y = int.Parse(x);
            if (y == 0 || y == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckAlarmStatus(string x)
        {
            if (String.IsNullOrEmpty(x)) { return false; }
            int y = int.Parse(x);
            if (y == 0 || y == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ControlTirePressure(double x)
        {
            if (x < 0 || x > 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CheckTirePressures(string tire1, string tire2, string tire3, string tire4)
        {
            
            if (String.IsNullOrEmpty(tire1) || String.IsNullOrEmpty(tire2) 
                || String.IsNullOrEmpty(tire3) || String.IsNullOrEmpty(tire4)) { return false; }

            if (tire1.Contains('.') || tire2.Contains('.') || tire3.Contains('.') || tire4.Contains('.')) { return false; }

            double t1 = double.Parse(tire1);
            double t2 = double.Parse(tire2);
            double t3 = double.Parse(tire3);
            double t4 = double.Parse(tire4);

            if (!ControlTirePressure(t1) || !ControlTirePressure(t2) || !ControlTirePressure(t3) || !ControlTirePressure(t4)) { return false; }
      

            return true;
        }

    }
}
