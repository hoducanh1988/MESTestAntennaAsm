using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30CX.UserDefine {
    public class initParameter {

        public static List<string> listStation = new List<string>() { "--", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        public static List<string> listJIG = new List<string>() { "--", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        public static List<string> listMeasurement_Name = new List<string>() { "E6640A", "MT8870A" };
        public static List<string> listMeasurement_Port = new List<string>() { "RFIO1", "RFIO2", "RFIO3", "RFIO4" };
        public static List<string> listMeasurement_PowerTrigger = new List<string>() { "-30", "-25", "-20", "-15", "-10", "-5", "0", "5", "10", "15", "20", "25", "30" };
        public static List<string> listFreq2G = new List<string>() { "2412", "2437", "2462" };
        public static List<string> listFreq5G = new List<string>() { "5180", "5260", "5500", "5610", "5775", "5825", "4920" };

    }
}
