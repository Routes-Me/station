using System;
namespace RoutesStation.ModelReturn
{
	public class ApplicationGeneralListWithCountSum<T> where T : class
	{
		public ApplicationGeneralListWithCountSum()
		{
		}
		public double Sum { get; set; }
		public int Count { get; set; }
		public List<T> List { get; set; }
	}
}

