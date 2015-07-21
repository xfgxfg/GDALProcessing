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
    public partial class VectorToRasterForm : Form
    {
        public VectorToRasterForm()
        {
            InitializeComponent();
           
        }

        public int ProgressBarInfo(double dfComplete, char[] strMessage, IntPtr pData)
        {
            VectorToRasterForm form = (VectorToRasterForm)Control.FromHandle(pData);

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
        /// 输出类型数据绑定，从XML文件中获取
        /// </summary>
        public void ComBoxDataBind(string strVectorFile)
        {
            char[] strVectorFileList = strVectorFile.ToCharArray();
            StringBuilder sSBuilder = new StringBuilder(300);
            GdalAlgInterface.GetVectorFields(strVectorFileList, sSBuilder, 300);
            //MessageBox.Show("sSBuilder=" + sSBuilder);
            List<string> list = getListByStringSplit(sSBuilder.ToString());
            this.cbx_FormatType.DataSource = list;


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
            dlg.Filter = "(*.shp)|*.shp|(*.*)|*.*";
            dlg.Multiselect = true;//设置属性为多选
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.txt_ImageInput.Text = dlg.FileName;
            }
            string sVectorInput = this.txt_ImageInput.Text.Trim();
            ComBoxDataBind(sVectorInput);
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
                MessageBox.Show("请选择输入矢量文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (this.cbx_FormatType.SelectedValue.ToString().Equals("请选择"))
            if (this.cbx_FormatType.Text.Equals(""))
            {
                MessageBox.Show("请选择转换字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sResolution = this.txt_resolution.Text;
            if (sResolution.Equals(""))
            {
                MessageBox.Show("请输入分辨率数值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int NResolution = 30;
            bool BResolution = int.TryParse(sResolution, out NResolution);

            if (!BResolution)
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
            string strVectorFile = txt_ImageInput.Text.Trim();
            //string strField = this.cbx_FormatType.SelectedValue.ToString();
            string strField = this.cbx_FormatType.Text.Trim();
            string strRasterFile = this.txt_ImageOutPath.Text.Trim();
            //string sFileName = FileManage.getFileName(strVectorFile);
            string sFileName = Path.GetFileNameWithoutExtension(strVectorFile);
            strRasterFile = strRasterFile + "\\" + sFileName+".tif";


            #endregion

            #region 调用转换算法
            //声明进度信息回调函数
            ProgressFunc pd = new ProgressFunc(this.ProgressBarInfo);
            IntPtr p = this.Handle;
            int ire = 0;
            this.progressBar.Visible = true;
            try
            {
                char[] strVectorFileList = strVectorFile.ToCharArray();

                char[] strRasterFileList = strRasterFile.ToCharArray();

                char[] strFieldList = strField.ToCharArray();

                ire = GdalAlgInterface.ShpRasterize(strVectorFileList, strRasterFileList, NResolution, 2, 0, strFieldList, "GTiff", pd, p);

                MessageBox.Show("矢量转栅格完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
