﻿using System;
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
using EW12S.UserDefine;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using EW12S.Function;
using EW12S.Base;


namespace EW12S {
    public partial class MainWindow : Window {

        private void set_setting_combobox_itemsource() {
            cbStation.ItemsSource = initParameter.listStation;
            cbJIG.ItemsSource = initParameter.listJIG;
            cbMeasurementName.ItemsSource = initParameter.listMeasurement_Name;
            cbMeasurementPort.ItemsSource = initParameter.listMeasurement_Port;
            cbMeasurementTriggeLevel2G.ItemsSource = cbMeasurementTriggeLevel5G.ItemsSource = initParameter.listMeasurement_PowerTrigger;
            cbMeasurementPowerTrigger2G.ItemsSource = cbMeasurementPowerTrigger5G.ItemsSource = initParameter.listMeasurement_PowerTrigger;
            cbFreq5G.ItemsSource = initParameter.listFreq5G;
            cbFreq2G.ItemsSource = initParameter.listFreq2G;
            cbb_logtype.ItemsSource = new List<string>() { "LogDetail", "LogSsh", "LogInstrument", "LogTotal" };
        }


    }
}
