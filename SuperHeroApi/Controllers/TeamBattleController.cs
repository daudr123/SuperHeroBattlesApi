using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperHeroApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class TeamBattleController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public TeamBattleController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("heroes/{hero1Id}/{hero2Id}/{hero3Id}/vs/villains/{villain1Id}/{villain2Id}/{villain3Id}")]
        public async Task<ActionResult<string>> StartTeamBattle(int hero1Id, int hero2Id, int hero3Id, int villain1Id, int villain2Id, int villain3Id)
        {
            var heroes = new List<SuperHero>
            {
                _dataContext.SuperHeroes.Find(hero1Id),
                _dataContext.SuperHeroes.Find(hero2Id),
                _dataContext.SuperHeroes.Find(hero3Id)
            };

            var villains = new List<SuperVillain>
            {
                _dataContext.SuperVillains.Find(villain1Id),
                _dataContext.SuperVillains.Find(villain2Id),
                _dataContext.SuperVillains.Find(villain3Id)
            };

            if (heroes.Any(hero => hero == null) || villains.Any(villain => villain == null))
            {
                return BadRequest("One or more heroes/villains not found!");
            }

            int heroTeamWins = 0;
            int villainTeamWins = 0;

            var battleDescriptions = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                string fightDescription = GetSpecialFightDescription(heroes[i].Name, villains[i].Name);
                if (string.IsNullOrEmpty(fightDescription))
                {
                    fightDescription = $"A battle ensues between {heroes[i].Name} and {villains[i].Name}.";
                }

                while (heroes[i].Health > 0 && villains[i].Health > 0)
                {
                    villains[i].Health -= heroes[i].AttackPower;

                    if (villains[i].Health <= 0)
                    {
                        battleDescriptions.Add($"{fightDescription} {heroes[i].Name} emerges victorious!");
                        heroTeamWins++;
                        break;
                    }

                    heroes[i].Health -= villains[i].AttackPower;

                    if (heroes[i].Health <= 0)
                    {
                        battleDescriptions.Add($"{fightDescription} {villains[i].Name} prevails!");
                        villainTeamWins++;
                        break;
                    }
                }

                if (heroes[i].Health > 0 && villains[i].Health > 0)
                {
                    battleDescriptions.Add($"{fightDescription} The battle ends in a draw!");
                }
            }

            string outcome;
            if (heroTeamWins > villainTeamWins)
            {
                outcome = "Heroes";
            }
            else if (heroTeamWins < villainTeamWins)
            {
                outcome = "Villains";
            }
            else
            {
                outcome = "Draw";
            }

            return Ok($"{string.Join("\n\n", battleDescriptions)}\n\nWinning Team: {outcome}");
        }

        private string GetSpecialFightDescription(string heroName, string villainName)
        {
            var specialFightDescriptions = new Dictionary<(string, string), string>
            {
                { ("Batman", "The Joker"), "In the dark and twisted alleys of Gotham, Batman faces off against his arch-nemesis, The Joker. The air is tense as they exchange blows, each trying to outwit the other." },
                { ("Superman", "Zod"), "Above the city, Superman and Zod engage in an earth-shattering battle. The clash of titans sends shockwaves through the skies as they fight for supremacy." },
                { ("Spiderman", "Green Goblin"), "Amidst the towering skyscrapers, Spiderman confronts the Green Goblin in an electrifying showdown. The fate of the city hangs in the balance as their rivalry reaches its peak." },
                { ("Green Lantern", "Sinestro"), "Across the galaxies, Green Lantern and Sinestro collide in an epic clash of willpower and fear. Their constructs light up the cosmic battleground as they fight for control of the universe." },
                { ("Goku", "Frieza"), "In a far-off realm on Planet Namek, Goku and Frieza engage in an explosive battle that shakes the very foundations of the planet. Energy blasts and power punches fill the air as they unleash their full strength, Goku becomes the Legendary Super Saiyan and Frieza achieves his final form, Goku uses his Kamehameha while Frieza unleashes his Death Star attack." },
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
