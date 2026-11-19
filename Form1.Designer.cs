namespace РечевойПомощник
{
    partial class Form1
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором Windows Forms

        private void InitializeComponent()
        {
            lblЗаголовок = new Label();
            grpРаспознавание = new GroupBox();
            lblМодель = new Label();
            btnНачатьЗапись = new Button();
            btnОстановитьЗапись = new Button();
            lblРаспознанныйТекст = new Label();
            txtРаспознанныйТекст = new TextBox();
            grpОзвучивание = new GroupBox();
            lblГолос = new Label();
            cmbГолоса = new ComboBox();
            btnОзвучить = new Button();
            btnОстановитьОзвучивание = new Button();
            lblТекстДляОзвучивания = new Label();
            txtТекстДляОзвучивания = new TextBox();
            btnОткрытьПапку = new Button();
            btnЗавершитьСеанс = new Button();
            lblСтатус = new Label();
            grpРаспознавание.SuspendLayout();
            grpОзвучивание.SuspendLayout();
            SuspendLayout();
            // 
            // lblЗаголовок
            // 
            lblЗаголовок.AutoSize = true;
            lblЗаголовок.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblЗаголовок.Location = new Point(26, 23);
            lblЗаголовок.Name = "lblЗаголовок";
            lblЗаголовок.Size = new Size(331, 40);
            lblЗаголовок.TabIndex = 0;
            lblЗаголовок.Text = "Голосовой помощник";
            // 
            // grpРаспознавание
            // 
            grpРаспознавание.BackColor = SystemColors.Info;
            grpРаспознавание.Controls.Add(lblМодель);
            grpРаспознавание.Controls.Add(btnНачатьЗапись);
            grpРаспознавание.Controls.Add(btnОстановитьЗапись);
            grpРаспознавание.Controls.Add(lblРаспознанныйТекст);
            grpРаспознавание.Controls.Add(txtРаспознанныйТекст);
            grpРаспознавание.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpРаспознавание.Location = new Point(26, 81);
            grpРаспознавание.Margin = new Padding(3, 4, 3, 4);
            grpРаспознавание.Name = "grpРаспознавание";
            grpРаспознавание.Padding = new Padding(3, 4, 3, 4);
            grpРаспознавание.Size = new Size(914, 424);
            grpРаспознавание.TabIndex = 1;
            grpРаспознавание.TabStop = false;
            grpРаспознавание.Text = "Распознавание речи";
            // 
            // lblМодель
            // 
            lblМодель.AutoSize = true;
            lblМодель.Font = new Font("Segoe UI", 9F);
            lblМодель.Location = new Point(26, 39);
            lblМодель.Name = "lblМодель";
            lblМодель.Size = new Size(258, 20);
            lblМодель.TabIndex = 0;
            lblМодель.Text = "Модель распознавания: проверка...";
            // 
            // btnНачатьЗапись
            // 
            btnНачатьЗапись.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnНачатьЗапись.Location = new Point(30, 76);
            btnНачатьЗапись.Margin = new Padding(3, 4, 3, 4);
            btnНачатьЗапись.Name = "btnНачатьЗапись";
            btnНачатьЗапись.Size = new Size(177, 45);
            btnНачатьЗапись.TabIndex = 1;
            btnНачатьЗапись.Text = "Начать запись";
            btnНачатьЗапись.UseVisualStyleBackColor = true;
            btnНачатьЗапись.Click += btnНачатьЗапись_Click;
            // 
            // btnОстановитьЗапись
            // 
            btnОстановитьЗапись.Enabled = false;
            btnОстановитьЗапись.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnОстановитьЗапись.Location = new Point(221, 76);
            btnОстановитьЗапись.Margin = new Padding(3, 4, 3, 4);
            btnОстановитьЗапись.Name = "btnОстановитьЗапись";
            btnОстановитьЗапись.Size = new Size(177, 71);
            btnОстановитьЗапись.TabIndex = 2;
            btnОстановитьЗапись.Text = "Остановить запись";
            btnОстановитьЗапись.UseVisualStyleBackColor = true;
            btnОстановитьЗапись.Click += btnОстановитьЗапись_Click;
            // 
            // lblРаспознанныйТекст
            // 
            lblРаспознанныйТекст.AutoSize = true;
            lblРаспознанныйТекст.Font = new Font("Segoe UI", 9F);
            lblРаспознанныйТекст.Location = new Point(26, 144);
            lblРаспознанныйТекст.Name = "lblРаспознанныйТекст";
            lblРаспознанныйТекст.Size = new Size(131, 20);
            lblРаспознанныйТекст.TabIndex = 3;
            lblРаспознанныйТекст.Text = "Результат записи:";
            // 
            // txtРаспознанныйТекст
            // 
            txtРаспознанныйТекст.BackColor = SystemColors.Info;
            txtРаспознанныйТекст.Font = new Font("Segoe UI", 10F);
            txtРаспознанныйТекст.Location = new Point(30, 175);
            txtРаспознанныйТекст.Margin = new Padding(3, 4, 3, 4);
            txtРаспознанныйТекст.Multiline = true;
            txtРаспознанныйТекст.Name = "txtРаспознанныйТекст";
            txtРаспознанныйТекст.ReadOnly = true;
            txtРаспознанныйТекст.ScrollBars = ScrollBars.Vertical;
            txtРаспознанныйТекст.Size = new Size(854, 219);
            txtРаспознанныйТекст.TabIndex = 4;
            // 
            // grpОзвучивание
            // 
            grpОзвучивание.BackColor = SystemColors.Info;
            grpОзвучивание.Controls.Add(lblГолос);
            grpОзвучивание.Controls.Add(cmbГолоса);
            grpОзвучивание.Controls.Add(btnОзвучить);
            grpОзвучивание.Controls.Add(btnОстановитьОзвучивание);
            grpОзвучивание.Controls.Add(lblТекстДляОзвучивания);
            grpОзвучивание.Controls.Add(txtТекстДляОзвучивания);
            grpОзвучивание.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpОзвучивание.Location = new Point(26, 523);
            grpОзвучивание.Margin = new Padding(3, 4, 3, 4);
            grpОзвучивание.Name = "grpОзвучивание";
            grpОзвучивание.Padding = new Padding(3, 4, 3, 4);
            grpОзвучивание.Size = new Size(914, 292);
            grpОзвучивание.TabIndex = 2;
            grpОзвучивание.TabStop = false;
            grpОзвучивание.Text = "Озвучивание текста";
            // 
            // lblГолос
            // 
            lblГолос.AutoSize = true;
            lblГолос.Font = new Font("Segoe UI", 9F);
            lblГолос.Location = new Point(26, 39);
            lblГолос.Name = "lblГолос";
            lblГолос.Size = new Size(52, 20);
            lblГолос.TabIndex = 0;
            lblГолос.Text = "Голос:";
            // 
            // cmbГолоса
            // 
            cmbГолоса.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbГолоса.Font = new Font("Segoe UI", 9F);
            cmbГолоса.FormattingEnabled = true;
            cmbГолоса.Location = new Point(79, 33);
            cmbГолоса.Margin = new Padding(3, 4, 3, 4);
            cmbГолоса.Name = "cmbГолоса";
            cmbГолоса.Size = new Size(310, 28);
            cmbГолоса.TabIndex = 1;
            cmbГолоса.SelectedIndexChanged += cmbГолоса_SelectedIndexChanged;
            // 
            // btnОзвучить
            // 
            btnОзвучить.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnОзвучить.Location = new Point(417, 29);
            btnОзвучить.Margin = new Padding(3, 4, 3, 4);
            btnОзвучить.Name = "btnОзвучить";
            btnОзвучить.Size = new Size(149, 39);
            btnОзвучить.TabIndex = 2;
            btnОзвучить.Text = "Озвучить";
            btnОзвучить.UseVisualStyleBackColor = true;
            btnОзвучить.Click += btnОзвучить_Click;
            // 
            // btnОстановитьОзвучивание
            // 
            btnОстановитьОзвучивание.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnОстановитьОзвучивание.Location = new Point(572, 28);
            btnОстановитьОзвучивание.Margin = new Padding(3, 4, 3, 4);
            btnОстановитьОзвучивание.Name = "btnОстановитьОзвучивание";
            btnОстановитьОзвучивание.Size = new Size(192, 71);
            btnОстановитьОзвучивание.TabIndex = 3;
            btnОстановитьОзвучивание.Text = "Остановить озвучивание";
            btnОстановитьОзвучивание.UseVisualStyleBackColor = true;
            btnОстановитьОзвучивание.Click += btnОстановитьОзвучивание_Click;
            // 
            // lblТекстДляОзвучивания
            // 
            lblТекстДляОзвучивания.AutoSize = true;
            lblТекстДляОзвучивания.Font = new Font("Segoe UI", 9F);
            lblТекстДляОзвучивания.Location = new Point(26, 79);
            lblТекстДляОзвучивания.Name = "lblТекстДляОзвучивания";
            lblТекстДляОзвучивания.Size = new Size(172, 20);
            lblТекстДляОзвучивания.TabIndex = 4;
            lblТекстДляОзвучивания.Text = "Введите текст для речи:";
            // 
            // txtТекстДляОзвучивания
            // 
            txtТекстДляОзвучивания.BackColor = SystemColors.Info;
            txtТекстДляОзвучивания.Font = new Font("Segoe UI", 10F);
            txtТекстДляОзвучивания.Location = new Point(30, 108);
            txtТекстДляОзвучивания.Margin = new Padding(3, 4, 3, 4);
            txtТекстДляОзвучивания.Multiline = true;
            txtТекстДляОзвучивания.Name = "txtТекстДляОзвучивания";
            txtТекстДляОзвучивания.ScrollBars = ScrollBars.Vertical;
            txtТекстДляОзвучивания.Size = new Size(854, 153);
            txtТекстДляОзвучивания.TabIndex = 5;
            // 
            // btnОткрытьПапку
            // 
            btnОткрытьПапку.Font = new Font("Segoe UI", 9F);
            btnОткрытьПапку.Location = new Point(26, 833);
            btnОткрытьПапку.Margin = new Padding(3, 4, 3, 4);
            btnОткрытьПапку.Name = "btnОткрытьПапку";
            btnОткрытьПапку.Size = new Size(218, 45);
            btnОткрытьПапку.TabIndex = 3;
            btnОткрытьПапку.Text = "Открыть папку сеансов";
            btnОткрытьПапку.UseVisualStyleBackColor = true;
            btnОткрытьПапку.Click += btnОткрытьПапку_Click;
            // 
            // btnЗавершитьСеанс
            // 
            btnЗавершитьСеанс.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnЗавершитьСеанс.Location = new Point(674, 833);
            btnЗавершитьСеанс.Margin = new Padding(3, 4, 3, 4);
            btnЗавершитьСеанс.Name = "btnЗавершитьСеанс";
            btnЗавершитьСеанс.Size = new Size(266, 45);
            btnЗавершитьСеанс.TabIndex = 4;
            btnЗавершитьСеанс.Text = "Завершить сеанс и сохранить";
            btnЗавершитьСеанс.UseVisualStyleBackColor = true;
            btnЗавершитьСеанс.Click += btnЗавершитьСеанс_Click;
            // 
            // lblСтатус
            // 
            lblСтатус.Font = new Font("Segoe UI", 9F);
            lblСтатус.Location = new Point(265, 833);
            lblСтатус.Name = "lblСтатус";
            lblСтатус.Size = new Size(394, 45);
            lblСтатус.TabIndex = 5;
            lblСтатус.Text = "Готово к работе.";
            lblСтатус.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Info;
            ClientSize = new Size(967, 905);
            Controls.Add(lblСтатус);
            Controls.Add(btnЗавершитьСеанс);
            Controls.Add(btnОткрытьПапку);
            Controls.Add(grpОзвучивание);
            Controls.Add(grpРаспознавание);
            Controls.Add(lblЗаголовок);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Голосовосой помощник";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            grpРаспознавание.ResumeLayout(false);
            grpРаспознавание.PerformLayout();
            grpОзвучивание.ResumeLayout(false);
            grpОзвучивание.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblЗаголовок = null!;
        private System.Windows.Forms.GroupBox grpРаспознавание = null!;
        private System.Windows.Forms.Label lblМодель = null!;
        private System.Windows.Forms.Button btnНачатьЗапись = null!;
        private System.Windows.Forms.Button btnОстановитьЗапись = null!;
        private System.Windows.Forms.Label lblРаспознанныйТекст = null!;
        private System.Windows.Forms.TextBox txtРаспознанныйТекст = null!;
        private System.Windows.Forms.GroupBox grpОзвучивание = null!;
        private System.Windows.Forms.Label lblГолос = null!;
        private System.Windows.Forms.ComboBox cmbГолоса = null!;
        private System.Windows.Forms.Button btnОзвучить = null!;
        private System.Windows.Forms.Button btnОстановитьОзвучивание = null!;
        private System.Windows.Forms.Label lblТекстДляОзвучивания = null!;
        private System.Windows.Forms.TextBox txtТекстДляОзвучивания = null!;
        private System.Windows.Forms.Button btnОткрытьПапку = null!;
        private System.Windows.Forms.Button btnЗавершитьСеанс = null!;
        private System.Windows.Forms.Label lblСтатус = null!;
    }
}
