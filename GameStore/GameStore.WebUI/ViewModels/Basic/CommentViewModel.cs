using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	[DataContract]
	public class CommentViewModel
	{
		[DataMember]
		public int CommentId { get; set; }

		[DataMember]
		public int? ParentCommentId { get; set; }

		[DataMember]
		public Guid? UserGuid { get; set; }

		[DataMember]
		public string GameKey { get; set; }

		[Required]
		[DataMember]
		[StringLength(100)]
		[Display(ResourceType = typeof(Resource), Name = "CommentViewModel_Name")]
		public string Name { get; set; }

		[Required]
		[DataMember]
		[Display(ResourceType = typeof(Resource), Name = "CommentViewModel_Body")]
		public string Body { get; set; }

		[DataMember(IsRequired=true)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime Date { get; set; }

		public CommentViewModel Parent { get; set; }

		public ICollection<CommentViewModel> Children { get; set; }
	}
}