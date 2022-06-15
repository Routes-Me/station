using RoutesStation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.ModelsView
{
    public class ApplicationBusView
    {

	public Guid id { get; set; }

	

	public Guid? CompanyID { get; set; }

	public Guid? RouteID { get; set; }

	public string PalteNumber { get; set; }

	public string? SocondID { get; set; }

	public string? Kind { get; set; }

	
  }
}