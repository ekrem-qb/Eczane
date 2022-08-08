using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.OleDb;

//Ekrem Bayram 1137 11-C

namespace Eczane
{
    public partial class Giris : MaterialForm
    {
        OleDbConnection baglanti = new OleDbConnection("PROVIDER = microsoft.ace.oledb.12.0; DATA SOURCE = Eczane.accdb");
        DataSet ds = new DataSet();

        public Giris()
        {
            InitializeComponent();

            //Form kenarları
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 0, 0));

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red500, Primary.Red600, Primary.Red200, Accent.Red400, TextShade.WHITE);
        }

        private void Giris_Load(object sender, EventArgs e)
        {
            this.Width = label1.Width;
            this.Height = label1.Height;

            this.CenterToScreen();


            kucult(pictureBox9);
            kucult(pictureBox10);
            kucult(pictureBox11);
            kucult(pictureBox12);
            kucult(pictureBox13);
            kucult(pictureBox14);
        }

        void baglantiKontrol()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
        }

        #region Simge animasyonları

        PictureBox pictureBox;
        Image yeniResim;

        void kucult(PictureBox pBox)
        {
            while (pBox.Width > 0)
            {
                pBox.Width -= 2;
                pBox.Height -= 2;

                pBox.Top++;
                pBox.Left++;
            }
        }

        void kuculmeAnimasyonu(PictureBox pBox, Image yResim = null)
        {
            pictureBox = pBox;

            if (yResim == null)
            {
                yeniResim = pBox.Image;
            }
            else
            {
                yeniResim = yResim;
            }
            timer1.Start();

        }

        void buyumeAnimasyonu(PictureBox picBox)
        {
            pictureBox = picBox;
            timer2.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox.Width > 0)
            {
                pictureBox.Width -= 2;
                pictureBox.Height -= 2;

                pictureBox.Left++;
                pictureBox.Top++;
            }
            else
            {
                pictureBox.Image = yeniResim;
                timer1.Stop();
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                if (pictureBox.Width < 24)
                {
                    pictureBox.Width += 2;
                    pictureBox.Height += 2;

                    pictureBox.Left--;
                    pictureBox.Top--;
                }
                else
                {
                    timer2.Stop();
                }
            }
        }

        Image orPic;

        private void TextBox_Enter(object sender, EventArgs e)
        {
            Control textBox = (Control)sender;
            foreach (Control item in splitContainer1.Panel1.Controls)
            {
                if (item is PictureBox && item.Name == textBox.Tag.ToString())
                {
                    PictureBox picBox = (PictureBox)item;
                    orPic = picBox.Image;
                    kuculmeAnimasyonu(picBox, picBox.InitialImage);
                    buyumeAnimasyonu(picBox);
                }
            }
            foreach (Control item in splitContainer1.Panel2.Controls)
            {
                if (item is PictureBox && item.Name == textBox.Tag.ToString())
                {
                    PictureBox picBox = (PictureBox)item;
                    orPic = picBox.Image;
                    kuculmeAnimasyonu(picBox, picBox.InitialImage);
                    buyumeAnimasyonu(picBox);
                }
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            Control textBox = (Control)sender;
            foreach (Control item in splitContainer1.Panel1.Controls)
            {
                if (item is PictureBox && item.Name == textBox.Tag.ToString())
                {
                    PictureBox picBox = (PictureBox)item;
                    picBox.Image = orPic;
                }
            }
            foreach (Control item in splitContainer1.Panel2.Controls)
            {
                if (item is PictureBox && item.Name == textBox.Tag.ToString())
                {
                    PictureBox picBox = (PictureBox)item;
                    picBox.Image = orPic;
                }
            }
        }

        #endregion

        #region Kayıt sayfası animasyonu

        bool ters;

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            timer3.Start();
            ters = false;
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            timer3.Start();
            ters = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.CenterToScreen();
            if (ters)
            {
                if (splitContainer1.SplitterDistance < 300)
                {
                    splitContainer1.SplitterDistance += 10;
                }
                else
                {
                    this.Text = "Giriş";
                    timer3.Stop();
                    timer4.Start();
                }
            }
            if (this.Width < 602)
            {
                this.Width += 10;
            }
            else if (!ters)
            {
                this.Text = "Kayıt Ol";
                timer3.Stop();
                timer4.Start();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            this.CenterToScreen();
            if (!ters)
            {
                if (splitContainer1.SplitterDistance > 0)
                {
                    splitContainer1.SplitterDistance -= 10;
                }
            }
            if (this.Width > 300)
            {
                this.Width -= 10;
            }
            else
            {
                timer4.Stop();
            }
        }

        #endregion

        #region Şifre gözü

        private void PictureBox7_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField2.UseSystemPasswordChar)
            {
                pictureBox7.Image = Eczane.Properties.Resources.goz_acik;
                materialSingleLineTextField2.UseSystemPasswordChar = false;
            }
            else
            {
                pictureBox7.Image = Eczane.Properties.Resources.goz;
                materialSingleLineTextField2.UseSystemPasswordChar = true;
            }
        }

        private void PictureBox8_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField5.UseSystemPasswordChar)
            {
                pictureBox8.Image = Eczane.Properties.Resources.goz_acik;
                materialSingleLineTextField5.UseSystemPasswordChar = false;
                materialSingleLineTextField6.UseSystemPasswordChar = false;
            }
            else
            {
                pictureBox8.Image = Eczane.Properties.Resources.goz;
                materialSingleLineTextField5.UseSystemPasswordChar = true;
                materialSingleLineTextField6.UseSystemPasswordChar = true;
            }
        }

        #endregion

        #region Güzel Kürsör

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int LoadCursor(int hInstance, int lpCursorName);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetCursor(int hCursor);

        private void CursorMouseMove(object sender, MouseEventArgs e)
        {
            SetCursor(LoadCursor(0, 32649));
        }

        private void CursorMouseDown(object sender, MouseEventArgs e)
        {
            SetCursor(LoadCursor(0, 32649));
        }

        #endregion

        #region Açılma animasyonu

        private void timer5_Tick(object sender, EventArgs e)
        {
            timer5.Stop();
            timer6.Start();
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (this.Width < 300)
            {
                this.Width += 2;
                this.Height += 2;

                label1.Left = (this.Width / 2) - (label1.Width / 2);
                label1.Top = (this.Height / 2) - (label1.Height / 2);

                label1.Width = this.Width;
                label1.Height = this.Height;

                this.Left--;
                this.Top--;
            }
            else
            {
                if (label1.Width > 0)
                {
                    label1.Width -= 10;
                    label1.Height -= 10;

                    label1.Top += 9;
                    label1.Left += 3;

                    label1.Font = new Font(label1.Font.Name, label1.Font.Size - 1.25f);
                }
                else
                {
                    label1.Hide();
                    timer6.Stop();
                }
            }
        }

        #endregion

        void PanelAlanlariTemizle(SplitterPanel panel)
        {
            foreach (Control item in panel.Controls)
            {
                if (item is MaterialSingleLineTextField)
                {
                    item.Text = "";
                }
            }
        }

        #region Kayıt Olma

        void KullanıcıKaydet()
        {
            baglantiKontrol();

            OleDbDataAdapter kullaniciAra = new OleDbDataAdapter("SELECT * FROM Kullanicilar WHERE KullaniciAdi = '" + materialSingleLineTextField3.Text + "' ", baglanti);
            kullaniciAra.Fill(ds, "BulunanKullanici");

            if (ds.Tables["BulunanKullanici"].Rows.Count > 0)
            {
                toolTip1.SetToolTip(pictureBox11, "Bu kullanıcı adı alınmış. Başka bir tane deneyin.");
                pictureBox11.Show();
                buyumeAnimasyonu(pictureBox11);
            }
            else
            {
                OleDbCommand kaydet = new OleDbCommand("INSERT INTO Kullanicilar VALUES('" + materialSingleLineTextField3.Text + "' , '" + materialSingleLineTextField5.Text + "' , '" + materialSingleLineTextField4.Text + "')", baglanti);
                kaydet.ExecuteNonQuery();

                PanelAlanlariTemizle(splitContainer1.Panel2);

                timer5.Start();
                timer3.Start();
                ters = true;
            }
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField3.Text != "")
            {
                kuculmeAnimasyonu(pictureBox11);

                if (materialSingleLineTextField5.Text != "")
                {
                    kuculmeAnimasyonu(pictureBox13);

                    if (materialSingleLineTextField6.Text != "" && materialSingleLineTextField5.Text == materialSingleLineTextField6.Text)
                    {
                        if (materialSingleLineTextField4.Text != "")
                        {
                            kuculmeAnimasyonu(pictureBox12);

                            if (materialSingleLineTextField4.Text.Contains('@') && materialSingleLineTextField4.Text.Contains('.') && !materialSingleLineTextField4.Text.EndsWith("."))
                            {
                                kuculmeAnimasyonu(pictureBox12);
                                kuculmeAnimasyonu(pictureBox14);

                                KullanıcıKaydet();
                            }
                            else
                            {
                                pictureBox12.Show();
                                buyumeAnimasyonu(pictureBox12);
                            }
                        }
                        else
                        {
                            kuculmeAnimasyonu(pictureBox12);
                            kuculmeAnimasyonu(pictureBox14);

                            KullanıcıKaydet();
                        }
                    }
                    else
                    {
                        pictureBox14.Show();
                        buyumeAnimasyonu(pictureBox14);
                    }
                }
                else
                {
                    pictureBox13.Show();
                    buyumeAnimasyonu(pictureBox13);
                }
            }
            else
            {
                toolTip1.SetToolTip(pictureBox11, "Kullanıcı Adı boş olamaz!");
                pictureBox11.Show();
                buyumeAnimasyonu(pictureBox11);
            }
        }

        private void materialSingleLineTextField3_TextChanged(object sender, EventArgs e)
        {
            kuculmeAnimasyonu(pictureBox11);
        }

        private void materialSingleLineTextField4_TextChanged(object sender, EventArgs e)
        {
            if (materialSingleLineTextField4.Text != "")
            {
                if (materialSingleLineTextField4.Text.Contains('@') && materialSingleLineTextField4.Text.Contains('.') && !materialSingleLineTextField4.Text.EndsWith("."))
                {
                    kuculmeAnimasyonu(pictureBox12);
                }
                else
                {
                    pictureBox12.Show();
                    buyumeAnimasyonu(pictureBox12);
                }
            }
            else
            {
                kuculmeAnimasyonu(pictureBox12);
            }

        }

        private void materialSingleLineTextField6_TextChanged(object sender, EventArgs e)
        {
            if (materialSingleLineTextField5.Text != "" && materialSingleLineTextField6.Text != "")
            {
                if (materialSingleLineTextField5.Text != materialSingleLineTextField6.Text)
                {
                    pictureBox14.Show();
                    buyumeAnimasyonu(pictureBox14);
                }
                else
                {
                    kuculmeAnimasyonu(pictureBox13);
                    kuculmeAnimasyonu(pictureBox14);
                }
            }
            else
            {
                kuculmeAnimasyonu(pictureBox13);
                kuculmeAnimasyonu(pictureBox14);
            }
        }

        #endregion

        #region Giriş

        string Kullanici;

        void GirisYap(String kullaniciAdi)
        {
            Kullanici = kullaniciAdi;

            label4.Location = new Point((this.Width / 2) - (label4.Width / 2), (this.Height / 2) - (label4.Height / 2));

            panel1.Size = materialRaisedButton1.Size;
            panel1.Location = materialRaisedButton1.Location;
            panel1.Top += splitContainer1.Top;

            label4.Show();
            panel1.Show();

            timer7.Start();

            PanelAlanlariTemizle(splitContainer1.Panel1);
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField1.Text != "")
            {
                kuculmeAnimasyonu(pictureBox9);

                baglantiKontrol();
                OleDbDataAdapter kullaniciAra = new OleDbDataAdapter("SELECT * FROM Kullanicilar WHERE KullaniciAdi = '" + materialSingleLineTextField1.Text + "' ", baglanti);
                kullaniciAra.Fill(ds, "Bulunan Kullanici");

                if (ds.Tables["Bulunan Kullanici"].Rows.Count > 0)
                {
                    kuculmeAnimasyonu(pictureBox9);

                    if (materialSingleLineTextField2.Text != "")
                    {
                        kuculmeAnimasyonu(pictureBox10);

                        if (materialSingleLineTextField2.Text == ds.Tables["Bulunan Kullanici"].Rows[0].ItemArray[1].ToString())
                        {
                            kuculmeAnimasyonu(pictureBox10);

                            GirisYap(ds.Tables["Bulunan Kullanici"].Rows[0].ItemArray[0].ToString());
                        }
                        else
                        {
                            toolTip1.SetToolTip(pictureBox10, "Şifreyi yanlış girdiniz!");

                            pictureBox10.Show();
                            buyumeAnimasyonu(pictureBox10);
                        }
                    }
                    else
                    {
                        toolTip1.SetToolTip(pictureBox10, "Şifeyi giriniz!");

                        pictureBox10.Show();
                        buyumeAnimasyonu(pictureBox10);
                    }
                }
                else
                {
                    toolTip1.SetToolTip(pictureBox9, "Kayıtlı kullanıcı bulunamadı!");

                    pictureBox9.Show();
                    buyumeAnimasyonu(pictureBox9);
                }
            }
            else
            {
                toolTip1.SetToolTip(pictureBox9, "Kullanıcı Adını giriniz!");

                pictureBox9.Show();
                buyumeAnimasyonu(pictureBox9);
            }
        }

        private void materialSingleLineTextField1_TextChanged(object sender, EventArgs e)
        {
            kuculmeAnimasyonu(pictureBox9);
        }

        private void materialSingleLineTextField2_TextChanged(object sender, EventArgs e)
        {
            kuculmeAnimasyonu(pictureBox10);
        }

        #endregion

        private void Giris_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            label4.Location = new Point((this.Width / 2) - (label4.Width / 2), (this.Height / 2) - (label4.Height / 2));

            if (this.Width < Screen.PrimaryScreen.Bounds.Width)
            {
                this.Width += 12;
                this.Left -= 6;
            }
            if (this.Height < Screen.PrimaryScreen.Bounds.Height)
            {
                this.Height += 12;
                this.Top -= 6;
            }

            if (panel1.Width < this.Width)
            {
                panel1.Width += 12;
            }
            if (panel1.Height < this.Height)
            {
                panel1.Height += 12;
            }

            if (panel1.Left > this.Left)
            {
                panel1.Left -= 12;
            }
            if (panel1.Top > this.Top)
            {
                panel1.Top -= 12;
            }

            if (panel1.Width >= this.Width && panel1.Height >= this.Height)
            {
                timer7.Stop();

                AnaSayfa anaSayfa = new AnaSayfa();
                AnaSayfa.Kullanici = Kullanici;
                anaSayfa.Show();
                this.Hide();
            }
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