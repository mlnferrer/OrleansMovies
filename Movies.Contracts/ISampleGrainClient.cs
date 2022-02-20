using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface ISampleGrainClient
	{
		Task<SampleDataModel> Get(string id);
		Task Set(string key, string name);
	}
}
