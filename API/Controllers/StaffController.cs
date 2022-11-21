using AutoMapper;
using AutoMapper.QueryableExtensions;
using PotShop.API.Data;
using PotShop.API.Helpers;
using PotShop.API.Models.Entities;
using PotShop.API.Services;
using PotShop.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicies.RequireManager)]
    [Route("[controller]")]
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public StaffController(ILogger<StaffController> logger, ApiDbContext context, IMapper mapper, UserManager<ApiUser> userManager, IMailService mailService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStaff()
        {
            IQueryable<Staff> request = _context.Staff;

            if (!User.IsAdmin())
            {
                //Check user access here.
                _logger.LogWarning("We need to check the users permissions");
                //request = request.Where(x => x.CompanyId == User.GetCompanyId());
            }

            var staff = await request.ProjectTo<StaffViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new OkObjectResult(staff);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaff(string id)
        {
            var staff = await _context.Staff
                .Where(x => x.Id == id)
                .ProjectTo<StaffViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (staff == null || (!User.IsAdmin() && _context.StaffAccess.Where(x => x.StaffId == User.GetUserId()).FirstOrDefault().Location.Id != staff.Location.Id))
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(staff);
        }

        [HttpPut]
        public async Task<IActionResult> ItemPut([FromBody] StaffEditViewModel viewModel)
        {
            Staff newStaff;

            if (!User.IsAdmin())
            {
                // additional checks for non-admins
                if (viewModel.Roles?.Any(x => x == AuthRoles.Admin) == true)
                {
                    _logger.LogError("User {username} does not have access to assign the specified roles, or access to the specified company", User.Identity.Name);

                    throw new NotAuthorizedException();
                }
            }

            // check for invalid roles
            if (viewModel.Roles?.Any(x => !AuthRoles.List.Contains(x)) == true)
            {
                _logger.LogError($"Invalid roles in request ({string.Join(", ", viewModel.Roles)})");

                return BadRequest();
            }

            if (viewModel.Id == null)
            {
                if (!User.HasAnyRole(AuthRoles.Manager, AuthRoles.Admin))
                {
                    _logger.LogError("User is not allowed to add users");

                    return BadRequest();
                }

                newStaff = new Staff()
                {
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    Name = viewModel.Name
                };

                if (User.IsAdmin())
                {
                    if (viewModel.LocationId.Length > 0)
                    {
                        newStaff.StaffAccess.Location.Id = Guid.Parse(viewModel.LocationId);
                    }
                    else
                    {
                        return BadRequest("No company ID specified");
                    }
                }
                else
                {
                    // non-admins can only add users to their own organization
                    newStaff.StaffAccess.Location.Id = _context.StaffAccess.Where(x => x.StaffId == User.GetUserId()).FirstOrDefault().Location.Id;
                }

                string userPassword = GeneratePassword();

                IdentityResult result = await _userManager.CreateAsync(newStaff, userPassword);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Unable to create user");
                }

                if (viewModel.Roles != null)
                {
                    await _userManager.AddToRolesAsync(newStaff, viewModel.Roles);
                }

                await _mailService.SendNewUserEmailAsync(newStaff.Email, newStaff.Name ?? newStaff.Email, userPassword);
                
                _logger.LogInformation("New user was added with email {email}", newStaff.Name);
            }
            else
            {
                newStaff = await _context.Staff.FirstOrDefaultAsync(x => x.Id == viewModel.Id);

                if (newStaff == null)
                {
                    _logger.LogError("User with id {userid} not found", viewModel.Id);

                    return NotFound();
                }
                bool isInLocation = _context.StaffAccess.Where(x => x.StaffId == User.GetUserId()).FirstOrDefault()?.Location.Id == newStaff.StaffAccess?.Location.Id;
                //if (!this.UserHasAccess(Guid.Parse(newUser.Id)))
                if(User.IsAdmin() || (User.IsManager() && isInLocation))
                {
                    _logger.LogError("User does not have company access to target user {userid}", newStaff.Id);

                    return NotFound();
                }

                newStaff.Email = viewModel.Email;
                newStaff.UserName = viewModel.Email;
                newStaff.PhoneNumber = viewModel.PhoneNumber;
                newStaff.Name = viewModel.Name;

                _logger.LogInformation($"User {newStaff.Email} with id {newStaff.Id} updated by {User.GetUserId()}");

                var result = await _userManager.UpdateAsync(newStaff);

                if (!result.Succeeded)
                {
                    _logger.LogError("Unable to update user {userid}", viewModel.Id);

                    return BadRequest(result);
                }

                await _userManager.SetRolesAsync(newStaff, viewModel.Roles);

                if (!string.IsNullOrEmpty(viewModel.Password))
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(newStaff);
                    await _userManager.ResetPasswordAsync(newStaff, token, viewModel.Password);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated user {userid}", newStaff.Id);

            return new OkResult();
        }

        [HttpPut("ChangeState/{id}")]
        public IActionResult ChangeStaffState(string id)
        {
            var staff = _context.Staff
                .Where(x => x.Id == id)
                .FirstOrDefault();
            
            if (staff == null)
            {
                return NotFound();
            }

            staff.IsDisabled = !staff.IsDisabled;

            _context.Staff.Update(staff);
            _context.SaveChanges();

            return Ok(staff);
        }

        private static readonly Random Random = new();

        private static string GeneratePassword()
        {
            byte[] buffer = new byte[48];
            Random.NextBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}

