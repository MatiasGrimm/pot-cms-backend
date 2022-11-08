using AutoMapper;
using AutoMapper.QueryableExtensions;
using PotShop.API.Data;
using PotShop.API.Helpers;
using PotShop.API.Models.Entities;
using PotShop.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = AuthPolicies.RequireAdmin)]
    public class CompanyController : Controller
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public CompanyController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<CompanyViewModel> Get()
        {
            return _context.Companies
                .ProjectTo<CompanyViewModel>(_mapper.ConfigurationProvider)
                .ToList();
        }

        [HttpGet("{id}")]
        public CompanyDetailsViewModel Get(Guid id)
        {
            return _context.Companies
                .Where(x => x.Id == id)
                .ProjectTo<CompanyDetailsViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault();
        }

        [HttpPut]
        public Guid Update([FromBody] CompanyViewModel viewModel)
        {
            var entity = _mapper.Map<Company>(viewModel);

            _context.Companies.Update(entity);

            _context.SaveChanges();

            return entity.Id;
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var entity = _context.Companies.Find(id);

            if (entity != null)
            {
                _context.Companies.Remove(entity);

                _context.SaveChanges();
            }
        }
    }
}
