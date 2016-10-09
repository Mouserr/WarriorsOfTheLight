using System.Collections.Generic;
using Assets.Scripts.Core.ConstantsContainers;

namespace Assets.Scripts.Units
{
    public class UnitType : IConstantsContainer
    {
        public const string Hero = "Hero";
        public const string MinionMelee = "MinionMelee";
        public const string MinionRange = "MinionRange";
        public const string MobMelee = "MobMelee";
        public const string MobRange = "MobRange";
        public const string MobBoss = "MobBoss";

        private static readonly List<string> constants = new List<string>
		{ 
			MinionMelee, 
			MinionRange, 
			MobMelee, 
			MobRange, 
			MobBoss, 
   			Hero, 
		};

        public List<string> GetConstants()
        {
            return constants;
        }
    }
}