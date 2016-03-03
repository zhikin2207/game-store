using System.Collections.Generic;
using System.Runtime.Serialization;
using GameStore.BLL.DTOs.EntitiesLocalization;

namespace GameStore.BLL.DTOs
{
	[DataContract]
	public class GenreDto
	{
		[DataMember]
		public int GenreId { get; set; }

		[DataMember]
		public int? ParentGenreId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public bool IsReadOnly { get; set; }

		public GenreDto Parent { get; set; }

		public ICollection<GenreDto> Children { get; set; }

		[DataMember]
		public ICollection<GenreLocalizationDto> GenreLocalizations { get; set; }
	}
}
