using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Route
{
    public class RouteRep:IRouteRep
    {
        private readonly ApplicationDb _db;
        

        public RouteRep(ApplicationDb Db)
        {
            _db = Db;
            
        }

        public async Task<StatuseModel> Add(ApplicationRouteView route)
        {
            try
            {
                var R = new ApplicationRoute
                {
                    Name_AR=route.Name_AR,
                    Name_EN=route.Name_EN,
                    Area_AR=route.Area_AR,
                    Area_EN=route.Area_EN,
                    From_To_EN=route.From_To_EN,
                    From_To_AR=route.From_To_AR,
                    Price=route.Price,
                    company=route.company
                };
                _db.Routes.Add(R);
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch(Exception ex)
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
                var R = await _db.Routes.FindAsync(id);
                _db.Routes.Remove(R);
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

        public async Task<StatuseModel> Edit(ApplicationRouteView route, Guid id)
        {
            try
            {
                var R = await _db.Routes.FindAsync(id);
                _db.Routes.Attach(R);
                R.Name_AR = route.Name_AR;
                R.Name_EN = route.Name_EN;
                R.Area_AR = route.Area_AR;
                R.Area_EN = route.Area_EN;
                R.From_To_EN = route.From_To_EN;
                R.From_To_AR = route.From_To_AR;
                R.Price = route.Price;
                R.company = route.company;
                _db.Entry(R).Property(x => x.Name_AR).IsModified = true;
                _db.Entry(R).Property(x => x.Name_EN).IsModified = true;
                _db.Entry(R).Property(x => x.Area_AR).IsModified = true;
                _db.Entry(R).Property(x => x.Area_EN).IsModified = true;
                _db.Entry(R).Property(x => x.Price).IsModified = true;
                _db.Entry(R).Property(x => x.company).IsModified = true;
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

        public async Task<ApplicationRoute> Get(Guid id)
        {
            try
            {
                var R = await _db.Routes.FindAsync(id);
                
                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new ApplicationRoute();
            }
        }

        public async Task<IEnumerable<ApplicationRoute>> List()
        {
            try
            {
                var R = await _db.Routes.OrderBy(x=>x.id).ToListAsync();

                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return  new List<ApplicationRoute>();
            }
        }
        public async Task<int> CountRoute()
        {
            return await _db.Routes.OrderBy(x => x.id).CountAsync();
        }
    }
}
