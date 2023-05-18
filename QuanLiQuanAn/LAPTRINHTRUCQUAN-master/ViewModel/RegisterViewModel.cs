using CustomMessageBox;
using MaterialDesignThemes.Wpf;
using QUANLICAPHE.Model;
using QUANLICAPHE.View;
using QuanLyKho.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace QUANLICAPHE.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private string _username;
        public string username  { get => _username; set { _username = value; OnPropertyChanged(); } }

        private string _password;
        public string password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public string _SelectedSex;
        public string SelectedSex { get => _SelectedSex; set { _SelectedSex = value; OnPropertyChanged(); } }

        public string _hoten;
        public string hoten { get => _hoten; set { _hoten = value; OnPropertyChanged(); } }

        private DateTime _ngaysinh = DateTime.Now;
        public DateTime ngaysinh { get => _ngaysinh; set { _ngaysinh = value; OnPropertyChanged(); } }

        public string _sdt;
        public string sdt { get => _sdt; set { _sdt = value; OnPropertyChanged(); } }

        public string _email;
        public string email { get => _email; set { _email = value; OnPropertyChanged(); } }

        public string _diachi;
        public string diachi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }

        public BitmapImage _imageuser=new BitmapImage(new Uri("pack://application:,,,/QUANLICAPHE;component/Image/pngtree-vector-businessman-icon-png-image_924876.jpg"));
        public BitmapImage imageuser { get => _imageuser; set { _imageuser = value; OnPropertyChanged(); } }

        public ICommand PasswordChangedCommand { get; set; }

        public ICommand CloseCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand SelectImage { get; set; }

        public RegisterViewModel()
        {
            password = "";
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { password =p.Password;OnPropertyChanged(); });
            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                var w = (p);
                if (w != null)
                {
                    w.Close();
                }
            });

            RegisterCommand = new RelayCommand<object>((p) =>
            {
                //Nếu chưa điền đủ thông tin thì không kích hoạt
                if (username == null || password == null || hoten == null || SelectedSex == null || sdt == null || email == null || diachi == null)
                    return false;
                return true;
            }, (p) =>
            {
                var displayList = DataProvider.Ins.DB.Users.Where(x => x.USERNAME == username);
                if (displayList == null || displayList.Count() != 0)
                {
                    MessageBoxCustom mess = new MessageBoxCustom("Tên tài khoản đã tồn tại", MessageType.Warning, MessageButtons.Ok);
                    mess.ShowDialog();
                    return;
                }
                var displayListEmail = DataProvider.Ins.DB.Users.Where(x => x.EMAIL == email);
                if (displayListEmail == null || displayListEmail.Count() != 0)
                {
                    MessageBoxCustom mess = new MessageBoxCustom("Email đã tồn tại", MessageType.Warning, MessageButtons.Ok);
                    mess.ShowDialog();
                    return;
                }
                if (IsEmail(email) == true)
                {
                    string from = "nguyenminhchanh3842@gmail.com";
                    string passfrom = "jwyubrbvowyrkphn";
                    string subject = "CHÚC MỪNG THÀNH VIÊN MỚI";
                    string body = "Chúc mừng bạn trở thành 1 thành viên của Suki Zuki";
                    SendPassWordToMail(from, passfrom, email, subject, body);
                    string passEncode = ToBase64Encode(password);
                    var User = new User() { USERNAME = username, PASSWORD = passEncode, HOTEN = hoten, 
                        GIOITINH = SelectedSex, NGAYSINH = ngaysinh, SDT = sdt, EMAIL = email, DIACHI = diachi, IMAGEUSER = imageuser.ToString() };
                    if (User != null)
                    {
                        DataProvider.Ins.DB.Users.Add(User);
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBoxCustom mess = new MessageBoxCustom("Chúc mừng bạn tạo thành công tài khoản", MessageType.Success, MessageButtons.Ok);
                        mess.ShowDialog();
                    }
                }
                else
                {
                    MessageBoxCustom mess = new MessageBoxCustom("Vui lòng nhập đúng dạng email", MessageType.Warning, MessageButtons.Ok);
                    mess.ShowDialog();
                    return;
                }
               
            });

            SelectImage = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
				openFileDialog.InitialDirectory = @"C:\";
				openFileDialog.Title = "Vui lòng chọn ảnh.";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    imageuser = new BitmapImage(fileUri);
                    string sourcefile = openFileDialog.FileName;
                    string resourceUri = "..//..//Image//" + System.IO.Path.GetFileName(openFileDialog.FileName);
                    var list1 = DataProvider.Ins.DB.Users.Where(x => x.IMAGEUSER == resourceUri);
                    if (list1 != null)
                    {

                    }
                    else
                    {
                        System.IO.File.Copy(sourcefile, resourceUri, true);

                    }
                }
            });

        }

        public static bool IsEmail(string email)
        {
            //Kiểm tra định dạng mail 
            if (string.IsNullOrEmpty(email))
                return false;

            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
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
        private void SendPassWordToMail(string From, string Pass, string To, string Subject, string Body)
        {
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
                client.Dispose();
            }
            catch
            {
            }

        }

    }
}
