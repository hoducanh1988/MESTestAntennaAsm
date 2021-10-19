using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using NationalInstruments.VisaNS; //Using .NET VISA DRIVER

namespace EW12C.Function {

    public class E6640A_VISA : Instrument {

        public E6640A_VISA(string MeasureEquip_IP) {
            try {
                g_logfilePath = @"WIFI_LOGFILE_E6640A.LOG";
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(MeasureEquip_IP);
                GlobalData.myTesting.InitInstrument = "Passed";
            }
            catch (Exception ex) {
                GlobalData.myTesting.LogSystem += ex.ToString() + "\r\n";
                GlobalData.myTesting.LogSystem += "[E6640A_VISA]Không kết nối được với máy đo IP = " + MeasureEquip_IP + "\r\n";
                GlobalData.myTesting.InitInstrument = "Failed";
            };
        }


        private void Write(string cmd) {
            mbSession.Write(cmd);
            GlobalData.myTesting.LogInstrument += cmd;
        }

        private string Query(string cmd) {
            string data = mbSession.Query(cmd);
            GlobalData.myTesting.LogInstrument += cmd + "\n";
            GlobalData.myTesting.LogInstrument += data + "\n";
            return data;
        }



        //------------------ Hàm thiết lập cấu hình cho máy đo lần đầu tiên ---------------------
        public override bool config_Instrument_Total(string port, int trigger_lever, string Standard) {
            bool enable_nSISO_Testing = false;
            try {
                string _wifiStandard = "";
                switch (Standard) {
                    case "g": { _wifiStandard = "GDO"; break; }
                    case "b": { _wifiStandard = "BG"; break; }
                    case "a": { _wifiStandard = "AG"; break; }
                    case "n20": { _wifiStandard = "N20"; break; }
                    case "n40": { _wifiStandard = "N40"; break; }
                    case "ac20": { _wifiStandard = "AC20"; break; }
                    case "ac40": { _wifiStandard = "AC40"; break; }
                    case "ac80": { _wifiStandard = "AC80"; break; }
                    case "ac160": { _wifiStandard = "AC160"; break; }
                }

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write(":SYSTem:PRESet\n"); //Preset máy đo

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write(":FEED:RF:PORT " + port + "\n"); //Chọn RF input

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write("INST:SEL WLAN\n"); //Thiết lập chế độ WLAN

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write(":RAD:STAN " + _wifiStandard + "\n"); //Thiết lập chuẩn b/g/n kèm bandwidth

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write(":INITiate:CHP\n"); //Thiết lập đo ở chế độ CHP

                enable_nSISO_Testing = true;
            }
            catch (Exception ex) {
                enable_nSISO_Testing = false;
                config_done = true;
                GlobalData.myTesting.LogSystem += ex.ToString() + "\r\n";
            }
            return enable_nSISO_Testing;
        }


        //------------------ Hàm thiết lập tần số kênh ---------------------
        public override bool config_Instrument_Channel(string channel_Freq) {
            bool enable_nSISO_Testing = false;
            try {
                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write("FREQ:CENT " + channel_Freq + "000000\n");
                enable_nSISO_Testing = true;
                config_done = true;
            }
            catch (Exception ex) {
                //error = ex.ToString();
                enable_nSISO_Testing = false;
                config_done = true;
            }
            return enable_nSISO_Testing;
        }


        //------------------ Hàm đọc về độ lệch tần số ---------------------
        public override string config_Instrument_get_FreqErr(string Trigger, string wifi) {
            try {
                string result_Value = string.Empty;

                this.Write("TRIG:EVM:SOUR " + Trigger + "\n");
                this.Write(":POW:RANG 15.0\n");
                this.Write(":INIT:CONT 0\n");
                result_Value = this.Query("READ:EVM?");
                result_Value = InsertCommonEscapeSequences(result_Value);
                if (result_Value.Trim() == "") {
                    return null;
                }
                else {
                    try {
                        string[] MODulation_Value = result_Value.Split(new Char[] { ',' });
                        Decimal measureResult = 0;
                        measureResult = Decimal.Parse(MODulation_Value[7], System.Globalization.NumberStyles.Float);
                        return measureResult.ToString();
                    }
                    catch {
                        return null;
                    }
                }
            }
            catch {
                return null;
            }
        }

