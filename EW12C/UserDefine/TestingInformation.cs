using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW12C.UserDefine {

    public class TestingInformation : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public TestingInformation() {
            initParam();
        }

        public void initParam() {
            MAC_Address = "";

            loadFilePathLoss = "-";
            pingToDut = "-";
            loginSSH = "-";
            getMacEthernet = "-";
            InitInstrument = "-";
            offWifi2G = "-";
            offWifi5G = "-";
            

            Result2G_Anten1 = "-";
            Result2G_Anten2 = "-";
            Result5G_Anten1 = "-";
            Result5G_Anten2 = "-";
            Result_Total = "-";

            LogSystem = "";
            LogSsh = "";
            LogInstrument = "";
            Power2G_Anten1 = "";
            Power2G_Anten2 = "";
            Power5G_Anten1 = "";
            Power5G_Anten2 = "";
            ButtonEnable = true;

            totalTime = "00:00:00";
        }

        public void waitParam() {
            Result_Total = "Waiting";
            ButtonEnable = false;
        }

        public bool passParam() {
            Result_Total = "Passed";
            ButtonEnable = true;
            return true;
        }

        public bool failParam() {
            Result_Total = "Failed";
            ButtonEnable = true;
            return true;
        }

        string _total_time;
        public string totalTime {
            get { return _total_time; }
            set {
                _total_time = value;
                OnPropertyChanged(nameof(totalTime));
            }
        }

        string _load_file_pathloss;
        public string loadFilePathLoss {
            get { return _load_file_pathloss; }
            set {
                _load_file_pathloss = value;
                OnPropertyChanged(nameof(loadFilePathLoss));
            }
        }
        string _ping_to_dut;
        public string pingToDut {
            get { return _ping_to_dut; }
            set {
                _ping_to_dut = value;
                OnPropertyChanged(nameof(pingToDut));
            }
        }
        string _login_ssh;
        public string loginSSH {
            get { return _login_ssh; }
            set {
                _login_ssh = value;
                OnPropertyChanged(nameof(loginSSH));
            }
        }
        string _init_instrument;
        public string InitInstrument {
            get { return _init_instrument; }
            set {
                _init_instrument = value;
                OnPropertyChanged(nameof(InitInstrument));
            }
        }
        string _get_mac_ethernet;
        public string getMacEthernet {
            get { return _get_mac_ethernet; }
            set {
                _get_mac_ethernet = value;
                OnPropertyChanged(nameof(getMacEthernet));
            }
        }
        string _off_wifi_2g;
        public string offWifi2G {
            get { return _off_wifi_2g; }
            set {
                _off_wifi_2g = value;
                OnPropertyChanged(nameof(offWifi2G));
            }
        }
        string _off_wifi_5g;
        public string offWifi5G {
            get { return _off_wifi_5g; }
            set {
                _off_wifi_5g = value;
                OnPropertyChanged(nameof(offWifi5G));
            }
        }
        bool _button_enable;
        public bool ButtonEnable {
            get { return _button_enable; }
            set {
                _button_enable = value;
                OnPropertyChanged(nameof(ButtonEnable));
            }
        }
        string _mac_address;
        public string MAC_Address {
            get { return _mac_address; }
            set {
                _mac_address = value;
                OnPropertyChanged(nameof(MAC_Address));
            }
        }
        string _result_2g_anten1;
        public string Result2G_Anten1 {
            get { return _result_2g_anten1; }
            set {
                _result_2g_anten1 = value;
                OnPropertyChanged(nameof(Result2G_Anten1));
            }
        }
        string _result_2g_anten2;
        public string Result2G_Anten2 {
            get { return _result_2g_anten2; }
            set {
                _result_2g_anten2 = value;
                OnPropertyChanged(nameof(Result2G_Anten2));
            }
        }
        string _result_5g_anten1;
        public string Result5G_Anten1 {
            get { return _result_5g_anten1; }
            set {
                _result_5g_anten1 = value;
                OnPropertyChanged(nameof(Result5G_Anten1));
            }
        }
        string _result_5g_anten2;
        public string Result5G_Anten2 {
            get { return _result_5g_anten2; }
            set {
                _result_5g_anten2 = value;
                OnPropertyChanged(nameof(Result5G_Anten2));
            }
        }


        string _power_2g_anten1;
        public string Power2G_Anten1 {
            get { return _power_2g_anten1; }
            set {
                _power_2g_anten1 = value;
                OnPropertyChanged(nameof(Power2G_Anten1));
            }
        }
        string _power_2g_anten2;
        public string Power2G_Anten2 {
            get { return _power_2g_anten2; }
            set {
                _power_2g_anten2 = value;
                OnPropertyChanged(nameof(Power2G_Anten2));
            }
        }
        string _power_5g_anten1;
        public string Power5G_Anten1 {
            get { return _power_5g_anten1; }
            set {
                _power_5g_anten1 = value;
                OnPropertyChanged(nameof(Power5G_Anten1));
            }
        }
        string _power_5g_anten2;
        public string Power5G_Anten2 {
            get { return _power_5g_anten2; }
            set {
                _power_5g_anten2 = value;
                OnPropertyChanged(nameof(Power5G_Anten2));
            }
        }

        string _result_total;
        public string Result_Total {
            get { return _result_total; }
            set {
                _result_total = value;
                OnPropertyChanged(nameof(Result_Total));
            }
        }
        string _log_system;
        public string LogSystem {
            get { return _log_system; }
            set {
                _log_system = value;
                OnPropertyChanged(nameof(LogSystem));
            }
        }
        string _log_ssh;
        public string LogSsh {
            get { return _log_ssh; }
            set {
                _log_ssh = value;
                OnPropertyChanged(nameof(LogSsh));
            }
        }
        string _log_instrument;
        public string LogInstrument {
            get { return _log_instrument; }
            set {
                _log_instrument = value;
                OnPropertyChanged(nameof(LogInstrument));
            }
        }

        #region limit

        double _lowerpower2g_chain1;
        public double Power_Min_2G_Chain1 {
            get { return _lowerpower2g_chain1; }
            set {
                _lowerpower2g_chain1 = value;
                OnPropertyChanged(nameof(Power_Min_2G_Chain1));
            }
        }
        double _upperpower2g_chain1;
        public double Power_Max_2G_Chain1 {
            get { return _upperpower2g_chain1; }
            set {
                _upperpower2g_chain1 = value;
                OnPropertyChanged(nameof(Power_Max_2G_Chain1));
            }
        }
        double _lowerpower2g_chain2;
        public double Power_Min_2G_Chain2 {
            get { return _lowerpower2g_chain2; }
            set {
                _lowerpower2g_chain2 = value;
                OnPropertyChanged(nameof(Power_Min_2G_Chain2));
            }
        }
        double _upperpower2g_chain2;
        public double Power_Max_2G_Chain2 {
            get { return _upperpower2g_chain2; }
            set {
                _upperpower2g_chain2 = value;
                OnPropertyChanged(nameof(Power_Max_2G_Chain2));
            }
        }

        double _lowerpower5g_chain1;
        public double Power_Min_5G_Chain1 {
            get { return _lowerpower5g_chain1; }
            set {
                _lowerpower5g_chain1 = value;
                OnPropertyChanged(nameof(Power_Min_5G_Chain1));
            }
        }
        double _upperpower5g_chain1;
        public double Power_Max_5G_Chain1 {
            get { return _upperpower5g_chain1; }
            set {
                _upperpower5g_chain1 = value;
                OnPropertyChanged(nameof(Power_Max_5G_Chain1));
            }
        }
        double _lowerpower5g_chain2;
        public double Power_Min_5G_Chain2 {
            get { return _lowerpower5g_chain2; }
            set {
                _lowerpower5g_chain2 = value;
                OnPropertyChanged(nameof(Power_Min_5G_Chain2));
            }
        }
        double _upperpower5g_chain2;
        public double Power_Max_5G_Chain2 {
            get { return _upperpower5g_chain2; }
            set {
                _upperpower5g_chain2 = value;
                OnPropertyChanged(nameof(Power_Max_5G_Chain2));
            }
        }

        #endregion


    }
}
