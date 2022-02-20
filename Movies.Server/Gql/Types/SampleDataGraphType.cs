using GraphQL.Types;
using Movies.Contracts;

namespace Movies.Server.Gql.Types
{
	public class SampleDataGraphType : ObjectGraphType<SampleDataModel>
	{
		public SampleDataGraphType()
		{
			Name = "Sample";
			Description = "A sample data graphtype.";

			Field(x => x.Id, nullable: true).Description("Unique key.");
			Field(x => x.Name, nullable: true).Description("Name.");
		}
	}
}