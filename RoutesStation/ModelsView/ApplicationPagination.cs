using System;
namespace RoutesStation.ModelsView
{
	public class ApplicationPagination
	{
		public ApplicationPagination()
		{
		}
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public Guid? id { get; set; }

		public string? name { get; set; }
	}
}

