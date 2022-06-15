using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Interface.Twillio;
using RoutesStation.Interface.Upload;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Account
{
    public class RegisterRep : IRegisterRep
    {
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IUploudRep _uploudRep;
        private readonly ITwillioRep _twillioRep;

        public RegisterRep(ApplicationDb db, UserManager<ApplicationUser> manager, IUploudRep uploudRep, ITwillioRep twillioRep)
        {
            _db = db;
            _manager = manager;
            _uploudRep = uploudRep;
            _twillioRep = twillioRep;

        }

        public async Task<bool> Activate(string UserID)
        {
            var user = await _db.Users.FindAsync(UserID);
            _db.Users.Attach(user);
            user.LockoutEnabled = false;
            user.LockoutEnd = null;
            await _db.SaveChangesAsync();
            return true;
        }

        

        public async Task<bool> ActiveCode(ApplicationUser user, string Code)
        {
            if (user.Code == Code)
            {
                var data = await _db.Users.FindAsync(user.Id);
                _db.Users.Attach(data);
                data.PhoneNumberConfirmed = true;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Deactivate(string UserID)
        {
            var user = await _db.Users.FindAsync(UserID);
            _db.Users.Attach(user);
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddYears(10);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Edite(ApplicationUser model)
        {
            var user = await _db.Users.FindAsync(model.Id);
            _db.Users.Attach(user);
            user = model;
            await _db.SaveChangesAsync();
            return true;
        }

        

        public async Task<bool> EditeImage(ApplicationImageView imageView, ApplicationUser user)
        {
            if (user == null) return false;
            var data = await _db.Users.FindAsync(user.Id);
            _db.Attach(data);
            if (imageView.Image != null)
            {
                string file = await _uploudRep.AddImageProfile(imageView.Image, "Profile", user.UserName);
                data.Image = file;
            }
            await _db.SaveChangesAsync();
            return true;

        }

        public async Task<bool> RestUser(ApplicationRestUserView model)
        {
            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            _db.Users.Attach(user);
            user.Code = sixDigitNumber;
            await _db.SaveChangesAsync();
            await _twillioRep.SendSMS(user.UserName, sixDigitNumber);
            return true;
        }

        public async Task<bool> Remove(string UserID)
        {
            var user = await _db.Users.FindAsync(UserID);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }

}