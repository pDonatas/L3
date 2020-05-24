using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    /// <summary>
    /// Kolekcionieriaus klasė
    /// </summary>
    public class Kolekcionierius : IComparable<Kolekcionierius>, IEquatable<Kolekcionierius>
    {
        public string Pavarde { get; set; }
        public string Vardas { get; set; }
        public string Zenklas { get; set; }
        public int Kiekis { get; set; }
        public double Kaina { get; set; }

        public Kolekcionierius(string pavarde, string vardas, string zenklas, int kiekis, double kaina)
        {
            Pavarde = pavarde;
            Vardas = vardas;
            Zenklas = zenklas;
            Kiekis = kiekis;
            Kaina = kaina;
        }

        public override string ToString()
        {
            string eilute;
            eilute = string.Format("{0, -20} {1, -20} {2,-20} {3,-20} {4,-20}",
            Pavarde, Vardas,Zenklas,Kiekis,Kaina);
            return eilute;
        }
        static public bool operator >(Kolekcionierius pirmas, Kolekcionierius antras)
        {
            return pirmas.CompareTo(antras) == 1;
        }
        static public bool operator <(Kolekcionierius pirmas,
        Kolekcionierius antras)
        {
            return pirmas.CompareTo(antras) == -1;
        }

        public static bool operator >=(Kolekcionierius pirmas, Kolekcionierius antras)
        {
            return !(pirmas < antras);
        }

        public static bool operator <=(Kolekcionierius pirmas, Kolekcionierius antras)
        {
            return !(pirmas > antras);
        }

        public bool Equals(Kolekcionierius kitas)
        {
            if (kitas == null) return false;
            if (this.Zenklas == kitas.Zenklas && this.Kaina == kitas.Kaina)
                return true;
            else return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Kolekcionierius kolekcionierius = obj as Kolekcionierius;
            if (kolekcionierius == null) return false;
            else return Equals(kolekcionierius);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Kolekcionierius pirmas, Kolekcionierius antras)
        {
            if (((object)pirmas) == null || (((object)antras) == null))
                return Object.Equals(pirmas, antras);
            return pirmas.Equals(antras);
        }

        public static bool operator !=(Kolekcionierius pirmas, Kolekcionierius antras)
        {
            if (((object)pirmas) == null || ((object)antras) == null)
                return !Object.Equals(pirmas, antras);
            return !pirmas.Equals(antras);
        }

        public int CompareTo(Kolekcionierius kitas)
        {
            if (kitas == null) return 1;
            if (Zenklas.CompareTo(kitas.Zenklas) != 0) return Zenklas.CompareTo(kitas.Zenklas);
            else return Kaina.CompareTo(kitas.Kaina);
        }
    }
}