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

namespace EW30CX {
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
                VERSION = "EW30CXVN0U0001",
                CONTENT = "- Xây dựng tool test antenna sản phẩm mesh EW30CX.",
                DATE = "28/09/2021",
                CHANGETYPE = "Tạo mới",
                PERSON = "Hồ Đức Anh"
            });
           

            this.GridAbout.ItemsSource = listHist;
        }

      

    }
}
