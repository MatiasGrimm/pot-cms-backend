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

            if (!User.IsAdmin() && !User.HasPermissions(_context, Access.GetLocation))
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

            if (!User.IsAdmin() && !User.HasPermissions(_context, Access.GetLocation))
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

            if (!User.IsAdmin() && !User.HasPermissions(_context, Access.GetLocation))
            {
                return NotFound();
            }

            location = _context.Locations
                .Where(x => x.Id == Guid.Parse(id))
                .ProjectTo<LocationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return Ok(location);
        }

        [Authorize(Policy = AuthPolicies.RequireAdmin)]
        [HttpPut("ChangeManager")]
        public IActionResult ChangeManager([FromBody] LocationStaffViewModel viewModel)
        {
            Location location;
            Staff staff;

            location = _context.Locations
                .Where(x => x.Id == viewModel.LocationId)
                .FirstOrDefault();
            staff = _context.Staff
                .Where(x => x.Id == viewModel.ManagerId)
                .FirstOrDefault();

            if (location == null || staff == null)
            {
                return NotFound();
            }

            location.Manager = staff;

            _context.Locations.Update(location);

            _context.SaveChanges();

            return Ok();
        }

        /// Id : Product id not the id of InventoryProduct
        [HttpPut("AddInvItem/{Id}")]
        public IActionResult AddInvItem([FromBody] InventoryProductEditViewModel invProduct, Guid id)
        {
            if (!User.IsManager(_context, id) && !User.HasPermissions(_context, Access.UpdateInventory))
            {
                return NotFound();
            }

            Inventory inventory = _context.Inventory
                .Where(x => x.locationId == id)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .FirstOrDefault();

            if (inventory == null)
            {
                inventory = new Inventory()
                {
                    Products = new List<InventoryProduct>()
                };
            }

            if (inventory.Products.Any(x => x.ProductId == invProduct.ProductId))
            {
                inventory.Products.Where(x => x.ProductId == invProduct.ProductId).FirstOrDefault().Quantity += invProduct.Quantity;
            }
            else
            {
                InventoryProduct _invProduct = new InventoryProduct()
                {
                    Quantity = invProduct.Quantity,
                    ProductId = invProduct.ProductId,
                };

                inventory.Products.Add(_invProduct);
            }

            _context.Update(inventory);
            _context.SaveChanges();

            InventoryViewModel _inv = _mapper.Map<InventoryViewModel>(inventory);

            return Ok(_inv);
        }

        [HttpGet("GetInv/{Id}")]
        public IActionResult GetInventory(Guid id)
        {
            if (!User.HasPermissions(_context, Access.GetInventory))
            {
                return NotFound();
            }

            InventoryViewModel inventory = _context.Inventory
                .Where(x => x.Location.Id == id)
                .ProjectTo<InventoryViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return Ok(inventory);
        }

        [HttpPut]
        public IActionResult UpdateLocation(LocationViewModel viewModel)
        {
            if (viewModel == null) return NotFound();

            Location entity = _mapper.Map<Location>(viewModel);

            _context.Locations.Update(entity);

            _context.SaveChanges();

            return Ok(entity);
        }

        [HttpPut("state/{id}")]
        public IActionResult UpdateState(Guid id)
        {
            Location entity = _context.Locations.Where(x => x.Id == id).FirstOrDefault();

            entity.IsDisabled = !entity.IsDisabled;

            _context.Locations.Update(entity);

            _context.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete]
        [Authorize(AuthRoles.Admin)]
        public IActionResult Delete(Guid id)
        {
            Location entity = _context.Locations.Where(x => x.Id == id).FirstOrDefault();
            _context.Locations.Remove(entity);
            return Ok($"Location {entity.Id} with name {entity.Name}, has been deleted from the database");
        }
    }
}

