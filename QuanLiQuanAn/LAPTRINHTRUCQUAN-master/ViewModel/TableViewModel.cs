using CustomMessageBox;
using QUANLICAPHE.Model;
using QUANLICAPHE.View;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace QUANLICAPHE.ViewModel
{
	public class TableViewModel : BaseViewModel
	{
		private ObservableCollection<BillInfo> _ListBillinfo;

		public ObservableCollection<BillInfo> ListBillInfo { get => _ListBillinfo; set { _ListBillinfo = value; OnPropertyChanged(); } }

		private ObservableCollection<TableFood> _ListTableFood;
		public ObservableCollection<TableFood> ListTableFood { get => _ListTableFood; set { _ListTableFood = value; OnPropertyChanged(); } }

		private ObservableCollection<TableFood> _ListEmptyTableFood;
		public ObservableCollection<TableFood> ListEmptyTableFood { get => _ListEmptyTableFood; set { _ListEmptyTableFood = value; OnPropertyChanged(); } }

		private ObservableCollection<BillInfoConvert> _ListInfo;
		public ObservableCollection<BillInfoConvert> ListInfo { get => _ListInfo; set { _ListInfo = value; OnPropertyChanged(); } }

		private double _TotalPrice;
		public double TotalPrice { get => _TotalPrice; set { _TotalPrice = value; OnPropertyChanged(); } }


		private TableFood _SelectedTable;
		public TableFood SelectedTable
		{
			get => _SelectedTable;
			set
			{
				_SelectedTable = value;
				OnPropertyChanged();
			}
		}


		private TableFood _SelectedTable1;
		public TableFood SelectedTable1
		{
			get => _SelectedTable1;
			set
			{
				_SelectedTable1 = value;
				OnPropertyChanged();
			}
		}

		private BillInfoConvert _SelectedInfo;

		public BillInfoConvert SelectedInfo
		{
			get => _SelectedInfo;
			set
			{
				_SelectedInfo = value;
				OnPropertyChanged();
			}
		}

		private Bill _SelectedBill;
		public Bill SelectedBill
		{
			get => _SelectedBill;
			set
			{
				_SelectedBill = value;
				OnPropertyChanged();
			}
		}

		public ICommand LoadListBillInfo { get; set; }

		public ICommand AddTable { get; set; }
		public ICommand Split { get; set; }

		public ICommand Checkout { get; set; }

		public ICommand SwitchTable { get; set; }

		public ICommand MergeTable { get; set; }


		public TableViewModel()
		{

			SelectedTable = null;
			SelectedTable1 = null;

			ListEmptyTableFood = new ObservableCollection<TableFood>(DataProvider.Ins.DB.TableFoods);
			ListTableFood = new ObservableCollection<TableFood>(DataProvider.Ins.DB.TableFoods);
			foreach (var item in ListTableFood)
			{
				if (item.Bills.Where(x => x.status == 0).Any())
				{
					Bill temp = item.Bills.Where(x => x.status == 0).First();
					item.idbill = temp.id;
				}
				else item.idbill = 0;
			}

			MergeTable = new RelayCommand<object>((p) =>
			{
				if (SelectedTable1 == null || !SelectedTable1.Bills.Where(x => x.status == 0).Any() || SelectedTable == null)
				{
					return false;
				}
				if (SelectedTable1 != null)
				{
					if (SelectedTable1.id == SelectedTable.id)
						return false;
					else return true;
				}
				else if (ListInfo.Count == 0)
					return false;

				else return true;
			}, (p) =>
			{
				var list = DataProvider.Ins.DB.Bills;
				foreach (Bill item in list)
				{
					item.PrintHD = 0;
				}
				Bill SelectedBill1 = DataProvider.Ins.DB.Bills.Where(x => x.idTable == SelectedTable1.id).Where(x => x.status == 0).FirstOrDefault();
				ListBillInfo = new ObservableCollection<BillInfo>(DataProvider.Ins.DB.BillInfoes.Where(x => x.idBill == SelectedBill1.id && SelectedBill1.status == 0));
				foreach (var item in ListBillInfo)
				{
					item.idBill = SelectedBill.id;
				}
				SelectedBill.Total += SelectedBill1.Total;
				DataProvider.Ins.DB.Bills.Remove(SelectedBill1);
				SelectedTable1.status = "Trống";
				SelectedTable1.idbill = 0;
				MessageBoxCustom mess = new MessageBoxCustom("Đã gộp bàn số " + SelectedTable1.id.ToString() + " vào bàn số " + SelectedTable.id.ToString(), MessageType.Info, MessageButtons.Ok);
				mess.ShowDialog();
				DataProvider.Ins.DB.SaveChanges();
				SelectedTable1 = null;
				ListEmptyTableFood = new ObservableCollection<TableFood>(DataProvider.Ins.DB.TableFoods);
				ListTableFood = new ObservableCollection<TableFood>(DataProvider.Ins.DB.TableFoods);


				TotalPrice = 0;
				ListInfo = null;
			});



			AddTable = new RelayCommand<object>((p) =>
			{
				return true;
			}, (p) =>
			{
				TableFood newtable = new TableFood();
				TableFood lasttable = DataProvider.Ins.DB.TableFoods.OrderByDescending(x => x.id).First();
				newtable.id = lasttable.id + 1;
				newtable.name = "Bàn " + (lasttable.id + 1).ToString();
				newtable.status = "Trống";
				DataProvider.Ins.DB.TableFoods.Add(newtable);
				DataProvider.Ins.DB.SaveChanges();
				ListTableFood.Add(newtable);
				ListEmptyTableFood.Add(newtable);
			});

			SwitchTable = new RelayCommand<object>((p) =>
			{

				if (ListBillInfo == null)
					return false;
				if (SelectedTable1 != null)
				{
					if (SelectedTable1.Bills.Where(x => x.status == 0).Any())
						return false;
					else return true;
				}
				if (SelectedTable1 == null)
					return false;
				else return true;
			}, (p) =>
			{
				Bill PreviousBill = DataProvider.Ins.DB.Bills.Where(x => x.id == SelectedBill.id).SingleOrDefault();

				PreviousBill.idTable = SelectedTable1.id;
				SelectedTable.status = "Trống";
				SelectedTable.idbill = 0;
				SelectedTable1.status = "Có người";
				MessageBoxCustom mess = new MessageBoxCustom("Đã chuyển bàn số " + SelectedTable.id.ToString() + " sang bàn số " + SelectedTable1.id.ToString(), MessageType.Info, MessageButtons.Ok);
				mess.ShowDialog();
				SelectedTable1.idbill = PreviousBill.id;
				DataProvider.Ins.DB.SaveChanges();
				SelectedTable1 = null;
				ListEmptyTableFood = new ObservableCollection<TableFood>(DataProvider.Ins.DB.TableFoods);
				ListTableFood = new ObservableCollection<TableFood>(DataProvider.Ins.DB.TableFoods);

				TotalPrice = 0;
				ListInfo = null;
				ListBillInfo = null;

			});



			Checkout = new RelayCommand<object>((p) =>
			{
				//SelectedTable1 = SelectedTable;
				//if (SelectedTable == null||!SelectedTable.Bills.Where(x=>x.status==0).Any()||SelectedTable1==null)
				//{
				//    return false;
				//}
				if (ListBillInfo == null || ListInfo == null)
					return false;
				else return true;
			}

			, (p) =>
			{
				SelectedBill = DataProvider.Ins.DB.Bills.Where(x => x.idTable == SelectedTable.id && x.status == 0).First();
				Bill PreviousBill = DataProvider.Ins.DB.Bills.Where(x => x.id == SelectedBill.id).SingleOrDefault();
				PreviousBill.status = 1;

				var list = DataProvider.Ins.DB.Bills;
				foreach (Bill item in list)
				{
					item.PrintHD = 0;
				}
				var Bill = DataProvider.Ins.DB.Bills.Where(x => x.id == PreviousBill.id).First();
				Bill.PrintHD = 1;
				DataProvider.Ins.DB.SaveChanges();
				bool? mess = new MessageBoxCustom("Bạn có muốn in hóa đơn không?", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();
				if (mess.Value)
				{
					PrintBillView print = new PrintBillView();
					print.ShowDialog();
				}
				MessageBoxCustom mess1 = new MessageBoxCustom("Thanh toán thành công!", MessageType.Info, MessageButtons.Ok);
				mess1.ShowDialog();
				DataProvider.Ins.DB.SaveChanges();
				ListBillInfo = new ObservableCollection<BillInfo>(DataProvider.Ins.DB.BillInfoes.Where(x => x.idBill == SelectedBill.id && SelectedBill.status == 0));
				//MessageBox.Show("Bạn đã thanh toán bàn số " + SelectedBill.idTable.ToString());
				List<BillInfoConvert> displayed = new List<BillInfoConvert>();
				double totalprice = 0;
				foreach (BillInfo item in ListBillInfo)
				{
					BillInfoConvert info = new BillInfoConvert();
					Food tempfood = DataProvider.Ins.DB.Foods.Where(x => x.id == item.idFood).First();
					info.IdBillInfo = item.id;
					info.Name = tempfood.name;
					info.Amount = item.count;
					info.Total = item.count * tempfood.price;
					totalprice += info.Total;
					displayed.Add(info);
				}
				TotalPrice = totalprice;
				TableFood CurrentTable = DataProvider.Ins.DB.TableFoods.Where(x => x.id == SelectedTable.id).SingleOrDefault();
				CurrentTable.status = "Trống";
				CurrentTable.idbill = 0;
				DataProvider.Ins.DB.SaveChanges();
				ListInfo = new ObservableCollection<BillInfoConvert>(displayed.ToList());
				ListBillInfo = null;



			});

			Split = new RelayCommand<object>((p) =>
			{
				int countSelected = 0;
				if (ListInfo != null)
				{

					foreach (var item in ListInfo)
					{
						if (item.IsSelected)
							countSelected++;
					}
				}
				if (ListBillInfo == null || countSelected == 0 || countSelected == ListBillInfo.Count())
					return false;
				else return true;
			}
			, (p) =>
			{
				Bill newBill = new Bill();
				newBill.idTable = SelectedTable.id;
				newBill.DateCheckOut = DateTime.Now;
				newBill.DateCheckIn = DateTime.Now;
				newBill.status = 1;
				DataProvider.Ins.DB.Bills.Add(newBill);
				DataProvider.Ins.DB.SaveChanges();

				var list = DataProvider.Ins.DB.Bills;
				foreach (Bill item in list)
				{
					item.PrintHD = 0;
				}
				DataProvider.Ins.DB.SaveChanges();



				foreach (var item in ListInfo)
				{
					if (item.IsSelected)
					{
						BillInfo selectedbillInfo = DataProvider.Ins.DB.BillInfoes.Where(x => x.id == item.IdBillInfo).FirstOrDefault();
						selectedbillInfo.idBill = newBill.id;
						newBill.Total += item.Total;
						SelectedBill.Total -= item.Total;
						DataProvider.Ins.DB.SaveChanges();
					}
				}
				var Bill = DataProvider.Ins.DB.Bills.Where(x => x.id == newBill.id).First();
				Bill.PrintHD = 1;
				DataProvider.Ins.DB.SaveChanges();
				bool? result = new MessageBoxCustom("Đã tách đơn \nBạn có muốn in hóa đơn không?", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();
				if (result.Value)
				{
					PrintBillView print = new PrintBillView();
					print.ShowDialog();
				}
				DataProvider.Ins.DB.SaveChanges();
				MessageBoxCustom mess1 = new MessageBoxCustom("Đã tách Bill cho bàn số " + SelectedTable.id.ToString() + "\nSang Bill số: " + newBill.id.ToString(), MessageType.Info, MessageButtons.Ok);
				mess1.ShowDialog();
				ListBillInfo = new ObservableCollection<BillInfo>(DataProvider.Ins.DB.BillInfoes.Where(x => x.idBill == SelectedBill.id));
				List<BillInfoConvert> displayed = new List<BillInfoConvert>();
				double totalprice = 0;
				foreach (BillInfo item in ListBillInfo)
				{
					BillInfoConvert info = new BillInfoConvert();
					Food tempfood = DataProvider.Ins.DB.Foods.Where(x => x.id == item.idFood).First();
					info.IdBillInfo = item.id;
					info.Name = tempfood.name;
					info.Amount = item.count;
					info.Total = item.count * tempfood.price;
					totalprice += info.Total;
					displayed.Add(info);
				}
				TotalPrice = totalprice;
				ListInfo = new ObservableCollection<BillInfoConvert>(displayed.ToList());
			}

	);



			LoadListBillInfo = new RelayCommand<TableFood>((p) =>
			{
				TableFood SelectedTable;
				SelectedTable = DataProvider.Ins.DB.TableFoods.Where(x => x.id == p.id).FirstOrDefault();
				Bill SelectedBill;
				if (DataProvider.Ins.DB.Bills.Where(x => x.idTable == SelectedTable.id && x.status == 0).Any())
				{
					SelectedBill = DataProvider.Ins.DB.Bills.Where(x => x.idTable == SelectedTable.id).First();
					return true;
				}

				else return false;

			}, (p) =>
			{
				SelectedTable = DataProvider.Ins.DB.TableFoods.Where(x => x.id == p.id).First();
				SelectedBill = DataProvider.Ins.DB.Bills.Where(x => x.idTable == SelectedTable.id && x.status == 0).First();
				ListBillInfo = new ObservableCollection<BillInfo>(DataProvider.Ins.DB.BillInfoes.Where(x => x.idBill == SelectedBill.id));
				List<BillInfoConvert> displayed = new List<BillInfoConvert>();
				double totalprice = 0;
				foreach (BillInfo item in ListBillInfo)
				{
					BillInfoConvert info = new BillInfoConvert();
					Food tempfood = DataProvider.Ins.DB.Foods.Where(x => x.id == item.idFood).First();
					info.IdBillInfo = item.id;
					info.Name = tempfood.name;
					info.Amount = item.count;
					info.Total = item.count * tempfood.price;
					totalprice += info.Total;
					displayed.Add(info);
				}
				TotalPrice = totalprice;
				ListInfo = new ObservableCollection<BillInfoConvert>(displayed.ToList());
			});


		}
	}
}
