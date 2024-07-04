using System;
using System.Collections.Generic;
using System.Data;
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
        //NetworkStream stream = filming_page.clients.GetStream();
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



        }
        class mail_recv_data
        {
            public string recv_name { get; set; }
            public string recv_phone { get; set; }
            public string recv_address1 { get; set; }
            public string recv_address2 { get; set; }
        }
        List<mail_data> mail_Datas = new List<mail_data>();

        private void check_Click(object sender, RoutedEventArgs e) //조회 요청 한번에 보내는 자료드 ㄹ자르기 
        {
            string msg = "PARK SEON HU";
            //send_REC_INF(msg);
            //recvMsgSync();
        }

        //public void recvMsgSync()
        //{
        //    int BUFFSIZE = 1024;

        //    //string msg = "PARK SEON HU";
        //    //byte[] data = Encoding.UTF8.GetBytes(msg);
        //    //stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

        //    StringBuilder ab = new StringBuilder();

        //    while (true)
        //    {
        //        byte[] data = new byte[BUFFSIZE];
        //        data = null;
        //        data = new byte[BUFFSIZE];
        //        int size = stream.Read(data, 0, data.Length); //받는 데이터의 바이트배열, 인덱스, 길이
        //        if (data[0] == (byte)REC_FIN)
        //        {
        //            break;
        //        }

        //        switch (data[0])
        //        {
        //            case (byte)REC_SEN:
        //                {
        //                    data = data.Skip(1).ToArray();// 프로토콜 제거 
        //                    ab.Append(Encoding.UTF8.GetString(data, 0, size - 1)); // 
        //                    Answer = ab.ToString(); //
        //                    testbox.Text += Answer;//
        //                    //MessageBox.Show(Answer);
        //                    //receiverinfo = Answer;
        //                    //Console.WriteLine(Answer);
        //                    ab.Clear();
        //                }
        //                break;
        //            case (byte)SEN_SEN:
        //                {
        //                    data = data.Skip(1).ToArray();
        //                    ab.Append(Encoding.UTF8.GetString(data, 0, size - 1));
        //                    Answer = ab.ToString();
        //                    //senderinfo = Answer;
        //                    //testbox.Text += Answer;

        //                    ab.Clear();
        //                }
        //                break;

        //            default:
        //                return;

        //        }
        //        data_view();

        //    }
        //}
        //public void send_REC_INF(string RNAME) // 받는사람 데이터 달라고 요청하기 
        //{

        //    try
        //    {
        //        string msg = REC_INF + RNAME;
        //        byte[] data = Encoding.UTF8.GetBytes(msg);
        //        stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}
        //public void data_view()// 나 칭찬해 아주 대견해
        //{
        //    string[] spliteddatas = Answer.Split('\n');// 데이터 하나씩 잘림

        //    List<string[]> data = new List<string[]>();// data
        //    for (int i = 0; i < spliteddatas.Count()-1; i++)
        //    {
        //        string[] strings;
        //        strings = spliteddatas[i].Split('!'); // data[i]에  한명의 데이터 들어감 
        //        data.Add(strings);

        //    }

        //    for (int i = 0; i < data.Count ; i++)
        //    {
        //        mail_data mail_Data = new mail_data();

        //        mail_Data.send_name = data[i][0];
        //        MessageBox.Show(data[i][0]);

        //        mail_Data.send_phone = data[i][1]; // 
        //        MessageBox.Show(data[i][1]);

        //        mail_Data.send_address1 = data[i][2];
        //        MessageBox.Show(data[i][2]);

        //        mail_Data.send_address2 = data[i][3];
        //        MessageBox.Show(data[i][3]);

        //        //mail_Data.recv_name = data[i][4];
        //        //MessageBox.Show(data[i][4]);

        //        //mail_Data.recv_phone = data[i][5];
        //        //MessageBox.Show(data[i][5]);

        //        //mail_Data.recv_address1 = data[i][6];
        //        //MessageBox.Show(data[i][6]);

        //        //mail_Data.recv_address2 = data[i][7];
        //        //MessageBox.Show(data[i][7]);

        //        mail_Datas.Add(mail_Data);

        //        //gg.Text += data[i];
        //    }

        //    datauser.ItemsSource = mail_Datas;
        //    datauser.Items.Refresh();
        //    Answer = "";
        //    // datas에 한줄씩 들어감 이거 다시한번 짤라야함 
        //    //string[] spliteddatas1 = spliteddatas


        //    //for (int i = 0; i < spliteddatas.Length; i++)
        //    //{
        //    //    gg.Text += spliteddatas[i] +"\n";
        //    //}

        //    //datauser.ItemsSource = spliteddatas;
        //}


    }
}
