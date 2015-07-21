using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GDALProcessing
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btn_clip_Click(object sender, EventArgs e)
        {
            ImageClipForm icf = new ImageClipForm();
            icf.ShowDialog();
        }

        private void btn_layerstack_Click(object sender, EventArgs e)
        {
            LayerStackForm lsf = new LayerStackForm();
            lsf.ShowDialog();
        }

        private void btn_resample_Click(object sender, EventArgs e)
        {
            ReSampleForm rsf = new ReSampleForm();
            rsf.ShowDialog();
        }

        private void btn_VToRASTER_Click(object sender, EventArgs e)
        {
            VectorToRasterForm VTRF = new VectorToRasterForm();
            VTRF.ShowDialog();
        }

        private void btn_VICalculation_Click(object sender, EventArgs e)
        {
            VICalculationForm vicf = new VICalculationForm();
            vicf.ShowDialog();
        }

        private void btn_VIStatistic_Click(object sender, EventArgs e)
        {
            VIStatictisForm visf = new VIStatictisForm();
            visf.ShowDialog();
        }

        private void btn_VIStatisticBatch_Click(object sender, EventArgs e)
        {
            VIStatictisBatchForm visf = new VIStatictisBatchForm();
            visf.ShowDialog();
        }

        private void btn_vstogrid_Click(object sender, EventArgs e)
        {
            VIStatictisBatchToGrid visf = new VIStatictisBatchToGrid();
            visf.ShowDialog();
        }

        private void btn_rar_Click(object sender, EventArgs e)
        {
            UnCompressionForm visf = new UnCompressionForm();
            visf.ShowDialog();
        }

        private void btn_RadiometricCorrection_Click(object sender, EventArgs e)
        {
            RadiometricCorrectionForm visf = new RadiometricCorrectionForm();
            visf.ShowDialog();
        }

        private void btn_RpcCorrect_Click(object sender, EventArgs e)
        {
            RPCBatchForm visf = new RPCBatchForm();
            visf.ShowDialog();
        }
    }
}
