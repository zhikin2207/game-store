using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using GameStore.BLL.Services;
using GameStore.BLL.Services.Interfaces;
using GameStore.DAL.UnitsOfWork;
using GameStore.DAL.UnitsOfWork.Interfaces;
using GameStore.Logging;
using GameStore.Logging.Interfaces;
using GameStore.WebUI;
using GameStore.WebUI.Filters;
using GameStore.WebUI.ViewModelBuilders;
using GameStore.WebUI.ViewModelBuilders.Interfaces;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;
using WebActivatorEx;
using WebApiContrib.IoC.Ninject;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace GameStore.WebUI
{
	public static class NinjectWebCommon
	{
		private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start()
		{
			DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
			Bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		/// Stops the application.
		/// </summary>
		public static void Stop()
		{
			Bootstrapper.ShutDown();
		}

		private static IKernel CreateKernel()
		{
			var kernel = new StandardKernel();
			try
			{
				kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
				kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

				RegisterBindings(kernel);

				GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);

				return kernel;
			}
			catch
			{
				kernel.Dispose();
				throw;
			}
		}

		private static void RegisterBindings(IKernel kernel)
		{
			RegisterViewModelBuilders(kernel);
			RegisterServices(kernel);
			RegisterLoging(kernel);

			kernel.Bind<IGameStoreUnitOfWork>().To<GameStoreUnitOfWork>().InSingletonScope();

			kernel.BindFilter<LogIpFilter>(FilterScope.Global, 0);
			kernel.BindFilter<LocalizationFilter>(FilterScope.Global, 0);
		}

		private static void RegisterLoging(IKernel kernel)
		{
			kernel.Bind<ILoggingService>()
				.To<LoggingService>()
				.WhenInjectedInto<LogIpFilter>()
				.WithConstructorArgument("loggerName", "RequestLogger");

			kernel.Bind<ILoggingService>()
				.To<LoggingService>()
				.WhenInjectedInto<HandleErrorAttribute>()
				.WithConstructorArgument("loggerName", "WebLogger");

			kernel.Bind<ILoggingService>()
				.To<LoggingService>()
				.WithConstructorArgument("loggerName", "WebLogger");
		}

		private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<IGameService>().To<GameService>().InRequestScope();
			kernel.Bind<IPublisherService>().To<PublisherService>().InRequestScope();
			kernel.Bind<IOrderService>().To<OrderService>().InRequestScope();
			kernel.Bind<ICommentService>().To<CommentService>().InRequestScope();
			kernel.Bind<IOperationHistoryService>().To<OperationHistoryService>().InRequestScope();
			kernel.Bind<IUserService>().To<UserService>().InRequestScope();
			kernel.Bind<IGenreService>().To<GenreService>().InRequestScope();
			kernel.Bind<IRoleService>().To<RoleService>().InRequestScope();
		}

		private static void RegisterViewModelBuilders(IKernel kernel)
		{
			kernel.Bind<IGameViewModelBuilder>().To<GameViewModelBuilder>();
			kernel.Bind<IOrderViewModelBuilder>().To<OrderViewModelBuilder>();
			kernel.Bind<IPublisherViewModelBuilder>().To<PublisherViewModelBuilder>();
			kernel.Bind<ICommentViewModelBuilder>().To<CommentViewModelBuilder>();
            kernel.Bind<IGenreViewModelBuilder>().To<GenreViewModelBuilder>();
			kernel.Bind<IUserViewModelBuilder>().To<UserViewModelBuilder>();
			kernel.Bind<IRoleViewModelBuilder>().To<RoleViewModelBuilder>();
		}
	}
}