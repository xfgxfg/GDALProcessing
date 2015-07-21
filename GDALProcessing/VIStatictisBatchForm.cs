using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GdalAlg;
using System.IO;
using System.Data.SqlClient;

namespace GDALProcessing
{
    public partial class VIStatictisBatchForm : Form
    {
        public VIStatictisBatchForm()
        {
            InitializeComponent();
        }

        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            VIStatictisBatchForm form = (VIStatictisBatchForm)Control.FromHandle(pData);

            int iValue = (int)(100 * dfComplete + 0.5);
            form.progressBar.Value = iValue;
            string strMsg = new string(strMessage);
            //form.labelMessage.Text = strMsg;
            return 1;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// InputFileselect,mutiply
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InPutFile_Click(object sender, EventArgs e)
        {
            this.listViewImage.GridLines = true;
            OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
            dlg.Filter = "(*.tif)|*.tif|(*.*)|*.*";
            dlg.Multiselect = true;//设置属性为多选
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string str = " ";
                for (int i = 0; i < dlg.FileNames.Length; i++)//根据数组长度定义循环次数
                {
                    str = dlg.FileNames.GetValue(i).ToString();//获取文件文件名
                    ListViewItem item = new ListViewItem() { Text = "  " + str };
                    this.listViewImage.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Delete SelectedItems, Mutipline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewImage.SelectedItems)
            {
                listViewImage.Items.Remove(item);
            }
        }

        private void btn_ReferenceImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
            dlg.Multiselect = false;//设置属性为单选
            dlg.Filter = "(*.tif;*.shp)|*.tif;*.shp|(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.txt_ReferenceImage.Text = dlg.FileName;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            #region 输入与输出路径条件判断
            if (this.listViewImage.Items.Count <= 0)
            {
                MessageBox.Show("请选择输入影像！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(this.txt_ReferenceImage.Text.Equals(""))
            {
                MessageBox.Show("请输入统计单元文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string txt_SHPFile = this.txt_ReferenceImage.Text.Trim();
            
            #endregion
            this.btn_ok.Enabled = false;

            #region 执行
            this.progressBar.Visible = true;
            try
            {
                foreach (ListViewItem item in this.listViewImage.Items)
                {
                    string sFile = item.SubItems[0].Text.Trim();
                    string filename = Path.GetFileNameWithoutExtension(sFile);
                    string []res = filename.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    string sMorTime = res[4];
                    string sVIType = res[res.Length - 1].ToString();
                    if (!sVIType.Contains("NDVI") && !sVIType.Contains("EVI") && !sVIType.Contains("Band1") && !sVIType.Contains("Band2") && !sVIType.Contains("Band3") && !sVIType.Contains("Band4"))
                    {
                        break;
                    }
                    string sSensorType = res[1];
                    sSensorType = sSensorType.ToUpper();
                    string sSensorCode = "1";
                    if (sSensorType.Contains("CCD"))
                    {
                        sSensorCode = "1";
                    }
                    else if (sSensorType.Contains("WFV"))
                    {
                        sSensorCode = "3";
                    }
                    else
                    {
                        break;
                    }
                    ProgressFunc pd = new ProgressFunc(this.ProgressBarInfo);
                    IntPtr pre = this.Handle;
                    int ire = 0;
                    //string strInFile = @"D:\share\Hongxingtest\wxf\text_data\soil\soil_organic1.tif";
                    char[] strInFileList = sFile.ToCharArray();

                    string strRegionFile = txt_SHPFile;
                    char[] strRegionFileList = strRegionFile.ToCharArray();

                    string strField = "RASTERID";
                    char[] strFieldList = strField.ToCharArray();

                    int nCount = ReadShape.getShapeCount(strRegionFile);

                    int[] pRegionCodeList = new int[nCount];

                    double[] padfResultList = new double[nCount];
                    if (Path.GetExtension(txt_SHPFile).Contains("shp"))
                    {
                        for (int iStatisticType = 0; iStatisticType < 3; iStatisticType++)
                        {
                            ire = GdalAlgInterface.ImageStatisticalByVector(strInFileList, strRegionFileList, strFieldList, iStatisticType, pRegionCodeList, padfResultList, padfResultList.Length, pd, pre);
                            int result = 0;
                            float fVI = 0.00f;
                            for (int i = 0; i < nCount; i++)
                            {

                                string sRASTERID = pRegionCodeList[i].ToString();

                                string sPlotID = DataBaseOperate.getPlotId(sRASTERID);

                                string sStatisticResult = padfResultList[i].ToString("0.00");
                                bool b = float.TryParse(sStatisticResult, out fVI);
                                if (!b || sStatisticResult.Contains("正"))
                                {
                                    sStatisticResult = "0.00";
                                }
                                string sVI_STATYPE = "Mean";
                                if (iStatisticType == 0)
                                {
                                    sVI_STATYPE = "Min";
                                }
                                else if (iStatisticType == 1)
                                {
                                    sVI_STATYPE = "Max";
                                }

                                SqlParameter[] param = new SqlParameter[] { 
                                    new SqlParameter("@PLOTID", sPlotID),
                                    new SqlParameter("@MONITORTIME", sMorTime),
                                    new SqlParameter("@CROP_CODE", 10),
                                    new SqlParameter("@VI_TYPE", sVIType),
                                    new SqlParameter("@VI_STATYPE", sVI_STATYPE),
                       
                                    new SqlParameter("@VI_VALUE", sStatisticResult),
                                    new SqlParameter("@SENSORTYPE", sSensorCode),
                                    new SqlParameter("@RECORDTIME", DateTime.Now)};
                                result = DataBaseOperate.InsertDatabase("insert_Plot_VI", param);
                            }
                        }
                    }
                    else
                    {
                        for (int iStatisticType = 0; iStatisticType < 3; iStatisticType++)
                        {
                            ire = GdalAlgInterface.ImageStatisticalByVector(strInFileList, strRegionFileList, strFieldList, iStatisticType, pRegionCodeList, padfResultList, padfResultList.Length, pd, pre);
                        }
                    }
                }
                
                MessageBox.Show("统计入库完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btn_ok.Enabled = true;
                //this.btn_OpenOutPut.Visible = true;
                this.progressBar.Visible = false;
            }
            catch (Exception ex)
            {
                this.progressBar.Visible = false;
                MessageBox.Show(ex.Message);
                return;
            }
            #endregion

        }
    }
}
