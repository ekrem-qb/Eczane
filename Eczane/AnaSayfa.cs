using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using MaterialSkin;
using MaterialSkin.Controls;

//Ekrem Bayram 1137 11-C

namespace Eczane
{
    public partial class AnaSayfa : MaterialForm
    {
        static public string Kullanici;

        OleDbConnection baglanti = new OleDbConnection("PROVIDER = microsoft.ace.oledb.12.0; DATA SOURCE = Eczane.accdb");
        DataSet ds = new DataSet();

        public AnaSayfa()
        {
            InitializeComponent();

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red500, Primary.Red600, Primary.Red200, Accent.Red400, TextShade.BLACK);
        }

        void ilaclariListele()
        {
            OleDbDataAdapter ilaclar = new OleDbDataAdapter("SELECT * FROM Ilaclar", baglanti);
            ds.Clear();
            ilaclar.Fill(ds, "Ilaclar");

            while (ds.Tables["Ilaclar"].Rows.Count < Directory.GetFiles("Resimler/", "*.jpg").Count())
            {
                File.Delete("Resimler/" + (ds.Tables["Ilaclar"].Rows.Count + 1) + ".jpg");
            }

            imageList1.Images.Clear();
            for (int i = 0; i < Directory.GetFiles("Resimler/", "*.jpg").Count(); i++)
            {
                imageList1.Images.Add(Image.FromFile("Resimler/" + (i + 1) + ".jpg"));
            }

            dataGridView1.Rows.Clear();

            dataGridView1.Rows.Add(imageList1.Images[0]);

            int resimID = 0;
            for (int satir = 0; satir < dataGridView1.Rows.Count; satir++)
            {
                for (int hucre = 0; hucre < dataGridView1.Rows[satir].Cells.Count; hucre++, resimID++)
                {
                    if (resimID < imageList1.Images.Count)
                    {
                        dataGridView1.Rows[satir].Cells[hucre].Value = imageList1.Images[resimID];

                        Image img = (Image)dataGridView1.Rows[satir].Cells[hucre].Value;
                        img.Tag = resimID + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                if (resimID < imageList1.Images.Count)
                {
                    dataGridView1.Rows.Add(imageList1.Images[0]);

                    Image img = (Image)dataGridView1.Rows[0].Cells[0].Value;
                    img.Tag = 0 + 1;
                }
                else
                {
                    break;
                }
            }

            dataGridView1.Size = this.Size;
        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            animasyon();
            
            OleDbDataAdapter ilaclar = new OleDbDataAdapter("SELECT * FROM Ilaclar", baglanti);
            ilaclar.Fill(ds, "Ilaclar");

            ilaclariListele();

            if (Kullanici == null)
            {
                materialFlatButton1.Text = "";
            }
            else
            {
                materialFlatButton1.Text = Kullanici;
            }
            
            flowLayoutPanel3.Height = 0;


            if (flowLayoutPanel3.Width < flowLayoutPanel4.Width)
            {
                flowLayoutPanel3.Left = flowLayoutPanel4.Left;
            }
            else
            {
                materialFlatButton2.Anchor = AnchorStyles.Right;
                materialFlatButton3.Anchor = AnchorStyles.Right;
                materialFlatButton4.Anchor = AnchorStyles.Right;
            }

            panel2.Width = 0;
            panel2.Left = this.Width;
        }

        #region Açılış animasyonu

        void animasyon()
        {
            panel1.Width = this.Width;
            panel1.Height = this.Height;
            panel1.Top = 0;
            panel1.Left = 0;
            shapeContainer1.Show();
            panel1.Show();
            rectangleShape2.Show();
            label4.Show();

            label4.Location = new Point((this.Width / 2) - (label4.Width / 2), (this.Height / 2) - (label4.Height / 2));

            timer3.Start();
        }
        private void Timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            timer4.Start();
        }

        private void Timer4_Tick(object sender, EventArgs e)
        {
            if (label4.Width > 76)
            {
                label4.Width -= 4;
                label4.Left += 2;
            }
            else
            {
                timer4.Stop();

                timer5.Start();

                foreach (Control item in Controls)
                {
                    item.Show();
                }

                flowLayoutPanel1.Hide();
                pictureBox1.Hide();
            }
        }

        private void Timer5_Tick(object sender, EventArgs e)
        {
            if (panel1.Top < 24)
            {
                panel1.Top++;
            }

            if (panel1.Height > 64)
            {
                panel1.Height -= 16;
            }
            else
            {
                panel1.Hide();
            }

            if (label4.Width > 26)
            {
                label4.Width -= 16;
                label4.Left += 8;
            }

            if (label4.Height > 26)
            {
                label4.Height -= 3;
                label4.Top += 1;
            }

            if (label4.Font.Size > 16)
            {
                label4.Font = new Font(label4.Font.Name, label4.Font.Size - 2);
            }

            if (label4.Left > 15)
            {
                label4.Left -= 15;
            }

            if (label4.Top > 36)
            {
                label4.Top -= 6;
            }
            else if (label4.Top <= 36 && label4.Left <= 15)
            {
                if (rectangleShape2.CornerRadius > 0)
                {
                    rectangleShape2.CornerRadius -= 4;
                }
                if (rectangleShape2.Width > 0)
                {
                    rectangleShape2.Width -= 2;
                    rectangleShape2.Left++;
                }
                if (rectangleShape2.Height > 0)
                {
                    rectangleShape2.Height -= 2;
                    rectangleShape2.Top++;
                }
                else
                {
                    if (panel1.Height <= 64)
                    {
                        timer5.Stop();
                    }
                }
            }
        }
        #endregion

        private void Label4_SizeChanged(object sender, EventArgs e)
        {
            if (rectangleShape2.Width != label4.Width + 10)
            {
                rectangleShape2.Width = label4.Width + 10;
            }
            if (rectangleShape2.Height != label4.Height + 10)
            {
                rectangleShape2.Height = label4.Height + 10;
            }
        }

        private void Label4_Move(object sender, EventArgs e)
        {
            if (rectangleShape2.Top != label4.Top - 5)
            {
                rectangleShape2.Top = label4.Top - 5;
            }
            if (rectangleShape2.Left != label4.Left - 5)
            {
                rectangleShape2.Left = label4.Left - 5;
            }
        }

        void baglantiKontrol()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
        }

