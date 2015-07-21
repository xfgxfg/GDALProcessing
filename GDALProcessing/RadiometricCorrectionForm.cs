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
    public partial class RadiometricCorrectionForm : Form
    {
        public RadiometricCorrectionForm()
        {
            InitializeComponent();
        }




        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            RadiometricCorrectionForm form = (RadiometricCorrectionForm)Control.FromHandle(pData);

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
            string str = " ";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < dlg.FileNames.Length; i++)//根据数组长度定义循环次数
                {
                    str = dlg.FileNames.GetValue(i).ToString();//获取文件文件名
                    ListViewItem item = new ListViewItem() { Text = "  "+str };
                    this.listViewImage.Items.Add(item);
                }
            }

            if (!str.Equals(" "))
            this.txt_ImageOutPath.Text = Path.GetDirectoryName(str);

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

                    if (!sFile.Contains("WFV") && !sFile.Contains("PMS") && !sFile.Contains("wfv") && !sFile.Contains("pms"))
                    {
                        MessageBox.Show("请输入包含传感器名称WFV或者PMS的影像！");
                        return;
                    }

                    string sFileName = Path.GetFileNameWithoutExtension(sFile);

                    string sSensorType = sFileName.Substring(4, 4);
                    sSensorType = sSensorType.ToUpper();
                    string[] BandCoefficients = new string[4];
                    switch (sSensorType)
                    {
                        case "PMS1":
                            BandCoefficients[0] = "*0.2082+4.6186";//第一波段定标系数
                            BandCoefficients[1] = "*0.1672+4.8768";//第二波段定标系数
                            BandCoefficients[2] = "*0.1748+4.8924";//第三波段定标系数
                            BandCoefficients[3] = "*0.1883-9.4771";//第四波段定标系数
                            break;
                        case "PMS2":
                            BandCoefficients[0] = "*0.2072+7.5348";//第一波段定标系数
                            BandCoefficients[1] = "*0.1776+3.9395";//第二波段定标系数
                            BandCoefficients[2] = "*0.177-1.7445";//第三波段定标系数
                            BandCoefficients[3] = "*0.1909-7.2053";//第四波段定标系数
                            break;
                        case "WFV1":
                            BandCoefficients[0] = "*0.1709-0.0039";//第一波段定标系数
                            BandCoefficients[1] = "*0.1398-0.0047";//第二波段定标系数
                            BandCoefficients[2] = "*0.1195-0.0030";//第三波段定标系数
                            BandCoefficients[3] = "*0.1338-0.0274";//第四波段定标系数
                            break;
                        case "WFV2":
                            BandCoefficients[0] = "*0.1588+5.5303";//第一波段定标系数
                            BandCoefficients[1] = "*0.1515-13.642";//第二波段定标系数
                            BandCoefficients[2] = "*0.1251-15.382";//第三波段定标系数
                            BandCoefficients[3] = "*0.1209-7.985";//第四波段定标系数
                            break;
                        case "WFV3":
                            BandCoefficients[0] = "*0.1556+12.28";//第一波段定标系数
                            BandCoefficients[1] = "*0.1700-7.9336";//第二波段定标系数
                            BandCoefficients[2] = "*0.1392-7.031";//第三波段定标系数
                            BandCoefficients[3] = "*0.1354-4.3578";//第四波段定标系数
                            break;
                        case "WFV4":
                            BandCoefficients[0] = "*0.1819+3.6469";//第一波段定标系数
                            BandCoefficients[1] = "*0.1762-13.54";//第二波段定标系数
                            BandCoefficients[2] = "*0.1463-10.998";//第三波段定标系数
                            BandCoefficients[3] = "*0.1522-12.142";//第四波段定标系数
                            break;
                    }
                    //默认为NDVI计算输入波段数组
                    int[] nBand = new int[1];
                    string sOutFilePath = sImageOutPath + "\\" + sFileName;
                    sOutFilePath = sOutFilePath.Replace("\\\\", "\\");

                    //波段组合文件名组合参数
                    string sLayerStackName = "";
                    List<string> filelist = new List<string>();

                    for (int i = 1; i <= 4; i++)
                    {
                        string strFormula = "b" + i + "*1" + BandCoefficients[i-1];//默认为NDVI计算公式
                        nBand[0] = i;
                        
                        string sOutFileName = sOutFilePath + "_Band" + i + ".tif";
                        //组装波段组合的输入参数名，所有文件名组成一个字符串，中间用*连接
                        sLayerStackName += sOutFileName + "*";
                        filelist.Add(Path.GetFileName(sOutFileName));
                        ProgressFunc pd = new ProgressFunc(this.ProgressBarInfo);
                        IntPtr pre = this.Handle;
                        int ire = 0;
                        char[] strInFileList = sFile.ToCharArray();
                        char[] strOutFileList = sOutFileName.ToCharArray();

                        ire = GdalAlgInterface.ImageCalculate(strInFileList, strOutFileList, strFormula, nBand, "GTiff", pd, pre);
                        //Console.Write(ire.ToString());
                    }

                    //波段叠加输入波段影像
                    sLayerStackName = sLayerStackName.Substring(0, sLayerStackName.Length - 1);
                    //波段组合后输出影像文件名
                    string sLayerStackResult = sImageOutPath + "\\" + sFileName+"_radiocorrect.tif";
                    


                    ProgressFunc pd2 = new ProgressFunc(this.ProgressBarInfo);
                    IntPtr pre2 = this.Handle;
                    int ire2 = 0;
                    char[] strLayerInFileList = sLayerStackName.ToCharArray();
                    char[] strLayerOutFileList = sLayerStackResult.ToCharArray();
                    ire2 = GdalAlgInterface.ImageLayerStack(strLayerInFileList, strLayerOutFileList, 0, false, "GTiff", pd2, pre2);
                    //MessageBox.Show("波段合成完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btn_ok.Enabled = true;
                    //this.btn_OpenOutPut.Visible = true;
                    //this.progressBar.Visible = false;
                    FileManage pFileManage = new FileManage();
                    pFileManage.DeteleFiles(filelist,sImageOutPath+"\\");

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
                MessageBox.Show("波段提取并辐射定标完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
