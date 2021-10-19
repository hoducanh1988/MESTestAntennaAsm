using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EW12CG.UserDefine;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using EW12CG.Function;
using EW12CG.Base;
using UtilityPack.IO;

namespace EW12CG {
    public partial class MainWindow : Window {

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button btn = sender as Button;
            switch (btn.Content.ToString()) {

                case "Go": {
                        string logtype = (string)cbb_logtype.SelectedValue;
                        if (logtype == null) return;
                        logtype = logtype.ToLower();

                        if (logtype.Equals("logdetail")) {
                            try {
                                Process.Start(GlobalData.detailDirectory);
                            } catch { Process.Start(GlobalData.dir_Path); }
                            
                        }
                        if (logtype.Equals("logssh")) {
                            try {
                                Process.Start(GlobalData.sshDirectory);
                            }
                            catch { Process.Start(GlobalData.dir_Path); }

                        }
                        if (logtype.Equals("loginstrument")) {
                            try {
                                Process.Start(GlobalData.instrDirectory);
                            }
                            catch { Process.Start(GlobalData.dir_Path); }

                        }
                        if (logtype.Equals("logtotal")) {
                            try {
                                Process.Start(GlobalData.totalDirectory);
                            } catch { Process.Start(GlobalData.dir_Path); }
                            
                        }
                        break;
                    }

                case "Save": {
                        XmlHelper<SettingInformation>.ToXmlFile(GlobalData.mySetting, GlobalData.settingFileFullName); //save setting to xml file
                        MessageBox.Show("Success.", "Save Setting", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }

                case "Start": {
                        _isScroll = true;
                        //Stopwatch _st = new Stopwatch();
                        //_st.Start();

                        Thread t = new Thread(new ThreadStart(() => {

                            Thread z = new Thread(new ThreadStart(() => {
                                Stopwatch st = new Stopwatch();
                                st.Start();
                                while (_isScroll) {
                                    Thread.Sleep(500);
                                    GlobalData.myTesting.totalTime = UtilityPack.Converter.myConverter.intToTimeSpan((int)st.ElapsedMilliseconds);
                                }
                                st.Stop();
                                st = null;
                            }));
                            z.IsBackground = true;
                            z.Start();

                            Stopwatch ttt = new Stopwatch();
                            ttt.Start();

                            GlobalData.myTesting.initParam();
                            GlobalData.myTesting.waitParam();

                            bool r = testWifi.TestWifi_Function();
                            GlobalData.myTesting.LogSystem += "\r\n\r\nEnd test: \r\n";
                            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
                            GlobalData.myTesting.LogSystem += r == true ? "... total result = Passed\r\n" : "... total result = Failed\r\n";
                            bool _ = r ? GlobalData.myTesting.passParam() : GlobalData.myTesting.failParam();

                            ttt.Stop();
                            GlobalData.myTesting.LogSystem += string.Format("... total time = {0} ms.\r\n", ttt.ElapsedMilliseconds);
                            GlobalData.LogInfo.Save_LogTotal();
                            GlobalData.LogInfo.Save_LogSystem();
                            GlobalData.LogInfo.Save_LogSsh();
                            GlobalData.LogInfo.Save_LogInstrument();
                            _isScroll = false;
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }
        }

    }
}
