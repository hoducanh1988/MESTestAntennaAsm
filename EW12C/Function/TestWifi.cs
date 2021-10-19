using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using EW12C.Base;

namespace EW12C.Function {
    class TestWifi : BaseFunction {


        public bool TestWifi_Function() {
            bool ret = false, r11 = false, r12 = false, r21 = false, r22 = false;

            //load file pathloss
            ret = load_file_pathloss();
            if (!ret) goto END;

            //ping to DUT
            ret = ping_to_dut();
            if (!ret) goto END;

            //login ssh
            ret = login_ssh_to_dut();
            if (!ret) goto END;

            //get mac ethernet
            get_mac_ethernet();

            //init instrument
            ret = init_instrument();
            if (!ret) goto END;

            //off wifi 5G
            //off_wifi_5g();

            //measure 2G - anten1
            r11 = measure_power_2g_anten1();

            //measure 2G - anten2
            r12 = measure_power_2g_anten2();

            //off wifi 2G
            //off_wifi_2g();

            //measure 5G - anten1
            r21 = measure_power_5g_anten1();

            //measure 5G - anten2
            r22 = measure_power_5g_anten2();

        //return value
        END:
            return ret && r11 && r12 && r21 && r22;
        }



        #region sub-function


        private bool load_file_pathloss() {
            Stopwatch st = new Stopwatch();
            st.Start();

            bool r = false;
            try {
                string f = string.Format("{0}DoSuyHaoXML\\PathLoss_TestAntenna_20200424_0937.xml", GlobalData.dir_Path);
                GlobalData.myTesting.loadFilePathLoss = "Waiting...";
                GlobalData.myTesting.LogSystem += string.Format("Đọc file suy hao : \r\n");
                GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
                GlobalData.myTesting.LogSystem += string.Format("... {0}\r\n", f);
                GlobalData.myTesting.LogSystem += string.Format("...\r\n");

                if (!File.Exists(f)) {
                    GlobalData.myTesting.LogSystem += "...\r\n... result: Failed, file is not existed.\r\n";
                    GlobalData.myTesting.loadFilePathLoss = "Failed";
                    goto END;
                } //check file exist

                string[] buffer = File.ReadAllLines(f); //read all line from file
                if (buffer.Length == 0) {
                    GlobalData.myTesting.LogSystem += "...\r\n... result: Failed, file is wrong format.\r\n";
                    GlobalData.myTesting.loadFilePathLoss = "Failed";
                    goto END;
                }

                GlobalData.PathLossInfo = new List<UserDefine.PathLossItem>();
                for (int i = 0; i < buffer.Length; i++) {
                    if (buffer[i].ToLower().Contains("frequency")) {
                        string freq = buffer[i].ToLower().Replace("<frequency>", "").Replace("</frequency>", "").Trim();
                        string val = buffer[i + 1].ToLower().Replace("<value>", "").Replace("</value>", "").Trim();
                        UserDefine.PathLossItem item = new UserDefine.PathLossItem() { Freq = freq, Value = Math.Abs(double.Parse(val)) };
                        GlobalData.PathLossInfo.Add(item);
                    }
                }

                r = GlobalData.PathLossInfo.Count > 0;
                if (!r) {
                    GlobalData.myTesting.LogSystem += "...\r\n... result: Failed, file is wrong format.\r\n";
                    GlobalData.myTesting.loadFilePathLoss = "Failed";
                    goto END;
                }

                foreach (var item in GlobalData.PathLossInfo) {
                    GlobalData.myTesting.LogSystem += string.Format( "... {0}\r\n", item.ToString());
                }
                GlobalData.myTesting.loadFilePathLoss = "Passed";
                GlobalData.myTesting.LogSystem += "...\r\n... result: Passed\r\n";
                goto END;

            } catch (Exception ex) {
                GlobalData.myTesting.LogSystem += string.Format("...\r\n... result: Failed, {0}.\r\n", ex.ToString());
                GlobalData.myTesting.loadFilePathLoss = "Failed";
                goto END;
            }


        END:
            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;
        }


