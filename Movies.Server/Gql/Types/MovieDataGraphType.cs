using GraphQL.Types;
using Movies.Contracts;

namespace Movies.Server.Gql.Types
{
	public class MovieDataGraphType : ObjectGraphType<Movie>
	{
		public MovieDataGraphType()
		{
			Name = "Movie";
			Description = "Movie graphtype.";

			Field(x => x.id, nullable: false).Description("Unique key.");
			Field(x => x.key, nullable: false).Description("Key.");
			Field(x => x.name, nullable: true).Description("Name.");
			Field(x => x.description, nullable: false).Description("Description.");
			Field(x => x.rate, nullable: false).Description("Rate.");
			Field(x => x.length, nullable: true).Description("Length.");
			Field(x => x.genres, nullable: true).Description("Genres.");
			Field(x => x.img, nullable: true).Description("Image.");
		}
	}
}
