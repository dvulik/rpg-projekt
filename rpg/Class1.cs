using System;
using System.Collections.Generic;

namespace rpg
{
    class Dungeon
    {
        public List<Mistnost> Mistnosti { get; private set; }

        public void Nastav()
        {
            Mistnosti = new List<Mistnost>();
            VytvorMistnosti();
        }

        private void VytvorMistnosti()
        {
            Mistnosti.Add(new Mistnost { Nazev = "Prazdna mistnost" });
            Mistnosti.Add(new Mistnost { Nazev = "Temna chodba", Nepritel = new Nepritel("Kostlivec", 10, 2) });
            Mistnosti.Add(new Mistnost { Nazev = "Pokladnice" });
        }
    }

    class Mistnost
    {
        public string Nazev { get; set; }
        public Nepritel Nepritel { get; set; }
    }

    class Hrac
    {
        public string Jmeno { get; set; }
        public int Zdravi { get; set; }
        public int UtokDmg { get; set; }
        public int Mince { get; set; }
        public List<Predmet> Inventar { get; private set; }

        public Hrac()
        {
            Inventar = new List<Predmet>();
            Mince = 0;
        }

        public void Nastav(string jmenoInput)
        {
            Jmeno = jmenoInput;
            Zdravi = 20;
            UtokDmg = 5;
        }

        public int Utok(Nepritel nepritel)
        {
            nepritel.VezmiZraneni(UtokDmg);
            return UtokDmg;
        }

        public void VezmiZraneni(int zraneni)
        {
            Zdravi -= zraneni;
            Console.WriteLine($"{Jmeno} dostal {zraneni} zraneni. Zbyva ti {Zdravi} zdravi.");
        }

        public void PouzijPredmet(string nazevPredmetu)
        {
            if (Inventar.Count == 0)
            {
                Console.WriteLine("Tvuj inventar je prazdny. Nemuzes nic pouzit.");
                Thread.Sleep(1500);
                return;
            }

            var predmet = Inventar.Find(i => i.Nazev == nazevPredmetu);
            if (predmet != null)
            {
                predmet.Pouzij(this);
                Inventar.Remove(predmet);
            }
            else
            {
                Console.WriteLine("Tento predmet nemas.");
            }
        }

        public void PridatPredmet(Predmet predmet)
        {
            Inventar.Add(predmet);
            Console.WriteLine($"Dostal jsi novy predmet: {predmet.Nazev}");
        }
    }

    class Nepritel
    {
        public string Nazev { get; set; }
        public int Zdravi { get; set; }
        public int UtokDmg { get; set; }

        public Nepritel(string nazev, int zdravi, int utokDmg)
        {
            Nazev = nazev;
            Zdravi = zdravi;
            UtokDmg = utokDmg;
        }

        public void Utok(Hrac hrac)
        {
            Console.WriteLine($"{Nazev} utoci a zpusobuje {UtokDmg} zraneni.");
            hrac.VezmiZraneni(UtokDmg);
        }

        public void VezmiZraneni(int zraneni)
        {
            Zdravi -= zraneni;
            Console.WriteLine($"{Nazev} dostal {zraneni} zraneni. Zbyva mu {Zdravi} zdravi.");
        }
    }

    class Predmet
    {
        public string Nazev { get; set; }

        public void Pouzij(Hrac hrac)
        {
            if (Nazev == "Lektvar")
            {
                hrac.VezmiZraneni(-10);
                Console.WriteLine("Pouzil jsi lektvar.");
            }
        }
    }
}