        private bool ping_to_dut() {
            Stopwatch st = new Stopwatch();
            st.Start();

            GlobalData.myTesting.pingToDut = "Waiting...";
            GlobalData.myTesting.LogSystem += string.Format("\r\n\r\nPing to DUT {0} - Retry 5: \r\n", GlobalData.mySetting.IPDUT);
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            int count = 0;
            bool r = false;
        RE:
            count++;
            r = BaseFunction.pingToIPAddress(GlobalData.mySetting.IPDUT);
            GlobalData.myTesting.LogSystem += r ? string.Format("... reply from {0}: bytes=32 time<1ms TTL=64\r\n", GlobalData.mySetting.IPDUT) : "... request timed out.\r\n";
            if (!r) {
                if (count < 5) { Thread.Sleep(500); goto RE; }
            }
            GlobalData.myTesting.LogSystem += r ? "... result: Passed\r\n" : "... result: Failed\r\n";
            GlobalData.myTesting.pingToDut = r ? "Passed" : "Failed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;
        }

        private bool login_ssh_to_dut() {
            Stopwatch st = new Stopwatch();
            st.Start();

            GlobalData.myTesting.loginSSH = "Waiting...";
            GlobalData.myTesting.LogSystem += string.Format("\r\n\r\nLogin ssh to DUT - Retry {0}: \r\n", GlobalData.mySetting.RetryTime);
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            GlobalData.myTesting.LogSystem += string.Format("... dut ip = {0}\r\n", GlobalData.mySetting.IPDUT);
            GlobalData.myTesting.LogSystem += string.Format("... ssh user = {0}\r\n", GlobalData.mySetting.Username);
            GlobalData.myTesting.LogSystem += string.Format("... ssh pass = {0}\r\n", GlobalData.mySetting.Password);

            int count = 0;
            bool r = false;
        RE:
            count++;
            r = GlobalData.SSHConn.Login(GlobalData.mySetting.IPDUT, GlobalData.mySetting.Username, GlobalData.mySetting.Password);
            GlobalData.myTesting.LogSystem += r ? string.Format("... result[{0}]: Passed\r\n", count) : string.Format("... result[{0}]: Failed\r\n", count);
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) { Thread.Sleep(300); goto RE; }
            }

