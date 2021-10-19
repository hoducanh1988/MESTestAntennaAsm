using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EW12SG.UserDefine;
using EW12SG.Protocol;
using EW12SG.Function;

namespace EW12SG {
    public static class GlobalData {

        public static string settingFileFullName = string.Format("{0}setting.xml", AppDomain.CurrentDomain.BaseDirectory);
        public static TestingInformation myTesting = new TestingInformation();
        public static SettingInformation mySetting = new SettingInformation();

        public static SSH SSHConn = new SSH();
        public static Instrument Measurement = null;
        public static LogInfomation LogInfo = new LogInfomation();

        public static string dir_Path = AppDomain.CurrentDomain.BaseDirectory.Replace("EW12SG\\", "");
        public static string detailDirectory = string.Format("{0}Logdetail\\EW12SG\\{1}", dir_Path, DateTime.Now.ToString("yyyy-MM-dd"));
        public static string sshDirectory = string.Format("{0}Logssh\\EW12SG\\{1}", dir_Path, DateTime.Now.ToString("yyyy-MM-dd"));
        public static string instrDirectory = string.Format("{0}Loginstrument\\EW12SG\\{1}", dir_Path, DateTime.Now.ToString("yyyy-MM-dd"));
        public static string totalDirectory = string.Format("{0}Logtotal\\EW12SG", dir_Path);

        public static List<PathLossItem> PathLossInfo = null;

    }
}
