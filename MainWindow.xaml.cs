using System;
using System.Collections.Generic;
using System.Linq;
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

namespace cvtest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    // OpenCvSharp 설치 시 Window를 명시적으로 사용해 주어야 함 (window -> System.Windows.Window)
    public partial class MainWindow : System.Windows.Window
    {

        // 필요한 변수 선언
        //VideoCapture cam = new VideoCapture(0);
        //Mat frame = new Mat();
        DispatcherTimer timer;
        bool is_initCam, is_initTimer;
        string save_name = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");
        public MainWindow()
        {
            InitializeComponent();
        }

        // 사진 저장버튼 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR/tessdata", "kor", EngineMode.Default)) 
            {
                string imagePath = "C:\\Users\\iot\\source\\repos\\kimgun140\\cvtest\\image2\\2024-07-01-11시48분09초.png";

                var img = Pix.LoadFromFile(imagePath);
                    {
                    using (var page = engine.Process(img))
                    {
                        // 인식된 텍스트 출력
                        string text = page.GetText();
                        asdf.Text = text;
                    }
                }

            }





        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VideoCapture cam = new VideoCapture(1);
            Mat frame = new Mat();
            int x = 350;
            int y= 125 ;
            int w =260 ;
            int h = 260;
            //Cv2.Rect rect;
            OpenCvSharp.Rect rect = new OpenCvSharp.Rect();
            //rect = [rect.Y,y+h,rect.X:];
            Mat dst = frame.SubMat(new OpenCvSharp.Rect(100, 100, 100, 100));

            Cv2.Rectangle(frame, rect, Scalar.White);

            //if rect.X ;

            while (Cv2.WaitKey(33) != 'q')
            {
                cam.Read(frame);
                Cv2.ImShow("frame", frame);
                //rect = Cv2.SelectROI("frame", frame, false);


            }
            string save = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");
            Cv2.ImWrite("C:\\Users\\iot\\source\\repos\\kimgun140\\cvtest\\image2/" + save + ".png", frame);
            //Tesseract.
            //frame[y: rect.Y + rect.Height, x: rect.X + rect.Width];
            frame.Dispose();
            cam.Release();
            Cv2.DestroyAllWindows();

        }
    }
}
