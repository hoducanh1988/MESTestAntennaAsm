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
using EW12SG.UserDefine;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using EW12SG.Function;
using EW12SG.Base;

namespace EW12SG {
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
                CONTENT = "- Phát hành lần đầu.",
                DATE = "10/06/2020",
                CHANGETYPE = "Lập mới",
                PERSON = "Hồ Đức Anh"
            });
            listHist.Add(new history() {
                ID = "2",
                VERSION = "1.0.0.1",
                CONTENT = "- Thay đổi interval phát beacon từ 100ms => 40ms.\n" +
                          "- Update gửi lệnh mesh từ sleep sang chờ chuỗi root@VNPT:~#.\n" +
                          "- Bỏ thời gian chờ giữa các lần đo antenna.\n" +
                          "- Bỏ off wifi khi chuyển từ 2G sang 5G và ngược lại.\n" +
                          "- Thêm hiển thị, lưu log ssh và log máy đo.",
                DATE = "23/09/2020",
                CHANGETYPE = "Chỉnh sửa",
                PERSON = "Hồ Đức Anh"
            });

            this.GridAbout.ItemsSource = listHist;
        }



    }
}
