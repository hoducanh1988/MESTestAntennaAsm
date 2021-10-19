using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using NationalInstruments.VisaNS; //Using .NET VISA DRIVER

namespace EW12SG.Function {

    public class MT8870A_VISA : Instrument {

        public MT8870A_VISA(string MeasureEquip_IP) {
            try {
                g_logfilePath = @"WIFI_LOGFILE_MT8870.LOG";
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(MeasureEquip_IP);
                GlobalData.myTesting.InitInstrument = "Passed";
            }
            catch (Exception ex) {
                GlobalData.myTesting.LogSystem += ex.ToString() + "\r\n";
                GlobalData.myTesting.LogSystem += "[MT8870A_VISA]Không kết nối được với máy đo IP = " + MeasureEquip_IP + "\r\n";
                GlobalData.myTesting.InitInstrument = "Failed";
            };
        }


        public override bool config_Instrument_Total(string port, int trigger_lever, string Standard) {
            try {
                bool r = false;
                string _wifiStandard = "";
                switch (Standard) {
                    case "b": {
                            _wifiStandard = "DSSS";
                            break;
                        }
                    default: {
                            _wifiStandard = "OFDM";
                            break;
                        }
                }

                string PortName = string.Format("PORT{0}", port.Replace("RFIO", "").Trim());
                r = this.Write("*RST\n"); if (!r) return false;
                r = this.Write(":CONF:SRW:SEGM:CLE\n"); if (!r) return false;
                r = this.Write(string.Format(":CONF:SRW:SEGM:APP AUTO{0}\n", _wifiStandard)); if (!r) return false;
                r = this.Write(":CONF:SRW:TRIG LEVEL\n"); if (!r) return false;
                r = this.Write(":CONF:SRW:TDEL -1E-05\n"); if (!r) return false;
                r = this.Write($":CONF:SRW:TLEV {trigger_lever}\n"); if (!r) return false;
                r = this.Write(":CONF:SRW:CAPT:MODE PACKET\n"); if (!r) return false;
                r = this.Write(":CONF:SRW:TTIM 1\n"); if (!r) return false;
                r = this.Write(":CONF:SRW:PACK 1\n"); if (!r) return false;
                r = this.Write(_wifiStandard == "DSSS" ? ":CONF:SRW:WLB:FTYP GAUSSIAN\n" : ":CONF:SRW:OFDM:CEST FULLPACKET\n"); if (!r) return false;
                r = this.Write(":CONF:SRW:SEL:WLAN:POW ON\n"); if (!r) return false;
                r = this.Write(":CONF:SRW:SEL:WLAN:EVM ON\n"); if (!r) return false;
                //r = this.Write(":CONF:SRW:POW 20\n"); if (!r) return false;
                r = this.Write(string.Format(":ROUT:PORT:CONN:DIR {0},{1}\n", PortName, PortName)); if (!r) return false;
                r = this.Write(":CONF:SRW:ALEV:TIME 0.01\n"); if (!r) return false;
                return r;
            }
            catch (Exception ex) {
                return false;
            }
        }


        //------------------ Hàm thiết lập tần số kênh ---------------------
        public override bool config_Instrument_Channel(string channel_Freq) {
            try {
                this.Write(":CONF:SRW:FREQ " + channel_Freq + "000000\n"); //Thiết lập chế độ WLAN
                return true;
            }
            catch (Exception ex) {
                //error = ex.ToString();
                return false;
            }
        }

        public override string config_Instrument_get_Power(string Trigger, int power_range, string wifi) {
            try {
                bool r = this.Write(":INIT:SRW:ALEV\n");
                this.Write("*WAI\n");
                this.Write(":CONF:SRW:POW " + power_range.ToString() + "\n"); //OK
                this.Write("*WAI\n");
                this.Query(":CONF:SRW:POW?\n");
                this.Write(":INIT:SRW\n");
                this.Write("*WAI\n");

                int counter = 0;
                while (true) {
                    counter++;
                    string x = this.Query(":STAT:SRW:MEAS?\n");
                    if (x.Replace("\r", "").Replace("\n", "").Trim() == "1") break;
                    Thread.Sleep(100);
                    if (counter >= 30) return "";
                }

                counter = 0;
            REP:
                counter++;
                string tmpStr = this.Query(":FETC:SRW:SUMM:WLAN:POW? 1,1\n");
                string[] buffer = tmpStr.Split(',');
                double data = 0;
                bool ret = false;
                try {
                    ret = double.TryParse(buffer[6], out data);
                }
                catch { }
                if (ret == false) {
                    if (counter < 3) goto REP;
                }
                return buffer[6];
            }
            catch (Exception ex) {
                GlobalData.myTesting.LogInstrument += ex.ToString() + "\n";
                return "";
            }
        }

       

        private bool Write(string cmd) {
            int count = 0;
        RE:
            count++;
            mbSession.Write(cmd);
            GlobalData.myTesting.LogInstrument += cmd;

            mbSession.Write(":SYSTem:ERRor?\n");
            string data = this.ReadString();
            bool r = data.ToLower().Contains("no error");
            if (!r) {
                if (count < 3) goto RE;
            }
            return r;
        }

        private string Query(string cmd) {
            string data = mbSession.Query(cmd);
            GlobalData.myTesting.LogInstrument += cmd + "\n";
            GlobalData.myTesting.LogInstrument += data + "\n";
            return data;
        }

        private string ReadString() {
            string data = mbSession.ReadString();
            GlobalData.myTesting.LogInstrument += data + "\n";
            return data;
        }

        public override string config_Instrument_get_FreqErr(string Trigger, string wifi) {
            throw new NotImplementedException();
        }

        public override string config_Instrument_get_EVM(string Trigger, string wifi) {
            throw new NotImplementedException();
        }

        public override string config_Instrument_get_TotalResult(string Trigger, string wifi) {
            throw new NotImplementedException();
        }

        public override bool config_HT20_RxTest_MAC(string channel, string power, string packet_number, string waveform_file, string port) {
            throw new NotImplementedException();
        }
    }

}
