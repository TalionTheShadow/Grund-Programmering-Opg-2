using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace SearchEngine
{
    public enum SøgeKriterier
    {
        Lærer,
        Elev,
        Fag
    }

    public abstract class Person
    {
        public string ForNavn { get; set; }
        public string EfterNavn { get; set; }

        public Person(string forNavn, string efterNavn)
        {
            ForNavn = forNavn;
            EfterNavn = efterNavn;
        }
    }

    public abstract class Lærer : Person
    {
        public Lærer(string forNavn, string efterNavn) : base(forNavn, efterNavn) { }

        public abstract List<Fag> GetSubjectsTaught();
    }

    public abstract class Elev : Person
    {
        public Elev(string forNavn, string efterNavn) : base(forNavn, efterNavn) { }

        public abstract List<Fag> GetSubjectsEnrolled();

    }

    public abstract class Fag
    {
        public string Navn { get; set; }
        public Lærer Lærer { get; set; }
        public List<Elev> Elever { get; set; }

        public Fag(string navn, Lærer lærer, List<Elev> elever)
        {
            Navn = navn;
            Lærer = lærer;
            Elever = elever;
        }
    }

    public class H1Lærer : Lærer
    {
        public H1Lærer(string forNavn, string efterNavn) : base(forNavn, efterNavn) { }

        public override List<Fag> GetSubjectsTaught()
        {
            List<Fag> fagLært = new List<Fag>();

            foreach (var fag in H1Fag2.AlleFag)
            {
                if (fag.Lærer == this)
                {
                    fagLært.Add(fag);
                }
            }

            return fagLært;
        }
    }

    public class H1Elev : Elev
    {
        public H1Elev(string forNavn, string efterNavn) : base(forNavn, efterNavn) { }

        public override List<Fag> GetSubjectsEnrolled()
        {
            List<Fag> subjectsEnrolled = new List<Fag>();

            foreach (var fag in H1Fag2.AlleFag)
            {
                if (fag.Elever.Contains(this))
                {
                    subjectsEnrolled.Add(fag);
                }
            }

            return subjectsEnrolled;
        }
    }

    public class H1Fag : Fag
    {
        public H1Fag(string navn, Lærer lærer, List<Elev> elever) : base(navn, lærer, elever) { }

    }

    public static class H1Fag2

    {
        public static List<Fag> AlleFag { get; set; }

        static H1Fag2()
        {
            var lærer1 = new H1Lærer("Niels", "Olesen");
            var lærer2 = new H1Lærer("Flemming", "Sørensen");
            var lærer3 = new H1Lærer("Peter", "Suni Lindeskov");
            var lærer4 = new H1Lærer("Henrik", "Vincents Poulsen");

            var elev1 = new H1Elev("Alexander", "Mathias Thamdru");
            var elev2 = new H1Elev("Allan", "Gawron");
            var elev3 = new H1Elev("Andreas", "Carl Balle");
            var elev4 = new H1Elev("Darab", "Haqnazar");
            var elev5 = new H1Elev("Felix", "Enok Berger");
            var elev6 = new H1Elev("Jamie", "J. d. I. S. E.-D.");
            var elev7 = new H1Elev("Jeppe", "Carlseng Pedersen");
            var elev8 = new H1Elev("Joseph", "Holland-Hale");
            var elev9 = new H1Elev("Kamil", "Marcin Kulpa");
            var elev10 = new H1Elev("Loke", "Emil Bendtsen");
            var elev11 = new H1Elev("Mark", "Hyrsting Larsen");
            var elev12 = new H1Elev("Niklas", "Kim Jensen");
            var elev13 = new H1Elev("Rasmus", "Peter Hjorth");
            var elev14 = new H1Elev("Sammy", "Damiri");
            var elev15 = new H1Elev("Thomas", "Mose Holmberg");
            var elev16 = new H1Elev("Tobias", "Casanas Besser");

            var fag1 = new H1Fag("Grundlæggende Programmering", lærer1, new List<Elev> { elev1, elev2, elev3, elev4, elev5, elev6, elev7, elev8, elev9, elev10, elev11, elev12, elev13, elev14, elev15, elev15, elev16 });


            AlleFag = new List<Fag> { fag1 };
        }
    }

    class Program

    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Indtast søgekriterier (Lærer, elev eller fag):");
                string criteriaString = Console.ReadLine();

                if (Enum.TryParse<SøgeKriterier>(criteriaString, out var Kriterier))
                {
                    switch (Kriterier)
                    {
                        case SøgeKriterier.Lærer:
                            Console.WriteLine("Indtast lærerens navn:");
                            string lærerNavn = Console.ReadLine();
                            var matchingTeachers = H1Fag2.AlleFag.Select(s => s.Lærer)
                                .Where(t => t.ForNavn.Equals(lærerNavn, StringComparison.OrdinalIgnoreCase));
                            Console.WriteLine($"Fag undervist af {lærerNavn}:");
                            foreach (var fag in matchingTeachers.SelectMany(t => t.GetSubjectsTaught()))
                            {
                                Console.WriteLine(fag.Navn);
                            }
                            break;

                        case SøgeKriterier.Elev:
                            Console.WriteLine("Indtast den studerendes navn:");
                            string elevNavn = Console.ReadLine();
                            var matchingStudents = H1Fag2.AlleFag.SelectMany(s => s.Elever)
                                .Where(p => p.ForNavn.Equals(elevNavn, StringComparison.OrdinalIgnoreCase));
                            Console.WriteLine($"Fag indskrevet af {elevNavn}:");
                            foreach (var fag in matchingStudents.SelectMany(s => s.GetSubjectsEnrolled()))
                            {
                                Console.WriteLine(fag.Navn);
                            }
                            break;

                        case SøgeKriterier.Fag:
                            Console.WriteLine("Indtast emnets navn:");
                            string fagNavn = Console.ReadLine();
                            var matchingSubjects = H1Fag2.AlleFag
                                .Where(s => s.Navn.Equals(fagNavn, StringComparison.OrdinalIgnoreCase));
                            Console.WriteLine($"Emne {fagNavn} er undervist af:");
                            foreach (var lærer in matchingSubjects.Select(s => s.Lærer))
                            {
                                Console.WriteLine($"{lærer.ForNavn} {lærer.EfterNavn}");
                            }
                            Console.WriteLine($"Fag {fagNavn} er indskrevet af:");
                            foreach (var elev in matchingSubjects.SelectMany(s => s.Elever))
                            {
                                Console.WriteLine($"{elev.ForNavn} {elev.EfterNavn}");
                            }
                            break;
                    }
                }
            }
        }
    }
}