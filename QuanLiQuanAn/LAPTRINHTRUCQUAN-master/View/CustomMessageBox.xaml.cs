using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomMessageBox
{
	public partial class MessageBoxCustom : Window
	{
		public MessageBoxCustom(string Message, MessageType Type, MessageButtons Buttons)//Nhận ba tham số đầu vào là message, loại message và loại nút
		{
			InitializeComponent();
			txtMessage.Content = Message;//Hiển thị đoạn message lên messagebox
			switch (Type)//Kiểm tra loại message được yêu cầu
			{
				case MessageType.Info:
					txtTitle.Text = "Thông báo";
					break;
				case MessageType.Confirmation:
					txtTitle.Text = "Xác nhận";
					break;
				case MessageType.Success:
					{
						string defaultColor = "#c9803c";
						Color bkColor = (Color)ColorConverter.ConvertFromString(defaultColor);
						changeBackgroundThemeColor(Colors.Green);
						txtTitle.Text = "Thành công";
					}
					break;
				case MessageType.Warning:
					txtTitle.Text = "Cảnh báo";
					break;
				case MessageType.Error:
					{
						string defaultColor = "#F44336";
						Color bkColor = (Color)ColorConverter.ConvertFromString(defaultColor);
						changeBackgroundThemeColor(bkColor);
						changeBackgroundThemeColor(Colors.Red);
						txtTitle.Text = "Lỗi";
					}
					break;
			}

			switch (Buttons)//Kiểm tra loại nút được yêu cầu
			{
				case MessageButtons.OkCancel:
					btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
					break;
				case MessageButtons.YesNo:
					btnOk.Visibility = Visibility.Collapsed; btnCancel.Visibility = Visibility.Collapsed;
					break;
				case MessageButtons.Ok:
					btnOk.Visibility = Visibility.Visible;
					btnCancel.Visibility = Visibility.Collapsed;
					btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
					break;
			}
		}
		public void changeBackgroundThemeColor(Color newColor)//Thay đổi màu sắc background theo từng loại messagebox
		{
			cardHeader.Background = new SolidColorBrush(newColor);
			btnClose.Foreground = new SolidColorBrush(newColor);
			btnYes.Background = new SolidColorBrush(newColor);
			btnNo.Background = new SolidColorBrush(newColor);

			btnOk.Background = new SolidColorBrush(newColor);
			btnCancel.Background = new SolidColorBrush(newColor);
		}
		private void btnYes_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void btnNo_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
	}
	public enum MessageType
	{
		Info,
		Confirmation,
		Success,
		Warning,
		Error,
	}
	public enum MessageButtons
	{
		OkCancel,
		YesNo,
		Ok,
	}
}