using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using Excel = Microsoft.Office.Interop.Excel;

namespace Kutuphane_Excel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool formYüklendi = false;
      
        string dosyaAdi = Path.Combine(Directory.GetCurrentDirectory(), "kitaplar_excel.xlsx");
        string kitapAdi;
        string yazar;
        string sayfaSayisi;
        string turu;
        string yayinEvi;
        private void Form1_Load(object sender, EventArgs e)
        {
            

            dgwKitaplar.ColumnCount = 5;
            dgwKitaplar.Columns[0].HeaderText = "Kitap Adı";
            dgwKitaplar.Columns[1].HeaderText = "Yazar";
            dgwKitaplar.Columns[2].HeaderText = "Sayfa sayısı";
            dgwKitaplar.Columns[3].HeaderText = "Türü";
            dgwKitaplar.Columns[4].HeaderText = "Yayın Evi";
            dgwKitaplar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (!File.Exists(dosyaAdi))
            {
                DosyaOlustur(dosyaAdi, dgwKitaplar);
            }
            else
            {
                DgwAktar();
            }
            formYüklendi = true;

        }



        private void btnEkle_Click(object sender, EventArgs e)
        {

            kitapAdi = txbKitapAdi.Text;
            yazar = txbYazar.Text;
            sayfaSayisi = txbSayfaSayisi.Text;
            turu = txbTuru.Text;
            yayinEvi = txbYayinEvi.Text;
            if (string.IsNullOrEmpty(kitapAdi) || string.IsNullOrEmpty(yazar) || string.IsNullOrEmpty(sayfaSayisi) ||
                string.IsNullOrEmpty(turu) || string.IsNullOrEmpty(yayinEvi))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                dgwKitaplar.Rows.Add(kitapAdi, yazar, sayfaSayisi, turu, yayinEvi);
            }
            

        }
      


        static void DosyaOlustur(string dosyaAdi, DataGridView dgwKitaplar)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Worksheets[1];

            for (int i = 0; i < dgwKitaplar.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dgwKitaplar.Columns[i].HeaderText;
            }

            workbook.SaveAs(dosyaAdi);
            workbook.Close();
            excelApp.Quit();
        }

        private void DgwAktar()
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(dosyaAdi);
            Excel.Worksheet worksheet = workbook.Worksheets[1];
            int sonSatir = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

            dgwKitaplar.Rows.Clear();

            for (int satir = 1; satir <= sonSatir; satir++)
            {
                kitapAdi = worksheet.Cells[satir, 1].Value?.ToString();
                yazar = worksheet.Cells[satir, 2].Value?.ToString();
                sayfaSayisi = worksheet.Cells[satir, 3].Value?.ToString();
                turu = worksheet.Cells[satir, 4].Value?.ToString();
                yayinEvi = worksheet.Cells[satir, 5].Value?.ToString();

                dgwKitaplar.Rows.Add(kitapAdi, yazar, sayfaSayisi, turu, yayinEvi);
            }
            workbook.Close();
            excelApp.Quit();
        }

        private void ExcelKayit()
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(dosyaAdi);
            Excel.Worksheet worksheet = workbook.Worksheets[1];
            if (worksheet.Rows.Count > 1)
            {
                worksheet.Rows[1].Delete();
            }

            for (int satir = 0; satir < dgwKitaplar.Rows.Count; satir++)
            {
                for (int sutun = 0; sutun < dgwKitaplar.Columns.Count; sutun++)
                {
                    worksheet.Cells[satir + 1, sutun + 1] = dgwKitaplar.Rows[satir].Cells[sutun].Value;
                }
            }
            workbook.Save();
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show("Değişiklikler Excel dosyasına kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void buttonKaydet_Click(object sender, EventArgs e)
        {
            if (formYüklendi)
                ExcelKayit();
        }

        private void dgwKitaplar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var satir = dgwKitaplar.HitTest(e.X,e.Y);
                if(satir.RowIndex >=0 && satir.RowIndex < dgwKitaplar.Rows.Count)
                {
                    dgwKitaplar.ClearSelection();
                    dgwKitaplar.Rows[satir.RowIndex].Selected = true;
                    DialogResult result= MessageBox.Show("Seçili Kaydı Silmek İstiyor Musunuz?","Kayıt Silme",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                
                    if(result == DialogResult.Yes)
                    {
                        dgwKitaplar.Rows.RemoveAt(satir.RowIndex);
                    }
                }
            }
        }
    }
}
