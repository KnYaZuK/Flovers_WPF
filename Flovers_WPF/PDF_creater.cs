using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Net;
using iTextSharp.text;
using iTextSharp;
using iTextSharp.text.pdf;
using System.Windows.Documents;

namespace Flovers_WPF
{
    class PDF_creater
    {
        /// <summary>
        /// создание PDF с маршрутом
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="win">родительское окно</param>
        public void Create_PDF_File(string path,Routes_Window win)
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            BaseFont base_font = BaseFont.CreateFont("arialn.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Phrase head = new Phrase("Маршрут", new iTextSharp.text.Font(base_font, 18, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Red)));
            iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph(head);
            header.Alignment = Element.ALIGN_CENTER;
            doc.Add(header);

            for (int i = 0; i < win.sorted_addresses.Count; i++)
            {
                string row = i.ToString() + ")" + win.sorted_addresses[i].ToString();
                iTextSharp.text.Phrase text = new Phrase(row, new iTextSharp.text.Font(base_font, 14, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black)));
                iTextSharp.text.Paragraph main_text = new iTextSharp.text.Paragraph(text);
                doc.Add(main_text);
            }

            doc.Close();
        }

        /// <summary>
        /// создание PDF со спецпредложением
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="win">родительское окно</param>
        public void Create_PDF_File(string path,Special_Deal_Window win)
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            BaseFont base_font = BaseFont.CreateFont("arialn.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Phrase head = new Phrase(win.textbox_theme.Text, new iTextSharp.text.Font(base_font, 18, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Red)));
            iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph(head);
            header.Alignment = Element.ALIGN_CENTER;
            doc.Add(header);

            iTextSharp.text.Phrase text = new Phrase(new TextRange(win.textbox_text.Document.ContentStart, win.textbox_text.Document.ContentEnd).Text, new iTextSharp.text.Font(base_font, 14, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black)));
            iTextSharp.text.Paragraph main_text = new iTextSharp.text.Paragraph(text);
            doc.Add(main_text);

            for (int i = 0; i < win.added_images.Count; i++)
            {
                iTextSharp.text.Image photo = iTextSharp.text.Image.GetInstance(win.added_images[i].ToString());
                doc.Add(photo);
            }
            doc.Close();
        }
    }
}
