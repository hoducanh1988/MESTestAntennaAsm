using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW12CG.UserDefine {

    public class SettingInformation : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public SettingInformation() {
            Station = "1";
            JIG = "1";

            IPDUT = "192.168.88.1";
            Username = "root";
            Password = "vnpt";

            Measurement_Name = "";
            Measurement_Addr = "";
            Measurement_Port = "";

            Measurement_PowerLevel2G = -20;
            Measurement_TriggerLevel2G = -20;
            Freq_Channel2G = "2437";

            Measurement_PowerLevel5G = -20;
            Measurement_TriggerLevel5G = -20;
            Freq_Channel5G = "5180";

            Power_Max_2G_Chain1 = 20;
            Power_Min_2G_Chain1 = -25;
            Power_Max_2G_Chain2 = 20;
            Power_Min_2G_Chain2 = -25;

            Power_Min_5G_Chain1 = -35;
             Power_Max_5G_Chain1 = 20;
            Power_Min_5G_Chain2 = -35;
            Power_Max_5G_Chain2 = 20;

            SleepTime = 5;
            RetryTime = 3;
        }


        string _station;
        public string Station {
            get { return _station; }
            set {
                _station = value;
                OnPropertyChanged(nameof(Station));
            }
        }
        string _jignumber;
        public string JIG {
            get { return _jignumber; }
            set {
                _jignumber = value;
                OnPropertyChanged(nameof(JIG));
            }
        }
        string _ip_dut;
        public string IPDUT {
            get { return _ip_dut; }
            set {
                _ip_dut = value;
                OnPropertyChanged(nameof(IPDUT));
            }
        }
        string _ssh_user;
        public string Username {
            get { return _ssh_user; }
            set {
                _ssh_user = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        string _ssh_pass;
        public string Password {
            get { return _ssh_pass; }
            set {
                _ssh_pass = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        string _measurement_name;
        public string Measurement_Name {
            get { return _measurement_name; }
            set {
                _measurement_name = value;
                OnPropertyChanged(nameof(Measurement_Name));
                switch (value) {
                    case "E6640A": {
                            Measurement_PowerLevel2G = 0;
                            Measurement_PowerLevel5G = 0;
                            break;
                        }
                    case "MT8870A": {
                            Measurement_PowerLevel2G = -15;
                            Measurement_PowerLevel5G = -20;
                            break;
                        }

                }
            }
        }
        string _measurement_addr;
        public string Measurement_Addr {
            get { return _measurement_addr; }
            set {
                _measurement_addr = value;
                OnPropertyChanged(nameof(Measurement_Addr));
            }
        }
        string _measurement_port;
        public string Measurement_Port {
            get { return _measurement_port; }
            set {
                _measurement_port = value;
                OnPropertyChanged(nameof(Measurement_Port));
            }
        }


        int _powerlevel2g;
        public int Measurement_PowerLevel2G {
            get { return _powerlevel2g; }
            set {
                _powerlevel2g = value;
                OnPropertyChanged(nameof(Measurement_PowerLevel2G));
            }
        }
        int _triggerlevel2g;
        public int Measurement_TriggerLevel2G {
            get { return _triggerlevel2g; }
            set {
                _triggerlevel2g = value;
                OnPropertyChanged(nameof(Measurement_TriggerLevel2G));
            }
        }
        string _channel2g;
        public string Freq_Channel2G {
            get { return _channel2g; }
            set {
                _channel2g = value;
                OnPropertyChanged(nameof(Freq_Channel2G));
            }
        }


        int _powerlevel5g;
        public int Measurement_PowerLevel5G {
            get { return _powerlevel5g; }
            set {
                _powerlevel5g = value;
                OnPropertyChanged(nameof(Measurement_PowerLevel5G));
            }
        }
        int _triggerlevel5g;
        public int Measurement_TriggerLevel5G {
            get { return _triggerlevel5g; }
            set {
                _triggerlevel5g = value;
                OnPropertyChanged(nameof(Measurement_TriggerLevel5G));
            }
        }
        string _channel5g;
        public string Freq_Channel5G {
            get { return _channel5g; }
            set {
                _channel5g = value;
                OnPropertyChanged(nameof(Freq_Channel5G));
            }
        }


        double _lowerpower2g_chain1;
        public double Power_Min_2G_Chain1 {
            get { return _lowerpower2g_chain1; }
            set {
                _lowerpower2g_chain1 = value;
                GlobalData.myTesting.Power_Min_2G_Chain1 = value;
                OnPropertyChanged(nameof(Power_Min_2G_Chain1));
            }
        }
        double _upperpower2g_chain1;
        public double Power_Max_2G_Chain1 {
            get { return _upperpower2g_chain1; }
            set {
                _upperpower2g_chain1 = value;
                GlobalData.myTesting.Power_Max_2G_Chain1 = value;
                OnPropertyChanged(nameof(Power_Max_2G_Chain1));
            }
        }
        double _lowerpower2g_chain2;
        public double Power_Min_2G_Chain2 {
            get { return _lowerpower2g_chain2; }
            set {
                _lowerpower2g_chain2 = value;
                GlobalData.myTesting.Power_Min_2G_Chain2 = value;
                OnPropertyChanged(nameof(Power_Min_2G_Chain2));
            }
        }
        double _upperpower2g_chain2;
        public double Power_Max_2G_Chain2 {
            get { return _upperpower2g_chain2; }
            set {
                _upperpower2g_chain2 = value;
                GlobalData.myTesting.Power_Max_2G_Chain2 = value;
                OnPropertyChanged(nameof(Power_Max_2G_Chain2));
            }
        }

        double _lowerpower5g_chain1;
        public double Power_Min_5G_Chain1 {
            get { return _lowerpower5g_chain1; }
            set {
                _lowerpower5g_chain1 = value;
                GlobalData.myTesting.Power_Min_5G_Chain1 = value;
                OnPropertyChanged(nameof(Power_Min_5G_Chain1));
            }
        }
        double _upperpower5g_chain1;
        public double Power_Max_5G_Chain1 {
            get { return _upperpower5g_chain1; }
            set {
                _upperpower5g_chain1 = value;
                GlobalData.myTesting.Power_Max_5G_Chain1 = value;
                OnPropertyChanged(nameof(Power_Max_5G_Chain1));
            }
        }
        double _lowerpower5g_chain2;
        public double Power_Min_5G_Chain2 {
            get { return _lowerpower5g_chain2; }
            set {
                _lowerpower5g_chain2 = value;
                GlobalData.myTesting.Power_Min_5G_Chain2 = value;
                OnPropertyChanged(nameof(Power_Min_5G_Chain2));
            }
        }
        double _upperpower5g_chain2;
        public double Power_Max_5G_Chain2 {
            get { return _upperpower5g_chain2; }
            set {
                _upperpower5g_chain2 = value;
                GlobalData.myTesting.Power_Max_5G_Chain2 = value;
                OnPropertyChanged(nameof(Power_Max_5G_Chain2));
            }
        }





        int _sleep_time;
        public int SleepTime {
            get { return _sleep_time; }
            set {
                _sleep_time = value;
                OnPropertyChanged(nameof(SleepTime));
            }
        }
        int _retry_time;
        public int RetryTime {
            get { return _retry_time; }
            set {
                _retry_time = value;
                OnPropertyChanged(nameof(RetryTime));
            }
        }
    }
}
