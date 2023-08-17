using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperHeroApi.Models;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController,Authorize]
    public class FightController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public FightController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("heroes/{heroId}/vs/villains/{villainId}")]
        public ActionResult<string> StartFight(int heroId, int villainId)
        {
            var hero = _dataContext.SuperHeroes.Find(heroId);
            var villain = _dataContext.SuperVillains.Find(villainId);

            if (hero == null || villain == null)
            {
                return BadRequest("Hero or villain not found!");
            }

            if (hero.Place != villain.Place)
            {
                return BadRequest("Hero and villain are not in the same city!");
            }

            string fightDescription = GetSpecialFightDescription(hero.Name, villain.Name);
            if (string.IsNullOrEmpty(fightDescription))
            {
                fightDescription = $"A battle ensues between {hero.Name} and {villain.Name}.";
            }

            // Simulate a back-and-forth fight: hero attacks villain, villain counter-attacks
            while (hero.Health > 0 && villain.Health > 0)
            {
                // Hero attacks villain
                villain.Health -= hero.AttackPower;

                // If villain is defeated
                if (villain.Health <= 0)
                {
                    return Ok($"{fightDescription} {hero.Name} emerges victorious!");
                }

                // Villain counter-attacks hero
                hero.Health -= villain.AttackPower;

                // If hero is defeated
                if (hero.Health <= 0)
                {
                    return Ok($"{fightDescription} {villain.Name} prevails!");
                }
            }

            // This part will only be reached if there is no clear winner
            return Ok($"{fightDescription} The battle ends in a draw!");
        }

        private string GetSpecialFightDescription(string heroName, string villainName)
        {
            var specialFightDescriptions = new Dictionary<(string, string), string>
            {
                { ("Batman", "The Joker"), "In the dark and twisted alleys of Gotham, Batman faces off against his arch-nemesis, The Joker. The air is tense as they exchange blows, each trying to outwit the other." },
                { ("Superman", "Zod"), "Above the city, Superman and Zod engage in an earth-shattering battle. The clash of titans sends shockwaves through the skies as they fight for supremacy." },
                { ("Spiderman", "Green Goblin"), "Amidst the towering skyscrapers, Spiderman confronts the Green Goblin in an electrifying showdown. The fate of the city hangs in the balance as their rivalry reaches its peak." },
                { ("Green Lantern", "Sinestro"), "Across the galaxies, Green Lantern and Sinestro collide in an epic clash of willpower and fear. Their constructs light up the cosmic battleground as they fight for control of the universe." },
                { ("Goku", "Frieza"), "In a far-off realm on Planet Namek, Goku and Frieza engage in an explosive battle that shakes the very foundations of the planet. Energy blasts and power punches fill the air as they unleash their full strength, Goku becomes the Legendary Super Saiyan and frieza achieves his final form, Goku uses his Kamehameha while frieza unleashes his Death star attack." },
                // Add more special fight descriptions here
            };

            if (specialFightDescriptions.TryGetValue((heroName, villainName), out var description))
            {
                return description;
            }

            return null;
        }
    }
}
