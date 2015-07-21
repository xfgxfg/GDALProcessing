using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GdalAlg;

namespace GDALProcessing
{
    public partial class ReSampleForm : Form
    {
        public ReSampleForm()
        {
            InitializeComponent();
           
        }

        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            ReSampleForm form = (ReSampleForm)Control.FromHandle(pData);

            int iValue = (int)(100 * dfComplete + 0.5);
            form.progressBar.Value = iValue;
            string strMsg = new string(strMessage);
            //form.labelMessage.Text = strMsg;
            return 1;
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
            OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
            dlg.Filter = "(*.tif)|*.tif|(*.*)|*.*";
            dlg.Multiselect = true;//设置属性为多选
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.txt_ImageInput.Text = dlg.FileName;
            }
            string sVectorInput = this.txt_ImageInput.Text.Trim();
            this.txt_ImageOutPath.Text = Path.GetDirectoryName(sVectorInput);
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
            if (this.txt_ImageInput.Text.Equals(""))
            {
                MessageBox.Show("请选择输入影像文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sX = this.txt_X.Text;
            if (sX.Equals(""))
            {
                MessageBox.Show("请输入X方向采样比！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double NX = 1.0;
            bool BX = double.TryParse(sX, out NX);

            if (!BX)
            {
                MessageBox.Show("请输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sY = this.txt_Y.Text;
            if (sY.Equals(""))
            {
                MessageBox.Show("请输入Y方向采样比！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double NY = 1.0;
            bool BY = double.TryParse(sY, out NY);

            if (!BY)
            {
                MessageBox.Show("请输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.txt_ImageOutPath.Text.Equals(""))
            {
                MessageBox.Show("请选择输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            #endregion
            this.btn_ok.Enabled = false;
            #region 界面参数获取
            string strInFile = txt_ImageInput.Text.Trim();
            //string strField = this.cbx_FormatType.SelectedValue.ToString();
            string strX = this.txt_X.Text.Trim();
            string strY = this.txt_Y.Text.Trim();
            string strOutFile = this.txt_ImageOutPath.Text.Trim();
            //string sFileName = FileManage.getFileName(strVectorFile);
            string sFileName = Path.GetFileNameWithoutExtension(strInFile);
            strOutFile = strOutFile + "\\" + sFileName +"_"+ strX +"_"+ strY+ ".tif";


            #endregion

            #region 调用转换算法
            //声明进度信息回调函数
            ProgressFunc pd = new ProgressFunc(this.ProgressBarInfo);
            IntPtr p = this.Handle;
            int ire = 0;
            this.progressBar.Visible = true;
            try
            {
                char[] strInFileList = strInFile.ToCharArray();

                char[] strOutFileList = strOutFile.ToCharArray();

                double DX = double.Parse(strX);
                double DY = double.Parse(strY);
                ire = GdalAlgInterface.ImageResample(strInFileList, strOutFileList, DX, DY, 0, "GTiff", pd, p);

                MessageBox.Show("重采样完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btn_ok.Enabled = true;
                this.btn_OpenOutPut.Visible = true;
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
            System.Diagnostics.Process.Start("explorer.exe",sPath);
        }
    }
}
