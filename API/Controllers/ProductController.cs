using AutoMapper;
using AutoMapper.QueryableExtensions;
using PotShop.API.Data;
using PotShop.API.Helpers;
using PotShop.API.Models.Entities;
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
    [Authorize(Policy = AuthPolicies.RequireManager)]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, ApiDbContext context, IMapper mapper, UserManager<ApiUser> userManager)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetProducts(bool showAll = false)
        {
            List<Product> products;

            products = _context.Products.Where(x => showAll ? true : x.IsDisabled == false).ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(string id)
        {
            var product = _context.Products
                .Where(x => x.Id == Guid.Parse(id))
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return Ok(product);
        }

        [HttpPut]
        public IActionResult UpdateProduct(ProductViewModel viewModel)
        {
            if (viewModel == null) return NotFound();

            var entity = _mapper.Map<Product>(viewModel);

            _context.Products.Update(entity);

            _context.SaveChanges();

            return Ok(entity);
        }

        [HttpPut("state/{id}")]
        public IActionResult UpdateState(Guid id)
        {
            Product entity = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            entity.IsDisabled = !entity.IsDisabled;

            _context.Products.Update(entity);

            _context.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete]
        [Authorize(AuthRoles.Admin)]
        public IActionResult Delete(Guid id)
        {
            Product entity = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            _context.Products.Remove(entity);
            return Ok($"Product {entity.Id} with name {entity.Name}, has been deleted from the database");
        }
    }
}

