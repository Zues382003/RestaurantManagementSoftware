using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QUANLICAPHE.Utils
{
	class AttachedProperties : DependencyObject
	{

		#region RegisterBlackoutDates

		/// <summary>
		/// Đăng ký một attached property 
		/// </summary>
		/// cách dùng 
		/// <DatePicker utils:AttachedProperties.RegisterBlackoutDates = "{Binding ...}" />
		/// 

		public static DependencyProperty RegisterBlackoutDatesProperty = DependencyProperty.RegisterAttached("RegisterBlackoutDates", typeof(System.Windows.Controls.CalendarBlackoutDatesCollection), typeof(AttachedProperties), new PropertyMetadata(null, OnRegisterCommandBindingChanged));

		//getter và setter mặc định 
		public static void SetRegisterBlackoutDates(UIElement element, CalendarBlackoutDatesCollection value)
		{
			element?.SetValue(RegisterBlackoutDatesProperty, value);
		}
		public static CalendarBlackoutDatesCollection GetRegisterBlackoutDates(UIElement element)
		{
			return (element != null ? (CalendarBlackoutDatesCollection)element.GetValue(RegisterBlackoutDatesProperty) : null);
		}
		/// <summary>
		/// Method sẽ thực hiện khi giá trị binding tại Attached Property này thay đổi 
		/// </summary>
		private static void OnRegisterCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			//chuyển kiểu sender
			DatePicker element = sender as DatePicker;
			if (element != null)
			{
				//chuyển kiểu và lấy giá trị mới tại property mà binding với attached property này
				CalendarBlackoutDatesCollection bindings = e.NewValue as CalendarBlackoutDatesCollection;
				if (bindings != null)
				{
					//Cập nhật BlackoutDates 
					//Xóa BlackoutDates cũ
					element.BlackoutDates.Clear();
					//binding sẽ chỉ chứa 1 phần tử
					foreach (var dateRange in bindings)
					{
						element.BlackoutDates.Add(dateRange);
					}
				}
			}
		}

		#endregion
	}
}
