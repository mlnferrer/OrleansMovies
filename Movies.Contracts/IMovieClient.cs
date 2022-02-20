using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieClient
	{
		Task<List<Movie>> Get();

		Task<List<Movie>> GetTop5();

		Task<List<Movie>> GetByGenre(string genre);

		Task<Movie> GetSelectedMovieDetail(string key);

		Task<List<Movie>> Search(string searchKey);

		Task UpdateMovie(int id, Movie movie);

		Task<Movie> CreateMovie(Movie movie);
	}
}
