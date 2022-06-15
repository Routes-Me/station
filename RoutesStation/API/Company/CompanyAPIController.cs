using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Bus;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class CompanyAPIController : ControllerBase
    {
        private readonly ICompanyRep _rep;

        public CompanyAPIController(ICompanyRep rep)
        {
            _rep = rep;
        }
        [HttpPost]
        [Route("AddCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> Add(ApplicationCompanyView companyView)
        {
            try
            {
                var c = new ApplicationCompany()
                {
                    Company=companyView.Company,
                };
                var result = await _rep.Add(c);
                if (result.Status) return Ok(new
                {
                    status = true,
                    description = result.Message
                });

                return StatusCode(200, new
                {
                    status = false,
                    description = "Dont Have Any Company"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = ex.ToString()
                });
            }

        }
        [HttpPost]
        [Route("EditCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> Edite(ApplicationCompanyView companyView)
        {
            try
            {
                var data = await _rep.Get(companyView.id.Value);
                if (companyView.Company != null) data.Company = companyView.Company;
                var result = await _rep.Edite(data);
                if (result.Status) return Ok(new
                {
                    status = true,
                    description = result.Message
                });

                return StatusCode(200, new
                {
                    status = false,
                    description = "Dont Have Any Company"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = ex.ToString()
                });
            }

        }
        [HttpPost]
        [Route("DeleteCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(ApplicationRequestId id)
        {
            try
            {

                var result = await _rep.Delete(id.id);

                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Message
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = ex.ToString()
                });
            }

        }
        [HttpPost]
        [Route("GetCompany")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(ApplicationRequestId id)
        {
            try
            {
                var result = await _rep.Get(id.id);

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id = result.id,
                            Company = result.Company,
                        },
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = ex.ToString()
                });
            }
        }
        [HttpPost]
        [Route("ListCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> List(ApplicationPagination pagination)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var result = await _rep.List();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            CompanyName = x.Company,
                        }),
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any company"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = ex.ToString()
                });
            }
        }
    }
}
