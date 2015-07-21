using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace GDALProcessing
{
     public  class ExportDataToExcel
    {
         public static void ExportExcel(System.Data.DataTable dtInfo)
         {
             SaveFileDialog saveFileDialog = new SaveFileDialog();
             saveFileDialog.Filter = "Excel files(*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls|CSV files (*.csv)|*.csv";
             saveFileDialog.FilterIndex = 1;
             saveFileDialog.RestoreDirectory = true;
             saveFileDialog.Title = "导出属性表";
             if (saveFileDialog.ShowDialog() == DialogResult.OK)
             {
                 try
                 {
                     ExportForDataGridview(dtInfo, saveFileDialog.FileName, false);
                     MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
             }
         }

         public static bool ExportForDataGridview(System.Data.DataTable dt, string fileName, bool isShowExcle)
         {

             Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
             try
             {
                 if (app == null)
                 {
                     return false;
                 }

                 app.Visible = isShowExcle;
                 Workbooks workbooks = app.Workbooks;
                 _Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                 Sheets sheets = workbook.Worksheets;
                 _Worksheet worksheet = (_Worksheet)sheets.get_Item(1);
                 if (worksheet == null)
                 {
                     return false;
                 }
                 string sLen = "";
                 //取得最后一列列名
                 char H = (char)(64 + dt.Columns.Count / 26);
                 char L = (char)(64 + dt.Columns.Count % 26);
                 if (dt.Columns.Count < 26)
                 {
                     sLen = L.ToString();
                 }
                 else
                 {
                     sLen = H.ToString() + L.ToString();
                 }


                 //标题
                 string sTmp = sLen + "1";
                 Range ranCaption = worksheet.get_Range(sTmp, "A1");
                 string[] asCaption = new string[dt.Columns.Count];
                 for (int i = 0; i < dt.Columns.Count; i++)
                 {
                     asCaption[i] = dt.Columns[i].ColumnName;
                 }
                 ranCaption.Value2 = asCaption;

                 //数据
                 object[] obj = new object[dt.Columns.Count];
                 for (int r = 0; r < dt.Rows.Count - 1; r++)
                 {
                     for (int l = 0; l < dt.Columns.Count; l++)
                     {
                         if (dt.Rows[r][l].GetType() == typeof(DateTime))
                         {
                             obj[l] = dt.Rows[r][l].ToString();
                         }
                         else
                         {
                             obj[l] = dt.Rows[r][l].ToString();
                         }
                     }
                     string cell1 = sLen + ((int)(r + 2)).ToString();
                     string cell2 = "A" + ((int)(r + 2)).ToString();
                     Range ran = worksheet.get_Range(cell1, cell2);
                     ran.Value2 = obj;
                 }
                 //保存
                 workbook.SaveCopyAs(fileName);
                 workbook.Saved = true;
             }
             finally
             {
                 //关闭
                 app.UserControl = false;
                 app.Quit();
             }
             return true;
         }
    }
}
