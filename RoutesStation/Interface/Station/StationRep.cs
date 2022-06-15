using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Station
{
    public class StationRep : IStationRep
	{
        private readonly ApplicationDb _db;


        public StationRep(ApplicationDb Db)
        {
            _db = Db;

        }

        public async Task<StatuseModel> Add(ApplicationStationView station)
        {

            try
            {
                var st = await _db.Stations.FirstOrDefaultAsync(x => x.Title_AR == station.Title_AR && x.Title_EN == station.Title_EN);
                if (st!=null&&st.DirectionStation == station.DirectionStation)
                {
                    var faild = new StatuseModel
                    {
                        Status = false,
                        Message = "Canat Add Station in Same Direction"
                    };
                }
                var R = new ApplicationStation
                {
                   Title_AR=station.Title_AR,
                   Title_EN=station.Title_EN,
                   Latitude=station.Latitude,
                   Longitude=station.Longitude,
                   DirectionStation=station.DirectionStation,
                  
                };
                _db.Stations.Add(R);
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch (Exception ex)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return faild;
            }
        }

        public async Task<StatuseModel> Delete(Guid id)
        {
            try
            {
                var R = await _db.Stations.FindAsync(id);
                _db.Stations.Remove(R);
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch (Exception ex)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return faild;
            }
        }

        public async Task<StatuseModel> Edit(ApplicationStationView station)
        {
            try
            {
                

                var st = await _db.Stations.FirstOrDefaultAsync(x =>x.Title_EN == station.Title_EN);
                if (st != null && st.DirectionStation == station.DirectionStation&&st.id!=station.id)
                {
                    var faild = new StatuseModel
                    {
                        Status = false,
                        Message = "Canat Add Station in Same Direction"
                    };
                }
                Console.WriteLine("RR"+station.id);
                var R = await _db.Stations.FindAsync(station.id);
                
                if (R == null)
                {
                    var faild = new StatuseModel
                    {
                        Status = false,
                        Message = "Check Station ID"
                    };
                }
                Console.WriteLine("StationID" + R.Latitude);
                _db.Stations.Attach(R);

                R.Title_AR = station.Title_AR;
                R.Title_EN = station.Title_EN;
                R.DirectionStation = station.DirectionStation;
                R.Latitude = station.Latitude;
                R.Longitude = station.Longitude;
                _db.Entry(R).Property(x => x.Title_AR).IsModified = true;
                _db.Entry(R).Property(x => x.Title_EN).IsModified = true;
                _db.Entry(R).Property(x => x.DirectionStation).IsModified = true;
                _db.Entry(R).Property(x => x.Longitude).IsModified = true;
                _db.Entry(R).Property(x => x.Latitude).IsModified = true;
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch (Exception ex)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return faild;
            }
        }

        public async Task<ApplicationStation> Get(Guid id)
        {
            try
            {
                IQueryable<ApplicationStation> queryable = _db.Set<ApplicationStation>();
                
                var R = await queryable.FirstOrDefaultAsync(x=>x.id==id);

                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new ApplicationStation();
            }
        }

        public async Task<IEnumerable<ApplicationStation>> List()
        {
            try
            {
                IQueryable<ApplicationStation> queryable = _db.Set<ApplicationStation>();
                
                var R = await queryable.OrderBy(x => x.id).ToListAsync();

                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new List<ApplicationStation>();
            }
        }
        public async Task<int> CountStation()
        {
            return await _db.Stations.OrderBy(x => x.id).CountAsync();
        }

        public async Task<IEnumerable<ApplicationStation>> GetByName(string Name)
        {
            try
            {
                IQueryable<ApplicationStation> queryable = _db.Set<ApplicationStation>();

                var R = await queryable.Where(x => x.Title_EN.Contains(Name)).ToListAsync();

                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new List<ApplicationStation>();
            }
        }
    }
    
}

