using AutoMapper;
using GameStore.BLL.Mappings;
using GameStore.DAL.Mappings;
using GameStore.WebUI.Mappings;

namespace GameStore.WebUI
{
	public static class AutoMapperConfig
	{
		public static void RegisterMappings()
		{
			Mapper.Initialize(cfg =>
			{
				cfg.AddProfile<MappingWebProfile>();
				cfg.AddProfile<MappingBllProfile>();
				cfg.AddProfile<MappingDalProfile>();
			});
		}
	}
}