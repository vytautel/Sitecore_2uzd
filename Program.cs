using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Akademija_Sitecore_Lipeikaite_Vytaute_2_uzd
{
    class Program
    {
        const int Cn = 10000;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string[] DuomFailVardai = new string[Cn]; // duomenu failu vardu masyvas
            string[] Tekstai = new string[Cn]; // tekstu masyvas
            int kiek_f = 4; // failu kiekis
            string[] TekstaiSuPasikartojimais = new string[Cn]; // tekstu su daugiausiai pasikartojanciomis raidemis masyvas            
            int kiek2 = 0; // TekstaiSuPasikartojimais masyvo ilgis

            bool f_egzistuoja; // ar duom.failai egzistuojantys

            // Veiksmai tekstu masyvo ivedimui:
            FormuotiVardus(DuomFailVardai, kiek_f);
            SkaitymaiIsFailu(DuomFailVardai, kiek_f, Tekstai, out f_egzistuoja);

            if (f_egzistuoja)
            { 
                if (Tekstai.Count() != 0)
                {
                    TekstaiSuPasikartojimais = TekstaiSuDaugiausiaiPasikartojimu(Tekstai, kiek_f, ref kiek2);
                    SpausdintiIKonsoleMasyva(TekstaiSuPasikartojimais, kiek2);
                }
                else
                {
                    Console.WriteLine("Neįvedėte tekstų failuose...");
                }
            }
        }

        static void FormuotiVardus(string[] DuomFailVardai, int kiek)
        {
            Console.WriteLine("Duomenų failų sąrašas: ");
            for (int i = 0; i < kiek; i++)
            {
                string vardas = "..\\..\\Tekstas" + (i + 1) + ".txt";
                Console.WriteLine("Tekstas" + (i + 1) + ".txt");
                DuomFailVardai[i] = vardas;
            }

            Console.WriteLine();
        }

        static void SkaitymaiIsFailu(string[] DuomFailVardai, int kiek, string[] Tekstai, out bool f_egzistuoja)
        {
            f_egzistuoja = true;
            for ( int i = 0; i < kiek; i++ )
            {
                SkaitymasIsFailo(DuomFailVardai[i], out Tekstai[i], out f_egzistuoja);
            }

            if ( !f_egzistuoja)
                Console.WriteLine("Sukurkite šiuos failus tame pačiame aplanke kaip Program.cs failas ir užpildykite tekstu.");
        }

        // f_egzistuoja - ar reikiamas failas rastas ir nuskaitomas
        static void SkaitymasIsFailo(string failo_vardas, out string tekstas, out bool f_egzistuoja)
        {
            tekstas = "";

            if (File.Exists(failo_vardas))
            {
                f_egzistuoja = true;
                using (StreamReader reader = new StreamReader(failo_vardas, Encoding.GetEncoding(1257)))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        tekstas += line;
                    }
                }
            }
            else
            {
                f_egzistuoja = false;
                Console.WriteLine("Failas {0} nerastas.", failo_vardas);
            }
        }

        // grazina daugiausiai is eiles pasikartojanciu raidziu skaiciu tekste
        static int DaugiausiaiIsEilesPasikartojanciuRaidziu(string tekstas)
        {
            int kiekis = 1; // daugiausiai pasikartojanciu raidziu is eiles kiekis
            int pap_kiekis = 0; // papildomas kiekis skaiciavimams
            string pirma_raide = ""; // pirmoji teksto raide

            while (tekstas.Length > 0)
            {
                pirma_raide = tekstas.Substring(0, 1);
                tekstas = tekstas.Remove(0, 1);

                pap_kiekis++;
                if (pap_kiekis > kiekis)
                    kiekis = pap_kiekis;

                if (tekstas.Length != 0)
                    if (pirma_raide != tekstas.Substring(0, 1))
                        pap_kiekis = 0;
            }

            return kiekis;
        }

        static string[] TekstaiSuDaugiausiaiPasikartojimu(string[] Tekstai, int kiek_f, ref int kiek2)
        {
            string[] TekstaiSuPasikartojimais = new string[Cn]; // tekstu masyvas
            kiek2 = 0;

            // isrenkamas didziausias pasikartojimu kiekis
            int max_pasikartojimai = 0;
            for ( int i = 0; i < kiek_f; i++ )
            {
                int pasikartojimai = DaugiausiaiIsEilesPasikartojanciuRaidziu(Tekstai[i]);
                if (pasikartojimai > max_pasikartojimai)
                    max_pasikartojimai = pasikartojimai;
            }

            for (int i = 0; i < kiek_f; i++)
            {
                int pasikartojimai = DaugiausiaiIsEilesPasikartojanciuRaidziu(Tekstai[i]);
                if (pasikartojimai == max_pasikartojimai)
                {
                    TekstaiSuPasikartojimais[kiek2++] = Tekstai[i];
                }
            }

            return TekstaiSuPasikartojimais;
        }

        static void SpausdintiIKonsoleMasyva(string[] Tekstai, int kiek)
        {
            Console.WriteLine("Masyvas tekstų (iš duomenų failų), turinčių daugiausiai iš eilės pasikartojančių raidžių: ");
            for ( int i = 0; i < kiek; i++ )
            {
                Console.WriteLine("{0}. {1} \r\n", i + 1, Tekstai[i]);
            }
        }
        

    }
}
