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
using EW12C.UserDefine;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using EW12C.Function;
using EW12C.Base;

namespace EW12C {
    public partial class MainWindow : Window {

        private class history {
            public string ID { get; set; }
            public string VERSION { get; set; }
            public string CONTENT { get; set; }
            public string DATE { get; set; }
            public string CHANGETYPE { get; set; }
            public string PERSON { get; set; }
        }
        List<history> listHist = new List<history>();

        private void add_history() {
            //history
            listHist.Add(new history() {
                ID = "1",
                VERSION = "1.0.0.0",
                CONTENT = "- Xây dựng tool test antenna sản phẩm EW12C dựa trên sản phẩm EW12S.",
                DATE = "28/09/2020",
                CHANGETYPE = "Lập mới",
                PERSON = "Hồ Đức Anh"
            });

            this.GridAbout.ItemsSource = listHist;
        }

      

    }
}
