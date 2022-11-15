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
    public class ProductController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public ProductController(ILogger<UserController> logger, ApiDbContext context, IMapper mapper, UserManager<ApiUser> userManager, IMailService mailService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            if (!User.IsAdmin() || !User.IsManager())
            {
                return NotFound();
            }

            products = _context.Products.Where(x => !x.IsDisabled).ToList();

            return Ok(products);
        }

        [HttpGet("all")]
        public IActionResult GetAllProducts()
        {
            List<Product> products = new List<Product>();

            if (!User.IsAdmin() || !User.IsManager())
            {
                return NotFound();
            }

            products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var product = await _context.Products
                .Where(x => x.Id == Guid.Parse(id))
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            //if (product == null || (!User.IsAdmin() && User.GetLocationId() != product.Location.Id))
            //{
            //    return new NotFoundResult();
            //}

            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductViewModel viewModel)
        {
            if (viewModel == null) return NotFound();

            var entity = _mapper.Map<Product>(viewModel);

            _context.Products.Update(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpPut("state/{id}")]
        public async Task<IActionResult> UpdateState(Guid id)
        {
            Product entity = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            entity.IsDisabled = !entity.IsDisabled;

            _context.Products.Update(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpDelete]
        [Authorize(AuthRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            Product entity = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Products.Remove(entity);
            return Ok($"Product {entity.Id} with name {entity.Name}, has been deleted from the database");
        }
    }
}

