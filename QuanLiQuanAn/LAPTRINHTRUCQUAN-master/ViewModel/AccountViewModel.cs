using CustomMessageBox;
using FontAwesome.Sharp;
using MaterialDesignThemes.Wpf;
using QUANLICAPHE.Model;
using QUANLICAPHE.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace QUANLICAPHE.ViewModel
{
    public class AccountViewModel:BaseViewModel
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private string _username;
        public string username { get => _username; set { _username = value; OnPropertyChanged(); } }

        private string _password;
        public string password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public string _SelectedSex;
        public string SelectedSex { get => _SelectedSex; set { _SelectedSex = value; OnPropertyChanged(); } }

        public string _hoten;
        public string hoten { get => _hoten; set { _hoten = value; OnPropertyChanged(); } }

        private DateTime _ngaysinh ;
        public DateTime ngaysinh { get => _ngaysinh; set { _ngaysinh = value; OnPropertyChanged(); } }

        public string _sdt;
        public string sdt { get => _sdt; set { _sdt = value; OnPropertyChanged(); } }

        public string _email;
        public string email { get => _email; set { _email = value; OnPropertyChanged(); } }

        public string _diachi;
        public string diachi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }

        public BitmapImage _imageuser = new BitmapImage(new Uri("pack://application:,,,/QUANLICAPHE;component/Image/pngtree-vector-businessman-icon-png-image_924876.jpg"));
        public BitmapImage imageuser { get => _imageuser; set { _imageuser = value; OnPropertyChanged(); } }

        public ICommand ChangeImage { get; set; }
        public ICommand ChangePassword { get; set; }
        public ICommand UpdateImfomation { get; set; }


        public AccountViewModel()
        {
            var list = DataProvider.Ins.DB.Users;
            foreach (var item in list)
            {
                if (item.DANGNHAP == 1)//hiển thị thông tin tài khoản đang đăng nhập
                {
                    username = item.USERNAME;
                    SelectedSex = item.GIOITINH;
                    hoten=item.HOTEN;
                    ngaysinh = item.NGAYSINH;
                    sdt = item.SDT;
                    email = item.EMAIL;
                    diachi = item.DIACHI;
                    imageuser = new BitmapImage(new Uri(item.IMAGEUSER));
                }
            }

            ChangeImage = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(username))
                    return false;
                return true;
            }, (p) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
				openFileDialog.InitialDirectory = @"C:\";
				openFileDialog.Title = "Vui lòng chọn ảnh.";
				if (openFileDialog.ShowDialog() == true)
                {
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    imageuser = new BitmapImage(fileUri);
                    string sourcefile = openFileDialog.FileName;
                    string resourceUri = "..//..//Image//" + System.IO.Path.GetFileName(openFileDialog.FileName);
                    var list1 = DataProvider.Ins.DB.Users.Where(x => x.IMAGEUSER == resourceUri);
                    if(list1!=null)
                    {

                    }
                    else
                    {
                        System.IO.File.Copy(sourcefile, resourceUri, true);
                    }

                }
            });

            UpdateImfomation = new RelayCommand<object>((p) =>
            {
                var displayList = DataProvider.Ins.DB.Users.Where(x => x.HOTEN==hoten&& x.USERNAME==username 
                &&x.SDT==sdt&&x.IMAGEUSER==imageuser.ToString()&&x.DIACHI==diachi&&x.EMAIL==email&&x.NGAYSINH==ngaysinh);
                if (displayList != null && displayList.Count() != 0)//Nếu điền đủ thông tin thì cập nhật
                    return true;
                return false;

            }, (p) =>
            {
                var User = DataProvider.Ins.DB.Users.Where(x => x.USERNAME == username).SingleOrDefault();
                User.SDT = sdt;
                User.HOTEN = hoten;
                User.DIACHI = diachi;
                User.NGAYSINH = ngaysinh;
                User.EMAIL = email;
                User.IMAGEUSER = imageuser.ToString();
				DataProvider.Ins.DB.SaveChanges();
				MessageBoxCustom mess = new MessageBoxCustom("Cập nhật thành công!", MessageType.Info, MessageButtons.Ok);
                mess.ShowDialog();
                List = new ObservableCollection<User>(DataProvider.Ins.DB.Users);
            });

            ChangePassword = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                ChangePasswordView pass=new ChangePasswordView();
                pass.ShowDialog();

            });

        }
    }
}
