namespace multiServerTestV01
{
    //partial class SimulationAlgorithm
    partial class DStarForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartButton = new System.Windows.Forms.Button();
            this.Point_Obs = new System.Windows.Forms.RadioButton();
            this.Goal = new System.Windows.Forms.RadioButton();
            this.Start = new System.Windows.Forms.RadioButton();
            this.Move_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ReStart_Button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(6, 20);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(97, 43);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.Start_Click);
            // 
            // Point_Obs
            // 
            this.Point_Obs.AutoSize = true;
            this.Point_Obs.Location = new System.Drawing.Point(328, 56);
            this.Point_Obs.Name = "Point_Obs";
            this.Point_Obs.Size = new System.Drawing.Size(78, 16);
            this.Point_Obs.TabIndex = 38;
            this.Point_Obs.TabStop = true;
            this.Point_Obs.Text = "Point Obs";
            this.Point_Obs.UseVisualStyleBackColor = true;
            // 
            // Goal
            // 
            this.Goal.AutoSize = true;
            this.Goal.Location = new System.Drawing.Point(328, 34);
            this.Goal.Name = "Goal";
            this.Goal.Size = new System.Drawing.Size(81, 16);
            this.Goal.TabIndex = 37;
            this.Goal.TabStop = true;
            this.Goal.Text = "Goal Point";
            this.Goal.UseVisualStyleBackColor = true;
            // 
            // Start
            // 
            this.Start.AutoSize = true;
            this.Start.Location = new System.Drawing.Point(328, 12);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(80, 16);
            this.Start.TabIndex = 36;
            this.Start.TabStop = true;
            this.Start.Text = "Start Point";
            this.Start.UseVisualStyleBackColor = true;
            // 
            // Move_Button
            // 
            this.Move_Button.Location = new System.Drawing.Point(109, 21);
            this.Move_Button.Name = "Move_Button";
            this.Move_Button.Size = new System.Drawing.Size(97, 43);
            this.Move_Button.TabIndex = 52;
            this.Move_Button.Text = "Move";
            this.Move_Button.UseVisualStyleBackColor = true;
            this.Move_Button.Click += new System.EventHandler(this.Move_Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ReStart_Button);
            this.groupBox1.Controls.Add(this.Point_Obs);
            this.groupBox1.Controls.Add(this.Goal);
            this.groupBox1.Controls.Add(this.Start);
            this.groupBox1.Controls.Add(this.StartButton);
            this.groupBox1.Controls.Add(this.Move_Button);
            this.groupBox1.Location = new System.Drawing.Point(4, 781);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(780, 93);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            // 
            // ReStart_Button
            // 
            this.ReStart_Button.Location = new System.Drawing.Point(212, 21);
            this.ReStart_Button.Name = "ReStart_Button";
            this.ReStart_Button.Size = new System.Drawing.Size(97, 43);
            this.ReStart_Button.TabIndex = 53;
            this.ReStart_Button.Text = "ReStart";
            this.ReStart_Button.UseVisualStyleBackColor = true;
            this.ReStart_Button.Click += new System.EventHandler(this.ReStart_Button_Click);
            // 
            // DStarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 875);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "DStarForm";
            this.Text = "SimulationAlgorithm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SimulationAlgorithm_Paint);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SimulationAlgorithm_MouseUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RadioButton Point_Obs;
        private System.Windows.Forms.RadioButton Goal;
        private System.Windows.Forms.RadioButton Start;
        private System.Windows.Forms.Button Move_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ReStart_Button;
    }
}