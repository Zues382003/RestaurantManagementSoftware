//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QUANLICAPHE.Model
{
	using QUANLICAPHE.ViewModel;
	using System;
	using System.Collections.Generic;

	public partial class FoodCategory : BaseViewModel
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public FoodCategory()
		{
			this.Foods = new HashSet<Food>();
		}

		private int _id;
		public int id { get { return _id; } set { _id = value; OnPropertyChanged(); } }

		private string _name;
		public string name { get { return _name; } set { _name = value; OnPropertyChanged(); } }

		private double _deleteFoodCategory;
		public double deleteFoodCategory { get { return _deleteFoodCategory; } set { _deleteFoodCategory = value; OnPropertyChanged(); } }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		private ICollection<Food> _Foods;
		public virtual ICollection<Food> Foods { get { return _Foods; } set { _Foods = value; OnPropertyChanged(); } }
	}
}
