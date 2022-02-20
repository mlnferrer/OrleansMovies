using GraphQL.Server;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.App
{
	public static class AppGqlExtensions
	{
		public static void AddAppGraphQL(this IServiceCollection services)
		{
			services.AddGraphQL(options =>
				{
					options.EnableMetrics = true;
					options.ExposeExceptions = true;
				})
				.AddNewtonsoftJson();

			services.AddSingleton<ISchema, AppSchema>();
			services.AddSingleton<AppGraphQuery>();
			services.AddSingleton<AppGraphMutation>();

			services.AddSingleton<SampleDataGraphType>();
			services.AddSingleton<MovieDataGraphType>();
		}
	}
}
