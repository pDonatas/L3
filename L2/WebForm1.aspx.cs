using System;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;

namespace L2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Button1_Click(object sender, EventArgs e)
        {
            string fv = "rezultatai.txt";

            Sąrašas<Kolekcionierius> sąrašas = (Sąrašas<Kolekcionierius>)Session["Kolekcionieriai"];
            
            Sąrašas<Ženklas> aukcionas = (Sąrašas<Ženklas>)Session["Ženklai"];
            if (sąrašas != null && aukcionas != null)
            {
                Spausdinti(sąrašas, Table3);

                Label9.Text = "Pradinis aukciono sąrašas: ";
                SpausdintiA(aukcionas, Table4);

                Ženklas pop = Populiariausias(sąrašas, aukcionas);
                Sąrašas<Kolekcionierius> neturi = null;
                if (pop != null)
                {
                    Label2.Text = "Populiariausias ženklas: " + pop.Pavadinimas;
                    Label3.Text = "Ženklų neturi šie žmonės: ";
                    neturi = NeturiPop(pop, sąrašas);
                    neturi.Rikiuoti();
                    Spausdinti(neturi, Table1);
                }

                string tekstas = TextBox1.Text;
                Ženklas įvestas = aukcionas.RastiŽenklą(tekstas);
                Sąrašas<Kolekcionierius> sudarytas = null;
                if (įvestas != null)
                {
                    Label7.Text = "Įvestas tekstas: " + tekstas;
                    sudarytas = Kolekcionieriai(įvestas, sąrašas);
                    Label4.Text = "Ženklą " + tekstas + " turi: ";
                    sudarytas.Rikiuoti();
                    Spausdinti(sudarytas, Table2);
                }
                Failas(fv, tekstas, aukcionas, sąrašas, neturi, sudarytas);
            }
        }
        /// <summary>
        /// Rašymas į duomenų failą
        /// </summary>
        /// <param name="fv">Duomenų failas</param>
        /// <param name="įvestas">Įvestas tekstas</param>
        /// <param name="A">Aukciono sąrašas</param>
        /// <param name="S">Kolekcionierių sąrašas</param>
        /// <param name="neturi">Kolekcionierių kurie neturi reikiamo ženklo sąrašas</param>
        /// <param name="sudarytas">Kolekcionierių kurie turi ženklus sąrašas</param>
        void Failas(string fv, string įvestas, Sąrašas<Ženklas> A, Sąrašas<Kolekcionierius> S, Sąrašas<Kolekcionierius> neturi, Sąrašas<Kolekcionierius> sudarytas)
        {
            using (StreamWriter file = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/" + fv)))
            {
                file.WriteLine("Pradiniai duomenys:");
                file.WriteLine("------------------------------------------------------------------------------------------");
                file.WriteLine("Įvestas ženklas: " + įvestas);
                file.WriteLine("------------------------------------------------------------------------------------------");
                file.WriteLine("Aukciono pašto ženklai: ");
                file.WriteLine("------------------------------------------------------------------------------------------");
                file.WriteLine("{0, -20} {1, -20} {2, -20}", "Pavadinimas", "Metai", "Kaina");
                // Sąrašo peržiūra, panaudojant sąsajos metodus
                for (A.Pradžia(); A.Yra(); A.Kitas())
                {
                    file.WriteLine(A.ImtiDuomenis());
                }
                file.WriteLine("------------------------------------------------------------------------------------------");
                file.WriteLine("Dalyvių sąrašas: ");
                file.WriteLine("------------------------------------------------------------------------------------------");
                file.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20} {4,-20}", "Pavardė", "Vardas", "Ženklas", "Kiekis", "Kaina");
                // Sąrašo peržiūra, panaudojant sąsajos metodus
                for (S.Pradžia(); S.Yra(); S.Kitas())
                {
                    file.WriteLine(S.ImtiDuomenis());
                }
                file.WriteLine("Rezultatai:");
                file.WriteLine("------------------------------------------------------------------------------------------");
                file.WriteLine("Neturinčių reikiamo ženklo sąrašas: ");
                file.WriteLine("------------------------------------------------------------------------------------------");
                file.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20} {4,-20}", "Pavardė", "Vardas", "Ženklas", "Kiekis", "Kaina");
                // Sąrašo peržiūra, panaudojant sąsajos metodus
                for (neturi.Pradžia(); neturi.Yra(); neturi.Kitas())
                {
                    file.WriteLine(neturi.ImtiDuomenis());
                }
                if (sudarytas != null)
                {
                    file.WriteLine("------------------------------------------------------------------------------------------");
                    file.WriteLine("Tinkamų dalyvių sąrašas: ");
                    file.WriteLine("------------------------------------------------------------------------------------------");
                    file.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20} {4,-20}", "Pavardė", "Vardas", "Ženklas", "Kiekis", "Kaina");
                    // Sąrašo peržiūra, panaudojant sąsajos metodus
                    for (sudarytas.Pradžia(); sudarytas.Yra(); sudarytas.Kitas())
                    {
                        file.WriteLine(sudarytas.ImtiDuomenis());
                    }
                    file.WriteLine("------------------------------------------------------------------------------------------");
                }
            }
        }
        /// <summary>
        /// Metodas skirtas spausdinti aukciono sąrašo narius
        /// </summary>
        /// <param name="s">Aukcionas</param>
        /// <param name="table">Paduodamas table klasės objektas</param>
        void SpausdintiA(Sąrašas<Ženklas> s, Table table)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "Pavadinimas";
            row.Cells.Add(cell);

            TableCell cell2 = new TableCell();
            cell2.Text = "Metai";
            row.Cells.Add(cell2);
            TableCell cell3 = new TableCell();
            cell3.Text = "Kaina";
            row.Cells.Add(cell3);

            table.Rows.Add(row);

            for (s.Pradžia(); s.Yra(); s.Kitas())
            {
                TableRow rowas = new TableRow();
                Ženklas ženklas = s.ImtiDuomenis();

                TableCell cel = new TableCell();
                cel.Text = ženklas.Pavadinimas;
                rowas.Cells.Add(cel);

                TableCell cel2 = new TableCell();
                cel2.Text = ženklas.Metai.ToString();
                rowas.Cells.Add(cel2);

                TableCell cel3 = new TableCell();
                cel3.Text = ženklas.Kaina.ToString();
                rowas.Cells.Add(cel3);

                table.Rows.Add(rowas);
            }

        }
        /// <summary>
        /// Spausdinamas sąrašo klasės objektas
        /// </summary>
        /// <param name="s">Sąrašo klasės objektas</param>
        /// <param name="table">Table klasės objektas</param>
        void Spausdinti(Sąrašas<Kolekcionierius> s, Table table)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "Pavardė";
            row.Cells.Add(cell);

            TableCell cell2 = new TableCell();
            cell2.Text = "Vardas";
            row.Cells.Add(cell2);
            TableCell cell3 = new TableCell();
            cell3.Text = "Ženklas";
            row.Cells.Add(cell3);
            TableCell cell4 = new TableCell();
            cell4.Text = "Kiekis";
            row.Cells.Add(cell4);
            TableCell cell5 = new TableCell();
            cell5.Text = "Kaina";
            row.Cells.Add(cell5);

            table.Rows.Add(row);

            foreach(var sa in s)
            {
                TableRow rowas = new TableRow();
                Kolekcionierius kolekcionierius = sa;

                TableCell cel = new TableCell();
                cel.Text = kolekcionierius.Pavarde;
                rowas.Cells.Add(cel);

                TableCell cel2 = new TableCell();
                cel2.Text = kolekcionierius.Vardas;
                rowas.Cells.Add(cel2);

                TableCell cel3 = new TableCell();
                cel3.Text = kolekcionierius.Zenklas;
                rowas.Cells.Add(cel3);

                TableCell cel4 = new TableCell();
                cel4.Text = kolekcionierius.Kiekis.ToString();
                rowas.Cells.Add(cel4);

                TableCell cel5 = new TableCell();
                cel5.Text = kolekcionierius.Kaina.ToString();
                rowas.Cells.Add(cel5);

                table.Rows.Add(rowas);
            }
            
        }
        /// <summary>
        /// Metodas skirtas rasti kolekcionierius kurie neturi populiariausio ženklo
        /// </summary>
        /// <param name="pop">Ženklo klasės objektas</param>
        /// <param name="sąrašas">Sąrašo klasės objektas</param>
        /// <returns>Sąrašas kolekcionierių neturinčių šio ženklo</returns>
        Sąrašas<Kolekcionierius> NeturiPop(Ženklas pop, Sąrašas<Kolekcionierius> sąrašas)
        {
            Sąrašas<Kolekcionierius> neturi = new Sąrašas<Kolekcionierius>();
            foreach(var s in sąrašas)
            {
                Kolekcionierius kolekcionierius = s;
                if (!kolekcionierius.Zenklas.Equals(pop.Pavadinimas)) neturi.DėtiDuomenisT(kolekcionierius);
            }
            return neturi;
        }
        /// <summary>
        /// Randa populiariausią ženklą
        /// </summary>
        /// <param name="s">Sąrašo objektas</param>
        /// <param name="aukcionas">Aukciono objektas</param>
        /// <returns>Grąžinamas populiariausias ženklas</returns>
        Ženklas Populiariausias(Sąrašas<Kolekcionierius> s, Sąrašas<Ženklas> aukcionas)
        {
            int max = 0;
            Ženklas pop = new Ženklas();
            foreach(var sa in s)
            {
                Label2.Text = "";
                Kolekcionierius kolekcionierius = sa;
                int kiekis = s.Kiekis(kolekcionierius.Zenklas);

                if (max <= kiekis)
                {
                    max = kiekis;
                    foreach(var a in aukcionas)
                    {
                        Ženklas ženklas = a;
                        if (ženklas.Pavadinimas.Equals(kolekcionierius.Zenklas))
                        {
                            pop = ženklas;
                            break;
                        }
                    }
                }
            }
            return pop;
        }
        /// <summary>
        /// Kolekcionierių atitinkančių kriterijus sąrašo sudarymas
        /// </summary>
        /// <param name="tekstas">Įvesto ženklo klasė</param>
        /// <param name="A">Sąrašo klasė</param>
        /// <returns>Kolekcionierių atitinkančių kriterijus sąrašas</returns>
        Sąrašas<Kolekcionierius> Kolekcionieriai(Ženklas tekstas, Sąrašas<Kolekcionierius> A)
        {
            Sąrašas<Kolekcionierius> sudarytas = new Sąrašas<Kolekcionierius>();
            foreach(var d in A)
            {
                if (d.Zenklas.Equals(tekstas.Pavadinimas) && d.Kaina <= tekstas.Kaina)
                {
                    sudarytas.DėtiDuomenisA(d);
                }
            }
            return sudarytas;
        }
        /// <summary>
        /// Ženklų nuskaitymo metodas
        /// </summary>
        /// <param name="failas">Failo pavadinimas</param>
        /// <param name="aukcionas">Aukciono klasė</param>
        void SkaitymasZenklu(Stream file, Sąrašas<Ženklas> aukcionas)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string eilute = null;
                while (null != (eilute = reader.ReadLine()))
                {
                    string[] eil = eilute.Split(' ');

                    Ženklas ženklas = new Ženklas(eil[0], int.Parse(eil[1]), double.Parse(eil[2]));

                    aukcionas.DėtiDuomenisA(ženklas);
                }
            }
        }

        /// <summary>
        /// Kolekcionierių nuskaitymo metodas
        /// </summary>
        /// <param name="failas">Failo pavadinimas</param>
        /// <param name="sąrašas">Sąrašo klasė</param>
        void Skaitymas(Stream file, Sąrašas<Kolekcionierius> sąrašas)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string eilute = null;
                while (null != (eilute = reader.ReadLine()))
                {
                    string[] eil = eilute.Split(' ');

                    Kolekcionierius kolekcionierius = new Kolekcionierius(eil[0], eil[1], eil[2], int.Parse(eil[3]), double.Parse(eil[4]));

                    sąrašas.DėtiDuomenisA(kolekcionierius);
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile && FileUpload1.FileName.EndsWith(".txt"))
            {
                if (FileUpload1.FileName.Equals("U20b.txt"))
                {
                    Sąrašas<Kolekcionierius> kSarasas = new Sąrašas<Kolekcionierius>();
                    Skaitymas(FileUpload1.FileContent, kSarasas);
                    Session["Kolekcionieriai"] = kSarasas;
                    Spausdinti(kSarasas, Table4);
                }
                if (FileUpload1.FileName.Equals("U20a.txt"))
                {
                    Sąrašas<Ženklas> zSarasas = new Sąrašas<Ženklas>();
                    SkaitymasZenklu(FileUpload1.FileContent, zSarasas);
                    Session["Ženklai"] = zSarasas;
                    SpausdintiA(zSarasas, Table3);
                }
            }
        }
    }
}