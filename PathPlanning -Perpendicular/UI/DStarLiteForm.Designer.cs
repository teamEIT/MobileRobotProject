namespace multiServerTestV01
{
    partial class DStarLiteForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Move_Button = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.Add_Object = new System.Windows.Forms.GroupBox();
            this.Obs_Radio_Button = new System.Windows.Forms.RadioButton();
            this.Start_Radio_Button = new System.Windows.Forms.RadioButton();
            this.Goal_Radio_Button = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.Add_Object.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Move_Button);
            this.groupBox1.Controls.Add(this.StartButton);
            this.groupBox1.Location = new System.Drawing.Point(1094, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(139, 112);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // Move_Button
            // 
            this.Move_Button.Location = new System.Drawing.Point(15, 59);
            this.Move_Button.Name = "Move_Button";
            this.Move_Button.Size = new System.Drawing.Size(97, 33);
            this.Move_Button.TabIndex = 52;
            this.Move_Button.Text = "Move";
            this.Move_Button.UseVisualStyleBackColor = true;
            this.Move_Button.Click += new System.EventHandler(this.Move_Button_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(15, 20);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(97, 33);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // Add_Object
            // 
            this.Add_Object.Controls.Add(this.Obs_Radio_Button);
            this.Add_Object.Controls.Add(this.Start_Radio_Button);
            this.Add_Object.Controls.Add(this.Goal_Radio_Button);
            this.Add_Object.Location = new System.Drawing.Point(1094, 143);
            this.Add_Object.Name = "Add_Object";
            this.Add_Object.Size = new System.Drawing.Size(139, 105);
            this.Add_Object.TabIndex = 56;
            this.Add_Object.TabStop = false;
            this.Add_Object.Text = "Add Objects";
            // 
            // Obs_Radio_Button
            // 
            this.Obs_Radio_Button.AutoSize = true;
            this.Obs_Radio_Button.Location = new System.Drawing.Point(15, 73);
            this.Obs_Radio_Button.Name = "Obs_Radio_Button";
            this.Obs_Radio_Button.Size = new System.Drawing.Size(73, 16);
            this.Obs_Radio_Button.TabIndex = 39;
            this.Obs_Radio_Button.TabStop = true;
            this.Obs_Radio_Button.Text = "Obstacle";
            this.Obs_Radio_Button.UseVisualStyleBackColor = true;
            // 
            // Start_Radio_Button
            // 
            this.Start_Radio_Button.AutoSize = true;
            this.Start_Radio_Button.Location = new System.Drawing.Point(15, 29);
            this.Start_Radio_Button.Name = "Start_Radio_Button";
            this.Start_Radio_Button.Size = new System.Drawing.Size(48, 16);
            this.Start_Radio_Button.TabIndex = 37;
            this.Start_Radio_Button.TabStop = true;
            this.Start_Radio_Button.Text = "Start";
            this.Start_Radio_Button.UseVisualStyleBackColor = true;
            // 
            // Goal_Radio_Button
            // 
            this.Goal_Radio_Button.AutoSize = true;
            this.Goal_Radio_Button.Location = new System.Drawing.Point(15, 51);
            this.Goal_Radio_Button.Name = "Goal_Radio_Button";
            this.Goal_Radio_Button.Size = new System.Drawing.Size(49, 16);
            this.Goal_Radio_Button.TabIndex = 38;
            this.Goal_Radio_Button.TabStop = true;
            this.Goal_Radio_Button.Text = "Goal";
            this.Goal_Radio_Button.UseVisualStyleBackColor = true;
            // 
            // DStarLiteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 1054);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Add_Object);
            this.DoubleBuffered = true;
            this.Name = "DStarLiteForm";
            this.Text = "DStarLiteForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DStarLiteForm_Paint);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DStarLiteForm_MouseUp);
            this.groupBox1.ResumeLayout(false);
            this.Add_Object.ResumeLayout(false);
            this.Add_Object.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Move_Button;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.GroupBox Add_Object;
        private System.Windows.Forms.RadioButton Obs_Radio_Button;
        private System.Windows.Forms.RadioButton Goal_Radio_Button;
        private System.Windows.Forms.RadioButton Start_Radio_Button;
    }
}