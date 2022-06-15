using System;
namespace RoutesStation.Models
{
	public class GeneralListWithCount<T> where T : class
	{
		public GeneralListWithCount()
		{
		}
		public int Count { get; set; }

		public T List { get; set; }
	}
}

