using QUANLICAPHE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLICAPHE.Model
{
    public class BillInfoConvert : BaseViewModel
    {
        private int _IdBillInfo;
        public int IdBillInfo { get=>_IdBillInfo; set { _IdBillInfo = value;OnPropertyChanged(); } }

        private double _Total;
        public double Total { get => _Total; set { _Total = value; OnPropertyChanged(); } }

        private bool _IsSelected;
        public bool IsSelected { get => _IsSelected; set { _IsSelected = value; OnPropertyChanged(); } }


        private double _Price;
        public double Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }

        private int _Amount;
        public int Amount { get => _Amount; set { _Amount = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private int _Delete = 0;
        public int Delete
        {
            get => _Delete; set { _Delete = value; OnPropertyChanged(); }
        }
    }
}
