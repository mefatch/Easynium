namespace Easynium
{
    partial class Form1
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
            this.Logo = new System.Windows.Forms.PictureBox();
            this.x = new System.Windows.Forms.Button();
            this.MinBtn = new System.Windows.Forms.Button();
            this.Code = new System.Windows.Forms.TextBox();
            this.FilePath = new System.Windows.Forms.TextBox();
            this.실행하기 = new System.Windows.Forms.Button();
            this.sublogo = new System.Windows.Forms.Label();
            this.불러오기 = new System.Windows.Forms.Button();
            this.저장하기 = new System.Windows.Forms.Button();
            this.다른이름저장 = new System.Windows.Forms.Button();
            this.MaxBtn = new System.Windows.Forms.Button();
            this.XY = new System.Windows.Forms.Label();
            this.크롬프로필 = new System.Windows.Forms.Button();
            this.엣지프로필 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // Logo
            // 
            this.Logo.Location = new System.Drawing.Point(20, 17);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(107, 56);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Logo.TabIndex = 1;
            this.Logo.TabStop = false;
            this.Logo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Logo_MouseDown);
            // 
            // x
            // 
            this.x.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x.FlatAppearance.BorderSize = 0;
            this.x.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.x.Font = new System.Drawing.Font("Webdings", 7.5F);
            this.x.ForeColor = System.Drawing.Color.White;
            this.x.Location = new System.Drawing.Point(611, 1);
            this.x.Name = "x";
            this.x.Size = new System.Drawing.Size(30, 30);
            this.x.TabIndex = 2;
            this.x.TabStop = false;
            this.x.Text = "r";
            this.x.UseVisualStyleBackColor = true;
            this.x.Click += new System.EventHandler(this.x_Click);
            // 
            // MinBtn
            // 
            this.MinBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinBtn.FlatAppearance.BorderSize = 0;
            this.MinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinBtn.Font = new System.Drawing.Font("Webdings", 7.5F);
            this.MinBtn.ForeColor = System.Drawing.Color.White;
            this.MinBtn.Location = new System.Drawing.Point(549, 1);
            this.MinBtn.Name = "MinBtn";
            this.MinBtn.Size = new System.Drawing.Size(30, 30);
            this.MinBtn.TabIndex = 3;
            this.MinBtn.TabStop = false;
            this.MinBtn.Text = "0";
            this.MinBtn.UseVisualStyleBackColor = true;
            this.MinBtn.Click += new System.EventHandler(this.@__Click);
            // 
            // Code
            // 
            this.Code.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Code.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Code.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Code.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.Code.ForeColor = System.Drawing.Color.White;
            this.Code.Location = new System.Drawing.Point(14, 108);
            this.Code.Multiline = true;
            this.Code.Name = "Code";
            this.Code.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Code.Size = new System.Drawing.Size(614, 494);
            this.Code.TabIndex = 4;
            this.Code.TabStop = false;
            this.Code.WordWrap = false;
            this.Code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Code_KeyDown);
            // 
            // FilePath
            // 
            this.FilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.FilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FilePath.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FilePath.ForeColor = System.Drawing.Color.Aquamarine;
            this.FilePath.Location = new System.Drawing.Point(16, 85);
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Size = new System.Drawing.Size(610, 16);
            this.FilePath.TabIndex = 5;
            this.FilePath.TabStop = false;
            this.FilePath.Text = "File path will be here";
            // 
            // 실행하기
            // 
            this.실행하기.FlatAppearance.BorderSize = 0;
            this.실행하기.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.실행하기.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.실행하기.ForeColor = System.Drawing.Color.White;
            this.실행하기.Location = new System.Drawing.Point(253, 48);
            this.실행하기.Name = "실행하기";
            this.실행하기.Size = new System.Drawing.Size(58, 31);
            this.실행하기.TabIndex = 6;
            this.실행하기.TabStop = false;
            this.실행하기.Text = "Run";
            this.실행하기.UseVisualStyleBackColor = true;
            this.실행하기.Click += new System.EventHandler(this.실행하기_Click);
            // 
            // sublogo
            // 
            this.sublogo.AutoSize = true;
            this.sublogo.Font = new System.Drawing.Font("맑은 고딕", 13F);
            this.sublogo.ForeColor = System.Drawing.Color.White;
            this.sublogo.Location = new System.Drawing.Point(132, 29);
            this.sublogo.Name = "sublogo";
            this.sublogo.Size = new System.Drawing.Size(97, 25);
            this.sublogo.TabIndex = 7;
            this.sublogo.Text = ": Easynium";
            this.sublogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Logo_MouseDown);
            // 
            // 불러오기
            // 
            this.불러오기.FlatAppearance.BorderSize = 0;
            this.불러오기.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.불러오기.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.불러오기.ForeColor = System.Drawing.Color.White;
            this.불러오기.Location = new System.Drawing.Point(317, 48);
            this.불러오기.Name = "불러오기";
            this.불러오기.Size = new System.Drawing.Size(65, 31);
            this.불러오기.TabIndex = 8;
            this.불러오기.TabStop = false;
            this.불러오기.Text = "Open";
            this.불러오기.UseVisualStyleBackColor = true;
            this.불러오기.Click += new System.EventHandler(this.불러오기_Click);
            // 
            // 저장하기
            // 
            this.저장하기.FlatAppearance.BorderSize = 0;
            this.저장하기.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.저장하기.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.저장하기.ForeColor = System.Drawing.Color.White;
            this.저장하기.Location = new System.Drawing.Point(388, 48);
            this.저장하기.Name = "저장하기";
            this.저장하기.Size = new System.Drawing.Size(61, 31);
            this.저장하기.TabIndex = 9;
            this.저장하기.TabStop = false;
            this.저장하기.Text = "Save";
            this.저장하기.UseVisualStyleBackColor = true;
            this.저장하기.Click += new System.EventHandler(this.저장하기_Click);
            // 
            // 다른이름저장
            // 
            this.다른이름저장.FlatAppearance.BorderSize = 0;
            this.다른이름저장.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.다른이름저장.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.다른이름저장.ForeColor = System.Drawing.Color.White;
            this.다른이름저장.Location = new System.Drawing.Point(455, 48);
            this.다른이름저장.Name = "다른이름저장";
            this.다른이름저장.Size = new System.Drawing.Size(73, 31);
            this.다른이름저장.TabIndex = 10;
            this.다른이름저장.TabStop = false;
            this.다른이름저장.Text = "Save As";
            this.다른이름저장.UseVisualStyleBackColor = true;
            this.다른이름저장.Click += new System.EventHandler(this.다른이름저장_Click);
            // 
            // MaxBtn
            // 
            this.MaxBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxBtn.FlatAppearance.BorderSize = 0;
            this.MaxBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaxBtn.Font = new System.Drawing.Font("Webdings", 7.5F);
            this.MaxBtn.ForeColor = System.Drawing.Color.White;
            this.MaxBtn.Location = new System.Drawing.Point(580, 1);
            this.MaxBtn.Name = "MaxBtn";
            this.MaxBtn.Size = new System.Drawing.Size(30, 30);
            this.MaxBtn.TabIndex = 11;
            this.MaxBtn.TabStop = false;
            this.MaxBtn.Text = "1";
            this.MaxBtn.UseVisualStyleBackColor = true;
            this.MaxBtn.Click += new System.EventHandler(this.MaxBtn_Click);
            // 
            // XY
            // 
            this.XY.AutoSize = true;
            this.XY.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.XY.ForeColor = System.Drawing.Color.White;
            this.XY.Location = new System.Drawing.Point(143, 54);
            this.XY.Name = "XY";
            this.XY.Size = new System.Drawing.Size(75, 19);
            this.XY.TabIndex = 12;
            this.XY.Text = ": Easynium";
            this.XY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Logo_MouseDown);
            // 
            // 크롬프로필
            // 
            this.크롬프로필.FlatAppearance.BorderSize = 0;
            this.크롬프로필.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.크롬프로필.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.크롬프로필.ForeColor = System.Drawing.Color.White;
            this.크롬프로필.Location = new System.Drawing.Point(253, 17);
            this.크롬프로필.Name = "크롬프로필";
            this.크롬프로필.Size = new System.Drawing.Size(116, 26);
            this.크롬프로필.TabIndex = 13;
            this.크롬프로필.TabStop = false;
            this.크롬프로필.Text = "Chrome Profile";
            this.크롬프로필.UseVisualStyleBackColor = true;
            this.크롬프로필.Click += new System.EventHandler(this.프로필_Click);
            // 
            // 엣지프로필
            // 
            this.엣지프로필.FlatAppearance.BorderSize = 0;
            this.엣지프로필.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.엣지프로필.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.엣지프로필.ForeColor = System.Drawing.Color.White;
            this.엣지프로필.Location = new System.Drawing.Point(375, 17);
            this.엣지프로필.Name = "엣지프로필";
            this.엣지프로필.Size = new System.Drawing.Size(116, 26);
            this.엣지프로필.TabIndex = 14;
            this.엣지프로필.TabStop = false;
            this.엣지프로필.Text = "Edge Profile";
            this.엣지프로필.UseVisualStyleBackColor = true;
            this.엣지프로필.Click += new System.EventHandler(this.엣지프로필_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(642, 614);
            this.Controls.Add(this.엣지프로필);
            this.Controls.Add(this.크롬프로필);
            this.Controls.Add(this.XY);
            this.Controls.Add(this.MaxBtn);
            this.Controls.Add(this.다른이름저장);
            this.Controls.Add(this.저장하기);
            this.Controls.Add(this.불러오기);
            this.Controls.Add(this.sublogo);
            this.Controls.Add(this.실행하기);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.MinBtn);
            this.Controls.Add(this.x);
            this.Controls.Add(this.Logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(560, 314);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Easynium";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Logo_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.Button x;
        private System.Windows.Forms.Button MinBtn;
        private System.Windows.Forms.TextBox Code;
        private System.Windows.Forms.TextBox FilePath;
        private System.Windows.Forms.Button 실행하기;
        private System.Windows.Forms.Label sublogo;
        private System.Windows.Forms.Button 불러오기;
        private System.Windows.Forms.Button 저장하기;
        private System.Windows.Forms.Button 다른이름저장;
        private System.Windows.Forms.Button MaxBtn;
        private System.Windows.Forms.Label XY;
        private System.Windows.Forms.Button 크롬프로필;
        private System.Windows.Forms.Button 엣지프로필;
    }
}

