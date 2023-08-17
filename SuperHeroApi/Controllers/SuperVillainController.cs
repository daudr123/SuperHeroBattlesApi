using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroApi.Dto;
using SuperHeroApi.Models;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class SuperVillainController : ControllerBase
    {

        private readonly DataContext _dataContext;

        public SuperVillainController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperVillainDto>>> Get()
        {
            var villains = await _dataContext.SuperVillains.Select(villain => new SuperVillainDto
            {
                Id = villain.Id,
                Name = villain.Name,
                FirstName = villain.FirstName,
                LastName = villain.LastName,
                Place = villain.Place
            }).ToListAsync();

            return Ok(villains);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SuperVillain>> Get(int id)
        {
            var villain = await _dataContext.SuperVillains.FindAsync(id);
            if (villain == null)
            {
                return BadRequest("Hero not found !");
            }
            return Ok(villain);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperVillain>>> AddVillain(SuperVillain villain)
        {
            _dataContext.SuperVillains.Add(villain);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperVillains.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperVillain>>> UpdateVillain(SuperVillain request)
        {
            var dbvillain = await _dataContext.SuperVillains.FindAsync(request.Id);
            if (dbvillain == null)
            {
                return BadRequest("Hero not found !");

            }

            dbvillain.Name = request.Name;
            dbvillain.FirstName = request.FirstName;
            dbvillain.LastName = request.LastName;
            dbvillain.Place = request.Place;
            dbvillain.Health = request.Health;
            dbvillain.AttackPower = request.AttackPower;


            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperVillains.ToListAsync());
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperVillain>>> DeleteVillain(int id)
        {
            var dbvillain = await _dataContext.SuperVillains.FindAsync(id);
            if (dbvillain == null)
            {
                return BadRequest("Hero not found !");
            }
            _dataContext.SuperVillains.Remove(dbvillain);

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperVillains.ToListAsync());
        }


    }
}
