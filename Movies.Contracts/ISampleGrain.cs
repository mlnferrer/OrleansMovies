using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface ISampleGrain : IGrainWithStringKey
	{
		Task<SampleDataModel> Get();
		Task Set(string name);
	}
}