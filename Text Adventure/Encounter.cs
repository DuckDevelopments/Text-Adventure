using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Adventure
{
    public class Encounters
    {
        public static Random rand = new Random();
        //Encounters Generic


        //Encounters
        public static void FirstEncounter()
        {
            Console.WriteLine("You shove over the large animal, grab a close by rusty dagger and charge at it.");
            Console.WriteLine("It turns...");
            Console.ReadKey();
            Combat(false, "Goose Rouge", 1, 4);
        }
        public static void BasicFightEncounter()
        {
            Console.Clear();
            Console.WriteLine("You turn the corner and see a hulking goose...");
            Console.ReadKey();
            Combat(true, "", 0, 0);
        }
        public static void WizardEncounter()
        {
            Console.Clear();
            Console.WriteLine("The door slowly creeks open. You start looking  around to see if there was anyone in sight.");
            Console.WriteLine("You see a tall goose with a long beard staring at a sculpture.");
            Console.ReadKey();
            if (IsChristmas())
                Combat(false, "Fake Santa Goose", 3, 4);
            else
                Combat(false, "Dark Wizard Goose", 4, 5);
        }
        public static void PuzzelOneEncounter()
        {
            Console.Clear();
            Program.Print("You are walking down a hall. You notice that the floor is in ruins");
            List<char> chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }.ToList();
            List<int> positions = new List<int>();
            char c = chars[rand.Next(0, 10)];
            chars.Remove(c);
            for (int y = 0; y < 4; y++)
            {
                int pos = rand.Next(0, 4);
                positions.Add(pos);
                for (int x = 0; x < 4; x++)
                {
                    if (x == pos)
                        Console.WriteLine(c);
                    else
                        Console.Write(chars[rand.Next(0, 8)]);
                }
                Console.Write("\n");
            }
            Program.Print("Choose your path:  (Type the position of the rune you want to stand on. Not the number.\nLeft to Right)");
            for (int i = 0; i < 4; i++)
            {
                while(true)
                {
                    if(int.TryParse(Console.ReadLine(), out int input) && input < 5 && input > 0)
                    {
                        if(positions[i] == input - 1)
                        {
                            break;
                        }
                        else
                        {
                            Program.Print("Geese darts fly out of the walls! You take 2 damage!");
                            Program.currentPlayer.health -= 2;
                            if (Program.currentPlayer.health <= 0)
                            {
                                Program.Print("You feel sick to you stomach. The poison from the darts slowly takes your life away.");
                                Console.ReadKey();
                                System.Environment.Exit(0);
                            }
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input: Whole numbers 1-4 only!");
                    }
                }
            }
            Program.Print("You have successfully crossed the hallway.");
            Console.ReadKey();
        }

        //Encounte tools
        public static void RandomEncounters()
        {
            switch (rand.Next(0, 3))
            {
                case 0:
                    BasicFightEncounter();
                    break;
                case 1:
                    WizardEncounter();
                    break;
                case 2:
                    PuzzelOneEncounter();
                    break;
            }
        }
        public static void Combat(bool random, string name, int power, int health)
        {
            string n = "";
            int p = 0;
            int h = 0;
            if (random)
            {
                n = GetName();
                p = Program.currentPlayer.GetPower();
                h = Program.currentPlayer.GetHealth();
            }
            else
            {
                n = name;
                p = power;
                h = health;
            }
            while (h > 0)
            {
                Console.Clear();
                Console.WriteLine(n);
                Console.WriteLine("Power: " + p + "/" + "Health: " + h);
                Console.WriteLine("======================");
                Console.WriteLine("| (A)ttack (D)effend |");
                Console.WriteLine("|   (R)un   (H)eal   |");
                Console.WriteLine("======================");
                Console.WriteLine(" Potions: " + Program.currentPlayer.potions + " Health: " + Program.currentPlayer.health);
                string input = Console.ReadLine();
                if(input.ToLower() == "a" || input.ToLower() == "attack")
                {
                    //Attack
                    Console.WriteLine("With your dagger, glide through the air! After you strike, the " + n + " strikes you with a counter blow!");
                    int damage = p - Program.currentPlayer.armorValue;
                    if (damage < 0)
                    {
                        damage = 0;
                    }
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1,4) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Hunter)?2:0);
                    Console.WriteLine("You loose " + damage + " health and deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    //Defend
                    Console.WriteLine("As the " + n + " prepares to strike, you take a defensive stance with your dagger in hand.");
                    int damage = (p/4) - Program.currentPlayer.armorValue;
                    if (damage < 0)
                    {
                        damage = 0;
                    }
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue) / 2 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Soldier)?2:0);
                    Console.WriteLine("You lose " + damage + " health and deal " + attack + " damage");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    //Run
                    if(Program.currentPlayer.currentClass != Player.PlayerClass.Archer && rand.Next(0, 2) == 0)
                    {
                        Program.Print("In attempt to escape the " + n + ", its strike catches your back, causing you to fall to the floor.");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                        {
                            damage = 0;
                        }
                        Program.Print("You lose " + damage + " and cannot escape!");
                        Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        //go to store
                        Program.Print("You dodge the " + n + "'s attack and successfully escape.");
                        Console.ReadKey();
                        Shop.LoadShop(Program.currentPlayer);
                    }
                }
                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                {
                    //Heal
                    if(Program.currentPlayer.potions == 0)
                    {
                        Program.Print("As you desperately look through your bag for a potion, all you find are empty flask.");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                        {
                            damage = 0;
                        }
                        Program.Print("The " + n + " strikes you with a painful slash and you lose " + damage + " health!");
                    }
                    else
                    {
                        Program.Print("You reach into your bag and pull out a glowing, pink flask. You take a long drink it");
                        int potionV = 5 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Mage)?+4 : 0);
                        Program.Print("You gain " + potionV + " health");
                        Program.currentPlayer.health += potionV;
                        Program.currentPlayer.potions--;
                        Program.Print("As you were drinking the potion the " + n + " charged and struck.");
                        int damage = (p / 2) - Program.currentPlayer.armorValue;
                        if (damage < 0)
                        {
                            damage = 0;
                        }
                        Program.Print("You lose " + damage + " health.");
                    }
                }
                if (Program.currentPlayer.health <= 0)
                {
                    Program.Print("The " + n + " stands with pride as it strikes you down! \n You have been slain by the sad " + n);
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();
            }
            int c = rand.Next(10, 50);
            int x = Program.currentPlayer.GetXP();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.Print("You stand victorious. As you stadover the " + n + ", its body dissolves into  " + c + " feather coins!");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Program.Print("Yo have gained " + x + " Xp");
            Program.currentPlayer.coins += c;
            Program.currentPlayer.xp += x;
            Console.ResetColor();

            if (Program.currentPlayer.CanLevelUp())
                Program.currentPlayer.LevelUp();

            Console.ReadKey();
        }

        public static string GetName()
        {
            if(IsChristmas())
            {
                switch (rand.Next(0, 6))
                {
                    case 0:
                        return "Elf Goose";
                    case 1:
                        return "Possesed Santa Duck";
                    case 2:
                        return "Toy Goose";
                    case 3:
                        return "Tree Goose";
                    case 4:
                        return "Forsty The Gooseman";
                    case 5:
                        return "Raindeer Goose";
                }
            }
            switch(rand.Next(0,6))
            {
                case 0:
                    return "Skeleton Goose";
                case 1:
                    return "Undead Goose";
                case 2:
                    return "Goose Fanatic";
                case 3:
                    return "Goose Theif";
                case 4:
                    return "War Goose";
                case 5:
                    return "Depressed Goose";
            }
            return "Goose Rogue";
        }
        public static bool IsChristmas()
        {
            DateTime time = DateTime.Now;
            if (time.Month == 12 && time.Day >= 7)
                return true;
            return false;
        }
    }
}
