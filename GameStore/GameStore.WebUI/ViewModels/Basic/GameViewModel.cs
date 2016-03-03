using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	[DataContract]
	public class GameViewModel
	{
		[DataMember]
		[Required]
		[StringLength(100)]
		[RegularExpression("^([A-z0-9]+-*)+[A-z0-9]+$")]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Key")]
		public string Key { get; set; }

		[DataMember]
		[HiddenInput]
		public int? PublisherId { get; set; }

		[DataMember]
		[Required]
		[StringLength(255)]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Name")]
		public string Name { get; set; }

		public string DisplayName { get; set; }

		[DataMember]
		[Required]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Description")]
		public string Description { get; set; }

		public string DisplayDescription { get; set; }

		[DataMember]
		[Range(0, 1000)]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Price")]
		public decimal Price { get; set; }

		[DataMember]
		[Range(0, 1000)]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_UnitsInStock")]
		public short UnitsInStock { get; set; }

		[DataMember]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Discontinued")]
		public bool Discontinued { get; set; }

		[DataMember(IsRequired = true)]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_DatePublished")]
		public DateTime DatePublished { get; set; }

		public bool IsReadOnly { get; set; }

		public bool IsDeleted { get; set; }
	}
}