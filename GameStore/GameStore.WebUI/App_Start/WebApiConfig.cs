using System.Net.Http.Formatting;
using System.Web.Http;
using GameStore.WebUI.Components;

namespace GameStore.WebUI
{
	public class WebApiConfig
	{
		public const string DefaultLanguage = Language.None;
		public static readonly string RouteLanguageConstraint = string.Join("|", Language.All);

		public static void Register(HttpConfiguration config)
		{
			config.Formatters.Clear();
			config.Formatters.Add(new JsonMediaTypeFormatter());
			config.Formatters.Add(new XmlMediaTypeFormatter());

			config.Formatters.XmlFormatter.AddQueryStringMapping("type", "xml", "text/xml");
			config.Formatters.JsonFormatter.AddQueryStringMapping("type", "json", "application/json");

			config.Routes.MapHttpRoute(
				name: "ApiLangGamesKeyCommentsId",
				routeTemplate: "api/{lang}/games/{key}/comments/{id}",
				defaults: new { action = "Default", id = RouteParameter.Optional, controller = "Comments" },
				constraints: new { lang = RouteLanguageConstraint });

			config.Routes.MapHttpRoute(
				name: "ApiControllerKey",
				routeTemplate: "api/{lang}/{controller}/{key}/{action}",
				defaults: new { lang = DefaultLanguage, key = RouteParameter.Optional, action = "Default" },
				constraints: new { lang = RouteLanguageConstraint }
				);
		}
	}
}