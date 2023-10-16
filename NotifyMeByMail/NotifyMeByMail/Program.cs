using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using HtmlAgilityPack;
using Internal;
using System.Threading;

namespace WebScrapperTest
{
    class Program
    {
        static string previousMessage = "";
        static void Main(string[] args)
        {
            string website = "https://www.petrolofisi.com.tr/akaryakit-fiyatlari";

            Timer timer = new Timer(ScrapAndSend, website, 0, 1 * 60 * 100);

            Console.WriteLine(" Please Press Enter to Exit");
            Console.ReadLine();

        }

        static void ScrapAndSend(object? website)
        {
            try
            {
                string senderEmail = "putyouremail@hotmail.com";
                string senderPassword = "PUT_YOUR_PASSWORD";

                string toEmailAddress = "receiveremail@gmail.com";
                string emailTitle = "Send Email with WebScrapper";
                string currentMessage = "";


                string websiteURL = (string)website;
                var web = new HtmlWeb();
                HtmlDocument doc = web.Load(websiteURL);
                var list = doc.DocumentNode.SelectNodes("/html/body/section[2]/div/div/div[2]/ul/li[2]/div");




                if (list != null)
                {
                    currentMessage = string.Join(Environment.NewLine, list.Select(x => x.InnerText));


                    if (currentMessage != previousMessage)
                    {

                        MailMessage mail = new MailMessage(senderEmail, toEmailAddress, emailTitle, currentMessage);

                        mail.IsBodyHtml = false;
                        mail.Priority = MailPriority.High;

                        // For gmail address smtp Host is "smtp.gmail.com".
                        //But there a few things you have to enable in your
                        // gmail account as Google cares about the security a lot.
                        var smtpClient = new SmtpClient("smtp-mail.outlook.com", 587) // Port for the SMTP server, e.g., 485 to 587 for Gmail
                        {
                            EnableSsl = true, // Enable SSL encryption for secure connection
                            UseDefaultCredentials = false, // Use the specified credentials
                            Credentials = new NetworkCredential(senderEmail, senderPassword),

                        };
                        // To get the specific data you want to Fetch, Use Inspect ( (Left-Side) Shift + Command + C )
                        // Copy the Data's Xpath from Elements

                        Console.WriteLine(currentMessage);
                        smtpClient.Send(mail);
                        Console.WriteLine("Email has been sent successfully!!!");
                        previousMessage = currentMessage;

                    }

                }
                else
                {

                    Console.WriteLine("Xpath Didnt Match any elements");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}