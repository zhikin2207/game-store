using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.WebUI.App_LocalResources;

namespace GameStore.WebUI.ViewModels.Basic
{
	public class GenreViewModel
	{
		public int GenreId { get; set; }

		public int? ParentGenreId { get; set; }

		[Required]
		[StringLength(50)]
		[Display(ResourceType = typeof(Resource), Name = "GameViewModel_Name")]
		public string Name { get; set; }

		public string DisplayName { get; set; }

		public bool IsReadOnly { get; set; }

		public GenreViewModel Parent { get; set; }

		public ICollection<GenreViewModel> Children { get; set; }
	}
}