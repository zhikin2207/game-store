using System.Web.Routing;
using GameStore.Tests.WebUI.Routes.HttpStubsForRouting;
using GameStore.WebUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameStore.Tests.WebUI.Routes
{
	[TestClass]
	public class RouteTest
	{
		[TestMethod]
		public void Default_Link()
		{
			var context = new StubHttpContextForRouting("~/none/");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("allgames", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("allgames", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_AllGames_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/allgames");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("allgames", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_GameKey_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/game-key");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("game-key", routeData.Values["key"].ToString().ToLower());
			Assert.AreEqual("details", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_GameKey_Remove_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/game-key/remove");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("game-key", routeData.Values["key"].ToString().ToLower());
			Assert.AreEqual("remove", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_GameKey_Update_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/game-key/update");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("game-key", routeData.Values["key"].ToString().ToLower());
			Assert.AreEqual("update", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_GameKey_Comments_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/game-key/comments");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("comment", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("game-key", routeData.Values["gameKey"].ToString().ToLower());
			Assert.AreEqual("comments", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_GameKey_NewComment_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/game-key/newcomment");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("comment", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("game-key", routeData.Values["gameKey"].ToString().ToLower());
			Assert.AreEqual("newcomment", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_GameKey_Download_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/game-key/download");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("game-key", routeData.Values["key"].ToString().ToLower());
			Assert.AreEqual("download", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_GameKey_Buy_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/game-key/buy");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("order", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("game-key", routeData.Values["gameKey"].ToString().ToLower());
			Assert.AreEqual("buy", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Game_New_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/game/new");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("game", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("new", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Publisher_CompanyName_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/publisher/company-name");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("publisher", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("company-name", routeData.Values["company"].ToString().ToLower());
			Assert.AreEqual("details", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Publisher_New_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/publisher/new");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("publisher", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("new", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Basket_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/basket");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("order", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("basket", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void Order_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/order");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("order", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("details", routeData.Values["action"].ToString().ToLower());
		}

		[TestMethod]
		public void User_Ban_Link()
		{
			var context = new StubHttpContextForRouting(requestUrl: "~/none/user/1/ban");
			var routes = new RouteCollection();
			RouteConfig.RegisterRoutes(routes);

			var routeData = routes.GetRouteData(context);

			Assert.IsNotNull(routeData);
			Assert.AreEqual("user", routeData.Values["controller"].ToString().ToLower());
			Assert.AreEqual("ban", routeData.Values["action"].ToString().ToLower());
			Assert.AreEqual("1", routeData.Values["userGuid"].ToString().ToLower());
		}
	}
}