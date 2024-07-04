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

        //static readonly HttpClient API = new HttpClient();
        ////byte[] data = new byte[256];
        //public static TcpClient clients = new TcpClient("10.10.21.105", 10001); //연결객체
        /////*        public static TcpClient clients = new TcpClient("10.10.21.111", 8001); //연결객체*/
        //static NetworkStream stream = clients.GetStream();

        // 필요한 변수 선언
        //VideoCapture cam = new VideoCapture(0);
        //Mat frame = new Mat();

        static string save = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");

        string save_pic = save;// 이 파일에서 텍스트 추출해서 쓸거임 
        string address = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2/"; // 저장 경로 

        DispatcherTimer timer;
        bool is_initCam, is_initTimer;
        string save_name = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");

        // 보낸 사람, 받는 사람 여기에 한번에 받아서 쓸거임 
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
                //string imagePath = @"C:\Users\LMS\source\repos\cvtest\image2\IMG_4430.png";
                string imagePath = @"C:\Users\iot\Source\Repos\kimgun140\cvtest\image2\2024-07-04-09시08분13초.png";
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
                //Mat grayImg = new Mat();
                //Cv2.CvtColor(resizedImg, grayImg, ColorConversionCodes.BGR2GRAY);

                ////이진화를 적용합니다.
                //Mat binaryImg = new Mat();
                ////Cv2.Threshold(grayImg, binaryImg, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                //Cv2.Threshold(grayImg, binaryImg, 0, 120, ThresholdTypes.Otsu);
                //Cv2.ImShow("binay", binaryImg);

                //// 노이즈 제거를 위해 GaussianBlur를 적용합니다.
                //Mat denoisedImg = new Mat();
                //Cv2.GaussianBlur(binaryImg, denoisedImg, new OpenCvSharp.Size(3, 3), 0); // 노란 바탕의 글은 노이즈 제거하면 되네 

                //// 이미지를 선명하게 합니다.
                //Mat sharpenedImg = new Mat();
                //Cv2.AddWeighted(denoisedImg, 1.5, grayImg, -0.5, 0, sharpenedImg);
                ////Cv2.AddWeighted(denoisedImg, 1.5, sharpenedImg, -0.5, 0, sharpenedImg);

                //OpenCvSharp.Rect roiRect = Cv2.SelectROI("img", resizedImg, false);

                //roiRect.X = 20;
                //roiRect.Y = 100;
                //roiRect.Width = 800 - roiRect.X;
                //roiRect.Height = 300 - roiRect.Y;
                //resizedImg.
                //if (roiRect.Width > 0 && roiRect.Height > 0)
                //{
                //    Mat roi = new Mat(resizedImg, roiRect);
                //    Cv2.ImShow("cropped", roi);
                //    Cv2.ImWrite("cropped.jpg", roi);
                //}
                int imgW = resizedImg.Cols;
                int imgH = resizedImg.Rows;
                int roiX = 60;
                int roiY = 50;
                int roiW = 400;
                int roiH = 100;
                if (roiX < 0) roiX = 0;
                if (roiY < 0) roiY = 0;
                if (roiX + roiW > imgW) roiW = imgW - roiX;
                if (roiY + roiH > imgH) roiH = imgH - roiY;
                int roirecvX = 300;
                int roirecvY = 240;
                int roirecvW = 270;
                int roirecvH = 200;
                if (roirecvX < 0) roiX = 0;
                if (roirecvY < 0) roiY = 0;
                if (roirecvX + roirecvW > imgW) roirecvW = imgW - roirecvX;
                if (roirecvY + roirecvH > imgH) roirecvH = imgH - roirecvY;


                OpenCvSharp.Rect roiRect = new OpenCvSharp.Rect(roiX, roiY, roiW, roiH); //x,y,w,h
                OpenCvSharp.Rect roiRectrecv = new OpenCvSharp.Rect(roirecvX, roirecvY, roirecvW, roirecvH); //x,y,w,h
                Mat matrecv = new Mat(resizedImg, roiRectrecv);
                Mat mat = new Mat(resizedImg, roiRect);
                // wpf에 띄우기~
                hh.Source = OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(mat);

                Cv2.ImShow("asdf", mat);// 정보 1
                Cv2.ImShow("asdfrecv", matrecv); // 정보 2 
                Cv2.ImWrite("cropped.jpg", mat);//정보1 
                Cv2.Rectangle(resizedImg, roiRect, OpenCvSharp.Scalar.Gray);// 그리기
                Cv2.ImShow("aaa", resizedImg); // 보이기 
                Cv2.ImWrite("croppedrecv.jpg", matrecv);// 정보 2

                // 텍스트 추출  보내는 사람 
                var img = Pix.LoadFromFile("cropped.jpg");
                {
                    using (var page = engine.Process(img))
                    {
                        string text = page.GetText();// 추출한 텍스트
                        asdf.Text = text;
                        string[] lines = text.Split('\n');// lines에 개행문자 기준으로 잘라서 각각 넣음 
                        foreach (var line in lines)
                        {
                            if (line != "")// 보내는 사람 
                            {
                                asdf1.Text += line + "\n";
                                list.Add(line);
                            }
                            asdf2.Text += line + "\n";
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            asdf3.Text += list[i];
                        }

                    }
                }


                var imgrecv = Pix.LoadFromFile("croppedrecv.jpg"); // 아래 있는 받는 사람 
                {
                    using (var page = engine.Process(imgrecv))
                    {
                        string text = page.GetText();// 추출한 텍스트
                        asdf.Text = text;
                        string[] lines = text.Split('\n');// lines에 개행문자 기준으로 잘라서 각각 넣음 
                        foreach (var line in lines)
                        {
                            if (line != "")// 보내는 사람 
                            {
                                asdf2.Text += line + "\n";
                                list.Add(line);
                            }
                            asdf4.Text += line + "\n";
                        }
                    }
                }
                send_MAILINFO(list);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VideoCapture cam = new VideoCapture(0);
            Mat frame = new Mat();


            OpenCvSharp.Rect rect = new OpenCvSharp.Rect(20, 100, 500, 300); // 
                                                                             //OpenCvSharp.Rect rect_recv = new OpenCvSharp.Rect(20, 100, 500, 300);
                                                                             //



            while (Cv2.WaitKey(33) != 'q')
            {
                cam.Read(frame);

                //Cv2.Rectangle(frame, rect, Scalar.Black, 1); // 프레임에 사각형 그리기 

                Mat gray = new Mat();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);


                Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(5, 5), 0);

                Mat edges = new Mat();
                Cv2.Canny(gray, edges, 250, 250);


                OpenCvSharp.Point[][] contours;
                HierarchyIndex[] hierarchy; // 계층적 인덱싱 인덱스타고 들어가면 다른 인덱스 그 인덱스 타고 들어가면 다른 인덱스 이런 식 
                Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

                foreach (var contour in contours)
                {
                    var approx = Cv2.ApproxPolyDP(contour, Cv2.ArcLength(contour, true) * 0.02, true);
                    if (approx.Length == 4 && Cv2.IsContourConvex(approx))
                    {
                        //Cv2.Polylines(frame, new[] { approx }, true, Scalar.Red, 2, LineTypes.AntiAlias);
                        OpenCvSharp.Rect roundrec = Cv2.BoundingRect(approx);

                        if (roundrec.Width > 400 && roundrec.Width < 700 && roundrec.Height > 250 && roundrec.Height < 500)
                        {
                            //Cv2.PutText(frame, roundrec.Width,1,Cv2.);
                            Mat mat123 = new Mat(frame, roundrec); // q누르면 종료 되면서 마지막 프레임이 저장 
                            Cv2.ImWrite(address + save + ".png", mat123);

                            frame.Dispose();
                            cam.Release();
                            Cv2.DestroyAllWindows();
                            break;
                        }

                    }
                }

                //Cv2.ImShow("frame", frame);
            }
            //Mat mat = new Mat(frame, rect); // q누르면 종료 되면서 마지막 프레임이 저장 

            // 파일이름 현재 시간
            //Cv2.ImWrite(address + save + ".png", mat);
            //Cv2.ImWrite(address + save + ".png", frame);
            //Cv2.ImShow("ttt", mat);
            //Cv2.WaitKey(0);
            //frame.Dispose();
            //cam.Release();
            //Cv2.DestroyAllWindows();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            send_MAILINFO(list);// 전부다 때려박기 목요일 아침에 해봐야징 

        }

        public void send_MAILINFO(List<string> datalist)// 
        {
            //list 

            try// 이거 되나? 해봐야지 
            {
                string msg = MAILINFO + datalist[0] + SEP + datalist[1] + SEP + datalist[2] + SEP + datalist[3] + SEP + datalist[4] + SEP + datalist[5] + SEP + datalist[6] + SEP + datalist[7];
                asdf4.Text = 0 + datalist[0];
                asdf4.Text += 1 + datalist[1];
                asdf4.Text += 2 + datalist[2];
                asdf4.Text += 3 + datalist[3];
                asdf4.Text += 4 + datalist[4];
                asdf4.Text += 5 + datalist[5];
                asdf4.Text += 6 + datalist[6];
                asdf4.Text += 7 + datalist[7];

                byte[] data = Encoding.UTF8.GetBytes(msg);
                //stream.Write(data, 0, data.Length);//전송할 데이터의 바이트 배열, 전송을 시작할 배열의 인덱스, 전송할 데이터의 길이.

                //Socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
