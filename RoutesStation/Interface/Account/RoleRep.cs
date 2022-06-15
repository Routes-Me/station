﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;

namespace RoutesStation.Interface.Account
{
    public class RoleRep : IRoleRep
    {
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public RoleRep(ApplicationDb db, UserManager<ApplicationUser> manager,
            RoleManager<ApplicationRole> role)
        {
            _db = db;
            _manager = manager;
            _roleManager = role;

        }

        public async Task<StatuseModel> CreateRole()
        {
            var rol = new ApplicationRole
            {
                Name = "superAdmin"
            };

            try
            {
                await _roleManager.CreateAsync(rol);

            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }
            rol = new ApplicationRole
            {
                Name = "Admin"
            };
            try
            {
                await _roleManager.CreateAsync(rol);

            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }

            try
            {
                await CreateSuperAdmin();
                var status = new StatuseModel
                {
                    Status = true,
                    Message = "Success"
                };
                return status;
            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }





        }


        public async Task<StatuseModel> AddRole(string Name)
        {
            var rol = new ApplicationRole
            {
                Name = Name
            };

            try
            {
                await _roleManager.CreateAsync(rol);

            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }
            var statusfinal = new StatuseModel
            {
                Status = true,
                Message = "Success"
            };
            return statusfinal;
        }

        public async Task<StatuseModel> EditeRole(string roleid, string Name)
        {
            var rol = await _roleManager.FindByIdAsync(roleid);
            if (rol == null)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = "This is't Role"
                };
                return status;
            }
            var roluser = await _db.UserRoles.Where(x => x.RoleId == roleid).ToListAsync();
            if (roluser != null && roluser.Count() > 0)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = "Can't Remove This Role Becuse Related with Users"
                };
                return status;
            }
            rol.Name = Name;
            try
            {
                await _roleManager.UpdateAsync(rol);
                var status = new StatuseModel
                {
                    Status = true,
                    Message = "Success"
                };
                return status;
            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }
        }

        public async Task<StatuseModel> RemoveRole(string roleid)
        {
            var rol = await _roleManager.FindByIdAsync(roleid);
            if (rol == null)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = "This is't Role"
                };
                return status;
            }

            if (rol.Name == "superAdmin" || rol.Name == "Admin" || rol.Name == "Teacher" || rol.Name == "User")
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = "Can't Remove This Role This Primary Role"
                };
                return status;
            }
            var roluser = await _db.UserRoles.Where(x => x.RoleId == roleid).ToListAsync();
            if (roluser != null && roluser.Count() > 0)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = "Can't Remove This Role Becuse Related with Users"
                };
                return status;
            }
            try
            {
                await _roleManager.DeleteAsync(rol);
                var status = new StatuseModel
                {
                    Status = true,
                    Message = "Success"
                };
                return status;
            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }
        }
        private async Task<StatuseModel> CreateSuperAdmin()
        {
            var super = new ApplicationUser
            {
                Email = "super@admin.com",
                PasswordHash = "Route@123",
                UserName = "RouteStation",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            try
            {
                var result = await _manager.CreateAsync(super, super.PasswordHash);
                if (result.Succeeded)
                {
                    await _manager.AddToRoleAsync(super, "superAdmin");
                }

            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }


            var Admin = new ApplicationUser
            {
                Email = "admin@admin.com",
                PasswordHash = "Route@123",
                UserName = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            try
            {
                var result1 = await _manager.CreateAsync(Admin, Admin.PasswordHash);
                if (result1.Succeeded)
                {
                    await _manager.AddToRoleAsync(Admin, "Admin");
                }
                var status = new StatuseModel
                {
                    Status = true,
                    Message = "Success"
                };
                return status;
            }
            catch (Exception ex)
            {
                var status = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return status;
            }
        }

        public async Task<ApplicationRole> GetRole(string roleid)
        {
           return await _db.Roles.FindAsync(roleid);
        }
    }
}

