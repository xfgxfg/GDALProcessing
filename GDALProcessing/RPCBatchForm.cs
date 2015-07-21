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
    public partial class RPCBatchForm : Form
    {
        public RPCBatchForm()
        {
            InitializeComponent();
        }




        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            RPCBatchForm form = (RPCBatchForm)Control.FromHandle(pData);

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
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txt_ImageInput.Text = folderBrowserDialog1.SelectedPath;

                string sInputPath = this.txt_ImageInput.Text;
                //获取输入目录下所有压缩包文件名
                List<string> listFileName = FileManage.getAllFileNameFromFolder(sInputPath, ".tar.gz");

                //将所有文件名绑定到LIST上显示在界面上
                foreach (var filename in listFileName)
	            {
		            ListViewItem item = new ListViewItem() { Text = "  " + filename };
                    this.listViewImage.Items.Add(item);
	            }
            }

            //输出路径=输入路径
            this.txt_ImageOutPath.Text = this.txt_ImageInput.Text;

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
                MessageBox.Show("请选择输入路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sImageInPath = this.txt_ImageInput.Text.Trim();
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
                    //去掉文件名中的.tar.gz
                    string subFolder = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sFile));
                    string sUPath = clsWinrar.unCompressRAR(sImageOutPath + "\\" + subFolder, sImageInPath, sFile);

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
                MessageBox.Show("解压完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btn_ok.Enabled = true;
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