        //------------------ Hàm đọc về công suất ---------------------
        public override string config_Instrument_get_Power(string Trigger, int power_range, string wifi) {
            try {
                string result_Value = string.Empty;
                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write("TRIG:CHP:SOUR " + Trigger + "\n");

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write(":POW:RANG " + power_range.ToString() + "\n");

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                this.Write(":INIT:CONT 0\n");

                checkBusyState(mbSession, "*ESR?");
                checkBusyState(mbSession, "*OPC?");
                result_Value = this.Query("READ:CHP?");
                result_Value = InsertCommonEscapeSequences(result_Value);
                if (result_Value.Trim() == "") {
                    return null;
                }
                else {
                    try {
                        string[] MODulation_Value = result_Value.Split(new Char[] { ',' });
                        Decimal measureResult = 0;
                        measureResult = Decimal.Parse(MODulation_Value[0], System.Globalization.NumberStyles.Float);
                        return measureResult.ToString();
                    }
                    catch {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalData.myTesting.LogSystem += ex.ToString() + "\r\n";
                return "-999";
            }
        }

        //------------------ Hàm đọc về EVM ---------------------
        public override string config_Instrument_get_EVM(string Trigger, string wifi) {
            try {
                string result_Value = string.Empty;
                this.Write("TRIG:EVM:SOUR " + Trigger + "\n");
                this.Write(":POW:RANG 15.0\n");
                this.Write(":INIT:CONT 0\n");
                result_Value = this.Query("READ:EVM?");
                result_Value = InsertCommonEscapeSequences(result_Value);
                if (result_Value.Trim() == "") {
                    return null;
                }
                else {
                    try {
                        string[] MODulation_Value = result_Value.Split(new Char[] { ',' });
                        Decimal measureResult = 0;
                        measureResult = Decimal.Parse(MODulation_Value[1], System.Globalization.NumberStyles.Float);
                        return measureResult.ToString();
                    }
                    catch {
                        return null;
                    }
                }
            }
            catch {
                return null;
            }
        }


        //------------------ Hàm đọc tất cả các kết quả, rồi mới Split ra từng thông số ---------------------
        public override string config_Instrument_get_TotalResult(string Trigger, string wifi) {
            string result_Value = string.Empty;
            try {
                this.Write("TRIG:EVM:SOUR " + Trigger + "\n");
                Thread.Sleep(50);
                this.Write(":POW:RANG 15.0\n");
                Thread.Sleep(50);
                this.Write(":INIT:CONT 0\n");
                Thread.Sleep(50);

                result_Value = this.Query("READ:EVM?");
                Thread.Sleep(100);
                result_Value = InsertCommonEscapeSequences(result_Value);
                if (result_Value.Trim() == "") {
                    return "NULL";
                }
                else {
                    return result_Value;
                }
            }
            catch {
                return "ERROR";
            }
        }


        //------------------ Hàm đọc cấu hình máy đo phát TX ---------------------
        public override bool config_HT20_RxTest_MAC(string channel, string power, string packet_number, string waveform_file, string port) {
            bool enable_nSISO_Testing = false;
            try {
                string frequency = "";
                frequency = ((int.Parse(channel) * 5) + 2407).ToString();

                //saveLogfile(g_logfilePath, "[BOL] Khởi tạo lần đầu cho thiết bị!\n");
                // Cấu hình khối Source
                this.Write(":SOURce:PRESet" + "\n");
                this.Write(":SOUR:RAD:ARB:LOAD \"D:\\\\Waveform\\\\" + waveform_file + "\"" + "\n");
                this.Write(":SOUR:LIST:NUMB:STEP 3" + "\n");   //Tạo 3 step
                this.Write(":SOUR:LIST:STEP1:SET IMM,0.00000E+00,NONE,DOWN," + frequency + "MHz" + ",-1.2000000E+02,\"CW\",TIME,1.0000E-03,0,1" + "\n");   //Thiết lập thông số cho step1
                this.Write(":SOUR:LIST:STEP1:SET INT,0.00000E+00,NONE,DOWN," + frequency + "MHz" + "," + power + "dBm" + ",\"" + waveform_file + "\",COUN," + packet_number + ",0,1" + "\n");   //Thiết lập thông số cho step2
                this.Write(":SOUR:LIST:STEP3:SET INT,1.00E-03,NONE,DOWN," + frequency + "MHz" + ",-1.2000000E+02,\"CW\",TIME,1.0000E-03,1,1" + "\n");  //Thiết lập thông số cho step3
                this.Write(":SOUR:LIST ON" + "\n");
                this.Write(":SOUR:LIST:TRIG" + "\n");  //Bắt đầu phát waveform

            }
            catch {
                enable_nSISO_Testing = false;
                config_done = true;
                saveLogfile(g_logfilePath, "[E6640A]ERROR CODE: [Equip_Config] \n Error tai qua trinh cau hinh cho thiet bi do E6640A \n");
            }
            return enable_nSISO_Testing;
        }

        protected bool checkBusyState(MessageBasedSession mbSession, string command)
        {
            string OK = "0";
            string Command_Line;
            string responseString = "1";
            int counter = 0;

            if ((command == "*ESE?") || (command == "*ESR?"))
                OK = "0";
            else
                if ((command == "*OPC?") || (command == ":STAT:SRW:MEAS?"))
                OK = "1";

            while ((responseString.Trim() != OK) && (counter < 255))
            {
                Command_Line = ReplaceCommonEscapeSequences(command + "\n");
                responseString = mbSession.Query(Command_Line);
                responseString = responseString.Substring(0, 1);
                Thread.Sleep(25);
                counter++;
            }

            if (counter == 1)
                return false;
            else
                return true;
        }
    }

}
