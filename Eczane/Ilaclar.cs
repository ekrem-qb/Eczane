using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MaterialSkin;
using MaterialSkin.Animations;
using MaterialSkin.Controls;
using System.Threading.Tasks;

//Ekrem Bayram 1137 11-C

namespace Eczane
{
    public partial class Ilaclar : MaterialForm
    {

        OleDbConnection baglanti = new OleDbConnection("PROVIDER = microsoft.ace.oledb.12.0; DATA SOURCE = Eczane.accdb");
        DataSet ds = new DataSet();

        public Ilaclar()
        {
            InitializeComponent();

            //Form kenarları
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 0, 0));

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red500, Primary.Red600, Primary.Red200, Accent.Red400, TextShade.WHITE);
        }

        void baglantiKontrol()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
        }

        private void Ilaclar_Load(object sender, EventArgs e)
        {
            baglantiKontrol();
            kategorileriListele();
            ilaclarListele();
            baglanti.Close();

            while (dataGridView1.Rows.Count < Directory.GetFiles("Resimler/", "*.jpg").Count())
            {
                File.Delete("Resimler/" + (dataGridView1.Rows.Count + 1) + ".jpg");
            }
        }

        void kategorileriListele()
        {
            OleDbDataAdapter kategoriAdapter = new OleDbDataAdapter("SELECT DISTINCT(Kategori) FROM Kategoriler", baglanti);

            kategoriAdapter.Fill(ds, "Kategoriler");

            comboBox1.Items.Clear();
            for (int i = 0; i < ds.Tables["Kategoriler"].Rows.Count; i++)
            {
                comboBox1.Items.Add(ds.Tables["Kategoriler"].Rows[i].ItemArray[0].ToString());
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                baglantiKontrol();

                OleDbDataAdapter altKategoriAdapter = new OleDbDataAdapter("SELECT AltKategori FROM Kategoriler WHERE Kategori = '" + comboBox1.Text + "' ", baglanti);
                altKategoriAdapter.Fill(ds, "AltKategoriler");

                comboBox2.Text = null;
                comboBox2.Items.Clear();
                for (int i = 0; i < ds.Tables["AltKategoriler"].Rows.Count; i++)
                {
                    comboBox2.Items.Add(ds.Tables["AltKategoriler"].Rows[i].ItemArray[0].ToString());
                }

                ds.Tables["AltKategoriler"].Clear();
            }

            baglanti.Close();
        }

        void ilaclarListele()
        {
            ds.Clear();

            OleDbDataAdapter urunlerAdapter = new OleDbDataAdapter("SELECT * FROM Ilaclar", baglanti);
            urunlerAdapter.Fill(ds, "Ilaclar");

            dataGridView1.DataSource = ds.Tables["Ilaclar"];
        }

        void alanlarKontrol(object sender, EventArgs e)
        {
            bool gecerli = false;

            if (materialSingleLineTextField1.Text != "" && materialSingleLineTextField1.Text.All(char.IsDigit))
            {
                materialSingleLineTextField1.BackColor = SystemColors.Window;

                if (materialSingleLineTextField2.Text != "")
                {
                    materialSingleLineTextField2.BackColor = SystemColors.Window;

                    if (comboBox1.Text != "")
                    {
                        comboBox1.BackColor = SystemColors.Window;

                        if (comboBox2.Text != "")
                        {
                            comboBox2.BackColor = SystemColors.Window;

                            if (materialSingleLineTextField3.Text != "" && (materialSingleLineTextField3.Text.All(char.IsDigit) || (materialSingleLineTextField3.Text.Any(char.IsPunctuation) && !materialSingleLineTextField3.Text.Any(char.IsLetter))))
                            {
                                materialSingleLineTextField3.BackColor = SystemColors.Window;

                                if (materialSingleLineTextField4.Text == "" || (materialSingleLineTextField4.Text.All(char.IsDigit) || (materialSingleLineTextField4.Text.Any(char.IsPunctuation) && !materialSingleLineTextField4.Text.Any(char.IsLetter))))
                                {
                                    materialSingleLineTextField4.BackColor = SystemColors.Window;

                                    gecerli = true;
                                }
                                else
                                {
                                    materialSingleLineTextField4.BackColor = Color.Red;
                                    gecerli = false;
                                }
                            }
                            else
                            {
                                materialSingleLineTextField3.BackColor = Color.Red;
                                gecerli = false;
                            }
                        }
                        else
                        {
                            comboBox2.BackColor = Color.Red;
                            gecerli = false;
                        }
                    }
                    else
                    {
                        comboBox1.BackColor = Color.Red;
                        gecerli = false;
                    }
                }
                else
                {
                    materialSingleLineTextField2.BackColor = Color.Red;
                    gecerli = false;
                }
            }
            else
            {
                materialSingleLineTextField1.BackColor = Color.Red;
                gecerli = false;
            }

            materialRaisedButton1.Enabled = gecerli;
        }

        bool ilacBul()
        {
            OleDbDataAdapter ilacBulma = new OleDbDataAdapter("SELECT * FROM Ilaclar WHERE UrunKodu = '" + materialSingleLineTextField1.Text + "' ", baglanti);
            ilacBulma.Fill(ds, "BulunanIlac");

            return ds.Tables["BulunanIlac"].Rows.Count > 0;
        }

        private void MaterialRaisedButton1_Click(object sender, EventArgs e)
        {
            baglantiKontrol();

            bool ilacBulundu = ilacBul();

            if (ilacBulundu)
            {
                DialogResult cevap = MessageBox.Show("İlaç bilgilerini güncellemek istermisiniz?", "Kayıt Bulundu!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (cevap == DialogResult.Yes)
                {
                    OleDbCommand ilacGuncelle = new OleDbCommand("UPDATE Ilaclar SET IlacAdi = '" + materialSingleLineTextField2.Text + "', Kategori = '" + comboBox1.Text + "', AltKategori = '" + comboBox2.Text + "', Fiyat = '" + materialSingleLineTextField3.Text + "', IndirimliFiyat = '" + materialSingleLineTextField4.Text + "', Ozellikler = '" + textBox5.Text + "' WHERE UrunKodu = '" + materialSingleLineTextField1.Text + "' ", baglanti);
                    ilacGuncelle.ExecuteNonQuery();

                    ilaclarListele();
                }
            }
            else
            {
                string ozellikler = "";

                if (textBox5.Text.Contains("'"))
                {
                    ozellikler = textBox5.Text.Remove(textBox5.Text.IndexOf("'"));
                }
                else
                {
                    ozellikler = textBox5.Text;
                }

                OleDbCommand ilacKaydet = new OleDbCommand("INSERT INTO Ilaclar VALUES('" + materialSingleLineTextField1.Text + "', '" + materialSingleLineTextField2.Text + "', '" + comboBox1.Text + "', '" + comboBox2.Text + "', '" + materialSingleLineTextField3.Text + "', '" + materialSingleLineTextField4.Text + "', '" + ozellikler + "')", baglanti);
                ilacKaydet.ExecuteNonQuery();

                pictureBox1.Image.Save("Resimler/" + materialSingleLineTextField1.Text + ".jpg");

                ilaclarListele();
                alanlariTemizle();
            }

            baglanti.Close();
        }

        void alanlariTemizle()
        {
            foreach (Control item in Controls)
            {
                if (item is TextBox || item is ComboBox || item is MaterialSingleLineTextField)
                {
                    item.Text = null;
                }
            }

            pictureBox1.Image = Eczane.Properties.Resources.resim_yok;
        }

        private void MaterialRaisedButton2_Click(object sender, EventArgs e)
        {
            alanlariTemizle();
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            materialSingleLineTextField1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            materialSingleLineTextField2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            materialSingleLineTextField3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            materialSingleLineTextField4.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();

            try
            {
                pictureBox1.Image = Image.FromFile("Resimler/" + materialSingleLineTextField1.Text + ".jpg");
            }
            catch (Exception)
            {
                pictureBox1.Image = pictureBox1.Image = Eczane.Properties.Resources.resim_yok;
            }
            
        }

        private void MaterialRaisedButton3_Click(object sender, EventArgs e)
        {
            bool ilacBulundu = ilacBul();

            if (ilacBulundu)
            {
                DialogResult cevap = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Kayıt Silinicek!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (cevap == DialogResult.Yes)
                {
                    baglantiKontrol();

                    OleDbCommand ilacSil = new OleDbCommand("DELETE * FROM Ilaclar WHERE UrunKodu = '" + materialSingleLineTextField1.Text + "' ", baglanti);
                    ilacSil.ExecuteNonQuery();

                    pictureBox1.Image = pictureBox1.Image = Eczane.Properties.Resources.resim_yok;

                    ilaclarListele();

                    baglanti.Close();
                }
            }
            else
            {
                MessageBox.Show("Böyle Ürün Kodu olan ilaç bulunamadı!");
            }
        }

        private void SilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult cevap = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Kayıt Silinicek!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (cevap == DialogResult.Yes)
            {
                baglantiKontrol();

                OleDbCommand ilacSil = new OleDbCommand("DELETE * FROM Ilaclar WHERE UrunKodu = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "' ", baglanti);
                ilacSil.ExecuteNonQuery();

                ilaclarListele();

                baglanti.Close();
            }
        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            try
            {
                pictureBox1.Image = Image.FromStream(openFileDialog1.OpenFile());
            }
            catch (Exception)
            {
            }

        }

        private void alanlarKontrol()
        {

        }

        #region Form Gölgesi

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
        }

        #endregion
    }
}