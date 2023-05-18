using MaterialDesignColors;
using QUANLICAPHE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Media;
using System.Net.Mail;
using CustomMessageBox;

namespace QUANLICAPHE.ViewModel
{
    public class QuenMkViewModel:BaseViewModel
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        public string _email;
        public string email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public ICommand CloseCommand { get; set; }
        public ICommand QuenMK { get; set; }
        public QuenMkViewModel()
        {
            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                var w = (p);
                if (w != null)
                {
                    w.Close();
                }
            });

            QuenMK = new RelayCommand<object>((p) =>
            {
                //nếu email chưa được điền thì không kích hoạt
                if(email ==null)
                    return false;
                //nếu email điền vào không tồn tại trong Database thì không kích hoạt
                var displayList = DataProvider.Ins.DB.Users.Where(x => x.EMAIL==email);
                if (displayList == null || displayList.Count() == 0)
                    return false;
                return true;
            }, (p) =>
            {
                string password = "";
                var displayList = DataProvider.Ins.DB.Users.Where(x => x.EMAIL == email);
                foreach (User item in displayList)
                {
                    password = ToBase64Decode(item.PASSWORD);
                }

                string from = "nguyenminhchanh3842@gmail.com";
                string passfrom = "jwyubrbvowyrkphn";
                string subject = "LẤY LẠI MẬT KHẨU";
                string body = "Mật khẩu của bạn là: " + password;
                SendPassWordToMail(from, passfrom, email, subject, body);
            });

        }

        public static string ToBase64Decode(string base64EncodedText)
        {
            if (String.IsNullOrEmpty(base64EncodedText))
            {
                return base64EncodedText;
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void SendPassWordToMail(string From, string Pass, string To, string Subject, string Body)
        {
            //Hàm gửi mail
            try
            {
                var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential(From, Pass),
                    EnableSsl = true
                };
                var message = new System.Net.Mail.MailMessage(From, To, Subject, Body);
                message.IsBodyHtml = true;
                client.Host = "smtp.gmail.com";
                client.Send(message);
                MessageBoxCustom mess = new MessageBoxCustom("Gửi mail thành công!", MessageType.Success, MessageButtons.Ok);
                mess.ShowDialog();
                client.Dispose();
                
            }
            catch
            {
            }
            
        }

        //private void sendpass(string From, string Pass, string To, string Subject, string Body)
        //{
        //    try
        //    {
        //        MailMessage mailMessage = new MailMessage(From, To);
        //        mailMessage.Priority = System.Net.Mail.MailPriority.High;
        //        mailMessage.Body = Body;
        //        mailMessage.IsBodyHtml = true;
        //        SmtpClient smtpClient = new SmtpClient("host/IP", 25);
        //        NetworkCredential credentials = new NetworkCredential(From, Pass);
        //        smtpClient.Credentials = credentials;
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.Credentials = credentials;
        //        smtpClient.Send(mailMessage);
        //        MessageBox.Show("Gửi mail thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //        smtpClient.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return;
        //    }
        //}
    }
   
}
