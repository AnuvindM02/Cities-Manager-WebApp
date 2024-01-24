using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.Core.Entities;
using CitiesManager.Infrastructure.DatabaseContext;


//This version have only 1 method
namespace CitiesManager.WebAPI.Controllers.v2
{
    [ApiVersion("2.0")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        /// <summary>
        /// To get list of cities (only city name) from cities table
        /// </summary>
        /// <returns>city names in xml format</returns>
        [HttpGet]
        [Produces("application/xml")]//Overrides global producer of app/json (check inside add controllers)
        public async Task<ActionResult<IEnumerable<string?>>> GetCities()
        {
            return await _context.Cities.OrderBy(city => city.CityName)
                .Select(city => city.CityName)
                .ToListAsync();
        }
    }
}
