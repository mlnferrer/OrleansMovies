using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Movies.Server.Gql
{
	[Route("api/graphql")]
	[Route("graphql")]
	public class GqlController : Controller
	{
		private readonly IDocumentExecuter _documentExecuter;
		private readonly ISchema _schema;
		private readonly IDiagnosticContext _diagnosticContext;
		private readonly ILogger<GqlController> _logger;

		public GqlController(
			IDocumentExecuter documentExecuter,
			ISchema schema,
			IDiagnosticContext diagnosticContext,
			ILogger<GqlController> logger
		)
		{
			_documentExecuter = documentExecuter;
			_schema = schema;
			_diagnosticContext = diagnosticContext;
			_logger = logger;
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] GqlRequest request)
		{
			if (request == null || !ModelState.IsValid)
				return BadRequest();

			var vars = request.Variables?.ToString();

			var result = await _documentExecuter.ExecuteAsync(opts =>
			{
				opts.Schema = _schema;
				opts.Query = request.Query;
				opts.ThrowOnUnhandledException = false;

				if (!string.IsNullOrEmpty(vars))
					opts.Inputs = vars.ToInputs();
			});

			try
			{
				var perf = result.Perf.ToDictionary(x => $"{x.Category}_{x.Subject?.Camelize()}_elapsed", x => x.Duration);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while building gql perf diagnostics gqlPerf={@gqlPerf}", result.Perf);
			}

			return result.ToActionResult(this, request);
		}
	}
}