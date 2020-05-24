using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    /// <summary>
    /// Ženklo klasė
    /// </summary>
    public class Ženklas : IComparable<Ženklas>, IEquatable<Ženklas>
    {
        public string Pavadinimas { get; set; }
        public int Metai { get; set; }
        public double Kaina { get; set; }

        public Ženklas() { }

        public Ženklas(string pavadinimas, int metai, double kaina)
        {
            Pavadinimas = pavadinimas;
            Metai = metai;
            Kaina = kaina;
        }

        public override string ToString()
        {
            string eilute;
            eilute = string.Format("{0, -20} {1, -20} {2, -20}",
            Pavadinimas, Metai, Kaina);
            return eilute;
        }

        static public bool operator >(Ženklas pirmas, Ženklas antras)
        {
            return pirmas.CompareTo(antras) == 1;
        }
        static public bool operator <(Ženklas pirmas,
        Ženklas antras)
        {
            return pirmas.CompareTo(antras) == -1;
        }

        public static bool operator >=(Ženklas pirmas, Ženklas antras)
        {
            return !(pirmas < antras);
        }

        public static bool operator <=(Ženklas pirmas, Ženklas antras)
        {
            return !(pirmas > antras);
        }

        public bool Equals(Ženklas kitas)
        {
            if (kitas == null) return false;
            if (this.Pavadinimas == kitas.Pavadinimas && this.Kaina == kitas.Kaina)
                return true;
            else return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Ženklas Ženklas = obj as Ženklas;
            if (Ženklas == null) return false;
            else return Equals(Ženklas);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Ženklas pirmas, Ženklas antras)
        {
            if (((object)pirmas) == null || (((object)antras) == null))
                return Object.Equals(pirmas, antras);
            return pirmas.Equals(antras);
        }

        public static bool operator !=(Ženklas pirmas, Ženklas antras)
        {
            if (((object)pirmas) == null || ((object)antras) == null)
                return !Object.Equals(pirmas, antras);
            return !pirmas.Equals(antras);
        }

        public int CompareTo(Ženklas kitas)
        {
            if (kitas == null) return 1;
            if (Pavadinimas.CompareTo(kitas.Pavadinimas) != 0) return Pavadinimas.CompareTo(kitas.Pavadinimas);
            else return Kaina.CompareTo(kitas.Kaina);
        }
    }

}