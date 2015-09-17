namespace multiServerTestV01
{
    partial class A_Star_Form
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
            this.Move_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Goal = new System.Windows.Forms.RadioButton();
            this.Start = new System.Windows.Forms.RadioButton();
            this.Add_Object = new System.Windows.Forms.GroupBox();
            this.Restart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.Add_Object.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(18, 20);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(97, 33);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // Point_Obs
            // 
            this.Point_Obs.AutoSize = true;
            this.Point_Obs.Location = new System.Drawing.Point(19, 64);
            this.Point_Obs.Name = "Point_Obs";
            this.Point_Obs.Size = new System.Drawing.Size(78, 16);
            this.Point_Obs.TabIndex = 38;
            this.Point_Obs.TabStop = true;
            this.Point_Obs.Text = "Point Obs";
            this.Point_Obs.UseVisualStyleBackColor = true;
            // 
            // Move_Button
            // 
            this.Move_Button.Location = new System.Drawing.Point(19, 59);
            this.Move_Button.Name = "Move_Button";
            this.Move_Button.Size = new System.Drawing.Size(97, 33);
            this.Move_Button.TabIndex = 52;
            this.Move_Button.Text = "Move";
            this.Move_Button.UseVisualStyleBackColor = true;
            this.Move_Button.Click += new System.EventHandler(this.Move_Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Restart);
            this.groupBox1.Controls.Add(this.Move_Button);
            this.groupBox1.Controls.Add(this.StartButton);
            this.groupBox1.Location = new System.Drawing.Point(791, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(139, 148);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // Goal
            // 
            this.Goal.AutoSize = true;
            this.Goal.Location = new System.Drawing.Point(18, 42);
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
            this.Start.Location = new System.Drawing.Point(19, 20);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(80, 16);
            this.Start.TabIndex = 36;
            this.Start.TabStop = true;
            this.Start.Text = "Start Point";
            this.Start.UseVisualStyleBackColor = true;
            // 
            // Add_Object
            // 
            this.Add_Object.Controls.Add(this.Point_Obs);
            this.Add_Object.Controls.Add(this.Goal);
            this.Add_Object.Controls.Add(this.Start);
            this.Add_Object.Location = new System.Drawing.Point(791, 194);
            this.Add_Object.Name = "Add_Object";
            this.Add_Object.Size = new System.Drawing.Size(139, 98);
            this.Add_Object.TabIndex = 53;
            this.Add_Object.TabStop = false;
            this.Add_Object.Text = "Add Objects";
            // 
            // Restart
            // 
            this.Restart.Location = new System.Drawing.Point(18, 98);
            this.Restart.Name = "Restart";
            this.Restart.Size = new System.Drawing.Size(97, 33);
            this.Restart.TabIndex = 53;
            this.Restart.Text = "Restart";
            this.Restart.UseVisualStyleBackColor = true;
            this.Restart.Click += new System.EventHandler(this.Restart_Click);
            // 
            // A_Star_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 767);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Add_Object);
            this.DoubleBuffered = true;
            this.Name = "A_Star_Form";
            this.Text = "A_Star_Form";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.A_Star_Form_Paint);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.A_Star_Form_MouseUp);
            this.groupBox1.ResumeLayout(false);
            this.Add_Object.ResumeLayout(false);
            this.Add_Object.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RadioButton Point_Obs;
        private System.Windows.Forms.Button Move_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Goal;
        private System.Windows.Forms.RadioButton Start;
        private System.Windows.Forms.GroupBox Add_Object;
        private System.Windows.Forms.Button Restart;
    }
}