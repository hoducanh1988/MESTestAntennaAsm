using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;
using System.Threading;
using System.IO;

namespace EW12CG.Protocol {
    public class SSH {
        private ShellStream shellStreamSSH;
        private SshClient sshClient;


        public bool Login(string IPAddress, string Username, string Pass) {
            //Control.CheckForIllegalCrossThreadCalls = false;
            try {
                this.sshClient = new SshClient(IPAddress, 22, Username, Pass);
                //Thực hiện kết nối
                this.sshClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10);
                this.sshClient.Connect();
                /// tạo shell stream để điều khiển command ssh
                this.shellStreamSSH = this.sshClient.CreateShellStream("vt100", 80, 60, 800, 600, 65536);
                return true;
            }
            catch (Exception d) {
                GlobalData.myTesting.LogSystem += d + "\r\n";
                return false;
            }
        }

        public void CloseConnect() {
            try {
                this.sshClient.Disconnect();
            }
            catch { }
        }

        public void WriteLine(string cmd) {
            this.shellStreamSSH.Write(cmd + "\n");
            this.shellStreamSSH.Flush();
        }

        public bool Query (string cmd, string pattern, int timeout_sec, bool isEnd) {
            this.WriteLine(cmd);
            int delay_ms = 50;
            int max_count = (timeout_sec * 1000) / delay_ms;
            bool r = false;
            int count = 0;
            string data = "";
        RE:
            count++;
            data += this.Read();
            r = isEnd ? data.ToLower().Trim().EndsWith(pattern.ToLower()) : data.ToLower().Trim().Contains(pattern.ToLower());
            if (r == false) {
                if (count < max_count) {
                    Thread.Sleep(delay_ms);
                    goto RE;
                }
            }

            return r;
        }

        public bool Query(string cmd, string pattern, int timeout_sec, bool isEnd, out string data) {
            this.WriteLine(cmd);
            int delay_ms = 50;
            int max_count = (timeout_sec * 1000) / delay_ms;
            bool r = false;
            int count = 0;
            data = "";
        RE:
            count++;
            data += this.Read();
            r = isEnd ? data.ToLower().Trim().EndsWith(pattern.ToLower()) : data.ToLower().Trim().Contains(pattern.ToLower());
            if (r == false) {
                if (count < max_count) {
                    Thread.Sleep(delay_ms);
                    goto RE;
                }
            }

            return r;
        }


        public string Read() {
            string value = "NULL";
            try {
                value = shellStreamSSH.Read();
                GlobalData.myTesting.LogSsh += value;
                return value;
            }
            catch { return value; }
        }
    }
}
