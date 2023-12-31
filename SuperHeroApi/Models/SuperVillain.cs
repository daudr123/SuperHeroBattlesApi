﻿namespace SuperHeroApi.Models
{
    public class SuperVillain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Place { get; set; } = string.Empty;
        public int Health { get; set; } // Add Health property
        public int AttackPower { get; set; }
    }
}
