using System;
using System.Threading;

namespace rpg
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vitej ve hre!");
            Console.Write("Zadej sve jmeno: ");
            string jmenoHrace = Console.ReadLine();

            Hrac hrac = new Hrac();
            hrac.Nastav(jmenoHrace);

            Dungeon dungeon = new Dungeon();
            dungeon.Nastav();

            Predmet lektvarHrac = new Predmet { Nazev = "Lektvar" };
            hrac.PridatPredmet(lektvarHrac);

            Mistnost aktualniMistnost = dungeon.Mistnosti[0];
            int pocetBoju = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Tve jmeno: {hrac.Jmeno} | Tve mince: {hrac.Mince}\n");
                Console.WriteLine($"Jsi v mistnosti: {aktualniMistnost.Nazev}");
                Thread.Sleep(1000);

                if (aktualniMistnost.Nepritel != null)
                {
                    bool platnyInput = false;
                    while (!platnyInput)
                    {
                        Console.WriteLine($"Narazil jsi na {aktualniMistnost.Nepritel.Nazev}! Co chces delat? (bojuj/utec)");
                        string akce = Console.ReadLine().ToLower();

                        if (akce == "bojuj")
                        {
                            platnyInput = true;
                            while (aktualniMistnost.Nepritel.Zdravi > 0 && hrac.Zdravi > 0)
                            {
                                Console.Clear();
                                Console.WriteLine($"Tve zdravi: {hrac.Zdravi} | {aktualniMistnost.Nepritel.Nazev} zdravi: {aktualniMistnost.Nepritel.Zdravi}\n");
                                TahHrace(hrac, aktualniMistnost.Nepritel);

                                if (aktualniMistnost.Nepritel.Zdravi <= 0)
                                {
                                    hrac.Mince += 10;
                                    Console.WriteLine($"Porazil jsi {aktualniMistnost.Nepritel.Nazev} a ziskal 10 minci!");
                                    break;
                                }

                                Console.Clear();
                                Console.WriteLine($"Tve zdravi: {hrac.Zdravi} | {aktualniMistnost.Nepritel.Nazev} zdravi: {aktualniMistnost.Nepritel.Zdravi}\n");
                                TahNepritele(aktualniMistnost.Nepritel, hrac);

                                if (hrac.Zdravi <= 0)
                                {
                                    Console.WriteLine("Byl jsi porazen. Konec hry.");
                                    break;
                                }

                                Console.WriteLine("Stiskni libovolnou klavesu pro pokracovani...");
                                Console.ReadKey();
                            }

                            pocetBoju++;

                            if (pocetBoju >= 2)
                            {
                                Console.WriteLine("Tvuj utok prestal fungovat! Budeš muset pouzit jine metody.");
                                break;
                            }
                        }
                        else if (akce == "utec")
                        {
                            platnyInput = true;
                            Console.WriteLine("Utekl jsi zpet do bezpeci.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Neplatna volba, prosim vyber bojuj nebo utec.");
                        }
                    }
                }
                else if (aktualniMistnost.Nazev == "Pokladnice")
                {
                    Console.WriteLine("Narazil jsi na pokladnici!");
                    int minceNalezeno = 20;
                    hrac.Mince += minceNalezeno;
                    Console.WriteLine($"Nasel jsi {minceNalezeno} minci!");
                    Thread.Sleep(2000);

                    Predmet lektvarPoklad = new Predmet { Nazev = "Lektvar" };
                    hrac.PridatPredmet(lektvarPoklad);
                    Thread.Sleep(2000);
                }

                bool platnyMoveInput = false;
                while (!platnyMoveInput)
                {
                    Console.WriteLine("\nKam se chces pohnout?");
                    Console.WriteLine("1. Pokracuj do dalse mistnosti");
                    Console.WriteLine("2. Konec hry");

                    string moveAkce = Console.ReadLine();
                    if (moveAkce == "1")
                    {
                        int nextRoomIndex = (dungeon.Mistnosti.IndexOf(aktualniMistnost) + 1) % dungeon.Mistnosti.Count;
                        aktualniMistnost = dungeon.Mistnosti[nextRoomIndex];
                        platnyMoveInput = true;
                    }
                    else if (moveAkce == "2")
                    {
                        Console.WriteLine("Konec hry.");
                        platnyMoveInput = true;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Neplatna volba, prosim vyber 1 nebo 2.");
                    }
                }
            }
        }

        static void TahHrace(Hrac hrac, Nepritel nepritel)
        {
            bool platnaAkce = false;
            while (!platnaAkce)
            {
                Console.WriteLine("Co chces delat?");
                Console.WriteLine("1. Utocit");
                Console.WriteLine("2. Pouzit lektvar");
                string akce = Console.ReadLine();

                if (akce == "1")
                {
                    int zraneni = hrac.Utok(nepritel);
                    Console.WriteLine($"Zpusobil jsi {zraneni} zraneni {nepritel.Nazev}.");
                    platnaAkce = true;
                }
                else if (akce == "2")
                {
                    if (hrac.Inventar.Count == 0)
                    {
                        Console.WriteLine("Tvuj inventar je prazdny. Nemuzes nic pouzit.");
                        Thread.Sleep(1500);
                        platnaAkce = true;
                    }
                    else
                    {
                        Console.WriteLine("Zde je tvuj inventar:");
                        for (int i = 0; i < hrac.Inventar.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {hrac.Inventar[i].Nazev}");
                        }

                        Console.WriteLine("Zadej cislo predmetu, ktery chces pouzit:");
                        string itemChoice = Console.ReadLine();
                        if (int.TryParse(itemChoice, out int index) && index > 0 && index <= hrac.Inventar.Count)
                        {
                            hrac.PouzijPredmet(hrac.Inventar[index - 1].Nazev);
                            platnaAkce = true;
                        }
                        else
                        {
                            Console.WriteLine("Neplatna volba.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Neplatna volba, prosim vyber 1 nebo 2.");
                }
            }
        }

        static void TahNepritele(Nepritel nepritel, Hrac hrac)
        {
            Console.WriteLine($"{nepritel.Nazev} se chysta utocit.");
            Thread.Sleep(1000);
            nepritel.Utok(hrac);
        }
    }
}
