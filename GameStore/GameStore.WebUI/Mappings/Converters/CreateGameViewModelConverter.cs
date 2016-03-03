using System.Collections.ObjectModel;
using AutoMapper;
using GameStore.BLL.DTOs;
using GameStore.BLL.DTOs.EntitiesLocalization;
using GameStore.WebUI.ViewModels.Game;

namespace GameStore.WebUI.Mappings.Converters
{
	public class CreateGameViewModelConverter : ITypeConverter<CreateEditGameViewModel, GameDto>
	{
		public GameDto Convert(ResolutionContext context)
		{
			var viewModel = (CreateEditGameViewModel)context.SourceValue;
			var game = Mapper.Map<GameDto>(viewModel.Game);

			game.GameLocalizations = new Collection<GameLocalizationDto>();

			if (viewModel.UkrainianGameLocalization != null)
			{
				var ukrainianLocalization = Mapper.Map<GameLocalizationDto>(viewModel.UkrainianGameLocalization);
				game.GameLocalizations.Add(ukrainianLocalization);
			}

			return game;
		}
	}
}