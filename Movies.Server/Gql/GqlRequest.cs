using Newtonsoft.Json.Linq;

namespace Movies.Server.Gql
{
	public class GqlRequest
	{
		protected string DebuggerDisplay => $"OperationName: {OperationName}, NamedQuery: '{NamedQuery}', Query: '{Query}'";

		public string OperationName { get; set; }
		public string NamedQuery { get; set; }
		public string Query { get; set; }

		public JObject Variables { get; set; }
	}
}