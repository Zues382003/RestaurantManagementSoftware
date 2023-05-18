using QUANLICAPHE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace QUANLICAPHE.ViewModel
{
	public class DiaryViewModel:BaseViewModel
	{
		private ObservableCollection<Food> _foodHistory;
		public ObservableCollection<Food> foodHistory { get => _foodHistory; set { _foodHistory = value; OnPropertyChanged(); } }
		public DiaryViewModel() {
			foodHistory = new ObservableCollection<Food>(DataProvider.Ins.DB.Foods.Where(x => x.deleteFood == 1));
		}
	}
}
