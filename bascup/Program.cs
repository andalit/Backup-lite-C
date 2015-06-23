using System;
using System.Diagnostics;
using System.Linq;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;


namespace bascup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //++++++++++++++++++++++++++++++++  налаштування пошти для відправлення звітів архівації
            SmtpClient SmtpOK = new SmtpClient("smtp.mail.ru", 25);
            SmtpOK.Credentials = new NetworkCredential("name@list.ru", "pass");
            SmtpOK.EnableSsl = true;
            MailMessage MessageOK = new MailMessage();
            MessageOK.From = new MailAddress("name@list.ru");
            MessageOK.To.Add(new MailAddress("name@list.ru"));
            MessageOK.Subject = "1С ОК резервна копия створена";
            MessageOK.Body = "1С ОК резервна копiя створена без помилок";
            //++++++++++++++++++++++++++++++++  Налаштування для пошти куди выдправляются помилки, за якою закрыплений мобыльний телефон.
            SmtpClient SmtpBD = new SmtpClient("smtp.mail.ru", 25);
            SmtpBD.Credentials = new NetworkCredential("name@list.ru", "pass");
            SmtpBD.EnableSsl = true;
            MailMessage MessageBD = new MailMessage();
            MessageBD.From = new MailAddress("name@list.ru");
            MessageBD.To.Add(new MailAddress("name@list.ru"));
            MessageBD.Subject = "1С ПОМИЛКА !!!";
            //++++++++++++++++++++++++++++++++  Налаштування шляхів бази 1с та архівів
            string startPath = @"V:\1C Money\"; // Каталог бази 1с
            string zipPath = @"c:\1\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".zip"; // каталог архівів
            //++++++++++++++++++++++++++++++++  

            try
            {
               
                Console.WriteLine("####################################################");
                Console.WriteLine("#                                                  #");
                Console.WriteLine("# Програма для резервного копiювання бази 1с v1.00 #");
                Console.WriteLine("#            ХНТУ 2015 5КСМ Созонтов Р.М.          #");
                Console.WriteLine("#                                                  #");
                Console.WriteLine("####################################################");
                Console.WriteLine("");
                var a = Process.GetProcessesByName("1cv8").Any(); // перевірка наявності процесу 1с
                if (a = true)
                {
                    Process[] ps1 = Process.GetProcessesByName("1cv8");
                    foreach (Process p1 in ps1)
                    {
                        Console.WriteLine("Закрыття 1с...");
                        p1.Kill(); // закріття усіх одноіменних процесів 1с
                    }
                }
                Console.WriteLine(DateTime.Now+ " Створення архiву...");
                ZipFile.CreateFromDirectory(startPath, zipPath); // Створення архіву
                Console.WriteLine(DateTime.Now + " Завершення свторення архiву...");
               // SmtpOK.Send(MessageOK); // Відправка листа з позитивними логами.

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageOK.Body = ex.ToString();
                SmtpBD.Send(MessageBD); // Відправка тексту помилки на пошту
            }
            Console.ReadLine();
        }
    }
}
