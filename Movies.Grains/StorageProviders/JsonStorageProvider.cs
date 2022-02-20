using Orleans;
using System;
using Orleans.Storage;
using Orleans.Runtime;
using System.Threading.Tasks;
using Orleans.Configuration;
using Newtonsoft.Json;
using Movies.Contracts;

namespace Movies.Grains.StorageProviders
{
	public class JsonStorageProvider : IGrainStorage
	{
		public string Name => "MovieJsonStorage";

		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();

		public Task Close() => throw new NotImplementedException();

		public Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			var jsonData = System.IO.File.ReadAllText(@"movies.json");
			grainState.State = JsonConvert.DeserializeObject<MovieDataModel>(jsonData);

			return Task.CompletedTask;
		}

		public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		=> throw new NotImplementedException();
	}
}
