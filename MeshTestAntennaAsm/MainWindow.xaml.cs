﻿using MeshTestAntennaAsm.Custom;
using MeshTestAntennaAsm.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using UtilityPack.IO;

namespace MeshTestAntennaAsm {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        string dir = AppDomain.CurrentDomain.BaseDirectory;

        public MainWindow() {
            InitializeComponent();

            //load setting from file
            if (File.Exists(myGlobal.settingFileFullName)) myGlobal.mySetting = XmlHelper<SettingInformation>.FromXmlFile(myGlobal.settingFileFullName);

            //set itemsource for combobox
            this.cbbModel.ItemsSource = new List<string>() { "EW12S", "EW12CG", "EW12SG", "EW12C", "EW30SX", "EW30CX" };

            //binding data
            this.DataContext = myGlobal.mySetting;

            //save pathloss path
            save_PathLossPath();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            string model = this.cbbModel.Text;
            string app_test = string.Format("{0}{1}\\{1}.exe", dir, model);

            switch (model) {
                case "EW30CX":
                case "EW30SX":
                case "EW12S":
                case "EW12C":
                case "EW12SG":
                case "EW12CG": {
                        //check đường dẫn log folder
                        if (!UtilityPack.Validation.Parse.isLogPathValid(66)) {
                            MessageBox.Show("Đường dẫn folder phần mềm quá dài hoặc có kí tự không hợp lệ.\r\nVui lòng copy folder phần mềm sang đường dẫn khác.", "Lỗi đường dẫn", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        Process.Start(app_test);
                        XmlHelper<SettingInformation>.ToXmlFile(myGlobal.mySetting, myGlobal.settingFileFullName); //save setting to xml file
                        this.Close();
                        break;
                    }
                default: {
                        MessageBox.Show("Vui lòng chọn model sản phẩm.", "Lỗi chọn model sản phẩm", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
            }
        }

        private void save_PathLossPath() {
            string f_path = string.Format(@"{0}Pathloss.dll", AppDomain.CurrentDomain.BaseDirectory);
            string f_content = string.Format(@"{0}DoSuyHaoXML\PathLoss_TestAntenna_20200424_0937.xml", AppDomain.CurrentDomain.BaseDirectory);
            File.WriteAllText(f_path, f_content);
        }



    }
}
