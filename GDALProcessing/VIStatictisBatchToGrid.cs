using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GdalAlg;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace GDALProcessing
{
    public partial class VIStatictisBatchToGrid : Form
    {
        public VIStatictisBatchToGrid()
        {
            InitializeComponent();
            ComBoxDataBind();
        }

        public void ComBoxDataBind()
        {

            List<string> list = new List<string>();
            list.Add("NDVI");
            list.Add("EVI");
            list.Add("Band1");
            list.Add("Band2");
            list.Add("Band3");
            list.Add("Band4");
            this.cbx_VIType.DataSource = list;


            List<string> vlist = new List<string>();
            vlist.Add("Mean");
            vlist.Add("Max");
            vlist.Add("Min");
            this.cbx_StaValueType.DataSource = vlist;
        }

        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            VIStatictisBatchToGrid form = (VIStatictisBatchToGrid)Control.FromHandle(pData);

            int iValue = (int)(100 * dfComplete + 0.5);
            form.progressBar.Value = iValue;
            string strMsg = new string(strMessage);
            form.labelMessage.Text = strMsg;
            return 1;
        }

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号
        int nCurrent = 0;      //当前记录
        System.Data.DataTable dtInfo;
        string[] res;//存储解析的文件名
        private void InitDataSet()
        {
            pageSize = 20;      //设置页面行数
            nMax = dtInfo.Rows.Count;
            pageCount = (nMax / pageSize);    //计算出总页数
            if ((nMax % pageSize) > 0) pageCount++;
            pageCurrent = 1;    //当前页数从1开始
            nCurrent = 0;       //当前记录数从0开始
            LoadData();
        }

        private void LoadData()
        {
            int nStartPos = 0;   //当前页面开始记录行
            int nEndPos = 0;     //当前页面结束记录行
            if (dtInfo.Rows.Count != 0)
            {
                System.Data.DataTable dtTemp = dtInfo.Clone();   //克隆DataTable结构框架

                if (pageCurrent == pageCount)
                {
                    nEndPos = nMax;
                }
                else
                {
                    nEndPos = pageSize * pageCurrent;
                }

                nStartPos = nCurrent;
                lblPageCount.Text = "/" + pageCount.ToString();
                txtCurrentPage.Text = Convert.ToString(pageCurrent);


                //从元数据源复制记录行
                for (int i = nStartPos; i < nEndPos; i++)
                {

                    dtTemp.ImportRow(dtInfo.Rows[i]);
                    nCurrent++;
                }
                bindingSource1.DataSource = dtTemp;
                bindingNavigator1.BindingSource = bindingSource1;
                dgvInfo.DataSource = bindingSource1;
                dgvInfo.Visible = true;
            }
        }


        private void btn_classstatis_Click(object sender, EventArgs e)
        {
            #region 输入与输出路径条件判断

            string sImageInput = this.txt_ImageInput.Text.Trim();
            if (sImageInput.Equals(""))
            {
                MessageBox.Show("请选择待统计影像！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //string filename = sImageInput.Substring(sImageInput.LastIndexOf("\\") + 1);
            //res = filename.Split(new char[] { '_', '.' }, StringSplitOptions.RemoveEmptyEntries);
            string filename = Path.GetFileNameWithoutExtension(sImageInput);
            if (filename.Contains("-"))
            {
                res = filename.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                res = filename.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            }
            string sMorTime = res[4];
            string sResultTitle = this.cbx_VIType.Text.Trim();
            string sField = this.cbx_FieldName.Text.Trim();
            string sStaType = this.cbx_StaValueType.Text.Trim();
            #endregion

            //调用进度条界面线程
            //Thread t = new Thread(new ThreadStart(thread1));
            //t.Start();
            #region 执行统计

            try
            {
                ProgressFunc pd = new ProgressFunc(this.ProgressBarInfo);
                IntPtr pre = this.Handle;
                int ire = 0;

                //string strInFile = @"D:\share\Hongxingtest\wxf\text_data\soil\soil_organic1.tif";
                char[] strInFileList = sImageInput.ToCharArray();

                string strRegionFile = this.txt_SHPFile.Text.Trim();
                char[] strRegionFileList = strRegionFile.ToCharArray();

                char[] strFieldList = sField.ToCharArray();



                List<string> fielddatalist = ReadShape.getShapeFieldDataList(strRegionFile, sField);
                int nCount = fielddatalist.Count;

                List<int> intfdlist = new List<int>(nCount);
                int[] pRegionCodeList = new int[nCount];
                //for (int i = 0; i < nCount; i++)
                //{
                //    //转LIST出错
                //    intfdlist.Add(int.Parse(fielddatalist[i]));
                //}

                //int[] pRegionCodeList = intfdlist.ToArray();

                double[] padfResultList = new double[nCount];

                int iStatisticType = 2; //默认为平均值
                if (sStaType == "Max")
                {
                    iStatisticType = 1;
                }
                else if (sStaType == "Min")
                {
                    iStatisticType = 0;
                }

                ire = GdalAlgInterface.ImageStatisticalByVector(strInFileList, strRegionFileList, strFieldList, iStatisticType, pRegionCodeList, padfResultList, padfResultList.Length, pd, pre);
                //第三步
                //运行成功或失败，停止线程，即终止进度条。
                //t.Abort();
                string[,] arrStatistic = new string[nCount, 4];
                for (int i = 0; i < nCount; i++)
                {
                    arrStatistic[i, 0] = sMorTime;//文件名中的时间，从文件名中解析，此处获取的系统时间只为测试
                    arrStatistic[i, 1] = pRegionCodeList[i].ToString();
                    arrStatistic[i, 2] = padfResultList[i].ToString("0.00");
                    arrStatistic[i, 3] = sStaType;
                }
                //string sResultTitle = DataBaseOperate.getNUTRIENTTableTitleName(res[2]);
                
                //表头
                string[] arrName = { "监测时间", "标识字段", sResultTitle, "统计类型" };
                //数组行按"，"拆分后转DataTable
                dtInfo = StringFormater.Convert(arrName, arrStatistic);
                InitDataSet();
                //dgvInfo.DataSource = dt;
            }
            catch (Exception ex)
            {
                //第三步
                //运行成功或失败，停止线程，即终止进度条。
                //t.Abort();
                MessageBox.Show(ex.Message);

            }
            #endregion
        }

        private void btn_InPutFile_Click(object sender, EventArgs e)
        {

            //修改，只能选择一个
            OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
            dlg.Filter = "(*.tif)|*.tif|(*.*)|*.*";
            dlg.Multiselect = false;//设置属性为多选
            string sFileName = "";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                sFileName = dlg.FileNames.GetValue(0).ToString();//获取文件文件名
                if (!sFileName.Contains("_"))
                {
                    MessageBox.Show("输入的数据命名不合法，文件名需要包括“_”在内，且后面接指数名称，如hj1b-ccd1-449-57-20131015_NDVI.tif！");
                    return;
                }

                else
                {
                    this.txt_ImageInput.Text = sFileName;
                }
            }
            
        }

        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "关闭")
            {
                this.Close();
            }
            if (e.ClickedItem.Text == "上一页")
            {
                pageCurrent--;
                if (pageCurrent <= 0)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
            if (e.ClickedItem.Text == "下一页")
            {
                pageCurrent++;
                if (pageCurrent > pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
        }

       

        private void toolStripButton1_Click(object sender, EventArgs e)
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

       /// <summary>
       /// 长字串拆分后封装到LIST中
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static List<string> getListByStringSplit(string str)
       {
           List<string> list = new List<string>();
           string[] sStrs = str.Substring(0, str.Length - 1).Split('@');
           foreach (var item in sStrs)
           {
               list.Add(item);
           }

           return list;
       }

       /// <summary>
       /// 输出类型数据绑定，从XML文件中获取
       /// </summary>
       public void ComBoxDataBind(string strVectorFile)
       {
           char[] strVectorFileList = strVectorFile.ToCharArray();
           StringBuilder sSBuilder = new StringBuilder(300);
           GdalAlgInterface.GetVectorFields(strVectorFileList, sSBuilder, 300);
           //MessageBox.Show("sSBuilder=" + sSBuilder);
           List<string> list = getListByStringSplit(sSBuilder.ToString());
           this.cbx_FieldName.DataSource = list;


       }

       private void btn_OpenSHP_Click(object sender, EventArgs e)
       {
           OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
           dlg.Filter = "(*.shp)|*.shp";
           dlg.Multiselect = false;//设置属性为多选
           if (dlg.ShowDialog() == DialogResult.OK)
           {
               this.txt_SHPFile.Text = dlg.FileNames.GetValue(0).ToString();//获取文件文件名
           }
           string sVectorInput = this.txt_SHPFile.Text.Trim();
           ComBoxDataBind(sVectorInput);
       }



    }
}
