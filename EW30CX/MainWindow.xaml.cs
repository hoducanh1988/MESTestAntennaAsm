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
using EW30CX.UserDefine;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using EW30CX.Function;
using EW30CX.Base;
using UtilityPack.IO;
using System.IO;

namespace EW30CX {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        TestWifi testWifi = new TestWifi();
        bool _isScroll = false;
        DispatcherTimer timer = null;

        public MainWindow() {
            //init control
            InitializeComponent();

            //add history
            add_history();

            //set itemsource for combobox
            set_setting_combobox_itemsource();

            //load setting
            if (File.Exists(GlobalData.settingFileFullName)) GlobalData.mySetting = XmlHelper<SettingInformation>.FromXmlFile(GlobalData.settingFileFullName);

            //binding data
            this.tbi_setting.DataContext = GlobalData.mySetting;
            this.tbi_testall.DataContext = GlobalData.myTesting;
            this.DataContext = GlobalData.myApp;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (sender, e) => {
                if (_isScroll == true) {
                    Scr_LogSystem.ScrollToEnd();
                    Scr_LogSsh.ScrollToEnd();
                    Scr_LogInstrument.ScrollToEnd();
                }
            };
            timer.Start();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            Label lbl = sender as Label;
            switch (lbl.Content.ToString()) {
                case "X": {
                        this.Close();
                        break;
                    }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

    }
}
