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

namespace GDALProcessing
{
    public partial class VICalculationForm : Form
    {
        public VICalculationForm()
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
        }


        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            VICalculationForm form = (VICalculationForm)Control.FromHandle(pData);

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
                    ListViewItem item = new ListViewItem() { Text = "  "+str };
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

        /// <summary>
        /// ImageOutPathSelect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImageOutPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txt_ImageOutPath.Text = folderBrowserDialog1.SelectedPath;
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
            if (this.txt_ImageOutPath.Text.Equals(""))
            {
                MessageBox.Show("请选择输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sImageOutPath = this.txt_ImageOutPath.Text.Trim();
            #endregion
            this.btn_ok.Enabled = false;

            #region 界面参数获取

            

            #endregion

            #region 执行合成
            this.progressBar.Visible = true;
            try
            {
                
                foreach (ListViewItem item in this.listViewImage.Items)
                {
                    string sFile = item.SubItems[0].Text.Trim();
                    string strFormula = "(b4-b3)/(b4+b3)*1.0";//默认为NDVI计算公式
                    string sFiles = sFile + "*" + sFile;//默认为NDVI计算输入数据个数，只用到2个波段，所以输入2遍，中间用*隔开
                    //默认为NDVI计算输入波段数组
                    int[] nBand = new int[2];
                    nBand[0] = 3;
                    nBand[1] = 4;
                    string sOutFileName = sImageOutPath +"\\"+ Path.GetFileNameWithoutExtension(sFile);
                    sOutFileName = sOutFileName.Replace("\\\\","\\");
                    switch (this.cbx_VIType.Text.Trim())
                    {
                        case "NDVI":
                            //sFiles = sFile + "*" + sFile;
                            //strFormula = "(b4-b3)/(b4+b3)*1.0";
                            //nBand = new int[2];
                            //nBand[0] = 3;
                            //nBand[1] = 4;
                            sOutFileName = sOutFileName + "_NDVI.tif";
                            break;
                        case "EVI":
                            sFiles = sFile + "*" + sFile + "*" + sFile;
                            strFormula = "2.5*(b4-b3)/(b4+6*b3-7.5*b1+1)";
                            nBand = new int[3];
                            nBand[0] = 1;
                            nBand[1] = 3;
                            nBand[2] = 4;
                            sOutFileName = sOutFileName + "_EVI.tif";
                            break;
                        case "Band1":
                            sFiles = sFile;
                            strFormula = "b1*1.0";
                            nBand = new int[1];
                            nBand[0] = 1;
                            sOutFileName = sOutFileName + "_Band1.tif";
                            break;
                        case "Band2":
                            sFiles = sFile;
                            strFormula = "b2*1.0";
                            nBand = new int[1];
                            nBand[0] = 2;
                            sOutFileName = sOutFileName + "_Band2.tif";
                            break;
                        case "Band3":
                            sFiles = sFile;
                            strFormula = "b3*1.0";
                            nBand = new int[1];
                            nBand[0] = 3;
                            sOutFileName = sOutFileName + "_Band3.tif";
                            break;
                        case "Band4":
                            sFiles = sFile;
                            strFormula = "b4*1.0";
                            nBand = new int[1];
                            nBand[0] = 4;
                            sOutFileName = sOutFileName + "_Band4.tif";
                            break;
                    }
                    

                    ProgressFunc pd = new ProgressFunc(this.ProgressBarInfo);
                    IntPtr pre = this.Handle;
                    int ire = 0;
                    char[] strInFileList = sFiles.ToCharArray();
                    char[] strOutFileList = sOutFileName.ToCharArray();

                    ire = GdalAlgInterface.ImageCalculate(strInFileList, strOutFileList, strFormula, nBand, "GTiff", pd, pre);
                    //Console.Write(ire.ToString());
                }
                
            }
            catch (Exception ex)
            {
                this.progressBar.Visible = false;
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                MessageBox.Show("指数计算完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btn_ok.Enabled = true;
                //this.btn_OpenOutPut.Visible = true;
                this.progressBar.Visible = false;
            }
            
            #endregion

        }

        /// <summary>
        /// 打开输出文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenOutPut_Click(object sender, EventArgs e)
        {
            string sPath = this.txt_ImageOutPath.Text.Trim();
            System.Diagnostics.Process.Start("explorer.exe",sPath);
        }
    }
}
