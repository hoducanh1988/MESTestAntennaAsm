using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EW30CX.UserDefine {
    public class LogInfomation {


        private void getSettingInfo(ref string str) {
            str += "SETTING INFO\n";
            PropertyInfo[] propers = GlobalData.mySetting.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in propers) {
                string data = $"{p.Name}={p.GetValue(GlobalData.mySetting, null)}";
                str += $"{data}\n";
            }
            str += "+++++++++++++++++++++++++++++++++++++++++\n";
        }

        private void getAppInfo(ref string str) {
            str += "APP INFO\n";
            str += $"{GlobalData.myApp.appInfo}\n";
            str += "+++++++++++++++++++++++++++++++++++++++++\n";
        }


        public void Save_LogSystem() {
            string log_data = "";
            getAppInfo(ref log_data);
            getSettingInfo(ref log_data);

            string rootPath = GlobalData.detailDirectory;
            if (Directory.Exists(rootPath) == false) { Directory.CreateDirectory(rootPath); }

            string file = string.Format("{0}\\EW30CX_{1}_{2}_{3}.txt", rootPath, GlobalData.myTesting.MAC_Address.Replace(":","").Replace("-","").Trim(), DateTime.Now.ToString("HHmmss"), GlobalData.myTesting.Result_Total);
            System.IO.StreamWriter st = new System.IO.StreamWriter(file, true);
            st.WriteLine(log_data);
            st.WriteLine(GlobalData.myTesting.LogSystem);
            st.Dispose();
        }

        public void Save_LogSsh() {
            string rootPath = GlobalData.sshDirectory;
            if (Directory.Exists(rootPath) == false) { Directory.CreateDirectory(rootPath); }

            string file = string.Format("{0}\\EW30CX_{1}_{2}_{3}.txt", rootPath, GlobalData.myTesting.MAC_Address.Replace(":", "").Replace("-", "").Trim(), DateTime.Now.ToString("HHmmss"), GlobalData.myTesting.Result_Total);
            System.IO.StreamWriter st = new System.IO.StreamWriter(file, true);
            st.WriteLine(GlobalData.myTesting.LogSsh);
            st.Dispose();
        }

        public void Save_LogInstrument() {
            string rootPath = GlobalData.instrDirectory;
            if (Directory.Exists(rootPath) == false) { Directory.CreateDirectory(rootPath); }

            string file = string.Format("{0}\\EW30CX_{1}_{2}_{3}.txt", rootPath, GlobalData.myTesting.MAC_Address.Replace(":", "").Replace("-", "").Trim(), DateTime.Now.ToString("HHmmss"), GlobalData.myTesting.Result_Total);
            System.IO.StreamWriter st = new System.IO.StreamWriter(file, true);
            st.WriteLine(GlobalData.myTesting.LogInstrument);
            st.Dispose();
        }

        public void Save_LogTotal() {
            string rootPath = GlobalData.totalDirectory;
            if (Directory.Exists(rootPath) == false) { Directory.CreateDirectory(rootPath); }

            string file = string.Format("{0}\\EW30CX_{1}.csv", rootPath, DateTime.Now.ToString("yyyyMMdd"));
            if (System.IO.File.Exists(file) == false) {
                StreamWriter csvWriter = new StreamWriter(file, true);
                csvWriter.WriteLine("Date_Time_Create,MacAddress,2G_Chain1,2G_Chain2,5G_Chain1,5G_Chain2,Result");
                csvWriter.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                                                  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                  GlobalData.myTesting.MAC_Address,
                                                  GlobalData.myTesting.Power2G_Anten1,
                                                  GlobalData.myTesting.Power2G_Anten2,
                                                  GlobalData.myTesting.Power5G_Anten1,
                                                  GlobalData.myTesting.Power5G_Anten2,
                                                  GlobalData.myTesting.Result_Total));
                csvWriter.Dispose();
            }
            else {
                StreamWriter csvWriter = new StreamWriter(file, true);
                csvWriter.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                                                 DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                 GlobalData.myTesting.MAC_Address,
                                                 GlobalData.myTesting.Power2G_Anten1,
                                                 GlobalData.myTesting.Power2G_Anten2,
                                                 GlobalData.myTesting.Power5G_Anten1,
                                                 GlobalData.myTesting.Power5G_Anten2,
                                                 GlobalData.myTesting.Result_Total));
                csvWriter.Dispose();
            }
        }

    }
}
