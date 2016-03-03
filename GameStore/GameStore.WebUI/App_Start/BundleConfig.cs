using System.Web.Optimization;

namespace GameStore.WebUI
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			RegisterScriptBundles(bundles);
			RegisterStyleBundles(bundles);
		}

		private static void RegisterScriptBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery")
				.Include("~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui")
				.Include("~/Scripts/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval")
				.Include("~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap")
				.Include("~/Scripts/bootstrap*"));

			bundles.Add(new ScriptBundle("~/bundles/languageControlPlugin")
				.Include("~/Scripts/Plugins/jquery.languageControl.js"));

			bundles.Add(new ScriptBundle("~/bundles/genreControlPlugin")
				.Include("~/Scripts/Plugins/jquery.genreControl.js"));
		}

		private static void RegisterStyleBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/content/bootstrap")
				.Include("~/Content/bootstrap*", "~/Content/bootstrap-validation.css"));

			bundles.Add(new StyleBundle("~/content/css")
				.Include("~/Content/bootstrap-validation.css", "~/Content/site.css"));
		}
	}
}