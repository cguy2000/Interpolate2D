namespace SimpleInterpolation
{
    partial class Test
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.lblRectangleDelta = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRectangleDelta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPowerOfDiatance = new System.Windows.Forms.TextBox();
            this.txtPointsCount = new System.Windows.Forms.TextBox();
            this.btnDelaunai = new System.Windows.Forms.Button();
            this.btnMakePoints = new System.Windows.Forms.Button();
            this.chkCloughTocher2D = new System.Windows.Forms.CheckBox();
            this.chkVoronoiLine = new System.Windows.Forms.CheckBox();
            this.chkDelaunayTriangle = new System.Windows.Forms.CheckBox();
            this.chkVoronoiDiagram = new System.Windows.Forms.CheckBox();
            this.chkInterpolation = new System.Windows.Forms.CheckBox();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(585, 438);
            this.pnlCanvas.TabIndex = 0;
            this.pnlCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCanvas_Paint);
            // 
            // pnlControl
            // 
            this.pnlControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlControl.Controls.Add(this.lblRectangleDelta);
            this.pnlControl.Controls.Add(this.label2);
            this.pnlControl.Controls.Add(this.txtRectangleDelta);
            this.pnlControl.Controls.Add(this.label1);
            this.pnlControl.Controls.Add(this.txtPowerOfDiatance);
            this.pnlControl.Controls.Add(this.txtPointsCount);
            this.pnlControl.Controls.Add(this.btnDelaunai);
            this.pnlControl.Controls.Add(this.btnMakePoints);
            this.pnlControl.Controls.Add(this.chkCloughTocher2D);
            this.pnlControl.Controls.Add(this.chkVoronoiLine);
            this.pnlControl.Controls.Add(this.chkDelaunayTriangle);
            this.pnlControl.Controls.Add(this.chkVoronoiDiagram);
            this.pnlControl.Controls.Add(this.chkInterpolation);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlControl.Location = new System.Drawing.Point(585, 0);
            this.pnlControl.Margin = new System.Windows.Forms.Padding(2);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(199, 438);
            this.pnlControl.TabIndex = 1;
            // 
            // lblRectangleDelta
            // 
            this.lblRectangleDelta.AutoSize = true;
            this.lblRectangleDelta.Location = new System.Drawing.Point(24, 107);
            this.lblRectangleDelta.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRectangleDelta.Name = "lblRectangleDelta";
            this.lblRectangleDelta.Size = new System.Drawing.Size(100, 12);
            this.lblRectangleDelta.TabIndex = 2;
            this.lblRectangleDelta.Text = "Rectangle delta :";
            this.lblRectangleDelta.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 81);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Power of distance :";
            this.label2.Visible = false;
            // 
            // txtRectangleDelta
            // 
            this.txtRectangleDelta.Location = new System.Drawing.Point(129, 105);
            this.txtRectangleDelta.Margin = new System.Windows.Forms.Padding(2);
            this.txtRectangleDelta.Name = "txtRectangleDelta";
            this.txtRectangleDelta.Size = new System.Drawing.Size(39, 21);
            this.txtRectangleDelta.TabIndex = 1;
            this.txtRectangleDelta.Text = "1";
            this.txtRectangleDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRectangleDelta.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 51);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Points count :";
            this.label1.Visible = false;
            // 
            // txtPowerOfDiatance
            // 
            this.txtPowerOfDiatance.Location = new System.Drawing.Point(129, 79);
            this.txtPowerOfDiatance.Margin = new System.Windows.Forms.Padding(2);
            this.txtPowerOfDiatance.Name = "txtPowerOfDiatance";
            this.txtPowerOfDiatance.Size = new System.Drawing.Size(39, 21);
            this.txtPowerOfDiatance.TabIndex = 1;
            this.txtPowerOfDiatance.Text = "4";
            this.txtPowerOfDiatance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPowerOfDiatance.Visible = false;
            // 
            // txtPointsCount
            // 
            this.txtPointsCount.Location = new System.Drawing.Point(129, 49);
            this.txtPointsCount.Margin = new System.Windows.Forms.Padding(2);
            this.txtPointsCount.Name = "txtPointsCount";
            this.txtPointsCount.Size = new System.Drawing.Size(39, 21);
            this.txtPointsCount.TabIndex = 1;
            this.txtPointsCount.Text = "3";
            this.txtPointsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPointsCount.Visible = false;
            // 
            // btnDelaunai
            // 
            this.btnDelaunai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelaunai.Location = new System.Drawing.Point(104, 9);
            this.btnDelaunai.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelaunai.Name = "btnDelaunai";
            this.btnDelaunai.Size = new System.Drawing.Size(82, 31);
            this.btnDelaunai.TabIndex = 0;
            this.btnDelaunai.Text = "Delaunai";
            this.btnDelaunai.UseVisualStyleBackColor = true;
            this.btnDelaunai.Click += new System.EventHandler(this.btnDelaunai_Click);
            // 
            // btnMakePoints
            // 
            this.btnMakePoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMakePoints.Location = new System.Drawing.Point(9, 9);
            this.btnMakePoints.Margin = new System.Windows.Forms.Padding(2);
            this.btnMakePoints.Name = "btnMakePoints";
            this.btnMakePoints.Size = new System.Drawing.Size(91, 31);
            this.btnMakePoints.TabIndex = 0;
            this.btnMakePoints.Text = "Make Points";
            this.btnMakePoints.UseVisualStyleBackColor = true;
            this.btnMakePoints.Click += new System.EventHandler(this.btnMakePoints_Click);
            // 
            // chkCloughTocher2D
            // 
            this.chkCloughTocher2D.AutoSize = true;
            this.chkCloughTocher2D.Checked = true;
            this.chkCloughTocher2D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCloughTocher2D.Location = new System.Drawing.Point(9, 217);
            this.chkCloughTocher2D.Margin = new System.Windows.Forms.Padding(2);
            this.chkCloughTocher2D.Name = "chkCloughTocher2D";
            this.chkCloughTocher2D.Size = new System.Drawing.Size(126, 16);
            this.chkCloughTocher2D.TabIndex = 3;
            this.chkCloughTocher2D.Text = "Clough Tocher 2D";
            this.chkCloughTocher2D.UseVisualStyleBackColor = true;
            this.chkCloughTocher2D.CheckedChanged += new System.EventHandler(this.chkInterpolation_CheckedChanged);
            // 
            // chkVoronoiLine
            // 
            this.chkVoronoiLine.AutoSize = true;
            this.chkVoronoiLine.Location = new System.Drawing.Point(9, 197);
            this.chkVoronoiLine.Margin = new System.Windows.Forms.Padding(2);
            this.chkVoronoiLine.Name = "chkVoronoiLine";
            this.chkVoronoiLine.Size = new System.Drawing.Size(95, 16);
            this.chkVoronoiLine.TabIndex = 3;
            this.chkVoronoiLine.Text = "Voronoi Line";
            this.chkVoronoiLine.UseVisualStyleBackColor = true;
            this.chkVoronoiLine.CheckedChanged += new System.EventHandler(this.chkInterpolation_CheckedChanged);
            // 
            // chkDelaunayTriangle
            // 
            this.chkDelaunayTriangle.AutoSize = true;
            this.chkDelaunayTriangle.Location = new System.Drawing.Point(9, 178);
            this.chkDelaunayTriangle.Margin = new System.Windows.Forms.Padding(2);
            this.chkDelaunayTriangle.Name = "chkDelaunayTriangle";
            this.chkDelaunayTriangle.Size = new System.Drawing.Size(127, 16);
            this.chkDelaunayTriangle.TabIndex = 3;
            this.chkDelaunayTriangle.Text = "Delaunay Triangle";
            this.chkDelaunayTriangle.UseVisualStyleBackColor = true;
            this.chkDelaunayTriangle.CheckedChanged += new System.EventHandler(this.chkInterpolation_CheckedChanged);
            // 
            // chkVoronoiDiagram
            // 
            this.chkVoronoiDiagram.AutoSize = true;
            this.chkVoronoiDiagram.Location = new System.Drawing.Point(9, 159);
            this.chkVoronoiDiagram.Margin = new System.Windows.Forms.Padding(2);
            this.chkVoronoiDiagram.Name = "chkVoronoiDiagram";
            this.chkVoronoiDiagram.Size = new System.Drawing.Size(118, 16);
            this.chkVoronoiDiagram.TabIndex = 3;
            this.chkVoronoiDiagram.Text = "Voronoi Diagram";
            this.chkVoronoiDiagram.UseVisualStyleBackColor = true;
            this.chkVoronoiDiagram.CheckedChanged += new System.EventHandler(this.chkInterpolation_CheckedChanged);
            // 
            // chkInterpolation
            // 
            this.chkInterpolation.AutoSize = true;
            this.chkInterpolation.Location = new System.Drawing.Point(9, 141);
            this.chkInterpolation.Margin = new System.Windows.Forms.Padding(2);
            this.chkInterpolation.Name = "chkInterpolation";
            this.chkInterpolation.Size = new System.Drawing.Size(92, 16);
            this.chkInterpolation.TabIndex = 3;
            this.chkInterpolation.Text = "Interpolation";
            this.chkInterpolation.UseVisualStyleBackColor = true;
            this.chkInterpolation.CheckedChanged += new System.EventHandler(this.chkInterpolation_CheckedChanged);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 438);
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.pnlControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Test";
            this.Text = "SimpleInterpolation";
            this.pnlControl.ResumeLayout(false);
            this.pnlControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPowerOfDiatance;
        private System.Windows.Forms.TextBox txtPointsCount;
        private System.Windows.Forms.Button btnMakePoints;
        private System.Windows.Forms.CheckBox chkInterpolation;
        private System.Windows.Forms.CheckBox chkVoronoiDiagram;
        private System.Windows.Forms.CheckBox chkDelaunayTriangle;
        private System.Windows.Forms.Label lblRectangleDelta;
        private System.Windows.Forms.TextBox txtRectangleDelta;
        private System.Windows.Forms.CheckBox chkVoronoiLine;
        private System.Windows.Forms.Button btnDelaunai;
        private System.Windows.Forms.CheckBox chkCloughTocher2D;
    }
}

