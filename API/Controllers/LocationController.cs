﻿using AutoMapper;
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
using PotShop.API.Models.Entities.Enums;

namespace PotShop.API.Controllers
{
    [ApiController]
    //[Authorize(Policy = AuthPolicies.RequireManager)]
    [Route("[controller]")]
    public class LocationController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public LocationController(ILogger<UserController> logger, ApiDbContext context, IMapper mapper, UserManager<ApiUser> userManager, IMailService mailService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult GetLocations()
        {
            List<SimpleLocationViewModel> locations;

            if (!User.IsAdmin() || User.HasPermissions(_context, Access.GetLocation))
            {
                return NotFound();
            }

            locations = _context.Locations.Where(x => !x.IsDisabled).ProjectTo<SimpleLocationViewModel>(_mapper.ConfigurationProvider).ToList();
            return Ok(locations);
        }


        [HttpGet("all")]
        public IActionResult GetAllLocations()
        {
            List<SimpleLocationViewModel> locations;

            if (!User.IsAdmin() || User.HasPermissions(_context, Access.GetLocation))
            {
                return NotFound();
            }

            locations = _context.Locations.ProjectTo<SimpleLocationViewModel>(_mapper.ConfigurationProvider).ToList();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public IActionResult GetLocation(string id)
        {
            LocationViewModel location;

            if (!User.IsAdmin() || User.HasPermissions(_context, Access.GetLocation))
            {
                return NotFound();
            }

            location = _context.Locations
                .Where(x => x.Id == Guid.Parse(id))
                .ProjectTo<LocationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return Ok(location);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLocation(LocationViewModel viewModel)
        {
            if (viewModel == null) return NotFound();

            Location entity = _mapper.Map<Location>(viewModel);

            _context.Locations.Update(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpPut("state/{id}")]
        public async Task<IActionResult> UpdateState(Guid id)
        {
            Location entity = _context.Locations.Where(x => x.Id == id).FirstOrDefault();

            entity.IsDisabled = !entity.IsDisabled;

            _context.Locations.Update(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpDelete]
        [Authorize(AuthRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            Location entity = await _context.Locations.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Locations.Remove(entity);
            return Ok($"Location {entity.Id} with name {entity.Name}, has been deleted from the database");
        }
    }
}
