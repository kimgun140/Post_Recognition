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
        NetworkStream stream = filming_page.clients.GetStream();
        const char REC_INF = (char)0x01; // 받은 편지 목록
        const char REC_SEN = (char)0x02; // 위에꺼 보내달라고 이름 보내기
        const char SEN_INF = (char)0x03; // 보낸 편지 목록
        const char SEN_SEN = (char)0x04; // 보낸 편지 목록 요청 
        const char MAILINFO = (char)0x05;
        const char PROGRAM_OVER = (char)0x06;
        const char REC_FIN = (char)0x10; // 정보 다 끝났다고 시그널 
        //const string VOID = "";
        string Answer;

        public check_page()
        {
            InitializeComponent();

        }
        class mail_data // 보낸 사람 정보
        {
            public string send_name { get; set; }
            public string send_phone { get; set; }
            public string send_address1 { get; set; }
            public string send_address2 { get; set; }
            //public string recv_name { get; set; }
            //public string recv_phone { get; set; }
            //public string recv_address1 { get; set; }
            //public string recv_address2 { get; set; }
        }
        class mail_recv_data // 받는 사람 정보
        {
            public string recv_name { get; set; }
            public string recv_phone { get; set; }
            public string recv_address1 { get; set; }
            public string recv_address2 { get; set; }

        }
        List<mail_data> mail_Datas = new List<mail_data>();
        List<mail_recv_data> mail_recv_Datas = new List<mail_recv_data>();


        private void check_Click(object sender, RoutedEventArgs e) //받는 사람 버튼  
        {
            string msg = recvname.Text;
            send_REC_INF(msg);// msg가 받는 사람
            recvMsgSync();
            //data_view(Answer);
            //data_view1();


        }

        private void check2_Click(object sender, RoutedEventArgs e) // 보낸 사람 버튼  
        {
            string name = sendanme.Text;
            //send_REC_INF(name);
            send_SEN_INF(name);
            recvMsgSync();
            //data_view1(Answer);

        }

        async public void recvMsgSync() // 오는 데이터 answer에 다 넣기 
        {
            int BUFFSIZE = 1024;

            //string msg = "PARK SEON HU";
            //byte[] data = Encoding.UTF8.GetBytes(msg);
            //stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

            StringBuilder ab = new StringBuilder();

            //await Task.Run
            while (true)
            {
                byte[] data = new byte[BUFFSIZE];
                data = null;
                data = new byte[BUFFSIZE];
                //await Dispatcher.BeginInvoke(new Action(() => { }));

                int size = await stream.ReadAsync(data, 0, data.Length); //받는 데이터의 바이트배열, 인덱스, 길이
                //testbox.Text += size;
                //testbox.Text += data[0] + "\n";
                //testbox.Text += data[1] + "\n";

                if (data[0] == (byte)REC_FIN) // 마지막에 오는 종료 시그널 
                {
                    break;
                }

                switch (data[0])
                {
                    //case (byte)REC_FIN:
                    //    {
                    //        MessageBox.Show("끝났습니다");
                    //        break;

                    //    }


                    case (byte)REC_SEN://이름이 보낸 목록 
                        {

                            //testbox.Text += "size";

                            data = data.Skip(1).ToArray();// 프로토콜 제거 
                            ab.Append(Encoding.UTF8.GetString(data, 0, size - 1)); // 
                            Answer = ab.ToString(); //
                            //testbox.Text += Answer;//
                            //MessageBox.Show(Answer);
                            //receiverinfo = Answer;
                            //Console.WriteLine(Answer);
                            ab.Clear();
                            //data_view(Answer);

                        }
                        break;
                    case (byte)SEN_SEN:// 받는 사람이 이름인 목록 
                        {
                            data = data.Skip(1).ToArray();
                            ab.Append(Encoding.UTF8.GetString(data, 0, size - 1));
                            Answer = ab.ToString();
                            //senderinfo = Answer;
                            //testbox.Text += Answer;

                            ab.Clear();
                            //data_view1(Answer);

                        }
                        break;

                    default:
                        return;

                }

                data_view(Answer);
                //break;

            }
        }
        public void send_REC_INF(string RNAME) //  이름 보내고 받는 사람이 이름인 정보 
        {

            try
            {
                string msg = REC_INF + RNAME;
                byte[] data = Encoding.UTF8.GetBytes(msg);
                stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void send_SEN_INF(string SNAME) //  이름 보내고  보내는 사람이 이름인 정보
        {

            try
            {
                string msg = SEN_INF + SNAME;
                byte[] data = Encoding.UTF8.GetBytes(msg);
                stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

                //Socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public void data_view(string Answer1)// 받는사람 버튼
        {
            //ItemsControl.ItemsSource.
            //int cnt = send_user_data.SelectedItems.Count;

            //for (int i = cnt - 1; i >= 0; i--) { send_user_data.Items.Remove(send_user_data.SelectedItems[i]); }
            mail_Datas.Clear();

            //send_user_data.Items.Remove(1); // 기존 데이터 지우기 
            string[] spliteddatas = Answer1.Split('\n');// 데이터 하나씩 잘림

            List<string[]> data = new List<string[]>();// data
            for (int i = 0; i < spliteddatas.Count() - 1; i++)
            {
                string[] strings;
                strings = spliteddatas[i].Split('!'); // data[i]에  한명의 데이터 들어감 
                data.Add(strings);

            }

            for (int i = 0; i < data.Count; i++)
            {
                mail_data mail_Data = new mail_data();

                mail_Data.send_name = data[i][0];
                //MessageBox.Show(data[i][0]);

                mail_Data.send_phone = data[i][1]; // 
                //MessageBox.Show(data[i][1]);

                mail_Data.send_address1 = data[i][2];
                //MessageBox.Show(data[i][2]);

                mail_Data.send_address2 = data[i][3];
                //MessageBox.Show(data[i][3]);

                mail_Datas.Add(mail_Data);

                //gg.Text += data[i];
            }

            send_user_data.ItemsSource = mail_Datas;
            send_user_data.Items.Refresh();
            Answer = "";
            // datas에 한줄씩 들어감 이거 다시한번 짤라야함 
            //string[] spliteddatas1 = spliteddatas


            //for (int i = 0; i < spliteddatas.Length; i++)
            //{
            //    gg.Text += spliteddatas[i] +"\n";
            //}

            //datauser.ItemsSource = spliteddatas;
        }




        //public void data_view1(string Answer1)// 보낸사람  업데이트 
        //{
        //    //recv_user_data.Items.Clear();
        //    string[] spliteddatas = Answer1.Split('\n');// 데이터 하나씩 잘림

        //    List<string[]> data = new List<string[]>();// data
        //    for (int i = 0; i < spliteddatas.Count() - 1; i++)
        //    {
        //        string[] strings;
        //        strings = spliteddatas[i].Split('!'); // data[i]에  한명의 데이터 들어감 
        //        data.Add(strings);

        //    }

        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        //mail_data mail_Data = new mail_data();
        //        mail_recv_data mail_Recv_Data = new mail_recv_data();
        //        //mail_Data.send_name = data[i][0];
        //        //MessageBox.Show(data[i][0]);

        //        //mail_Data.send_phone = data[i][1]; // 
        //        //MessageBox.Show(data[i][1]);

        //        //mail_Data.send_address1 = data[i][2];
        //        //MessageBox.Show(data[i][2]);

        //        //mail_Data.send_address2 = data[i][3];
        //        //MessageBox.Show(data[i][3]);

        //        mail_Recv_Data.recv_name = data[i][0];
        //        //MessageBox.Show(data[i][4]);

        //        mail_Recv_Data.recv_phone = data[i][1];
        //        //MessageBox.Show(data[i][5]);

        //        mail_Recv_Data.recv_address1 = data[i][2];
        //        //MessageBox.Show(data[i][6]);

        //        mail_Recv_Data.recv_address2 = data[i][3];
        //        //MessageBox.Show(data[i][7]);

        //        //mail_Datas.Add(mail_Data);
        //        mail_recv_Datas.Add(mail_Recv_Data);
        //        //gg.Text += data[i];
        //        // 리스트뷰 하나 더
        //    }

        //    recv_user_data.ItemsSource = mail_recv_Datas;
        //    recv_user_data.Items.Refresh();
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








//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;

//namespace cvtest
//{
//    public partial class check_page : Page
//    {
//        NetworkStream stream = filming_page.clients.GetStream();
//        const char REC_INF = (char)0x01;
//        const char REC_SEN = (char)0x02;
//        const char SEN_INF = (char)0x03;
//        const char SEN_SEN = (char)0x04;
//        const char MAILINFO = (char)0x05;
//        const char PROGRAM_OVER = (char)0x06;
//        const char REC_FIN = (char)0x10;
//        string Answer;

//        public check_page()
//        {
//            InitializeComponent();
//        }

//        class mail_data
//        {
//            public string send_name { get; set; }
//            public string send_phone { get; set; }
//            public string send_address1 { get; set; }
//            public string send_address2 { get; set; }
//            public string recv_name { get; set; }
//            public string recv_phone { get; set; }
//            public string recv_address1 { get; set; }
//            public string recv_address2 { get; set; }
//        }

//        class mail_recv_data
//        {
//            public string recv_name { get; set; }
//            public string recv_phone { get; set; }
//            public string recv_address1 { get; set; }
//            public string recv_address2 { get; set; }
//        }

//        List<mail_data> mail_Datas = new List<mail_data>();
//        List<mail_recv_data> mail_recv_Datas = new List<mail_recv_data>();

//        private async void check_Click(object sender, RoutedEventArgs e)
//        {
//            string msg = recvname.Text;
//            send_REC_INF(msg);
//            await Task.Run(() => recvMsgSync());
//            data_view(Answer);
//        }

//        private async void check2_Click(object sender, RoutedEventArgs e)
//        {
//            string name = sendanme.Text;
//            send_SEN_INF(name);
//            await Task.Run(() => recvMsgSync());
//            data_view1(Answer);
//        }

//        public void recvMsgSync()
//        {
//            int BUFFSIZE = 1024;
//            StringBuilder ab = new StringBuilder();

//            while (true)
//            {
//                byte[] data = new byte[BUFFSIZE];
//                int size = stream.Read(data, 0, data.Length);
//                if (size == 0)
//                    break;

//                //testbox.Text += size;
//                //testbox.Text += data[0] + "\n";
//                //testbox.Text += data[1] + "\n";

//                switch (data[0])
//                {
//                    case (byte)REC_FIN:
//                        {
//                            MessageBox.Show("끝났습니다");
//                            return;
//                        }

//                    case (byte)REC_SEN:
//                        {
//                            //testbox.Text += "size";
//                            data = data.Skip(1).ToArray();
//                            ab.Append(Encoding.UTF8.GetString(data, 0, size - 1));
//                            Answer = ab.ToString();
//                            //testbox.Text += Answer;
//                            ab.Clear();
//                            break;
//                        }
//                    case (byte)SEN_SEN:
//                        {
//                            data = data.Skip(1).ToArray();
//                            ab.Append(Encoding.UTF8.GetString(data, 0, size - 1));
//                            Answer = ab.ToString();
//                            //testbox.Text += Answer;
//                            ab.Clear();
//                            break;
//                        }
//                    default:
//                        return;
//                }

//                if (data[0] == (byte)REC_FIN)
//                {
//                    break;
//                }
//            }
//        }

//        public void send_REC_INF(string RNAME)
//        {
//            try
//            {
//                string msg = REC_INF + RNAME;
//                byte[] data = Encoding.UTF8.GetBytes(msg);
//                stream.Write(data, 0, data.Length);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }
//        }

//        public void send_SEN_INF(string SNAME)
//        {
//            try
//            {
//                string msg = SEN_INF + SNAME;
//                byte[] data = Encoding.UTF8.GetBytes(msg);
//                stream.Write(data, 0, data.Length);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }
//        }

//        public void data_view(string Answer1)
//        {
//            string[] spliteddatas = Answer1.Split('\n');
//            List<string[]> data = new List<string[]>();
//            for (int i = 0; i < spliteddatas.Count() - 1; i++)
//            {
//                string[] strings = spliteddatas[i].Split('!');
//                data.Add(strings);
//            }

//            for (int i = 0; i < data.Count; i++)
//            {
//                mail_data mail_Data = new mail_data
//                {
//                    send_name = data[i][0],
//                    send_phone = data[i][1],
//                    send_address1 = data[i][2],
//                    send_address2 = data[i][3]
//                };

//                mail_Datas.Add(mail_Data);
//            }

//            send_user_data.ItemsSource = mail_Datas;
//            send_user_data.Items.Refresh();
//        }

//        public void data_view1(string Answer1)
//        {
//            string[] spliteddatas = Answer1.Split('\n');
//            List<string[]> data = new List<string[]>();
//            for (int i = 0; i < spliteddatas.Count() - 1; i++)
//            {
//                string[] strings = spliteddatas[i].Split('!');
//                data.Add(strings);
//            }

//            for (int i = 0; i < data.Count; i++)
//            {
//                mail_recv_data mail_Recv_Data = new mail_recv_data
//                {
//                    recv_name = data[i][4],
//                    recv_phone = data[i][5],
//                    recv_address1 = data[i][6],
//                    recv_address2 = data[i][7]
//                };

//                mail_recv_Datas.Add(mail_Recv_Data);
//            }

//            recv_user_data.ItemsSource = mail_recv_Datas;
//            recv_user_data.Items.Refresh();
//            Answer = "";
//        }
//    }
//}
