using LiveCharts.Wpf;
using LiveCharts;
using QUANLICAPHE.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Windows.Ink;
using System.Data.SqlClient;
using MaterialDesignThemes.Wpf.Transitions;
using System.Data;
using System.Reflection.Emit;
using System.Windows.Markup;
using System.Reflection;
using System.Windows.Controls;
using System.Security.AccessControl;
using System.CodeDom.Compiler;
using CustomMessageBox;
using QUANLICAPHE.View;

namespace QUANLICAPHE.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        #region prop
        //PieChart
        private SeriesCollection pieChartSeries;
        public SeriesCollection PieChartSeries
        {
            get => pieChartSeries;
            set
            {
                pieChartSeries = value;
                OnPropertyChanged(nameof(pieChartSeries));
            }
        }

        public DateTime startDateTimePieChart;
        public DateTime StartDateTimePieChart
        {
            get => startDateTimePieChart;
            set
            {
                startDateTimePieChart = value;
                OnPropertyChanged();
            }
        }

        public DateTime endDateTimePieChart;
        public DateTime EndDateTimePieChart
        {
            get => endDateTimePieChart;
            set
            {
                endDateTimePieChart = value;
                OnPropertyChanged();
            }
        }
        //Top Table
        private DataTable topTable;
        public DataTable TopTable
        {
            get => topTable;
            set
            {
                topTable = value;
                OnPropertyChanged(nameof(topTable));
            }
        }
        private DateTime startDateTimeTopTable;
        public DateTime StartDateTimeTopTable
        {
            get { return startDateTimeTopTable; }
            set
            {
                startDateTimeTopTable = value;
                OnPropertyChanged();
            }
        }

        private DateTime endDateTimeTopTable;
        public DateTime EndDateTimeTopTable
        {
            get => endDateTimeTopTable;
            set
            {
                endDateTimeTopTable = value;
                OnPropertyChanged();
            }
        }

        private int monthTopTable;
        public int MonthTopTable
        {
            get => monthTopTable;
            set
            {
                monthTopTable = value;
                OnPropertyChanged(nameof(monthTopTable));
            }
        }

        private int yearTopTable;
        public int YearTopTable
        {
            get => yearTopTable;
            set
            {
                yearTopTable = value;
                OnPropertyChanged(nameof(yearTopTable));
            }
        }
        //Cartesian Chart

        private SeriesCollection incomeByMonthSeries;

        public SeriesCollection IncomeByMonthSeries
        {
            get => incomeByMonthSeries;
            set
            {
                incomeByMonthSeries = value;
                OnPropertyChanged();
            }
        }

        private string[] labels;

        public string[] Labels
        {
            get => labels;
            set
            {
                labels = value;
                OnPropertyChanged();
            }
        }
        private int yearCartesianChart;

        public int YearCartesianChart
        {
            get => yearCartesianChart;
            set
            {
                yearCartesianChart = value; OnPropertyChanged();
            }
        }

        private List<int> yearsCartesianChart;
        public List<int> YearsCartesianChart
        {
            get => yearsCartesianChart;
            set
            {
                yearsCartesianChart = value;
                OnPropertyChanged();
            }
        }
        private string script;
        public string Script
        {
            get => script;
            set
            {
                script = value;
                OnPropertyChanged();
            }
        }

        //This month income
        private double thisMonthIncome;
        public double ThisMonthIncome
        {
            get => thisMonthIncome;
            set
            {
                thisMonthIncome = value;
                OnPropertyChanged();
            }

        }

        private string month;
        public string Month
        {
            get => month;
            set
            {
                month = value;
                OnPropertyChanged();
            }
        }
        //Top product
        private DataTable topProduct;
        public DataTable TopProduct
        {
            get => topProduct;
            set
            {
                topProduct = value;
                OnPropertyChanged();
            }
        }

        //Today income
        private double todayIncome;
        public double TodayIncome
        {
            get => todayIncome;
            set
            {
                todayIncome = value;
                OnPropertyChanged();
            }
        }
        //CONST
        const int monthOfYear = 12;

        #endregion

        #region command
        public ICommand MoveForwardCommand { get; set; }
        public ICommand MoveBackwardCommand { get; set; }
        public ICommand LoadPieChartCommand { get; set; }
        public ICommand LoadTopTableCommand { get; set; }
        public ICommand LoadInputYearsCartesianChartCommand { get; set; }

        #endregion

        #region method
        //PieChart
        public void LoadPieChart(DateTime startDate, DateTime endDate)
        {
            //Test pie chart
            QUANLIQUANANGKEntities1 db = new QUANLIQUANANGKEntities1();

            string conStr = db.Database.Connection.ConnectionString;

            SqlConnection connection = new SqlConnection(conStr);

            connection.Open();

            string query = String.Format("SET DATEFORMAT DMY\nEXEC dbo.IncomeByFoodCategory @s = '{0}',@e = '{1}'", startDate.ToShortDateString(), endDate.ToShortDateString());

            SqlCommand command = new SqlCommand(query, connection);

            DataTable IncomByFoodCategory = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(IncomByFoodCategory);

            connection.Close();
            PieChartSeries = new SeriesCollection();

            try
            {

                foreach (DataRow row in IncomByFoodCategory.Rows)
                {
                    PieChartSeries.Add(new PieSeries() { Title = row["Category"].ToString(), Values = new ChartValues<double> { Convert.ToInt64(row["Total"]) } });
                }
            }
            catch
            {
                //Lấy data qua pie chart



                MessageBoxCustom mess = new MessageBoxCustom("Không có dữ liệu!", MessageType.Warning, MessageButtons.Ok);
                mess.ShowDialog();
            }


        }

        public void LoadPieChart()
        {
            LoadPieChart(Convert.ToDateTime(null), Convert.ToDateTime(null));
        }
        //Cartesian Chart
        public void LoadCartesianChart(int year)
        {
            IncomeByMonthSeries = new SeriesCollection();

            ChartValues<double> data = new ChartValues<double>();

            QUANLIQUANANGKEntities1 db = new QUANLIQUANANGKEntities1();

            double[] Money = new double[12];

            for (int i = 0; i < monthOfYear; i++)
            {
                Money[i] = 0;
            }
            var BILL = from b in db.Bills
                       select new { id = b.id, DateCheckOut = b.DateCheckOut, Total = b.Total, status = b.status };
            //Set Money value

            for (int i = 0; i < monthOfYear; i++)
            {
                foreach (var item in BILL)
                {
                    if (item.DateCheckOut.Value.Month == i + 1 && item.DateCheckOut.Value.Year == year && (item.status == 1 || item.status == 2))
                    {
                        Money[i] += item.Total;
                    }
                }
            }

            //Set data values
            for (int i = 0; i < monthOfYear; i++)
            {
                data.Add(Money[i]);
            }
            //
            IncomeByMonthSeries.Add(new ColumnSeries() { Values = data });
        }
        public void LoadCartesianChart(List<int> years)
        {
            IncomeByMonthSeries = new SeriesCollection();

            List<ChartValues<double>> data = new List<ChartValues<double>>(years.Count);

            QUANLIQUANANGKEntities1 db = new QUANLIQUANANGKEntities1();

            var BILLS = from b in db.Bills select new { b.id, b.DateCheckIn, b.DateCheckOut, b.TableFood, b.Total, b.status };

            foreach (int year in years)
            {
                ChartValues<double> temp = new ChartValues<double>();
                for (int i = 0; i < monthOfYear; i++)
                {
                    temp.Add(0);
                }
                for (int i = 0; i < monthOfYear; i++)
                {
                    foreach (var bill in BILLS)
                    {
                        if (bill.DateCheckOut.Value.Year == year && bill.DateCheckOut.Value.Month == i + 1 && (bill.status == 1 || bill.status == 2))
                        {
                            temp[i] += bill.Total;
                        }
                    }
                }
                data.Add(temp);// last modify
            }

            for (int i = 0; i < years.Count; i++)
            {
                IncomeByMonthSeries.Add(new ColumnSeries { Values = data[i], Title = years[i].ToString() });
            }
        }
        public void LoadScript(List<int> years)
        {
            Script = "\0";
            for (int i = 0; i < years.Count; i++)
            {
                if (i != years.Count - 1)
                {
                    Script += years[i].ToString() + " ";

                }
                else
                {
                    Script += years[i].ToString();

                }
            }

        }
        public void LoadYears(string scp)
        {

        }

        //Today Income
        public void LoadTodayIncome(DateTime date)
        {
            QUANLIQUANANGKEntities1 db = new QUANLIQUANANGKEntities1();
            var BILL = from b in db.Bills
                       select new { id = b.id, DateCheckOut = b.DateCheckOut, Total = b.Total, status = b.status };
            foreach (var item in BILL)
            {
                if (item.status == 2 || item.status == 1)
                {
                    if (item.DateCheckOut.Value.Date == date.Date)
                    {
                        TodayIncome += item.Total;
                    }
                }
            }
        }
        //This Month Income
        public void LoadMonthIncome(int month)
        {
            if (this.IncomeByMonthSeries.Count == 0)
            {
                ThisMonthIncome = 0;
            }
            else
            {
                QUANLIQUANANGKEntities1 db = new QUANLIQUANANGKEntities1();

                string conStr = db.Database.Connection.ConnectionString;

                SqlConnection connection = new SqlConnection(conStr);

                connection.Open();

                string query = String.Format("EXEC dbo.MonthIncome @m = {0}", month);

                SqlCommand command = new SqlCommand(query, connection);

                DataTable MonthIncomeTable = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(MonthIncomeTable);

                connection.Close();

                try
                {
                    ThisMonthIncome = Convert.ToDouble(MonthIncomeTable.Rows[0][0]);
                }
                catch
                {

                }

            }
        }
        //Top Product
        public void LoadTopProduct(int top)
        {

            QUANLIQUANANGKEntities1 db = new QUANLIQUANANGKEntities1();

            string conStr = db.Database.Connection.ConnectionString;

            SqlConnection connection = new SqlConnection(conStr);

            connection.Open();

            string query = String.Format("EXECUTE TOP_SALES @TOP = {0}", top);

            SqlCommand command = new SqlCommand(query, connection);

            DataTable UsersTalbe = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(UsersTalbe);

            connection.Close();

            TopProduct = UsersTalbe;
        }
        //Top Table
        public void LoadTopTable(int top, DateTime startDate, DateTime endDate)
        {
            QUANLIQUANANGKEntities1 db = new QUANLIQUANANGKEntities1();

            string conStr = db.Database.Connection.ConnectionString;

            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();

            string query = String.Format("SET DATEFORMAT DMY\nEXECUTE IncomeByTable @s = '{0}', @e = '{1}'", startDate.ToShortDateString(), endDate.ToShortDateString());

            SqlCommand command = new SqlCommand(query, connection);

            DataTable UserTable = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(UserTable);

            connection.Close();

            TopTable = UserTable;

        }
        #endregion



        public HomeViewModel()
        {

            this.StartDateTimeTopTable = DateTime.ParseExact("1999-01-01", "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
            this.EndDateTimeTopTable = DateTime.Today;


            this.StartDateTimePieChart = DateTime.ParseExact("1999-01-01", "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
            this.EndDateTimePieChart = DateTime.Today;

            this.YearCartesianChart = DateTime.Today.Year;


            //set default TodayIncome, ThisMonthIncome
            this.TodayIncome = 0;
            this.ThisMonthIncome = 0;

            //Loadding

            //LoadScript(this.YearsCartesianChart);
            LoadTopTable(5, StartDateTimeTopTable, EndDateTimeTopTable);

            LoadCartesianChart(YearCartesianChart);

            LoadTodayIncome(DateTime.Now);

            LoadMonthIncome(DateTime.Now.Month);

            LoadTopProduct(4);

            LoadPieChart(StartDateTimePieChart, EndDateTimePieChart);


            Labels = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };

            Month = Labels[DateTime.Now.Month - 1];

            //Command
            

            LoadTopTableCommand = new RelayCommand<object>(
                (p) => {
                    return true;
                },
                (p) =>
                {
                    LoadTopTable(4, StartDateTimeTopTable, EndDateTimeTopTable);
                }
                );

            LoadPieChartCommand = new RelayCommand<object>(
                (p) => {
                    if (StartDateTimePieChart > EndDateTimePieChart)
                        return false;
                    if (EndDateTimePieChart > DateTime.Now)
                        return false;
                    return true;
                },
                (p) =>
                {

                    LoadPieChart(StartDateTimePieChart, EndDateTimePieChart);
                }
                );

            MoveForwardCommand = new RelayCommand<object>(
               (p) =>
               {
                   if (YearCartesianChart == DateTime.Now.Year)
                   {
                       return false;
                   }
                   else
                       return true;
               },
               (p) =>
               {
                   this.YearCartesianChart = this.YearCartesianChart + 1;
                   LoadCartesianChart(this.YearCartesianChart);


               });

            MoveBackwardCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    this.YearCartesianChart--;
                    LoadCartesianChart(this.YearCartesianChart);

                });
        }
    }
}
