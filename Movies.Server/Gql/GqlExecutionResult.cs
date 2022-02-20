using System;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Server.Gql
{
	public class GqlExecutionResult : ActionResult
	{
		private const string ContentType = "application/json";

		private readonly ExecutionResult _executionResult;
		private readonly int _statusCode;

		public GqlExecutionResult(
			ExecutionResult executionResult,
			int statusCode = StatusCodes.Status200OK
		)
		{
			_executionResult = executionResult ?? throw new ArgumentNullException(nameof(executionResult));
			_statusCode = statusCode;
		}

		public override Task ExecuteResultAsync(ActionContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			var documentWriter = context.HttpContext.RequestServices.GetRequiredService<IDocumentWriter>();

			var response = context.HttpContext.Response;
			response.ContentType = ContentType;
			response.StatusCode = _statusCode;
			return documentWriter.WriteAsync(response.Body, _executionResult);
		}
	}
}
