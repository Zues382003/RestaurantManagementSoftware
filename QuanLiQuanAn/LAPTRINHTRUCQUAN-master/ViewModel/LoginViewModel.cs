using CustomMessageBox;
using QUANLICAPHE.Model;

using QUANLICAPHE.View;
using QUANLICAPHE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using MessageBox = System.Windows.Forms.MessageBox;



namespace QUANLICAPHE.ViewModel

{
    class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public ICommand CloseCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ShowDangKi { get; set; }
        public ICommand ShowQuenMK { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        // mọi thứ xử lý sẽ nằm trong này 
        public LoginViewModel()

        {
            IsLogin = false;
            Password = "";
            UserName = "";
            var list = DataProvider.Ins.DB.Users;
            foreach (User item in list)
            {
                item.DANGNHAP = 0;
            }
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                Login(p);
            });

            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                bool? result = new MessageBoxCustom("Bạn có chắc là muốn thoát?", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();
                if (result.Value)
                {
                    p.Close();
                }
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ShowQuenMK = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                QuenMK quenmk = new QuenMK();
                quenmk.ShowDialog();
            });
        }
        void Login(Window p)

        {
            if (p == null)
                return;
            string passEncode = ToBase64Encode(Password);//Mã hóa mật khẩu
            var accCount = DataProvider.Ins.DB.Users.Where(x => x.USERNAME == UserName && x.PASSWORD == passEncode).Count();//Kiểm tra mật khẩu giống trong Database không
            if (accCount > 0)
            {
                IsLogin = true;
                p.Close();
                foreach (var user in DataProvider.Ins.DB.Users.Where(x => x.USERNAME == UserName && x.PASSWORD == passEncode))
                {
                    user.DANGNHAP = 1;
                }
            }
            else
            {
                IsLogin = false;
                MessageBoxCustom mess = new MessageBoxCustom("Sai tài khoản hoặc mật khẩu!", MessageType.Warning, MessageButtons.Ok);
                mess.ShowDialog();
            }
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
        public static string ToBase64Decode(string base64EncodedText)
        {
            if (String.IsNullOrEmpty(base64EncodedText))
            {
                return base64EncodedText;
            }
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}

