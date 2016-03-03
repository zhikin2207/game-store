using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GameStore.BLL.DTOs
{
	[DataContract]
	public class CommentDto
	{
		[DataMember]
		public int CommentId { get; set; }

		[DataMember]
		public int? ParentCommentId { get; set; }

		[DataMember]
		public Guid? UserGuid { get; set; }

		[DataMember]
		public string GameKey { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Body { get; set; }

		[DataMember]
		public DateTime Date { get; set; }

		public CommentDto Parent { get; set; }

		public ICollection<CommentDto> Children { get; set; }
	}
}