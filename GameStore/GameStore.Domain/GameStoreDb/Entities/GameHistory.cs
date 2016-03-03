using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.GameStoreDb.Entities
{
	[Table("GameHistory")]
	public class GameHistory : OperationHistory
	{
		[Required]
		public string GameKey { get; set; }

		public virtual Game Game { get; set; }
	}
}