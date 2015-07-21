namespace GDALProcessing
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btn_clip = new System.Windows.Forms.Button();
            this.btn_layerstack = new System.Windows.Forms.Button();
            this.btn_resample = new System.Windows.Forms.Button();
            this.btn_VToRASTER = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_VICalculation = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_VIStatistic = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_VIStatisticBatch = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_vstogrid = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_RadiometricCorrection = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_rar = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_RpcCorrect = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_clip
            // 
            this.btn_clip.Image = ((System.Drawing.Image)(resources.GetObject("btn_clip.Image")));
            this.btn_clip.Location = new System.Drawing.Point(12, 12);
            this.btn_clip.Name = "btn_clip";
            this.btn_clip.Size = new System.Drawing.Size(90, 95);
            this.btn_clip.TabIndex = 0;
            this.btn_clip.UseVisualStyleBackColor = true;
            this.btn_clip.Click += new System.EventHandler(this.btn_clip_Click);
            // 
            // btn_layerstack
            // 
            this.btn_layerstack.Image = ((System.Drawing.Image)(resources.GetObject("btn_layerstack.Image")));
            this.btn_layerstack.Location = new System.Drawing.Point(108, 12);
            this.btn_layerstack.Name = "btn_layerstack";
            this.btn_layerstack.Size = new System.Drawing.Size(102, 95);
            this.btn_layerstack.TabIndex = 1;
            this.btn_layerstack.UseVisualStyleBackColor = true;
            this.btn_layerstack.Click += new System.EventHandler(this.btn_layerstack_Click);
            // 
            // btn_resample
            // 
            this.btn_resample.Image = ((System.Drawing.Image)(resources.GetObject("btn_resample.Image")));
            this.btn_resample.Location = new System.Drawing.Point(216, 12);
            this.btn_resample.Name = "btn_resample";
            this.btn_resample.Size = new System.Drawing.Size(105, 95);
            this.btn_resample.TabIndex = 1;
            this.btn_resample.UseVisualStyleBackColor = true;
            this.btn_resample.Click += new System.EventHandler(this.btn_resample_Click);
            // 
            // btn_VToRASTER
            // 
            this.btn_VToRASTER.Image = ((System.Drawing.Image)(resources.GetObject("btn_VToRASTER.Image")));
            this.btn_VToRASTER.Location = new System.Drawing.Point(327, 12);
            this.btn_VToRASTER.Name = "btn_VToRASTER";
            this.btn_VToRASTER.Size = new System.Drawing.Size(102, 95);
            this.btn_VToRASTER.TabIndex = 1;
            this.btn_VToRASTER.UseVisualStyleBackColor = true;
            this.btn_VToRASTER.Click += new System.EventHandler(this.btn_VToRASTER_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "裁切";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "波段组合";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "重采样";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(345, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "矢量转栅格";
            // 
            // btn_VICalculation
            // 
            this.btn_VICalculation.Image = ((System.Drawing.Image)(resources.GetObject("btn_VICalculation.Image")));
            this.btn_VICalculation.Location = new System.Drawing.Point(435, 12);
            this.btn_VICalculation.Name = "btn_VICalculation";
            this.btn_VICalculation.Size = new System.Drawing.Size(102, 95);
            this.btn_VICalculation.TabIndex = 1;
            this.btn_VICalculation.UseVisualStyleBackColor = true;
            this.btn_VICalculation.Click += new System.EventHandler(this.btn_VICalculation_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(448, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "植被指数计算";
            // 
            // btn_VIStatistic
            // 
            this.btn_VIStatistic.Image = ((System.Drawing.Image)(resources.GetObject("btn_VIStatistic.Image")));
            this.btn_VIStatistic.Location = new System.Drawing.Point(543, 12);
            this.btn_VIStatistic.Name = "btn_VIStatistic";
            this.btn_VIStatistic.Size = new System.Drawing.Size(102, 95);
            this.btn_VIStatistic.TabIndex = 1;
            this.btn_VIStatistic.UseVisualStyleBackColor = true;
            this.btn_VIStatistic.Click += new System.EventHandler(this.btn_VIStatistic_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(556, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "植被指数统计";
            // 
            // btn_VIStatisticBatch
            // 
            this.btn_VIStatisticBatch.Image = ((System.Drawing.Image)(resources.GetObject("btn_VIStatisticBatch.Image")));
            this.btn_VIStatisticBatch.Location = new System.Drawing.Point(651, 12);
            this.btn_VIStatisticBatch.Name = "btn_VIStatisticBatch";
            this.btn_VIStatisticBatch.Size = new System.Drawing.Size(102, 95);
            this.btn_VIStatisticBatch.TabIndex = 1;
            this.btn_VIStatisticBatch.UseVisualStyleBackColor = true;
            this.btn_VIStatisticBatch.Click += new System.EventHandler(this.btn_VIStatisticBatch_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(655, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "植被指数统计批量";
            // 
            // btn_vstogrid
            // 
            this.btn_vstogrid.Image = ((System.Drawing.Image)(resources.GetObject("btn_vstogrid.Image")));
            this.btn_vstogrid.Location = new System.Drawing.Point(765, 12);
            this.btn_vstogrid.Name = "btn_vstogrid";
            this.btn_vstogrid.Size = new System.Drawing.Size(102, 95);
            this.btn_vstogrid.TabIndex = 1;
            this.btn_vstogrid.UseVisualStyleBackColor = true;
            this.btn_vstogrid.Click += new System.EventHandler(this.btn_vstogrid_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(763, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "植被指数统计到表单";
            // 
            // btn_RadiometricCorrection
            // 
            this.btn_RadiometricCorrection.Image = ((System.Drawing.Image)(resources.GetObject("btn_RadiometricCorrection.Image")));
            this.btn_RadiometricCorrection.Location = new System.Drawing.Point(228, 154);
            this.btn_RadiometricCorrection.Name = "btn_RadiometricCorrection";
            this.btn_RadiometricCorrection.Size = new System.Drawing.Size(102, 95);
            this.btn_RadiometricCorrection.TabIndex = 1;
            this.btn_RadiometricCorrection.UseVisualStyleBackColor = true;
            this.btn_RadiometricCorrection.Click += new System.EventHandler(this.btn_RadiometricCorrection_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "批量解压缩";
            // 
            // btn_rar
            // 
            this.btn_rar.Image = ((System.Drawing.Image)(resources.GetObject("btn_rar.Image")));
            this.btn_rar.Location = new System.Drawing.Point(12, 154);
            this.btn_rar.Name = "btn_rar";
            this.btn_rar.Size = new System.Drawing.Size(102, 95);
            this.btn_rar.TabIndex = 1;
            this.btn_rar.UseVisualStyleBackColor = true;
            this.btn_rar.Click += new System.EventHandler(this.btn_rar_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(241, 265);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "批量辐射定标";
            // 
            // btn_RpcCorrect
            // 
            this.btn_RpcCorrect.Image = ((System.Drawing.Image)(resources.GetObject("btn_RpcCorrect.Image")));
            this.btn_RpcCorrect.Location = new System.Drawing.Point(120, 154);
            this.btn_RpcCorrect.Name = "btn_RpcCorrect";
            this.btn_RpcCorrect.Size = new System.Drawing.Size(102, 95);
            this.btn_RpcCorrect.TabIndex = 1;
            this.btn_RpcCorrect.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(123, 265);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "批量RPC正射校正";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 307);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_RpcCorrect);
            this.Controls.Add(this.btn_rar);
            this.Controls.Add(this.btn_RadiometricCorrection);
            this.Controls.Add(this.btn_vstogrid);
            this.Controls.Add(this.btn_VIStatisticBatch);
            this.Controls.Add(this.btn_VIStatistic);
            this.Controls.Add(this.btn_VICalculation);
            this.Controls.Add(this.btn_VToRASTER);
            this.Controls.Add(this.btn_resample);
            this.Controls.Add(this.btn_layerstack);
            this.Controls.Add(this.btn_clip);
            this.Name = "frmMain";
            this.Text = "遥感影像预处理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_clip;
        private System.Windows.Forms.Button btn_layerstack;
        private System.Windows.Forms.Button btn_resample;
        private System.Windows.Forms.Button btn_VToRASTER;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_VICalculation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_VIStatistic;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_VIStatisticBatch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_vstogrid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_RadiometricCorrection;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_rar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_RpcCorrect;
        private System.Windows.Forms.Label label11;
    }
}

