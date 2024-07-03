using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Tesseract;
using Python.Runtime;
using System.Net.Http;
using System.Net.Sockets;

namespace cvtest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    // OpenCvSharp 설치 시 Window를 명시적으로 사용해 주어야 함 (window -> System.Windows.Window)
    public partial class MainWindow : System.Windows.Window
    {

        ////static readonly HttpClient API = new HttpClient();
        ////byte[] data = new byte[256];
        ////public static TcpClient clients = new TcpClient("10.10.21.118", 5005); //연결객체
        /////*        public static TcpClient clients = new TcpClient("10.10.21.111", 8001); //연결객체*/
        ////static NetworkStream stream = clients.GetStream();



        //// 필요한 변수 선언
        ////VideoCapture cam = new VideoCapture(0);
        ////Mat frame = new Mat();

        //static string save = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");

        //string save_pic = save;// 이 파일에서 텍스트 추출해서 쓸거임 
        //string address = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2/"; // 저장 경로 

        //DispatcherTimer timer;
        //bool is_initCam, is_initTimer;
        //string save_name = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");
        public MainWindow()
        {
            InitializeComponent();
        }


        // 사진 저장버튼 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ////엔진 초기화
            //using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR/tessdata", "kor", EngineMode.Default))

            //{
            //    //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\recipt.jpg";
            //    //string imagePath = @".\image2\IE001338485_STD.jpg";
            //    string imagePath = @"C:\Users\LMS\source\repos\cvtest\image2\IE001338485_STD.jpg";

            //    //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\mail.jpg"; // 
            //    //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\20240628_130449.jpg"; // 영수증 이건 전처리 안해준게 더 낫네? 
            //    //string imagePath = "C:\\Users\\LMS\\source\\repos\\cvtest\\image2\\20240702_190556.jpg"; // 메가



            //    //string imagePath = address + save_pic + ".png"; // 촬영한 이미지 

            //    Mat asd = Cv2.ImRead(imagePath);
            //    //Mat asd = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
            //    //Cv2.ImRead("images/recipt.jpg");
            //    //// 확대 
            //    Mat resizedImg = new Mat();
                
            //    if(asd.Empty())
            //    {
            //        MessageBox.Show("이미지없음");
            //    }

            //    Cv2.Resize(asd, resizedImg,new OpenCvSharp.Size(), 3, 3, InterpolationFlags.Linear);

            //    //// 이미지를 그레이스케일로 변환합니다.
            //    Mat grayImg = new Mat();
            //    Cv2.CvtColor(resizedImg, grayImg, ColorConversionCodes.BGR2GRAY);

            //    //이진화를 적용합니다.
            //    Mat binaryImg = new Mat();
            //    //Cv2.Threshold(grayImg, binaryImg, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            //    Cv2.Threshold(grayImg, binaryImg, 0, 120, ThresholdTypes.Otsu);
            //    Cv2.ImShow("binay", binaryImg);

            //    // 노이즈 제거를 위해 GaussianBlur를 적용합니다.
            //    Mat denoisedImg = new Mat();
            //    Cv2.GaussianBlur(binaryImg, denoisedImg, new OpenCvSharp.Size(3, 3), 0); // 노란 바탕의 글은 노이즈 제거하면 되네 

            //    // 이미지를 선명하게 합니다.
            //    Mat sharpenedImg = new Mat();
            //    Cv2.AddWeighted(denoisedImg, 1.5, grayImg, -0.5, 0, sharpenedImg);
            //    //Cv2.AddWeighted(denoisedImg, 1.5, sharpenedImg, -0.5, 0, sharpenedImg);




            //    OpenCvSharp.Rect roiRect = Cv2.SelectROI("img", sharpenedImg, false);
            //    if (roiRect.Width > 0 && roiRect.Height > 0)
            //    {
            //        Mat roi = new Mat(sharpenedImg, roiRect);
            //        Cv2.ImShow("cropped", roi);
            //        Cv2.ImWrite("cropped.jpg", roi);
            //    }


            //    // 텍스트 추출 
            //    var img = Pix.LoadFromFile("cropped.jpg");
            //    {
            //        using (var page = engine.Process(img))
            //        {
            //            // 인식된 텍스트 출력
            //            string text = page.GetText();
            //            asdf.Text = text;
            //            asdf1.Text = text.Split('\n')[0];
            //            string[] lines = text.Split('\n');
            //            foreach (var line in lines)
            //            {
            //                asdf1.Text += line + "\n";
            //            }
            //        }
            //    }
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //VideoCapture cam = new VideoCapture(0);
            //Mat frame = new Mat();

            ////Cv2.Rect rect;
            //OpenCvSharp.Rect rect = new OpenCvSharp.Rect();
            ////rect = [rect.Y,y+h,rect.X:];
            ////Mat dst = frame.SubMat(new OpenCvSharp.Rect(100, 100, 100, 100));

            //Cv2.Rectangle(frame, rect, Scalar.White);


            //while (Cv2.WaitKey(33) != 'q')
            //{
            //    cam.Read(frame);
            //    Cv2.ImShow("frame", frame);
            //    //rect = Cv2.SelectROI("frame", frame, false);


            //}
            //// 파일이름 현재 시간

            //Cv2.ImWrite(address + save + ".png", frame);

            //frame.Dispose();
            //cam.Release();
            //Cv2.DestroyAllWindows();

        }
    }
}
