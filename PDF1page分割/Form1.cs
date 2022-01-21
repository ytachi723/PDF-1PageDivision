using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace PDF1page分割
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            // 区切り文字設定
            var delimiter = "-";
            if (textBox.TextLength > 0)
            {
                delimiter = textBox.Text;
            }

            // フォルダー内のPDFファイルリスト取得
            System.IO.DirectoryInfo nowFolder = new System.IO.DirectoryInfo(@".");
            System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly;
            IEnumerable<System.IO.FileInfo> now = nowFolder.EnumerateFiles("*.pdf", searchOption);
            PdfDocumentOpenMode pdfOpenMode = PdfDocumentOpenMode.Import;
            PdfDocument targetPDF;

            // フォルダー内のPDFをリストから１つずつ実施
            foreach (var item in now)
            {
                try
                {
                    targetPDF = PdfReader.Open(item.DirectoryName + "\\" + item.ToString(), pdfOpenMode);
                    var page = 1;

                    // １ページずつ保存
                    foreach (PdfPage PDFpage in targetPDF.Pages)
                    {
                        // PDF頁を追加
                        PdfDocument outputPDF = new PdfDocument();
                        outputPDF.AddPage(PDFpage);
                        outputPDF.Save(now.First().DirectoryName + "\\" + Path.GetFileNameWithoutExtension(item.ToString()) + delimiter + page.ToString() + ".pdf");
                        outputPDF.Close();
                        page++;
                    }
                    targetPDF.Close();
                }
                catch(PdfSharp.Pdf.IO.PdfReaderException)
                {
                    continue;
                }
            }
            Application.Exit();
        }
    }
}
