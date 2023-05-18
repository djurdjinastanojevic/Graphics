using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace grafika3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
           
            Point centar = new Point(ClientRectangle.Width / 2, ClientRectangle.Height / 2);
            int dimenzija = vratiManjuDimenziju();
            nacrtajTeren(g, centar, dimenzija);

            Bitmap bitmap = new Bitmap(1000, 600);
           
            Graphics g1 = Graphics.FromImage(bitmap); 
            g1.SmoothingMode = SmoothingMode.AntiAlias;
            centar = new Point(500, 300);
            nacrtajTeren(g1, centar, 1000);
            bitmap.Save(@"odbojka.png", ImageFormat.Png);
        }
        private void nacrtajIgraca(Graphics g, Point centar, int precnik, Pen olovka, Color bojaIgraca, int broj, Color bojaBroja)
        {
            Point pozKruga = new Point(centar.X - (precnik / 2), centar.Y - (precnik / 2));
            Rectangle krug = new Rectangle(pozKruga, new Size(precnik, precnik));
            g.DrawEllipse(olovka, krug);
            g.FillEllipse(new SolidBrush(bojaIgraca), krug);

            Font font = new Font("Comic Sans MS", precnik / 2);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            
            g.DrawString(broj.ToString(), font, new SolidBrush(bojaBroja), centar, format);
        }

        private void nacrtajTeren(Graphics g, Point centarTerena, int duzinaTerena)
        {
            int sirinaTerena = (int)(0.6 * duzinaTerena);
            Point pozTerena = new Point(centarTerena.X - (duzinaTerena / 2), centarTerena.Y - (sirinaTerena / 2));
            Rectangle teren = new Rectangle(pozTerena, new Size(duzinaTerena, sirinaTerena));
            g.DrawRectangle(new Pen(Color.Black, 2.5f), teren);
            g.FillRectangle(new SolidBrush(Color.Orange), teren);
            nacrtajLinijeTerena(g, pozTerena, duzinaTerena, sirinaTerena, new Pen(Color.White, 2.5f));
        }
        private int vratiManjuDimenziju()
        {
            int duzina = this.ClientRectangle.Width;
            int sirina = this.ClientRectangle.Height;
            if (duzina < sirina)
            {
                return duzina;
            }
            else return sirina;
        }
        private void nacrtajLinijeTerena(Graphics g, Point pocetakTerena, int duzinaTerena, int sirinaTerena, Pen olovka)
        {
            int margina = (int)(sirinaTerena * 0.07);
            Point pozAutLinije = new Point(pocetakTerena.X + margina, pocetakTerena.Y + margina);
            int duzinaAutLinije = duzinaTerena - 2 * margina;
            int sirinaAutLinije = sirinaTerena - 2 * margina;
            Rectangle autLinija = new Rectangle(pozAutLinije, new Size(duzinaAutLinije, sirinaAutLinije));
            g.DrawRectangle(olovka, autLinija);

            Rectangle mreza = new Rectangle(pozAutLinije, new Size(duzinaAutLinije / 2, sirinaAutLinije));
            g.DrawRectangle(olovka, mreza);

            Rectangle napadLijevo = new Rectangle(pozAutLinije, new Size((duzinaAutLinije/2) - (duzinaAutLinije / 6), sirinaAutLinije));
            g.DrawRectangle(olovka, napadLijevo);

            Rectangle napadDesno = new Rectangle(pozAutLinije, new Size((duzinaAutLinije / 2) + (duzinaAutLinije / 6), sirinaAutLinije));
            g.DrawRectangle(olovka, napadDesno);

            int procenatMargine = (int)(margina * 0.8);
            Point t1Lijevo = new Point(pozAutLinije.X + (duzinaAutLinije / 2) - (duzinaAutLinije / 6), pozAutLinije.Y - procenatMargine);
            Point t2Lijevo = new Point(pozAutLinije.X + (duzinaAutLinije / 2) - (duzinaAutLinije / 6), pozAutLinije.Y +sirinaAutLinije+ procenatMargine);
            olovka.DashStyle = DashStyle.Dash;
            g.DrawLine(olovka, t1Lijevo, t2Lijevo);

            Point t1Desno = new Point(pozAutLinije.X + (duzinaAutLinije / 2) + (duzinaAutLinije / 6), pozAutLinije.Y -procenatMargine);
            Point t2Desno = new Point(pozAutLinije.X + (duzinaAutLinije / 2) + (duzinaAutLinije / 6), pozAutLinije.Y + sirinaAutLinije + procenatMargine);
            g.DrawLine(olovka, t1Desno, t2Desno);

            Point centarSlike1 = new Point(pozAutLinije.X + ((duzinaAutLinije / 2) - (duzinaAutLinije / 6)) / 2, pozAutLinije.Y+sirinaAutLinije/2);
            Point centarSlike2 = new Point(pozAutLinije.X + duzinaAutLinije - (((duzinaAutLinije / 2) - (duzinaAutLinije / 6)) / 2), pozAutLinije.Y + sirinaAutLinije / 2);
            nacrtajLogoe(g, centarSlike1, centarSlike2, sirinaTerena / 3);
            nacrtajIgrace(g, pocetakTerena, duzinaTerena, sirinaTerena, sirinaTerena/10);

            Point pozNaziva = new Point(pocetakTerena.X + margina, pocetakTerena.Y + sirinaAutLinije+2*margina);
            nacrtajNaziv(g, pozNaziva, sirinaTerena / 30, Color.White);

            Point pozPotpisa = new Point(pocetakTerena.X + (int)(duzinaTerena-(duzinaTerena/20)), pocetakTerena.Y + sirinaAutLinije+ 2 * margina);
            nacrtajPotpis(g, pozPotpisa, sirinaTerena / 30, Color.White);
        
        }

        private void nacrtajLogoe(Graphics g, Point centarSlike1, Point centarSlike2, int velicina)
        {
            Image odbojka = Properties.Resources.odbojka;
            int duzina = (int)(velicina * 0.8);
            Rectangle slika1 = new Rectangle(centarSlike1, new Size(duzina, velicina));
            g.DrawImage(odbojka, slika1, new Rectangle(0, 0, duzina, velicina), GraphicsUnit.Pixel);
            Rectangle slika2 = new Rectangle(centarSlike2, new Size(duzina, velicina));
            g.DrawImage(odbojka, slika2, new Rectangle(0, 0, duzina, velicina), GraphicsUnit.Pixel);
        }
        private void nacrtajIgrace(Graphics g, Point pocetakTerena, int duzinaTerena, int sirinaTerena, int precnikIgraca)
        {
            Color ljubicasta = Color.MediumVioletRed;
            Color zelena = Color.ForestGreen;
            Color bojaBroja = Color.White;
            Pen olovka = new Pen(Color.Black, 2.5f);
            

            Point pozIgraca = new Point(pocetakTerena.X + duzinaTerena / 4, pocetakTerena.Y + sirinaTerena / 2);
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, ljubicasta, 4, bojaBroja);
            pozIgraca = new Point(pocetakTerena.X + duzinaTerena / 4, pocetakTerena.Y + sirinaTerena / 3);
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, ljubicasta, 7, bojaBroja);
            pozIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 6), pocetakTerena.Y + (int)(sirinaTerena / 2.7));
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, ljubicasta, 18, bojaBroja);
            pozIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 8), pocetakTerena.Y + (int)(sirinaTerena / 1.4));
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, ljubicasta, 2, bojaBroja);
            pozIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.7), pocetakTerena.Y + (int)(sirinaTerena / 2.7));
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, zelena, 33, bojaBroja);
            pozIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.2), pocetakTerena.Y + (int)(sirinaTerena / 4));
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, zelena, 25, bojaBroja);
            pozIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.7), pocetakTerena.Y + (int)(sirinaTerena / 1.2));
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, zelena, 8, bojaBroja);
            pozIgraca = new Point(pocetakTerena.X + (int)(duzinaTerena / 1.2), pocetakTerena.Y + (int)(sirinaTerena / 1.4));
            nacrtajIgraca(g, pozIgraca, precnikIgraca, olovka, zelena,36, bojaBroja);
        }
        private void nacrtajNaziv(Graphics g, Point tacka, float velicinaFonta, Color boja)
        {
            string naziv = "Volleyball, 12.07.2018.“";
            Font font = new Font("Cambria", velicinaFonta, FontStyle.Bold | FontStyle.Italic);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Far;
            g.DrawString(naziv, font, new SolidBrush(Color.Black), tacka, format);
            tacka.X -= 1;
            tacka.Y -= 1;
            g.DrawString(naziv, font, new SolidBrush(boja), tacka, format);

        }
        private void nacrtajPotpis(Graphics g, Point tacka, float velicinaFonta, Color boja)
        {
            string naziv = "by Djurdjina Stanojevi, 1890";
            Font font = new Font("Cambria", velicinaFonta, FontStyle.Bold | FontStyle.Italic);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Far;
            format.LineAlignment = StringAlignment.Far;
            g.DrawString(naziv, font, new SolidBrush(Color.Black), tacka, format);
            tacka.X -= 1;
            tacka.Y -= 1;
            g.DrawString(naziv, font, new SolidBrush(boja), tacka, format);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
