using QUANLICAPHE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace QUANLICAPHE.ViewModel
{
    public class PrintBillViewModel:BaseViewModel
    {
        private ObservableCollection<BillInfoConvert> _ListBillInfoConvert;
        public ObservableCollection<BillInfoConvert> ListBillInfoConvert { get => _ListBillInfoConvert; set { _ListBillInfoConvert = value; OnPropertyChanged(); } }
        private int _SoHD;
        public int SoHD { get => _SoHD; set { _SoHD = value; OnPropertyChanged(); } }

        private DateTime _time = DateTime.Now;
        public DateTime time { get => _time; set { _time = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private double _Money;
        public double Money { get => _Money; set { _Money = value; OnPropertyChanged(); } }

        public ICommand CloseCommand { get; set; }

        public PrintBillViewModel()
        {
            ListBillInfoConvert = new ObservableCollection<BillInfoConvert>();
            var list2 = new ObservableCollection<BillInfo>(DataProvider.Ins.DB.BillInfoes);

            var list = new ObservableCollection<Bill>(DataProvider.Ins.DB.Bills);
            foreach (var item in list)
            {
                if (item.PrintHD == 1)
                {
                    SoHD = item.id;
                    time=item.DateCheckIn;
                    foreach (var item2 in list2)
                    {
                        if (item2.idBill == SoHD)
                        {
                            
                            var food = DataProvider.Ins.DB.Foods.Where(x => x.id == item2.idFood);
                            foreach(Food itemFood in food)
                            {
                               
                                var BillInfoConvert = new BillInfoConvert() { Name = itemFood.name, Amount = item2.count,Price= itemFood.price, Total = itemFood.price * item2.count, IdBillInfo = SoHD};
                                ListBillInfoConvert.Add(BillInfoConvert);
                                Money+=itemFood.price * item2.count;
                            }
                        }
                        
                    }
                }
            }
            var list1 = new ObservableCollection<User>(DataProvider.Ins.DB.Users);
            foreach (var item in list1)
            {
                if (item.DANGNHAP == 1)
                {
                    DisplayName = item.HOTEN;
                }
            }

            CloseCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                var w = (p);
                if (w != null)
                {
                    w.Close();
                }
            });
        }
    }
}
