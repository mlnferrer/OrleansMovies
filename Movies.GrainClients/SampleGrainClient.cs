using Movies.Contracts;
using Orleans;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class SampleGrainClient : ISampleGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public SampleGrainClient(
			IGrainFactory grainFactory
		)
		{
			_grainFactory = grainFactory;
		}

		public Task<SampleDataModel> Get(string id)
		{
			var grain = _grainFactory.GetGrain<ISampleGrain>(id);
			return grain.Get();
		}

		public Task Set(string key, string name)
		{
			var grain = _grainFactory.GetGrain<ISampleGrain>(key);
			return grain.Set(name);
		}
	}
}