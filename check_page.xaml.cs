using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cvtest
{
    /// <summary>
    /// check_page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class check_page : Page
    {
        NetworkStream stream = filming_page.clients.GetStream();
        const char REC_INF = (char)0x01;
        const char REC_SEN = (char)0x02;
        const char SEN_INF = (char)0x03;
        const char SEN_SEN = (char)0x04;
        const char MAILINFO = (char)0x05;
        const char PROGRAM_OVER = (char)0x06;
        const char REC_FIN = (char)0x10;
        const string VOID = "";
        string Answer;

        public check_page()
        {
            InitializeComponent();

        }
        class mail_data
        {
            public string send_name { get; set; }
            public string send_phone { get; set; }
            public string send_address1 { get; set; }
            public string send_address2 { get; set; }
            public string recv_name { get; set; }
            public string recv_phone { get; set; }
            public string recv_address1 { get; set; }
            public string recv_address2 { get; set; }


        }

        private void check_Click(object sender, RoutedEventArgs e) //조회 요청 한번에 보내는 자료드 ㄹ자르기 
        {
            string msg = "PARK SEON HU";
            send_REC_INF(msg);
            recvMsgSync();
        }

        public void recvMsgSync()
        {
            int BUFFSIZE = 1024;

            //string msg = "PARK SEON HU";
            //byte[] data = Encoding.UTF8.GetBytes(msg);
            //stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

            StringBuilder ab = new StringBuilder();
            // 여기 while 
            while (true)
            {
                byte[] data = new byte[BUFFSIZE];
                //이방법대로 하면 

                data = null;
                data = new byte[BUFFSIZE];
                int size = stream.Read(data, 0, data.Length); //받는 데이터의 바이트배열, 인덱스, 길이
                if (data[0] == (byte)REC_FIN)
                {
                    break;
                }//responses = Encoding.UTF8.GetString(data, 0, bytes);
                                                              //int size = Socket.Receive(data);
                switch (data[0])
                {
                    case (byte)REC_SEN:
                        {
                            data = data.Skip(1).ToArray();// 프로토콜 제거 
                            ab.Append(Encoding.UTF8.GetString(data, 0, size - 1)); // 
                            Answer = ab.ToString(); //
                            testbox.Text += Answer;//
                            //MessageBox.Show(Answer);
                            //receiverinfo = Answer;
                            //Console.WriteLine(Answer);
                            ab.Clear();
                        }
                        break;
                    case (byte)SEN_SEN:
                        {
                            data = data.Skip(1).ToArray();
                            ab.Append(Encoding.UTF8.GetString(data, 0, size - 1));
                            Answer = ab.ToString();
                            //senderinfo = Answer;
                            //testbox.Text += Answer;

                            ab.Clear();
                        }
                        break;

                    default:
                        return;
                }
            }
        }
        public void send_REC_INF(string RNAME) // 받는사람
        {

            try
            {
                string msg = REC_INF + RNAME;
                byte[] data = Encoding.UTF8.GetBytes(msg);
                stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

                //Socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}
