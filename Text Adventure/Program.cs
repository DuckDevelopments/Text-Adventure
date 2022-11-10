using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using System.Reflection.PortableExecutable;

namespace Text_Adventure
{
    public class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;
        public static Random rand = new Random();
        static void Main(string[] args) 
        {
            if(!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            currentPlayer = Load(out bool newP);
            if (newP)
                Encounters.FirstEncounter();
            while (mainLoop)
            {
                Encounters.RandomEncounters();
            }
        }


        static Player NewStart(int i)
        {
            Console.Clear();
            Player p = new Player();
            Print(" Welcome to...\nText Adventure!");
            Print("Name: ");
            p.name = Console.ReadLine();
            p.id = i;
            Print("Class: Mage, Archer, Warrior.");
            bool flag = false;
            while (flag == false)
            {
                flag = true;
                string input = Console.ReadLine().ToLower();
                if (input == "mage")
                    p.currentClass = Player.PlayerClass.Mage;
                else if (input == "archer")
                    p.currentClass = Player.PlayerClass.Archer;
                else if (input == "hunter")
                    p.currentClass = Player.PlayerClass.Hunter;
                else if (input == "soldier")
                    p.currentClass = Player.PlayerClass.Soldier;
                else
                {
                    Print("Please choose a valid class");
                    flag = false;
                }
            }
            p.id = i;
            Console.Clear();
            Print("You awake in a what seems to be a hut. You are very warm and have ");
            Print("no memory of your past.");
            if(currentPlayer.name == "")
            {
                Print("You can't even remember your own name...");
            }
            else
            {
                Print("But you know your name is " + p.name);
            }

            //Wait before closing
            Console.ReadKey();
            Console.Clear();
            Print("You walk around wondering where you are until you find an exit that seems to be blocked.");
            Print("You Put your hands on the blockade and it felt like feathers and is squishy,");
            Print("you continue to push it until it moves foreward. It looks like some sort of large animal");
            Print("standing with its back towards you.");

            Console.ReadKey();
            return p;
        }

        public static void Quit()
        {
            Save();
            Environment.Exit(0);
        }

        public static void Save()
        {
            //creates file but doesn't write any data to the file
            Print("Saving...");
            BinaryFormatter binform = new BinaryFormatter();
            string path = "saves/" + currentPlayer.id.ToString() + ".playersave";
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            binform.Serialize(file, currentPlayer);
            file.Close();
        }
        public static Player Load(out bool newP)
        {
            //can't read save file because save has no data (save function issue)
            newP = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            BinaryFormatter binForm = new BinaryFormatter();
            foreach (string p in paths)
            {
                Console.WriteLine(p);
                FileStream file = File.Open(p, FileMode.Open);
                Player player = (Player)binForm.Deserialize(file);
                file.Close();
                players.Add(player);
            }

            idCount = players.Count;
            
            while (true)
            {
                Console.Clear();
                Print("Choose your player:", 60);

                foreach (Player p in players)
                {
                    Console.WriteLine(p.id + ": " + p.name);                }

                Print("Please input player name or id (id# or playername). Additionally, 'create' will start a new save!");
                string[] data = Console.ReadLine().Split(':');
                try
                {
                    if(data[0] == "id")
                    {
                        if(int.TryParse(data[1], out int id))
                        {
                            foreach (Player player in players)
                            {
                                if(player.id == id)
                                {
                                    return player;
                                }
                                
                            }
                            Print("There is no player with that name!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Print("id invalid. input must be a number! \nPress any key to continue!");
                            Console.ReadKey();
                        }
                    }
                    else if(data[0] == "create")
                    {
                       Player newPlayer = NewStart(idCount);
                        newP = true;
                        return newPlayer;
                    }
                    else
                    {
                        foreach (Player player in players)
                        {
                            if(player.name == data[0])
                            {
                                return player;
                            }
                        }
                        Print("There is no player with that name!");
                        Console.ReadKey();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                   Print("id invalid. input must be a number! \nPress any key to continue!");
                    Console.ReadKey();
                }
            }

        }
        public static void Print(string text, int speed = 48)
        {
            //SoundPlayer soundPlayer = new SoundPlayer("Sounds/type.wav");
            //soundPlayer.Load();
            //soundPlayer.PlayLooping();
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(speed);
            }
            //soundPlayer.Stop();
            Console.WriteLine();
        }
        public static void ProgressBar(string fillerChar, string backroundChar, decimal value, int size)
        {
            int dif = (int)(value * size);
            for(int i = 0; i < size; i++)
            {
                if (i < dif)
                    Console.Write(fillerChar);
                else
                    Console.Write(backroundChar);
            }
        }
    }
}
