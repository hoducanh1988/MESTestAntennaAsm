using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using EW30SX.UserDefine;
using System.Net;
using System.Net.NetworkInformation;
using EW30SX.Function;
using System.Text.RegularExpressions;

namespace EW30SX.Base {

    public class BaseFunction {

        public static bool WriteLineSSH(string cmd) {
            try {
                GlobalData.SSHConn.WriteLine(cmd);
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public static string getMAC() {
            try {
                //-----------c2--------------------------------
                string data = "";
                int count = 0;
            RE:
                count++;
                bool r = GlobalData.SSHConn.Query("cat /sys/class/net/eth0/address", "root@VNPT:~#", 3, true, out data);//
               if (r == false) {
                    if (count < 3) goto RE;
                    else return "";
                }

                string[] buffer = data.Split(new string[] { "cat /sys/class/net/eth0/address" }, StringSplitOptions.None);
                string mac = buffer.Length == 2 ? buffer[1] : buffer[2];
                mac = mac.Split(new string[] { "root@VNPT:~#" }, StringSplitOptions.None)[0].Trim();
                mac = mac.Replace(":", "").Replace("-", "").Replace("\n", "").Replace("\r", "").Trim();
                return mac;
            }
            catch {
                return "";
            }
        }


        public static bool pingToIPAddress(string ip) {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted. 
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 60;

            try {
                PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                if (reply.Status == IPStatus.Success) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch {
                return false;
            }
        }
    }
}
