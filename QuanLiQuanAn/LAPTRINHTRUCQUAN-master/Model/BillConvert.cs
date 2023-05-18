using QUANLICAPHE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLICAPHE.Model
{
    public class BillConvert :  BaseViewModel
    {
        public int id { get; set; }
        public System.DateTime DateCheckIn { get; set; }
        public Nullable<System.DateTime> DateCheckOut { get; set; }
        public string idTable { get; set; }
        public string status { get; set; }
        public double Total { get; set; }
        public Nullable<int> PrintHD { get; set; }
        private bool _IsSelected;
        public bool IsSelected { get => _IsSelected; set { _IsSelected = value; OnPropertyChanged(); } }
    }
}
