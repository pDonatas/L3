using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2
{
    public sealed class Sąrašas<tipas> : IEnumerable<tipas> where tipas : IComparable<tipas>, IEquatable<tipas>
    {
        private sealed class Mazgas<type> where type : IComparable<type>, IEquatable<type>
        {
            public type Duom { get; set; }
            public Mazgas<type> Kaire { get; set; }
            public Mazgas<type> Desine { get; set; }
            public Mazgas(type reiksme, Mazgas<type> adrk, Mazgas<type> adrd)
            {
                Duom = reiksme;
                Kaire = adrk;
                Desine = adrd;
            }
        }
        private Mazgas<tipas> pr;
        private Mazgas<tipas> pb;
        private Mazgas<tipas> d;

        public Sąrašas()
        {
            this.pr = null;
            this.pb = null;
            this.d = null;
        }
        /** Sąsajai priskiriama sąrašo pradžia */
        public void Pradžia() { d = pr; }
        // d = pb.Kaire; -- kita kryptis
        /** Sąsajai priskiriamas tolesnis sąrašo elementas */
        public void Kitas() { d = d.Desine; }
        // d = d.Kaire;
        /** Grąžina true, jeigu sąrašas netuščias */
        public bool Yra() { return d != null; }
        public tipas ImtiDuomenis()
        {
            return d.Duom;
        }

        public void DėtiDuomenisA(tipas naujas)
        {
            var dd = new Mazgas<tipas>(naujas, null, pr);
            if (pr != null)
                pr.Kaire = dd;
            else pb = dd;
            pr = dd;
        }

        public void DėtiDuomenisT(tipas naujas)
        {
            var dd = new Mazgas<tipas>(naujas,pb, null);
            if (pr != null)
            {
                pb.Desine = dd;
                pb = dd;
            }
            else
            {
                pr = dd;
                pb = dd;
            }
        }

        public void Rikiuoti()
        {
            for (Mazgas<tipas> d1 = pr; d1 != null; d1 = d1.Desine)
            {
                Mazgas<tipas> maxv = d1;
                for (Mazgas<tipas> d2 = d1; d2 != null; d2 = d2.Desine)
                {
                    if (d2.Duom.CompareTo(maxv.Duom) < 0) maxv = d2;
                    tipas St = d1.Duom;
                    d1.Duom = maxv.Duom;
                    maxv.Duom = St;
                }
            }
        }

        public IEnumerator<tipas> GetEnumerator()
        {
            for (Mazgas<tipas> dd = pr; dd != null; dd = dd.Desine)
            {
                yield return dd.Duom;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Ženklas RastiŽenklą(string tekstas)
        {
            for (Mazgas<tipas> d1 = pr; d1 != null; d1 = d1.Desine)
            {
                Ženklas ženklas = d1.Duom as Ženklas;
                if (ženklas.Pavadinimas.Equals(tekstas))
                {
                    return ženklas;
                }
            }
            return null;
        }

        public int Kiekis(string pavadinimas)
        {
            int kiek = 0;
            for (Mazgas<tipas> a = pr; a != null; a = a.Desine)
            {
                Kolekcionierius kol = a.Duom as Kolekcionierius;
                if (kol.Zenklas.Equals(pavadinimas)) kiek++;
            }
            return kiek;
        }

    }
}