using CustomMessageBox;
using QUANLICAPHE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace QUANLICAPHE.ViewModel
{
    public class ChangePasswordViewModel:BaseViewModel
    {
        private string _password1;
        public string password1 { get => _password1; set { _password1 = value; OnPropertyChanged(); } }
        private string _password2;
        public string password2 { get => _password2; set { _password2 = value; OnPropertyChanged(); } }
        private string _password3;
        public string password3 { get => _password3; set { _password3 = value; OnPropertyChanged(); } }
        public ICommand CloseCommand { get; set; }
        public ICommand PasswordChangedCommand1 { get; set; }
        public ICommand PasswordChangedCommand2 { get; set; }
        public ICommand PasswordChangedCommand3 { get; set; }
        public ICommand SaveCommand { get; set; }

        public ChangePasswordViewModel()
        {
            password1 = "";
            PasswordChangedCommand1 = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { password1 = p.Password; OnPropertyChanged(); });
            password2 = "";
            PasswordChangedCommand2 = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { password2 = p.Password; OnPropertyChanged(); });
            password3 = "";
            PasswordChangedCommand3 = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { password3 = p.Password; OnPropertyChanged(); });

            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                var w = (p);
                if (w != null)
                {
                    w.Close();
                }
            });

            SaveCommand = new RelayCommand<object>((p) =>
            {   
                //1 là mật khẩu cũ, 2 là mật khẩu mới, 3 là xác nhận mật khẩu mới
                if(password1 ==""||password2==""||password3=="")
                    return false;
                return true;
            }, (p) =>
            {
                try
                {
                    string PassWord1 = ToBase64Encode(password1);
                    var pass = DataProvider.Ins.DB.Users.Where(x => x.PASSWORD == PassWord1).First();
                    if (pass != null)
                    {
                        if (password2 == password3)
                        {
                            string PassWord3 = ToBase64Encode(password3);
                            pass.PASSWORD = PassWord3;
                            DataProvider.Ins.DB.SaveChanges();
                            MessageBoxCustom mess = new MessageBoxCustom("Đổi mật khẩu thành công!", MessageType.Success, MessageButtons.Ok);
                            mess.ShowDialog();
                        }
                        else
                        {
                            MessageBoxCustom mess = new MessageBoxCustom("Nhập lại mật khẩu mới không giống nhập mật khẩu mới!", 
                                MessageType.Info, MessageButtons.Ok);
                            mess.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBoxCustom mess = new MessageBoxCustom("Mật khẩu cũ không chính xác!", MessageType.Info, MessageButtons.Ok);
                        mess.ShowDialog();
                    }
                }
                catch(Exception ex)
                {
                    MessageBoxCustom mess = new MessageBoxCustom("Mật khẩu cũ không chính xác!", MessageType.Info, MessageButtons.Ok);
                    mess.ShowDialog();
                }
            });
        }
        public static string ToBase64Encode(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

    }
}
