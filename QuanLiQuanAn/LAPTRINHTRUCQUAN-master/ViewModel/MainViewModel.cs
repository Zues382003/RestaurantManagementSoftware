using CustomMessageBox;
using FontAwesome.Sharp;
using QUANLICAPHE.Model;
using QUANLICAPHE.View;
using QUANLICAPHE.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QUANLICAPHE.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        private BaseViewModel _currentChildView;
        private string _caption;
        private IconChar _icon;

        private BitmapImage _imageuser = new BitmapImage(new Uri("pack://application:,,,/QUANLICAPHE;component/Image/pngtree-vector-businessman-icon-png-image_924876.jpg"));
        public BitmapImage imageuser { get => _imageuser; set { _imageuser = value; OnPropertyChanged(); } }


        public BaseViewModel CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }


        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        private string _nam;
        public string name
        {
            get
            {
                return _nam;
            }
            set
            {
                _nam = value;
                OnPropertyChanged();
            }
        }
        private bool _defaultCheck;
        public bool defaultCheck
        {
            get
            {
                return _defaultCheck;
            }
            set
            {
                _defaultCheck = value;
                OnPropertyChanged();
            }
        }
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowMenuViewCommand { get; }
        public ICommand ShowOrdersViewCommand { get; }
        private int _type;
        public int type { get => _type; set { _type = value; OnPropertyChanged(); } }

        private string _stringType;
        public string stringType { get => _stringType; set { _stringType = value; OnPropertyChanged(); } }

        public string _hoten;
        public string hoten { get => _hoten; set { _hoten = value; OnPropertyChanged(); } }

        public string _sdt;
        public string sdt { get => _sdt; set { _sdt = value; OnPropertyChanged(); } }

        public string _email;
        public string email { get => _email; set { _email = value; OnPropertyChanged(); } }

        public string _diachi;
        public string diachi { get => _diachi; set { _diachi = value; OnPropertyChanged(); } }

        private bool _Visibility;
        public bool Visibility { get => _Visibility; set { _Visibility = value; OnPropertyChanged(); } }
        public ICommand ShowCustomerViewCommand { get; }
        public ICommand ShowTableViewCommand { get; }
        public ICommand LoadedWindowCommand { get; }
        public ICommand ShowAccountViewCommand { get; }
        public ICommand ShowPaymentCommand { get; }
		public ICommand ShowDiaryViewCommand { get; }
		public ICommand ShowRegister { get; }
        public ICommand CloseCommand { get; set; }

        public bool IsLoaded = false;
        public MainViewModel()

        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                OnPropertyChanged(nameof(LoadedWindowCommand));
                IsLoaded = true;
                if (p == null)
                    return;
                p.Hide();
                LoginView LoginWindow = new LoginView();
                LoginWindow.ShowDialog();
                if (LoginWindow.DataContext == null)
                    return;
                var loginVM = LoginWindow.DataContext as LoginViewModel;
                var list1 = new ObservableCollection<User>(DataProvider.Ins.DB.Users);
                foreach (var item in list1)
                {
                    if (item.DANGNHAP == 1)
                    {
                        name = item.USERNAME;
                        type = item.TYPE;
                        imageuser = new BitmapImage(new Uri(item.IMAGEUSER));
                        hoten = item.HOTEN;
                        sdt = item.SDT;
                        email = item.EMAIL;
                        diachi = item.DIACHI;
                    }
                }
                if (loginVM.IsLogin)
                {
                    if (type == 0)
                    {
                        stringType = "Staff";
                        Visibility = false;
                    }
                    if (type == 1)
                    {
                        stringType = "Admin";
                        Visibility = true;

                    }
                    p.Show();
                    ShowHomeViewCommand.Execute(null);
                    defaultCheck = true;
                    MessageBoxCustom mess = new MessageBoxCustom("Chào mừng bạn đã trở lại với Suki Zuki!", MessageType.Info, MessageButtons.Ok);
                    mess.ShowDialog();
                }
                else
                {
                    p.Close();
                }
            }
              );

            //ShowHome 
            ShowHomeViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new HomeViewModel();
                Caption = "TRANG CHỦ";
                Icon = IconChar.Home;
            });

            //ShowMenu 
            ShowMenuViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new MenuViewModel();
                Caption = "THỰC ĐƠN";
                Icon = IconChar.Bars;
            });
            ShowPaymentCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new PayViewModel();
                Caption = "THANH TOÁN";
                Icon = IconChar.CashRegister;
            });

            //ShowOrders 
            ShowOrdersViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new OrdersViewModel();
                Caption = "LỊCH SỬ";
                Icon = IconChar.ClockRotateLeft;
            });

            //ShowCustomer 
            ShowCustomerViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new CustomerViewModel();
                Caption = "KHÁCH HÀNG";
                Icon = IconChar.UserGroup;
            });

            //ShowTable 
            ShowTableViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new TableViewModel();
                Caption = "BÀN";
                Icon = IconChar.Couch;
            });

            //ShowAccount 
            ShowAccountViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new AccountViewModel();
                Caption = "TÀI KHOẢN";
                Icon = IconChar.PersonCircleCheck;
                var displayList = DataProvider.Ins.DB.Users;
            });

            //ShowRegister
            ShowRegister = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new RegisterViewModel();
                Caption = "ĐĂNG KÝ  ";
                Icon = IconChar.Person;
            });

			ShowDiaryViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
			{
				CurrentChildView = new DiaryViewModel();
				Caption = "NHẬT KÍ";
				Icon = IconChar.Book;
			});

			CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                var w = p;
                if (w != null)
                {
                    w.Close();
                }
            }
            );
        }
        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}