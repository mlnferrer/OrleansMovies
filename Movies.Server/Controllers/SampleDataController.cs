using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class SampleDataController : Controller
	{
		private readonly ISampleGrainClient _client;

		public SampleDataController(
			ISampleGrainClient client
		)
		{
			_client = client;
		}

		// GET api/sampledata/1234
		[HttpGet("{id}")]
		public async Task<SampleDataModel> Get(string id)
		{
			var result = await _client.Get(id).ConfigureAwait(false);
			return result;
		}

		// POST api/sampledata/1234
		[HttpPost("{id}")]
		public async Task Set([FromRoute] string id, [FromForm] string name)
			=> await _client.Set(id, name).ConfigureAwait(false);
	}
}