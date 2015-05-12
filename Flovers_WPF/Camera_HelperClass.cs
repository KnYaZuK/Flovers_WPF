using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;

namespace Flovers_WPF
{
    class Camera_HelperClass
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handle);
        public static BitmapSource bs;
        public static IntPtr ip;

        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            ip = source.GetHbitmap();

            bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ip);

            return bs;
        }

        public static void SaveImageCapture(BitmapSource bmp)
        {
            JpegBitmapEncoder enc = new JpegBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));
            enc.QualityLevel = 100;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Captured_Image"; 
            dlg.DefaultExt = ".Jpg"; 
            dlg.Filter = "Image (.jpg)|*.jpg";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                FileStream fstream = new FileStream(filename, FileMode.Create);
                enc.Save(fstream);
                fstream.Close();
            }
        }
    }
}
