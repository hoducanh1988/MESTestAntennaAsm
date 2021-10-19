using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30CX.UserDefine {

    public class PathLossItem {
        public PathLossItem() {
            Freq = "";
            Value = 0;
        }

        public string Freq { get; set; }
        public double Value { get; set; }

        public override string ToString() {
            return string.Format("freq = {0}, value = {1}", Freq, Value);
        }

    }
}
