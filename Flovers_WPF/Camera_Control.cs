using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using WebCam_Capture;

namespace Flovers_WPF
{
    class Camera_Control
    {
        public WebCamCapture camera;
        private System.Windows.Controls.Image Frame_Img;
        public int Framerate = 30;

        ///<summary>
        ///Метод инициализации камеры
        ///</summary>
        ///<param name="Img">Ссылка на входное изображение</param>
        ///
        public void Ini_Web_Camera(ref System.Windows.Controls.Image Img)
        {
            camera = new WebCamCapture();
            camera.FrameNumber = ((ulong)(0ul));
            camera.TimeToCapture_milliseconds = Framerate;
            camera.ImageCaptured += new WebCamCapture.WebCamEventHandler(webcam_ImageCaptured);
            Frame_Img = Img;
        }
        /// <summary>
        /// Захват изображения
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
            Frame_Img.Source = Camera_HelperClass.LoadBitmap((System.Drawing.Bitmap)e.WebCamImage);
        }
        /// <summary>
        /// включение камеры
        /// </summary>
        public void Start()
        {
            camera.TimeToCapture_milliseconds = Framerate;
            camera.Start(0);
        }
        /// <summary>
        /// остановка камеры
        /// </summary>
        public void Stop()
        {
            camera.Stop();
        }
    }
}
