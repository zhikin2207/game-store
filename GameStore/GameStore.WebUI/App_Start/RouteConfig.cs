using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.WebUI.Components;

namespace GameStore.WebUI
{
	public class RouteConfig
	{
		public const string DefaultLanguage = Language.None;
		public static readonly string RouteLanguageConstraint = string.Join("|", Language.All);

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.MapRoute(
				name: "GameActions",
				url: "game/{action}",
				defaults: new { controller = "Game", action = "AllGames" },
				constraints: new { action = "(New|AllGames)" });

			routes.MapRoute(
				name: "GameDetails",
				url: "game/{key}",
				defaults: new { controller = "Game", action = "Details" });

			routes.MapRoute(
				name: "GameComments",
				url: "game/{gameKey}/{action}",
				defaults: new { controller = "Comment" },
				constraints: new { action = "(Comments|NewComment)" });

			routes.MapRoute(
				name: "GameBuy",
				url: "game/{gameKey}/{action}",
				defaults: new { controller = "Order" },
				constraints: new { action = "(Buy)" });

			routes.MapRoute(
				name: "GameAction",
				url: "game/{key}/{action}",
				defaults: new { controller = "Game", action = "Details" });

			routes.MapRoute(
				name: "GenreActions",
				url: "genre/{action}",
				defaults: new { controller = "Genre" },
				constraints: new { action = "(Create|Genres)" });

			routes.MapRoute(
				name: "GenreAction",
				url: "genre/{name}/{action}",
				defaults: new { controller = "Genre", action = "Details" });

			routes.MapRoute(
				name: "PublisherActions",
				url: "publisher/{action}",
				defaults: new { controller = "Publisher" },
				constraints: new { action = "(New|Publishers)" });

			routes.MapRoute(
				name: "PublisherAction",
				url: "publisher/{company}/{action}",
				defaults: new { controller = "Publisher", action = "Details" });

			routes.MapRoute(
				name: "Basket",
				url: "basket",
				defaults: new { controller = "Order", action = "Basket" });

			routes.MapRoute(
				name: "Order",
				url: "order/{action}",
				defaults: new { controller = "Order", action = "Details" },
				constraints: new { action = "(Details)" });

			routes.MapRoute(
				name: "Orders",
				url: "orders/{action}",
				defaults: new { controller = "Order" });

			routes.MapRoute(
				name: "UserAction",
				url: "user/{action}",
				defaults: new { controller = "User" },
				constraints: new { action = "(Users)" });

			routes.MapRoute(
				name: "User",
				url: "user/{userGuid}/{action}",
				defaults: new { controller = "User", action = "Details" });

			routes.MapRoute(
				name: "RoleAction",
				url: "role/{action}",
				defaults: new { controller = "Role", action = "Roles" },
				constraints: new { action = "(Create|Roles)" });

			routes.MapRoute(
				name: "RoleNameAction",
				url: "role/{roleId}/{action}",
				defaults: new { controller = "Role", action = "Details" });

			routes.MapRoute(
				name: "DefaultLang",
				url: "{controller}/{action}",
				defaults: new { controller = "Game", action = "AllGames" });

			LocalizeRoutes(routes);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}",
				defaults: new { controller = "Game", action = "AllGames", lang = DefaultLanguage });

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
		}

		private static void LocalizeRoutes(IEnumerable<RouteBase> routes)
		{
			foreach (RouteBase routeBase in routes)
			{
				var route = routeBase as Route;

				if (route == null)
				{
					continue;
				}

				route.Url = "{lang}/" + route.Url;

				// adding default culture 
				if (route.Defaults == null)
				{
					route.Defaults = new RouteValueDictionary();
				}

				route.Defaults.Add("lang", DefaultLanguage);

				// adding constraint for culture param
				if (route.Constraints == null)
				{
					route.Constraints = new RouteValueDictionary();
				}

				route.Constraints.Add("lang", RouteLanguageConstraint);
			}
		}
	}
}