using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.UserDefine {
    public class AppInfo : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public AppInfo() {
            appInfo = "Version: EW30SXVN0U0001 - Build time: 28/09/2021 15:30";
        }

        string _app_info;
        public string appInfo {
            get { return _app_info; }
            set {
                _app_info = value;
                OnPropertyChanged(nameof(appInfo));
            }
        }

    }
}
