using Movies.Contracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieClient : IMovieClient
	{
		private readonly IGrainFactory _grainFactory;

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="grainFactory"></param>
		public MovieClient(IGrainFactory grainFactory)
		{
			_grainFactory = grainFactory ?? throw new ArgumentNullException(nameof(grainFactory));
		}

		/// <summary>
		/// Gets the grain from the factory, made sure that the grain with a string key "Movie" is selected
		/// otherwise the preloaded data will not be fetched.
		/// </summary>
		private IMovieGrain Grain => _grainFactory.GetGrain<IMovieGrain>("Movie");

		public Task<List<Movie>> Get() => Grain.Get();

		public Task<List<Movie>> GetTop5() => Grain.GetTop5();

		public Task<List<Movie>> GetByGenre(string genre) => Grain.GetByGenre(genre);

		public Task<Movie> GetSelectedMovieDetail(string key) => Grain.GetSelectedMovieDetail(key);

		public Task<List<Movie>> Search(string searchKey) => Grain.Search(searchKey);

		public Task UpdateMovie(int id, Movie movie) => Grain.UpdateMovie(id, movie);

		public Task<Movie> CreateMovie(Movie movie) => Grain.CreateMovie(movie);
	}
}
