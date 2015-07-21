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
    public partial class ImageClipForm : Form
    {
        public ImageClipForm()
        {
            InitializeComponent();
        }

        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            ImageClipForm form = (ImageClipForm)Control.FromHandle(pData);

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

            if(this.txt_ReferenceImage.Text.Equals(""))
            {
                MessageBox.Show("请选择输出范围！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string strMskFile = this.txt_ReferenceImage.Text.Trim();
            if (this.txt_ImageOutPath.Text.Equals(""))
            {
                MessageBox.Show("请选择输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sImageOutPath = this.txt_ImageOutPath.Text.Trim();
            #endregion
            this.btn_ok.Enabled = false;

            #region 执行
            this.progressBar.Visible = true;
            try
            {
                foreach (ListViewItem item in this.listViewImage.Items)
                {
                    string sFile = item.SubItems[0].Text.Trim();
                    string sOutFileName = sImageOutPath + "\\" + Path.GetFileNameWithoutExtension(sFile);
                    sOutFileName = sOutFileName.Replace("\\\\", "\\");
                    sOutFileName = sOutFileName + "_Clip.tif";
                    ProgressFunc pd = new ProgressFunc(this.ProgressBarInfo);
                    IntPtr pre = this.Handle;
                    int ire = 0;
                    char[] strInFileList = sFile.ToCharArray();
                    char[] strOutFileList = sOutFileName.ToCharArray();
                    char[] pszMskFileList = strMskFile.ToCharArray();

                    //int[] nBand = new int[1];
                    //nBand[0] = 1;

                    if (Path.GetExtension(strMskFile).Contains("shp"))
                    {
                        ire = GdalAlgInterface.ImageSubsetAoi(strInFileList, strOutFileList, pszMskFileList, null,null, 0, "GTiff", pd, pre);
                    }
                    else
                    {
                        ire = GdalAlgInterface.ImageSubsetMask(strInFileList, strOutFileList, pszMskFileList, null, 0, "GTiff", pd, pre);
                    }
                }
                
                MessageBox.Show("裁剪完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// 打开输出文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenOutPut_Click(object sender, EventArgs e)
        {
            string sPath = this.txt_ImageOutPath.Text.Trim();
            System.Diagnostics.Process.Start("explorer.exe", sPath);
        }
    }
}
