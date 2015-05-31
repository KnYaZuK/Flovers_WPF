using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flovers_WPF.Repository;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;
using System.Net.Mail;
using System.Net;
using System.Windows.Documents;

namespace Flovers_WPF
{
    class MailClass
    {
        /// <summary>
        /// метод рассылки писем
        /// </summary>
        /// <param name="win">родительское окно</param>
        /// <param name="theme">тема сообщения</param>
        /// <param name="text">текст сообщения</param>
        /// <param name="PDF_path">путь к pdf предложения</param>
        /// <param name="added_images">список изображений для прикреплений</param>
        public async void Send_Messages(Special_Deal_Window win, string theme, System.Windows.Controls.RichTextBox text, string PDF_path, List<object> added_images, ClientsRepository oClients, ConstantsRepository oConstants)
        {
            win.mail_addresses = await oConstants.Select_Constants_Async("select value from Constants where name=\'e_mail_address\'");
            string mail_adr = win.mail_addresses.First().value.ToString();
            win.mail_passwords = await oConstants.Select_Constants_Async("select value from Constants where name=\'e_mail_pass\'");
            string mail_pas = win.mail_passwords.First().value.ToString();

            if (PDF_path != null)
            {
                if (mail_adr != null && mail_pas != null)
                {
                    try
                    {
                        string smtpHost = "smtp.yandex.ru";
                        SmtpClient client = new SmtpClient(smtpHost, 25);
                        var cred = new NetworkCredential(mail_adr, mail_pas);
                        client.Credentials = cred;
                        client.EnableSsl = true;

                        string from = mail_adr;
                        string subject = theme;
                        string to;
                        string body = "<html><body><div><div style=\"height:10%; background-color:#6DD4FF; border-radius:10px\"><p style=\"margin-left:20px; color:red\">Внимание! для тех,у кого не поддерживается HTML - снизу письма дубликат в формате PDF</p></div><div style=\"height:300px; border: 1px solid black; border-radius:10px\"><p style=\"margin-left:20px\">" + new TextRange(text.Document.ContentStart, text.Document.ContentEnd).Text.ToString() + "</p>";
                        AlternateView htmlv = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                        for (int i = 0; i < added_images.Count; i++)
                        {
                            LinkedResource imageResource = new LinkedResource(added_images[i].ToString(), "image/jpg");
                            imageResource.ContentId = "photo" + i.ToString();
                            imageResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                            htmlv.LinkedResources.Add(imageResource);

                            body += "<img style=\"margin-left:20px\" src=\"cid:" + imageResource.ContentId.ToString() + "\" alt='photo' />";
                        }
                        body += "</div></div></body></html>";
                        Attachment pdfFile = new Attachment(PDF_path);

                        List<Clients> all_clients = await oClients.Select_All_Clients_Async();
                        foreach (var c in all_clients)
                        {
                            to = c.email;
                            MailMessage mes = new MailMessage(from, to, subject, body);
                            mes.IsBodyHtml = true;
                            mes.SubjectEncoding = Encoding.GetEncoding(1251);
                            mes.BodyEncoding = Encoding.GetEncoding(1251);
                            mes.AlternateViews.Add(htmlv);
                            mes.Attachments.Add(pdfFile);
                            client.Send(mes);
                        }
                        System.Windows.MessageBox.Show("Рассылка завершена!");
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("Ваш адреc почты или пароль некорректны!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Проверьте правильность адреса почты или пароль в окне Констант");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Сначала необходимо сохранить или загрузить готовый PDF файл");
            }
        }
    }
}