        void resimeGoreBilgi(DataGridViewCellMouseEventArgs e)
        {
            Image secilenResim = null;

            if ((e.RowIndex <= dataGridView1.Rows.Count && e.RowIndex >= 0) && (e.ColumnIndex <= dataGridView1.Columns.Count) && e.ColumnIndex >= 0)
            {
                secilenResim = (Image)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }

            if (secilenResim != null)
            {
                flowLayoutPanel1.Show();

                baglantiKontrol();

                OleDbDataAdapter ilacAra = new OleDbDataAdapter("SELECT * FROM Ilaclar WHERE UrunKodu = '" + secilenResim.Tag.ToString() + "' ", baglanti);
                ilacAra.Fill(ds, "BulunanIlac");

                label1.Text = ds.Tables["BulunanIlac"].Rows[0].ItemArray[1].ToString();
                label2.Text = ds.Tables["BulunanIlac"].Rows[0].ItemArray[4].ToString() + " ₺";

                if (ds.Tables["BulunanIlac"].Rows[0].ItemArray[5].ToString() == "")
                {
                    label2.Font = new Font(label2.Font, FontStyle.Regular);
                    label3.Text = "İndirim Yok";
                }
                else
                {
                    label2.Font = new Font(label2.Font, FontStyle.Strikeout);
                    label3.Text = ds.Tables["BulunanIlac"].Rows[0].ItemArray[5].ToString() + " ₺";
                }

                ds.Tables.Clear();
            }
            else
            {
                flowLayoutPanel1.Hide();
            }
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (flowLayoutPanel1.Tag.ToString() == "true")
            {
                resimeGoreBilgi(e);

                flowLayoutPanel1.Left = Cursor.Position.X + 16;
                flowLayoutPanel1.Top = Cursor.Position.Y + 16;

                pictureBox1.Hide();
            }
            else
            {
                flowLayoutPanel1.Show();
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (flowLayoutPanel1.Tag.ToString() == "true")
            {
                flowLayoutPanel1.Hide();
            }
            
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            resimeGoreBilgi(e);

            if (flowLayoutPanel1.Tag.ToString() == "true")
            {
                flowLayoutPanel1.Tag = "false";

                timer1_Ters = false;
            }
            else
            {
                flowLayoutPanel1.Tag = "true";

                timer1_Ters = true;
            }

            timer1.Start();
        }

        bool timer1_Ters;

        private void timer1_Tick(object sender, EventArgs e)
        {
            flowLayoutPanel1.AutoSize = false;
            if (timer1_Ters)
            {
                if (flowLayoutPanel1.Height > 134)
                {
                    flowLayoutPanel1.Height -= 8;
                }
                else
                {
                    timer1.Stop();
                }
            }
            else
            {
                if (flowLayoutPanel1.Height < 182)
                {
                    flowLayoutPanel1.Height += 8;
                }
                else
                {
                    timer1.Stop();
                    miktarKontrol();
                }
            }
            
        }

        void miktarKontrol()
        {
            if (materialSingleLineTextField1.Text == "")
            {
                pictureBox1.Show();
                pictureBox1.Left = materialSingleLineTextField1.Left + flowLayoutPanel1.Left + flowLayoutPanel2.Left + 12;
                pictureBox1.Top = materialSingleLineTextField1.Top + flowLayoutPanel1.Top + flowLayoutPanel2.Top - 36 + 24;

                pictureBox1.Width = 0;
                pictureBox1.Height = 0;

                timer2_Ters = false;
                timer2.Start();

                toolTip1.SetToolTip(pictureBox1, "Miktar belirtmeniz lazım!!");
            }
            else
            {
                if (!materialSingleLineTextField1.Text.All(char.IsDigit))
                {
                    pictureBox1.Show();
                    pictureBox1.Left = materialSingleLineTextField1.Left + flowLayoutPanel1.Left + flowLayoutPanel2.Left + 12;
                    pictureBox1.Top = materialSingleLineTextField1.Top + flowLayoutPanel1.Top + flowLayoutPanel2.Top - 36 + 24;

                    pictureBox1.Width = 0;
                    pictureBox1.Height = 0;

                    timer2_Ters = false;
                    timer2.Start();

                    toolTip1.SetToolTip(pictureBox1, "Sayı giriniz!!");
                }
                else
                {
                    if (Convert.ToInt32(materialSingleLineTextField1.Text) > 100 || Convert.ToInt32(materialSingleLineTextField1.Text) <= 0)
                    {
                        pictureBox1.Show();
                        pictureBox1.Left = materialSingleLineTextField1.Left + flowLayoutPanel1.Left + flowLayoutPanel2.Left + 12;
                        pictureBox1.Top = materialSingleLineTextField1.Top + flowLayoutPanel1.Top + flowLayoutPanel2.Top - 36 + 24;

                        pictureBox1.Width = 0;
                        pictureBox1.Height = 0;

                        timer2_Ters = false;
                        timer2.Start();

                        toolTip1.SetToolTip(pictureBox1, "1 ve 100 arası sayı giriniz!!");
                    }
                    else
                    {
                        timer2_Ters = true;
                        timer2.Start();
                    }
                }
            }
        }

        private void materialSingleLineTextField1_TextChanged(object sender, EventArgs e)
        {
            miktarKontrol();
        }

        bool timer2_Ters;

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!timer2_Ters)
            {
                if (pictureBox1.Width < 24)
                {
                    pictureBox1.Width += 2;
                    pictureBox1.Height += 2;

                    pictureBox1.Left--;
                    pictureBox1.Top -= 2;
                }
                else
                {
                    timer2.Stop();
                }
            }
            else
            {
                if (pictureBox1.Width > 0)
                {
                    pictureBox1.Width -= 2;
                    pictureBox1.Height -= 2;

                    pictureBox1.Left++;
                    pictureBox1.Top += 2;
                }
                else
                {
                    timer2.Stop();
                }
            }
        }

        bool menuTers;

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel3.Height > 0)
            {
                menuTers = true;
                timer6.Start();
            }
            else
            {
                menuTers = false;
                timer6.Start();
            }

            if (panel2.Width > 0)
            {
                sepetTers = true;
                timer7.Start();
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (menuTers)
            {
                if (flowLayoutPanel3.Height > 0)
                {
                    flowLayoutPanel3.Height -= 9;
                }
                else
                {
                    timer6.Stop();
                }
            }
            else
            {
                if (flowLayoutPanel3.Height < 108)
                {
                    flowLayoutPanel3.Height += 9;
                }
                else
                {
                    timer6.Stop();
                }
            }
            
        }

        private void materialFlatButton3_Click(object sender, EventArgs e)
        {
            if (Kullanici == "admin")
            {
                Ilaclar ilaclar = new Ilaclar();
                ilaclar.Show();
            }
            else
            {
                MessageBox.Show("Admin paneline sadece Admin girebilir!");
            }
        }

        bool cikis = true;

        private void materialFlatButton4_Click(object sender, EventArgs e)
        {
            cikis = false;
            this.Close();

            Giris giris = new Giris();
            giris.Show();
        }

        private void AnaSayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cikis)
            {
                Application.Exit();
            }
        }

        void sepetListele()
        {
            baglantiKontrol();

            OleDbDataAdapter sepet = new OleDbDataAdapter("SELECT UrunAdi, Fiyat, Miktar FROM Sepet WHERE KullaniciAdi = '" + Kullanici + "' ", baglanti);
            DataSet ds = new DataSet();
            sepet.Fill(ds, "Sepet");

            dataGridView2.DataSource = ds.Tables["Sepet"];

            double tutar = 0;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                tutar += (Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) * Convert.ToDouble(dataGridView2.Rows[i].Cells[2].Value.ToString()));
            }

            label6.Text = tutar.ToString() + " ₺";
        }

        bool sepetTers;

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            if (panel2.Width > 0)
            {
                sepetTers = true;
            }
            else
            {
                sepetTers = false;
                sepetListele();
            }

            timer7.Start();
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            if (sepetTers)
            {
                if (panel2.Width > 0)
                {
                    panel2.Width -= 30;
                    panel2.Left += 30;
                }
                else
                {
                    timer7.Stop();
                }
            }
            else
            {
                if (panel2.Width < 300)
                {
                    panel2.Width += 30;
                    panel2.Left -= 30;
                }
                else
                {
                    timer7.Stop();
                }
            }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            baglantiKontrol();

            string fiyat = "0";

            if (label3.Text == "İndirim Yok")
            {
                fiyat = label2.Text.Remove(label2.Text.IndexOf('₺'));
            }
            else
            {
                fiyat = label3.Text.Remove(label3.Text.IndexOf('₺'));
            }

            OleDbCommand sepeteEkle = new OleDbCommand("INSERT INTO Sepet VALUES('" + label1.Text + "', '" + fiyat + "', '" + materialSingleLineTextField1.Text + "', '" + Kullanici + "')", baglanti);
            sepeteEkle.ExecuteNonQuery();

            sepetListele();
        }

        private void materialFlatButton5_Click(object sender, EventArgs e)
        {
            DialogResult cevap = MessageBox.Show("Emin misiniz?", "Sepetteki bütün ürünler silinicek!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (cevap == DialogResult.Yes)
            {
                baglantiKontrol();

                OleDbCommand sepetTemizle = new OleDbCommand("DELETE * FROM Sepet WHERE KullaniciAdi = '"+ Kullanici +"' ", baglanti);
                sepetTemizle.ExecuteNonQuery();

                sepetListele();
            }
        }

        private void materialFlatButton6_Click(object sender, EventArgs e)
        {
            baglantiKontrol();

            OleDbCommand sepetTemizle = new OleDbCommand("DELETE * FROM Sepet WHERE KullaniciAdi = '" + Kullanici + "'  AND UrunAdi = '"+ dataGridView2.SelectedRows[0].Cells[0].Value.ToString() +"' ", baglanti);
            sepetTemizle.ExecuteNonQuery();

            sepetListele();
        }

        private void AnaSayfa_Activated(object sender, EventArgs e)
        {
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            ilaclariListele();
        }

        private void materialFlatButton7_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                DialogResult cevap = MessageBox.Show("Tutar: " + label6.Text + ". Emin misiniz?", "Sepetteki bütün ürünler satın alınıcak!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (cevap == DialogResult.Yes)
                {
                    baglantiKontrol();

                    OleDbCommand sepetTemizle = new OleDbCommand("DELETE * FROM Sepet WHERE KullaniciAdi = '" + Kullanici + "' ", baglanti);
                    sepetTemizle.ExecuteNonQuery();

                    sepetListele();
                }
            }
            else
            {
                MessageBox.Show("Sepette ilac yok!");
            }
        }
    }
}