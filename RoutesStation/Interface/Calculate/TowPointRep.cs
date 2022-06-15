using System;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Calculate
{
	public class TowPointRep: ITowPointRep
	{
        const double PIx = 3.141592653589793;
        const double RADIUS = 6378.16;
        public TowPointRep()
		{
		}

        public async Task<double> Dist(ApplicationTwoPointView pointView)
        {
            try
            {
                double dlon = Radians(pointView.Longitude2 - pointView.Longitude1);
                double dlat = Radians(pointView.Latitude2 - pointView.Latitude1);

                double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(pointView.Latitude1)) * Math.Cos(Radians(pointView.Latitude2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
                double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return angle * RADIUS;
            }catch(Exception ex)
            {
                return 0.0;
            }
        }

        private double Radians(double x)
        {
            try
            {
                return x * PIx / 180;
            }catch(Exception ex)
            {
                return 0.0;
            }
        }
    }
}

