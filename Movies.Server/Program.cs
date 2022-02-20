using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Core;
using Movies.Server.Infrastructure;
using Orleans;
using Orleans.Hosting;
using Serilog;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Movies.Grains;

namespace Movies.Server
{
	public class Program
	{
		public static Task Main(string[] args)
		{
			var hostBuilder = new HostBuilder();

			IAppInfo appInfo = null;
			hostBuilder
				.ConfigureHostConfiguration(cfg =>
				{
					cfg.SetBasePath(Directory.GetCurrentDirectory())
						.AddEnvironmentVariables("ASPNETCORE_")
						.AddCommandLine(args);
				})
				.ConfigureServices((ctx, services) =>
				{
					appInfo = new AppInfo(ctx.Configuration);
					Console.Title = $"{appInfo.Name} - {appInfo.Environment}";

					services.AddSingleton(appInfo);
					services.Configure<ApiHostedServiceOptions>(options =>
					{
						options.Port = GetAvailablePort(6600, 6699);
					});

					services.Configure<ConsoleLifetimeOptions>(options =>
					{
						options.SuppressStatusMessages = true;
					});
				})
				.ConfigureAppConfiguration((ctx, cfg) =>
				{
					var shortEnvName = AppInfo.MapEnvironmentName(ctx.HostingEnvironment.EnvironmentName);
					cfg.AddJsonFile("appsettings.json")
						.AddJsonFile($"appsettings.{shortEnvName}.json", optional: true)
						.AddJsonFile("app-info.json")
						.AddEnvironmentVariables()
						.AddCommandLine(args);

					appInfo = new AppInfo(cfg.Build());

					if (!appInfo.IsDockerized) return;

					cfg.Sources.Clear();

					cfg.AddJsonFile("appsettings.json")
						.AddJsonFile($"appsettings.{shortEnvName}.json", optional: true)
						.AddJsonFile("app-info.json")
						.AddEnvironmentVariables()
						.AddCommandLine(args);
				})
				.UseSerilog((ctx, loggerConfig) =>
				{
					loggerConfig.Enrich.FromLogContext()
						.ReadFrom.Configuration(ctx.Configuration)
						.Enrich.WithMachineName()
						.Enrich.WithDemystifiedStackTraces()
						.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext:l}] {Message:lj}{NewLine}{Exception}");

					loggerConfig.WithAppInfo(appInfo);
				})
				.UseOrleans((ctx, builder) =>
				{
					builder
						.UseAppConfiguration(new AppSiloBuilderContext
						{
							AppInfo = appInfo,
							HostBuilderContext = ctx,
							SiloOptions = new AppSiloOptions
							{
								SiloPort = GetAvailablePort(11111, 12000),
								GatewayPort = 30001
							}
						})
						.ConfigureApplicationParts(parts => parts
							.AddApplicationPart(typeof(SampleGrain).Assembly).WithReferences()
						)
						.AddIncomingGrainCallFilter<LoggingIncomingCallFilter>()
					;

				})
				.ConfigureServices((ctx, services) =>
				{
					services.AddHostedService<ApiHostedService>();
				})
				;

			return hostBuilder.RunConsoleAsync();
		}

		private static int GetAvailablePort(int start, int end)
		{
			for (var port = start; port < end; ++port)
			{
				var listener = TcpListener.Create(port);
				listener.ExclusiveAddressUse = true;
				try
				{
					listener.Start();
					return port;
				}
				catch (SocketException)
				{
				}
				finally
				{
					listener.Stop();
				}
			}

			throw new InvalidOperationException();
		}
	}
}