            GlobalData.myTesting.loginSSH = r ? "Passed" : "Failed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;


        }

        private bool init_instrument() {
            Stopwatch st = new Stopwatch();
            st.Start();

            GlobalData.myTesting.InitInstrument = "Waiting...";
            GlobalData.myTesting.LogSystem += string.Format("\r\n\r\nInit instrument - Retry {0}: \r\n", GlobalData.mySetting.RetryTime);
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            GlobalData.myTesting.LogSystem += string.Format("... instrument name = {0}\r\n", GlobalData.mySetting.Measurement_Name);
            GlobalData.myTesting.LogSystem += string.Format("... instrument address = {0}\r\n", GlobalData.mySetting.Measurement_Addr);
            GlobalData.myTesting.LogSystem += string.Format("... instrument port = {0}\r\n", GlobalData.mySetting.Measurement_Port);

            bool r = false;
            if (GlobalData.mySetting.Measurement_Name == "E6640A") {
                GlobalData.Measurement = new E6640A_VISA(GlobalData.mySetting.Measurement_Addr);
            }
            else if (GlobalData.mySetting.Measurement_Name == "MT8870A") {
                GlobalData.Measurement = new MT8870A_VISA(GlobalData.mySetting.Measurement_Addr);
            }
            r = GlobalData.myTesting.InitInstrument == "Passed";
            GlobalData.myTesting.LogSystem += r ? "... result: Passed\r\n" : "... result: Failed\r\n";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;

        }

        private void get_mac_ethernet() {
            Stopwatch st = new Stopwatch();
            st.Start();

            GlobalData.myTesting.getMacEthernet = "Waiting...";
            GlobalData.myTesting.LogSystem += "\r\n\r\nGet mac ethernet: \r\n";
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            GlobalData.myTesting.MAC_Address = getMAC();
            GlobalData.myTesting.LogSystem += string.Format("... {0} \r\n", GlobalData.myTesting.MAC_Address);
            GlobalData.myTesting.LogSystem += string.Format("... Completed\r\n");

            GlobalData.myTesting.getMacEthernet = "Passed";
            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
        }

        /// <summary>
        /// 
        /// </summary>
        private void off_wifi_5g() {
            Stopwatch st = new Stopwatch();
            st.Start();

            GlobalData.myTesting.offWifi5G = "Waiting...";
            GlobalData.myTesting.LogSystem += "\r\n\r\nOff wifi 5G: \r\n";
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";

            GlobalData.SSHConn.Query("uci set wireless.wifi0.disabled=0", "root@VNPT:~#", 10, true);
            GlobalData.SSHConn.Query("uci set wireless.wifi1.disabled=1", "root@VNPT:~#", 10, true);
            GlobalData.SSHConn.Query("uci commit wireless", "root@VNPT:~#", 10, true);
            GlobalData.SSHConn.Query("wifi", "root@VNPT:~#", 10, true);

            GlobalData.myTesting.LogSystem += string.Format("... Completed\r\n");
            GlobalData.myTesting.offWifi5G = "Passed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
        }


        /// <summary>
        /// 
        /// </summary>
        private void off_wifi_2g() {
            Stopwatch st = new Stopwatch();
            st.Start();

            GlobalData.myTesting.offWifi2G = "Waiting...";
            GlobalData.myTesting.LogSystem += "\r\n\r\nOff wifi 2G: \r\n";
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";

            GlobalData.SSHConn.Query("uci set wireless.wifi0.disabled=1", "root@VNPT:~#", 10, true);
            GlobalData.SSHConn.Query("uci set wireless.wifi1.disabled=0", "root@VNPT:~#", 10, true);
            GlobalData.SSHConn.Query("uci commit wireless", "root@VNPT:~#", 10, true);
            GlobalData.SSHConn.Query("wifi", "root@VNPT:~#", 10, true);

            GlobalData.myTesting.LogSystem += string.Format("... Completed\r\n");
            GlobalData.myTesting.offWifi2G = "Passed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool measure_power_2g_anten1() {
            Stopwatch st = new Stopwatch();
            st.Start();

            int count = 0;
            string power_str = "";
            double power_dbl = 0;
            bool r = false;

            GlobalData.myTesting.Result2G_Anten1 = "Waiting...";
            GlobalData.myTesting.LogSystem += string.Format("\r\n\r\nMeasure Wifi 2G - Antenna 1 - Retry {0}: \r\n", GlobalData.mySetting.RetryTime);
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            GlobalData.myTesting.LogSystem += string.Format("... power limit: {0} dBm ~ {1} dBm\r\n", GlobalData.mySetting.Power_Min_2G_Chain1, GlobalData.mySetting.Power_Max_2G_Chain1);

            //get pathloss data
            double pathloss = 0;
            if (GlobalData.PathLossInfo != null && GlobalData.PathLossInfo.Count > 0) {
                foreach (var item in GlobalData.PathLossInfo) {
                    if (item.Freq.Equals(GlobalData.mySetting.Freq_Channel2G)) {
                        pathloss = item.Value;
                        break;
                    }
                }
            }
            GlobalData.myTesting.LogSystem += string.Format("... pathloss value: {0} dBm\r\n", pathloss);

            //config instrument
            GlobalData.myTesting.LogSystem += string.Format("... config instrument: Port {0}, standard {1}, freq {2}\r\n", GlobalData.mySetting.Measurement_Port, "b", GlobalData.mySetting.Freq_Channel2G);
            GlobalData.Measurement.config_Instrument_Total(GlobalData.mySetting.Measurement_Port, GlobalData.mySetting.Measurement_TriggerLevel2G, "b");
            GlobalData.Measurement.config_Instrument_Channel(GlobalData.mySetting.Freq_Channel2G);

            //config dut
            GlobalData.myTesting.LogSystem += string.Format("... config dut: iwpriv wifi0 txchainmask 0x1\r\n");
            GlobalData.SSHConn.Query("iwpriv wifi0 txchainmask 0x1", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("ifconfig ath0 down", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("ifconfig ath0 up", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("iwpriv ath0 bintval 40", "root@VNPT:~#", 3, true);

            Thread.Sleep(500);

        RE:
            count++;
            //get power from instrument
            GlobalData.myTesting.LogSystem += string.Format("... get power from instrument[{0}]: ", count);
            power_str = GlobalData.Measurement.config_Instrument_get_Power("VID", GlobalData.mySetting.Measurement_PowerLevel2G, "b");
            GlobalData.myTesting.LogSystem += power_str == null ? "null" : power_str + " dBm\r\n";

            if (power_str == null) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            r = double.TryParse(power_str, out power_dbl);
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            power_dbl = power_dbl + pathloss;
            GlobalData.myTesting.Power2G_Anten1 = Math.Round(power_dbl, 2).ToString();

            GlobalData.myTesting.LogSystem += string.Format("... power after add pathloss [{0}]: {1} dBm\r\n", count, power_dbl);
            r = power_dbl > GlobalData.mySetting.Power_Min_2G_Chain1 && power_dbl < GlobalData.mySetting.Power_Max_2G_Chain1;
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }


        END:
            GlobalData.myTesting.LogSystem += r ? "... result: Passed\r\n" : "... result: Failed\r\n";
            GlobalData.myTesting.Result2G_Anten1 = r ? "Passed" : "Failed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool measure_power_2g_anten2() {
            Stopwatch st = new Stopwatch();
            st.Start();

            int count = 0;
            string power_str = "";
            double power_dbl = 0;
            bool r = false;

            GlobalData.myTesting.Result2G_Anten2 = "Waiting...";
            GlobalData.myTesting.LogSystem += string.Format("\r\n\r\nMeasure Wifi 2G - Antenna 2 - Retry {0}: \r\n", GlobalData.mySetting.RetryTime);
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            GlobalData.myTesting.LogSystem += string.Format("... power limit: {0} dBm ~ {1} dBm\r\n", GlobalData.mySetting.Power_Min_2G_Chain2, GlobalData.mySetting.Power_Max_2G_Chain2);

            //get pathloss data
            double pathloss = 0;
            if (GlobalData.PathLossInfo != null && GlobalData.PathLossInfo.Count > 0) {
                foreach (var item in GlobalData.PathLossInfo) {
                    if (item.Freq.Equals(GlobalData.mySetting.Freq_Channel2G)) {
                        pathloss = item.Value;
                        break;
                    }
                }
            }
            GlobalData.myTesting.LogSystem += string.Format("... pathloss value: {0} dBm\r\n", pathloss);

            //config instrument
            GlobalData.myTesting.LogSystem += string.Format("... config instrument: Port {0}, standard {1}, freq {2}\r\n", GlobalData.mySetting.Measurement_Port, "b", GlobalData.mySetting.Freq_Channel2G);
            //GlobalData.Measurement.config_Instrument_Total(GlobalData.mySetting.Measurement_Port, GlobalData.mySetting.Measurement_TriggerLevel2G, "b");
            GlobalData.Measurement.config_Instrument_Channel(GlobalData.mySetting.Freq_Channel2G);

            //config dut
            GlobalData.myTesting.LogSystem += string.Format("... config dut: iwpriv wifi0 txchainmask 0x2\r\n");
            GlobalData.SSHConn.Query("iwpriv wifi0 txchainmask 0x2", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("ifconfig ath0 down", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("ifconfig ath0 up", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("iwpriv ath0 bintval 40", "root@VNPT:~#", 3, true);

            //sleep
            Thread.Sleep(500);

        RE:
            count++;
            //get power from instrument
            GlobalData.myTesting.LogSystem += string.Format("... get power from instrument[{0}]: ", count);
            power_str = GlobalData.Measurement.config_Instrument_get_Power("VID", GlobalData.mySetting.Measurement_PowerLevel2G, "b");
            GlobalData.myTesting.LogSystem += power_str == null ? "null" : power_str + " dBm\r\n";

            if (power_str == null) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            r = double.TryParse(power_str, out power_dbl);
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            power_dbl = power_dbl + pathloss;
            GlobalData.myTesting.Power2G_Anten2 = Math.Round(power_dbl, 2).ToString();
            GlobalData.myTesting.LogSystem += string.Format("... power after add pathloss [{0}]: {1} dBm\r\n", count, power_dbl);
            r = power_dbl > GlobalData.mySetting.Power_Min_2G_Chain2 && power_dbl < GlobalData.mySetting.Power_Max_2G_Chain2;
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }


        END:
            GlobalData.myTesting.LogSystem += r ? "... result: Passed\r\n" : "... result: Failed\r\n";
            GlobalData.myTesting.Result2G_Anten2 = r ? "Passed" : "Failed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool measure_power_5g_anten1() {
            Stopwatch st = new Stopwatch();
            st.Start();

            int count = 0;
            string power_str = "";
            double power_dbl = 0;
            bool r = false;

            GlobalData.myTesting.Result5G_Anten1 = "Waiting...";
            GlobalData.myTesting.LogSystem += string.Format("\r\n\r\nMeasure Wifi 5G - Antenna 1 - Retry {0}: \r\n", GlobalData.mySetting.RetryTime);
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            GlobalData.myTesting.LogSystem += string.Format("... power limit: {0} dBm ~ {1} dBm\r\n", GlobalData.mySetting.Power_Min_5G_Chain1, GlobalData.mySetting.Power_Max_5G_Chain1);

            //get pathloss data
            double pathloss = 0;
            if (GlobalData.PathLossInfo != null && GlobalData.PathLossInfo.Count > 0) {
                foreach (var item in GlobalData.PathLossInfo) {
                    if (item.Freq.Equals(GlobalData.mySetting.Freq_Channel5G)) {
                        pathloss = item.Value;
                        break;
                    }
                }
            }
            GlobalData.myTesting.LogSystem += string.Format("... pathloss value: {0} dBm\r\n", pathloss);

            //config instrument
            GlobalData.myTesting.LogSystem += string.Format("... config instrument: Port {0}, standard {1}, freq {2}\r\n", GlobalData.mySetting.Measurement_Port, "a", GlobalData.mySetting.Freq_Channel5G);
            GlobalData.Measurement.config_Instrument_Total(GlobalData.mySetting.Measurement_Port, GlobalData.mySetting.Measurement_TriggerLevel5G, "a");
            GlobalData.Measurement.config_Instrument_Channel(GlobalData.mySetting.Freq_Channel5G);

            //config dut
            GlobalData.myTesting.LogSystem += string.Format("... config dut: iwpriv wifi1 txchainmask 0x1\r\n");
            GlobalData.SSHConn.Query("iwpriv wifi1 txchainmask 0x1", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("iwpriv ath1 bintval 40", "root@VNPT:~#", 3, true);

            //sleep
            Thread.Sleep(500);

        RE:
            count++;
            //get power from instrument
            GlobalData.myTesting.LogSystem += string.Format("... get power from instrument[{0}]: ", count);
            power_str = GlobalData.Measurement.config_Instrument_get_Power("VID", GlobalData.mySetting.Measurement_PowerLevel5G, "a");
            GlobalData.myTesting.LogSystem += power_str == null ? "null" : power_str + " dBm\r\n";

            if (power_str == null) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            r = double.TryParse(power_str, out power_dbl);
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            power_dbl = power_dbl + pathloss;
            GlobalData.myTesting.Power5G_Anten1 = Math.Round(power_dbl, 2).ToString();

            GlobalData.myTesting.LogSystem += string.Format("... power after add pathloss [{0}]: {1} dBm\r\n", count, power_dbl);
            r = power_dbl > GlobalData.mySetting.Power_Min_5G_Chain1 && power_dbl < GlobalData.mySetting.Power_Max_5G_Chain1;
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }


        END:
            GlobalData.myTesting.LogSystem += r ? "... result: Passed\r\n" : "... result: Failed\r\n";
            GlobalData.myTesting.Result5G_Anten1 = r ? "Passed" : "Failed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool measure_power_5g_anten2() {
            Stopwatch st = new Stopwatch();
            st.Start();


            int count = 0;
            string power_str = "";
            double power_dbl = 0;
            bool r = false;

            GlobalData.myTesting.Result5G_Anten2 = "Waiting...";
            GlobalData.myTesting.LogSystem += string.Format("\r\n\r\nMeasure Wifi 5G - Antenna 2 - Retry {0}: \r\n", GlobalData.mySetting.RetryTime);
            GlobalData.myTesting.LogSystem += "++++++++++++++++++++++++++++++++++++++++++ \r\n";
            GlobalData.myTesting.LogSystem += string.Format("... power limit: {0} dBm ~ {1} dBm\r\n", GlobalData.mySetting.Power_Min_5G_Chain2, GlobalData.mySetting.Power_Max_5G_Chain2);

            //get pathloss data
            double pathloss = 0;
            if (GlobalData.PathLossInfo != null && GlobalData.PathLossInfo.Count > 0) {
                foreach (var item in GlobalData.PathLossInfo) {
                    if (item.Freq.Equals(GlobalData.mySetting.Freq_Channel5G)) {
                        pathloss = item.Value;
                        break;
                    }
                }
            }
            GlobalData.myTesting.LogSystem += string.Format("... pathloss value: {0} dBm\r\n", pathloss);

            //config instrument
            GlobalData.myTesting.LogSystem += string.Format("... config instrument: Port {0}, standard {1}, freq {2}\r\n", GlobalData.mySetting.Measurement_Port, "a", GlobalData.mySetting.Freq_Channel5G);
            //GlobalData.Measurement.config_Instrument_Total(GlobalData.mySetting.Measurement_Port, GlobalData.mySetting.Measurement_TriggerLevel5G, "a");
            GlobalData.Measurement.config_Instrument_Channel(GlobalData.mySetting.Freq_Channel5G);

            //config dut
            GlobalData.myTesting.LogSystem += string.Format("... config dut: iwpriv wifi1 txchainmask 0x2\r\n");
            GlobalData.SSHConn.Query("iwpriv wifi1 txchainmask 0x2", "root@VNPT:~#", 3, true);
            GlobalData.SSHConn.Query("iwpriv ath1 bintval 40", "root@VNPT:~#", 3, true);

            //sleep
            Thread.Sleep(500);

        RE:
            count++;
            //get power from instrument
            GlobalData.myTesting.LogSystem += string.Format("... get power from instrument[{0}]: ", count);
            power_str = GlobalData.Measurement.config_Instrument_get_Power("VID", GlobalData.mySetting.Measurement_PowerLevel5G, "a");
            GlobalData.myTesting.LogSystem += power_str == null ? "null" : power_str + " dBm\r\n";

            if (power_str == null) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            r = double.TryParse(power_str, out power_dbl);
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }

            power_dbl = power_dbl + pathloss;
            GlobalData.myTesting.Power5G_Anten2 = Math.Round(power_dbl, 2).ToString();

            GlobalData.myTesting.LogSystem += string.Format("... power after add pathloss [{0}]: {1} dBm\r\n", count, power_dbl);
            r = power_dbl > GlobalData.mySetting.Power_Min_5G_Chain2 && power_dbl < GlobalData.mySetting.Power_Max_5G_Chain2;
            if (!r) {
                if (count < GlobalData.mySetting.RetryTime) {
                    goto RE;
                }
                else goto END;
            }


        END:
            GlobalData.myTesting.LogSystem += r ? "... result: Passed\r\n" : "... result: Failed\r\n";
            GlobalData.myTesting.Result5G_Anten2 = r ? "Passed" : "Failed";

            st.Stop();
            GlobalData.myTesting.LogSystem += $"... time elapsed: {st.ElapsedMilliseconds} ms\r\n";
            return r;

        }

        
        #endregion


    }
}
