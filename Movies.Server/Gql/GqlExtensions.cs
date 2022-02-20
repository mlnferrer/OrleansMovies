using System.Net;
using GraphQL;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Core;
using Movies.Core.Web;

namespace Movies.Server.Gql
{
	/// <summary>
	/// GraphQL extensions.
	/// </summary>
	public static class GqlExtensions
	{
		/// <summary>
		/// Converts execution result to <see cref="ActionResult"/> or throw.
		/// </summary>
		/// <param name="result">Result to convert.</param>
		/// <param name="controller">Current controller.</param>
		/// <param name="request"></param>
		/// <returns>Returns action result to return.</returns>
		public static ActionResult ToActionResult(this ExecutionResult result, ControllerBase controller, GqlRequest request)
		{
			if (!(result.Errors?.Count > 0))
				return new GqlExecutionResult(result);

			foreach (var error in result.Errors)
			{
				var actionResult = GetResultForException(error, controller, request);
				if (actionResult != null)
					return actionResult;
			}
			return new GqlExecutionResult(result, StatusCodes.Status400BadRequest);
		}

		private static ActionResult GetResultForException(ExecutionError error, ControllerBase controller, GqlRequest request)
		{
			var exception = error.GetBaseException();
			if (exception is ValidationError)
				return null;

			if (exception is UnauthorizedException uex)
				return controller.StatusCode((int)HttpStatusCode.Forbidden);

			throw new GqlException(exception.Message, exception, request, error.Code);
		}

		/// <summary>
		/// Get valid naming for GQL GraphType when using generics.
		/// <example>
		/// 'PageMetaGraphType`1' => 'PageMeta_Gaming'
		/// </example>
		/// </summary>
		/// <param name="graphType"></param>
		public static string GetDemystifiedGraphName(this INamedType graphType)
			=> graphType.GetType().GetDemystifiedName()
				.Replace("GraphType", "")
				.Replace("<", "_")
				.Replace(">", "");
	}
}
