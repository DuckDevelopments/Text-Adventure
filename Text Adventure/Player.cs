using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Text_Adventure
{
    [Serializable]
    public class Player
    {
        Random rand = new Random();

        public string name;
        public int id;
        public int coins = 0;
        public int level = 1;
        public int xp = 0;
        public int health = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potions = 5;
        public int weaponValue = 1;

        public int mods = 0;

        public enum PlayerClass {Mage, Archer, Hunter, Soldier};
        public PlayerClass currentClass = PlayerClass.Hunter;

        public int GetHealth()
        {
            int upper = (2 * mods + 5);
            int lower = (mods + 2);
            return rand.Next(lower, upper);
        }
        public int GetPower()
        {
            int upper = (2 * mods + 2);
            int lower = (mods + 1);
            return rand.Next(lower, upper);
        }

        public int GetXP()
        {
            int upper = (20 * mods + 50);
            int lower = (15 + mods + 30);
            return rand.Next(lower, upper);
        }

        public int GetLevelupValue()
        {
            return 100 * level + 400;
        }
        
        public bool CanLevelUp()
        {
            if (xp >= GetLevelupValue())
                return true;
            else
                return false;
        }

        public void LevelUp()
        {
            while(CanLevelUp())
            {
                xp -= GetLevelupValue();
                level++;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Program.Print("Congrats! You have leveled up to level " + level + "!");
            Console.ResetColor();
        }

    }
}
