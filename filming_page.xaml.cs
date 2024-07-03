using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Tesseract;
using static IronPython.Modules._ast;

namespace cvtest
{
    /// <summary>
    /// filming_page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class filming_page : System.Windows.Controls.Page
    {


        //통신 
        const char MAILINFO = (char)0x05;
        const string SEP = "\n";
        const int BUFFSIZE = 1024;

        static readonly HttpClient API = new HttpClient();
        //byte[] data = new byte[256];
        public static TcpClient clients = new TcpClient("10.10.21.105", 10001); //연결객체
        ///*        public static TcpClient clients = new TcpClient("10.10.21.111", 8001); //연결객체*/
        static NetworkStream stream = clients.GetStream();

        int cnt = 0;

        // 필요한 변수 선언
        //VideoCapture cam = new VideoCapture(0);
        //Mat frame = new Mat();

        static string save = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");

        string save_pic = save;// 이 파일에서 텍스트 추출해서 쓸거임 
        string address = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2/"; // 저장 경로 

        DispatcherTimer timer;
        bool is_initCam, is_initTimer;
        string save_name = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");

        // 보낸 사람
        List<string> list = new List<string>();
        string[] testdata_recv; // 받음 
        string[] test = new string[4];


        public filming_page()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //엔진 초기화
            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR/tessdata", "eng", EngineMode.Default))

            {
                //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\recipt.jpg";
                //string imagePath = @".\image2\IE001338485_STD.jpg";
                //string imagePath = @"C:\Users\LMS\source\repos\cvtest\image2\IE001338485_STD.jpg";
                string imagePath = @"C:\Users\LMS\source\repos\cvtest\image2\IMG_4430.png";


                //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\mail.jpg"; // 
                //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\20240628_130449.jpg"; // 영수증 이건 전처리 안해준게 더 낫네? 
                //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\20240702_190556.jpg"; // 메가

                string RNAME; // 보내는 사람 
                string RADDRESS; // 보내는 주소
                string RPNUM; // 보내는 우편번호
                string SNAME; // 
                string PNUM;






                //string imagePath = address + save_pic + ".png"; // 촬영한 이미지 

                Mat asd = Cv2.ImRead(imagePath);
                //Mat asd = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
                //Cv2.ImRead("images/recipt.jpg");
                //// 확대 
                Mat resizedImg = new Mat();

                if (asd.Empty())
                {
                    MessageBox.Show("이미지없음");
                }

                Cv2.Resize(asd, resizedImg, new OpenCvSharp.Size(), 1, 1, InterpolationFlags.Linear);

                //// 이미지를 그레이스케일로 변환합니다.
                Mat grayImg = new Mat();
                Cv2.CvtColor(resizedImg, grayImg, ColorConversionCodes.BGR2GRAY);

                //이진화를 적용합니다.
                Mat binaryImg = new Mat();
                //Cv2.Threshold(grayImg, binaryImg, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                Cv2.Threshold(grayImg, binaryImg, 0, 120, ThresholdTypes.Otsu);
                Cv2.ImShow("binay", binaryImg);

                // 노이즈 제거를 위해 GaussianBlur를 적용합니다.
                Mat denoisedImg = new Mat();
                Cv2.GaussianBlur(binaryImg, denoisedImg, new OpenCvSharp.Size(3, 3), 0); // 노란 바탕의 글은 노이즈 제거하면 되네 

                // 이미지를 선명하게 합니다.
                Mat sharpenedImg = new Mat();
                Cv2.AddWeighted(denoisedImg, 1.5, grayImg, -0.5, 0, sharpenedImg);
                //Cv2.AddWeighted(denoisedImg, 1.5, sharpenedImg, -0.5, 0, sharpenedImg);


                // 팀,경제 이 부분이 두줄로 나오네 

                OpenCvSharp.Rect roiRect = Cv2.SelectROI("img", resizedImg, false);
                if (roiRect.Width > 0 && roiRect.Height > 0)
                {
                    Mat roi = new Mat(resizedImg, roiRect);
                    Cv2.ImShow("cropped", roi);
                    Cv2.ImWrite("cropped.jpg", roi);
                }


                // 텍스트 추출 
                var img = Pix.LoadFromFile("cropped.jpg");
                {
                    using (var page = engine.Process(img))
                    {
                        cnt += 1;
                        //첫번째는 보내는 사람 두 번째는 받는 사람 
                        // 인식된 텍스트 출력
                        string text = page.GetText();// 추출한 텍스트
                        asdf.Text = text;
                        //asdf1.Text = text.Split('\n')[0]; // 배열의 첫번째 
                        string[] lines = text.Split('\n');// lines에 개행문자 기준으로 잘라서 각각 넣음 


                        foreach (var line in lines)
                        {
                            if (line != "")// 보내는 사람 
                            {
                                asdf1.Text += line + "\n";
                                int i = 0;
                                i++;
                                list.Add(line);
                                //test.Append(text);

                                //asdf3.Text = testdata_send[i];
                            }
                            //else // 받는 사람 
                            //{
                            asdf2.Text += line + "\n";
                            //}




                        }

                        for (int i = 0; i < list.Count; i++)
                        {
                            asdf3.Text += list[i];
                            MessageBox.Show(list[i]);
                        }
                        send_MAILINFO(list);

                        ////byte[] data;
                        ////string send_msg;
                        ////string responses;
                        ////// 특정 학생 아이디 보내기 
                        ////data = null;
                        ////data = new byte[256];
                        //for (int i = 0; i < list.Count; i++)
                        //{
                        //    //byte[] data;
                        //    //string send_msg = null;
                        //    //string responses;
                        //    //// 특정 학생 아이디 보내기 
                        //    //data = null;
                        //    //data = new byte[256];

                        //    //send_msg = list[i]; //  유저어아디 보내기  
                        //    //data = Encoding.UTF8.GetBytes(send_msg);
                        //    //stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

                        //    //Thread.Sleep(100);
                        //}

                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VideoCapture cam = new VideoCapture(0);
            Mat frame = new Mat();


            OpenCvSharp.Rect rect = new OpenCvSharp.Rect();


            Cv2.Rectangle(frame, rect, Scalar.White);


            while (Cv2.WaitKey(33) != 'q')
            {
                cam.Read(frame);
                Cv2.ImShow("frame", frame);



            }
            // 파일이름 현재 시간

            Cv2.ImWrite(address + save + ".png", frame);

            frame.Dispose();
            cam.Release();
            Cv2.DestroyAllWindows();
        }
        public void send_MAILINFO(List<string> datalist)// 
        {
            //list 

            try
            {
                string msg = MAILINFO + datalist[0] + SEP + datalist[1] + SEP + datalist[2] + SEP + datalist[3];
                asdf4.Text = 0 + datalist[0];
                asdf4.Text += 1 + datalist[1];
                asdf4.Text += 2 + datalist[2];
                asdf4.Text += 3 + datalist[3];

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
