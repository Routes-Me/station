using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;

namespace RoutesStation.Interface.Bus
{
	public class CompanyRep:ICompanyRep
	{
        private readonly ApplicationDb _db;

        public CompanyRep(ApplicationDb db)
        {
            _db = db;

        }

        public async Task<StatuseModel> Add(ApplicationCompany model)
        {
            _db.Companies.Add(model);
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;
        }

        public async Task<StatuseModel> Delete(Guid id)
        {
            var data = await _db.Companies.FindAsync(id);
            _db.Companies.Remove(data);
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;
        }

        public async Task<StatuseModel> Edite(ApplicationCompany model)
        {
            var data = await _db.Companies.FindAsync(model.id);
            _db.Companies.Attach(data);
            data = model;
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;
        }

        public async Task<ApplicationCompany> Get(Guid id)
        {
            return await _db.Companies.FindAsync(id);
        }

        public async Task<IEnumerable<ApplicationCompany>> List()
        {
            return await _db.Companies.OrderBy(x => x.Company).ToListAsync();
        }
    }
}

