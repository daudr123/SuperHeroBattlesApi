using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroApi.Dto;
using SuperHeroApi.Models;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController,Authorize]
    public class SuperHeroController : ControllerBase
    {

        private readonly DataContext _dataContext;

        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHeroDto>>> Get()
        {
            var heroes = await _dataContext.SuperHeroes.Select(hero => new SuperHeroDto
            {
                Id = hero.Id,
                Name = hero.Name,
                FirstName = hero.FirstName,
                LastName = hero.LastName,
                Place = hero.Place
            }).ToListAsync();

            return Ok(heroes);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                return BadRequest("Hero not found !");
            }
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            var dbhero = await _dataContext.SuperHeroes.FindAsync(request.Id);
            if (dbhero == null)
            {
                return BadRequest("Hero not found !");

            }

            dbhero.Name = request.Name;
            dbhero.FirstName = request.FirstName;
            dbhero.LastName = request.LastName;
            dbhero.Place = request.Place;
            dbhero.Health = request.Health;
            dbhero.AttackPower = request.AttackPower;


            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            var dbhero = await _dataContext.SuperHeroes.FindAsync(id);
            if (dbhero == null)
            {
                return BadRequest("Hero not found !");
            }
            _dataContext.SuperHeroes.Remove(dbhero);

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }


    }
}
