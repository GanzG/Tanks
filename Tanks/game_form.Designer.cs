namespace Tanks
{
    partial class game_form
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.search_bt = new System.Windows.Forms.Button();
            this.join_bt = new System.Windows.Forms.Button();
            this.create_bt = new System.Windows.Forms.Button();
            this.close_bt = new System.Windows.Forms.Button();
            this.ipaddr_tb = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.game_pb = new System.Windows.Forms.PictureBox();
            this.map_cb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.game_pb)).BeginInit();
            this.SuspendLayout();
            // 
            // search_bt
            // 
            this.search_bt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.search_bt.Enabled = false;
            this.search_bt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.search_bt.Location = new System.Drawing.Point(932, 120);
            this.search_bt.Name = "search_bt";
            this.search_bt.Size = new System.Drawing.Size(143, 33);
            this.search_bt.TabIndex = 0;
            this.search_bt.Text = "Поиск";
            this.search_bt.UseVisualStyleBackColor = true;
            // 
            // join_bt
            // 
            this.join_bt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.join_bt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.join_bt.Location = new System.Drawing.Point(932, 41);
            this.join_bt.Name = "join_bt";
            this.join_bt.Size = new System.Drawing.Size(143, 33);
            this.join_bt.TabIndex = 1;
            this.join_bt.Text = "Присоединиться";
            this.join_bt.UseVisualStyleBackColor = true;
            this.join_bt.Click += new System.EventHandler(this.join_bt_Click);
            // 
            // create_bt
            // 
            this.create_bt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.create_bt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.create_bt.Location = new System.Drawing.Point(932, 81);
            this.create_bt.Name = "create_bt";
            this.create_bt.Size = new System.Drawing.Size(143, 33);
            this.create_bt.TabIndex = 2;
            this.create_bt.Text = "Создать игру";
            this.create_bt.UseVisualStyleBackColor = true;
            this.create_bt.Click += new System.EventHandler(this.create_bt_Click);
            // 
            // close_bt
            // 
            this.close_bt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.close_bt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.close_bt.Location = new System.Drawing.Point(932, 595);
            this.close_bt.Name = "close_bt";
            this.close_bt.Size = new System.Drawing.Size(143, 33);
            this.close_bt.TabIndex = 3;
            this.close_bt.Text = "Выход из игры";
            this.close_bt.UseVisualStyleBackColor = true;
            this.close_bt.Click += new System.EventHandler(this.close_bt_Click);
            // 
            // ipaddr_tb
            // 
            this.ipaddr_tb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipaddr_tb.Location = new System.Drawing.Point(932, 13);
            this.ipaddr_tb.Name = "ipaddr_tb";
            this.ipaddr_tb.Size = new System.Drawing.Size(143, 22);
            this.ipaddr_tb.TabIndex = 4;
            this.ipaddr_tb.Text = "192.168.1.2";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(932, 159);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(143, 215);
            this.dataGridView1.TabIndex = 5;
            // 
            // game_pb
            // 
            this.game_pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.game_pb.Location = new System.Drawing.Point(13, 13);
            this.game_pb.Name = "game_pb";
            this.game_pb.Size = new System.Drawing.Size(900, 615);
            this.game_pb.TabIndex = 6;
            this.game_pb.TabStop = false;
            // 
            // map_cb
            // 
            this.map_cb.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.map_cb.Items.AddRange(new object[] {
            "Winter map",
            "Autumn map"});
            this.map_cb.Location = new System.Drawing.Point(932, 407);
            this.map_cb.Name = "map_cb";
            this.map_cb.Size = new System.Drawing.Size(121, 25);
            this.map_cb.TabIndex = 7;
            this.map_cb.Text = "Winter map";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(932, 381);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "Выбор карты:";
            // 
            // game_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 640);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.map_cb);
            this.Controls.Add(this.game_pb);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ipaddr_tb);
            this.Controls.Add(this.close_bt);
            this.Controls.Add(this.create_bt);
            this.Controls.Add(this.join_bt);
            this.Controls.Add(this.search_bt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "game_form";
            this.Text = "Tanks";
            this.Load += new System.EventHandler(this.game_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.game_pb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button search_bt;
        private System.Windows.Forms.Button join_bt;
        private System.Windows.Forms.Button create_bt;
        private System.Windows.Forms.Button close_bt;
        private System.Windows.Forms.TextBox ipaddr_tb;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.PictureBox game_pb;
        private System.Windows.Forms.ComboBox map_cb;
        private System.Windows.Forms.Label label1;
    }
}

