namespace Projekt_zaawansowane_metody_obliczeniowe
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            button1 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            textBox3 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            label2 = new Label();
            label5 = new Label();
            textBox4 = new TextBox();
            label6 = new Label();
            textBox5 = new TextBox();
            buttonPause = new Button();
            buttonStop = new Button();
            label7 = new Label();
            textBox6 = new TextBox();
            label8 = new Label();
            label9 = new Label();
            textBox7 = new TextBox();
            label10 = new Label();
            textBox8 = new TextBox();
            textBox9 = new TextBox();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            label15 = new Label();
            label16 = new Label();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            textBox10 = new TextBox();
            textBox11 = new TextBox();
            textBox12 = new TextBox();
            textBox13 = new TextBox();
            textBox14 = new TextBox();
            textBox15 = new TextBox();
            button4 = new Button();
            label20 = new Label();
            label22 = new Label();
            label23 = new Label();
            textBox16 = new TextBox();
            label21 = new Label();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            label24 = new Label();
            label25 = new Label();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            checkBox1 = new CheckBox();
            textBox17 = new TextBox();
            textBox18 = new TextBox();
            label26 = new Label();
            label27 = new Label();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(41, 248);
            button1.Name = "button1";
            button1.Size = new Size(136, 46);
            button1.TabIndex = 0;
            button1.Text = "Generuj instancję";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label1.Location = new Point(137, 326);
            label1.Name = "label1";
            label1.Size = new Size(76, 20);
            label1.TabIndex = 1;
            label1.Text = "Instancja:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(41, 56);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(290, 27);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(41, 109);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(290, 27);
            textBox2.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 33);
            label3.Name = "label3";
            label3.Size = new Size(223, 20);
            label3.TabIndex = 5;
            label3.Text = "Podaj ilość sekwencji w instancji:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 86);
            label4.Name = "label4";
            label4.Size = new Size(172, 20);
            label4.TabIndex = 6;
            label4.Text = "Podaj długość sekwencji:";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(15, 349);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ScrollBars = ScrollBars.Both;
            textBox3.Size = new Size(349, 255);
            textBox3.TabIndex = 7;
            textBox3.Text = resources.GetString("textBox3.Text");
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // button2
            // 
            button2.Location = new Point(183, 248);
            button2.Name = "button2";
            button2.Size = new Size(148, 46);
            button2.TabIndex = 8;
            button2.Text = "Sprawdź instancję\r\n";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Enabled = false;
            button3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            button3.Location = new Point(728, 344);
            button3.Name = "button3";
            button3.Size = new Size(299, 52);
            button3.TabIndex = 9;
            button3.Text = "Uruchom algorytm genetyczny";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(184, 297);
            label2.Name = "label2";
            label2.Size = new Size(162, 20);
            label2.TabIndex = 10;
            label2.Text = "To poprawna instancja!";
            label2.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(41, 139);
            label5.Name = "label5";
            label5.Size = new Size(251, 20);
            label5.TabIndex = 11;
            label5.Text = "Podaj długość sekwencji optymalnej:";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(41, 162);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(290, 27);
            textBox4.TabIndex = 12;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(41, 192);
            label6.Name = "label6";
            label6.Size = new Size(210, 20);
            label6.TabIndex = 13;
            label6.Text = "Podaj ilość błędów w instancji:";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(41, 215);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(290, 27);
            textBox5.TabIndex = 14;
            // 
            // buttonPause
            // 
            buttonPause.Location = new Point(880, 345);
            buttonPause.Name = "buttonPause";
            buttonPause.Size = new Size(146, 50);
            buttonPause.TabIndex = 15;
            buttonPause.Text = "Pauza";
            buttonPause.UseVisualStyleBackColor = true;
            buttonPause.Visible = false;
            buttonPause.Click += buttonPause_Click;
            // 
            // buttonStop
            // 
            buttonStop.Location = new Point(728, 344);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(146, 50);
            buttonStop.TabIndex = 16;
            buttonStop.Text = "Stop";
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Visible = false;
            buttonStop.Click += buttonStop_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(728, 405);
            label7.Name = "label7";
            label7.Size = new Size(0, 20);
            label7.TabIndex = 17;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(1050, 54);
            textBox6.Multiline = true;
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.ScrollBars = ScrollBars.Both;
            textBox6.Size = new Size(396, 252);
            textBox6.TabIndex = 7;
            textBox6.Text = "tu się pojawia aktualna populacja";
            textBox6.Visible = false;
            textBox6.WordWrap = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label8.Location = new Point(1230, 5);
            label8.Name = "label8";
            label8.Size = new Size(57, 20);
            label8.TabIndex = 19;
            label8.Text = "Wyniki";
            label8.Visible = false;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(1050, 364);
            label9.Name = "label9";
            label9.Size = new Size(223, 20);
            label9.TabIndex = 20;
            label9.Text = "Aktualne najlepsze rozwiązanie: ";
            label9.Visible = false;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(1050, 387);
            textBox7.Multiline = true;
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.ScrollBars = ScrollBars.Both;
            textBox7.Size = new Size(396, 50);
            textBox7.TabIndex = 21;
            textBox7.Visible = false;
            textBox7.WordWrap = false;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(1050, 309);
            label10.Name = "label10";
            label10.Size = new Size(269, 20);
            label10.TabIndex = 22;
            label10.Text = "Aktualna najlepsza wartość funkcji celu:";
            label10.Visible = false;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(1050, 332);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(396, 27);
            textBox8.TabIndex = 23;
            textBox8.Visible = false;
            // 
            // textBox9
            // 
            textBox9.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 238);
            textBox9.Location = new Point(1050, 443);
            textBox9.Multiline = true;
            textBox9.Name = "textBox9";
            textBox9.ReadOnly = true;
            textBox9.ScrollBars = ScrollBars.Both;
            textBox9.Size = new Size(396, 253);
            textBox9.TabIndex = 24;
            textBox9.Visible = false;
            textBox9.WordWrap = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(1050, 699);
            label11.Name = "label11";
            label11.Size = new Size(209, 20);
            label11.TabIndex = 25;
            label11.Text = "Czas działania metaheurystyki:";
            label11.Visible = false;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(1259, 699);
            label12.Name = "label12";
            label12.Size = new Size(0, 20);
            label12.TabIndex = 26;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label13.Location = new Point(446, 4);
            label13.Name = "label13";
            label13.Size = new Size(197, 20);
            label13.TabIndex = 27;
            label13.Text = "Parametry metaheurystyki";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(396, 30);
            label14.Name = "label14";
            label14.Size = new Size(247, 20);
            label14.TabIndex = 28;
            label14.Text = "Rozmiar populacji (domyślnie 100): ";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(396, 83);
            label15.Name = "label15";
            label15.Size = new Size(273, 20);
            label15.TabIndex = 29;
            label15.Text = "Ilość iteracji algorytmu (domyślnie 200):";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(396, 136);
            label16.Name = "label16";
            label16.Size = new Size(290, 40);
            label16.TabIndex = 30;
            label16.Text = "Prawdopodobieństwo wystąpienia mutacji\r\n(domyślnie 0,0003):";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(396, 209);
            label17.Name = "label17";
            label17.Size = new Size(260, 40);
            label17.TabIndex = 31;
            label17.Text = "Procent osobników pozostawianych w\r\npopulacji (domyślnie 0,25):";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(396, 282);
            label18.Name = "label18";
            label18.Size = new Size(302, 40);
            label18.TabIndex = 32;
            label18.Text = "Presja reprodukcyjna (premiowanie lepszych\r\nosobników; domyślnie 15,0):";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(396, 355);
            label19.Name = "label19";
            label19.Size = new Size(287, 40);
            label19.TabIndex = 33;
            label19.Text = "Liczba prób wydłużania nowego osobnika\r\n(domyślnie 2):";
            // 
            // textBox10
            // 
            textBox10.Location = new Point(396, 53);
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(302, 27);
            textBox10.TabIndex = 34;
            // 
            // textBox11
            // 
            textBox11.Location = new Point(396, 106);
            textBox11.Name = "textBox11";
            textBox11.Size = new Size(302, 27);
            textBox11.TabIndex = 35;
            // 
            // textBox12
            // 
            textBox12.Location = new Point(396, 179);
            textBox12.Name = "textBox12";
            textBox12.Size = new Size(302, 27);
            textBox12.TabIndex = 36;
            // 
            // textBox13
            // 
            textBox13.Location = new Point(396, 252);
            textBox13.Name = "textBox13";
            textBox13.Size = new Size(302, 27);
            textBox13.TabIndex = 37;
            // 
            // textBox14
            // 
            textBox14.Location = new Point(396, 325);
            textBox14.Name = "textBox14";
            textBox14.Size = new Size(302, 27);
            textBox14.TabIndex = 38;
            // 
            // textBox15
            // 
            textBox15.Location = new Point(396, 398);
            textBox15.Name = "textBox15";
            textBox15.Size = new Size(302, 27);
            textBox15.TabIndex = 39;
            // 
            // button4
            // 
            button4.Location = new Point(727, 210);
            button4.Name = "button4";
            button4.Size = new Size(146, 68);
            button4.TabIndex = 40;
            button4.Text = "Utwórz instancje testowe\r\n(10 instancji)";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(12, 623);
            label20.Name = "label20";
            label20.Size = new Size(191, 20);
            label20.TabIndex = 41;
            label20.Text = "Dopasowanie z generatora:";
            label20.Visible = false;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(15, 702);
            label22.Name = "label22";
            label22.Size = new Size(248, 20);
            label22.TabIndex = 43;
            label22.Text = "Rozmiar dopasowania z generatora:";
            label22.Visible = false;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(259, 701);
            label23.Name = "label23";
            label23.Size = new Size(87, 20);
            label23.TabIndex = 44;
            label23.Text = "Nie dotyczy";
            label23.Visible = false;
            // 
            // textBox16
            // 
            textBox16.Location = new Point(15, 646);
            textBox16.Multiline = true;
            textBox16.Name = "textBox16";
            textBox16.ReadOnly = true;
            textBox16.ScrollBars = ScrollBars.Horizontal;
            textBox16.Size = new Size(349, 50);
            textBox16.TabIndex = 45;
            textBox16.Visible = false;
            textBox16.WordWrap = false;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label21.Location = new Point(113, 5);
            label21.Name = "label21";
            label21.Size = new Size(142, 20);
            label21.TabIndex = 46;
            label21.Text = "Generator instancji";
            // 
            // chart1
            // 
            chart1.BackColor = Color.Transparent;
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(370, 439);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(674, 294);
            chart1.TabIndex = 47;
            chart1.Text = "chart1";
            chart1.Visible = false;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label24.Location = new Point(821, 183);
            label24.Name = "label24";
            label24.Size = new Size(123, 20);
            label24.TabIndex = 48;
            label24.Text = "Metaheurystyka";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(1050, 28);
            label25.Name = "label25";
            label25.Size = new Size(140, 20);
            label25.TabIndex = 49;
            label25.Text = "Aktualna populacja:";
            label25.Visible = false;
            // 
            // button5
            // 
            button5.Enabled = false;
            button5.Location = new Point(880, 210);
            button5.Name = "button5";
            button5.Size = new Size(146, 68);
            button5.TabIndex = 50;
            button5.Text = "Uruchom test\r\n(na 10 instancjach testowych)";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Enabled = false;
            button6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button6.Location = new Point(728, 284);
            button6.Name = "button6";
            button6.Size = new Size(146, 54);
            button6.TabIndex = 51;
            button6.Text = "Zapisz instancje testowe do pliku";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(880, 284);
            button7.Name = "button7";
            button7.Size = new Size(146, 54);
            button7.TabIndex = 52;
            button7.Text = "Wczytaj instancje testowe z pliku";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(727, 28);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(308, 64);
            checkBox1.TabIndex = 53;
            checkBox1.Text = "Czy automatyczna zmiana wartości\r\nprawdopodobieństwa mutacji na większą,\r\ngdy przestój?";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // textBox17
            // 
            textBox17.Location = new Point(727, 138);
            textBox17.Name = "textBox17";
            textBox17.Size = new Size(160, 27);
            textBox17.TabIndex = 54;
            textBox17.Visible = false;
            // 
            // textBox18
            // 
            textBox18.Location = new Point(893, 138);
            textBox18.Name = "textBox18";
            textBox18.Size = new Size(136, 27);
            textBox18.TabIndex = 55;
            textBox18.Visible = false;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(727, 95);
            label26.Name = "label26";
            label26.Size = new Size(147, 40);
            label26.TabIndex = 56;
            label26.Text = "Tymczasowa wartość\r\n(domyślnie 0,004):";
            label26.Visible = false;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(893, 95);
            label27.Name = "label27";
            label27.Size = new Size(128, 40);
            label27.TabIndex = 57;
            label27.Text = "Długość przestoju\r\n(domyślnie 20):";
            label27.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1462, 745);
            Controls.Add(label27);
            Controls.Add(label26);
            Controls.Add(textBox18);
            Controls.Add(textBox17);
            Controls.Add(checkBox1);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(label25);
            Controls.Add(label24);
            Controls.Add(chart1);
            Controls.Add(label21);
            Controls.Add(textBox16);
            Controls.Add(label23);
            Controls.Add(label22);
            Controls.Add(label20);
            Controls.Add(button4);
            Controls.Add(textBox15);
            Controls.Add(textBox14);
            Controls.Add(textBox13);
            Controls.Add(textBox12);
            Controls.Add(textBox11);
            Controls.Add(textBox10);
            Controls.Add(label19);
            Controls.Add(label18);
            Controls.Add(label17);
            Controls.Add(label16);
            Controls.Add(label15);
            Controls.Add(label14);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(textBox9);
            Controls.Add(textBox8);
            Controls.Add(label10);
            Controls.Add(textBox7);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(textBox6);
            Controls.Add(label7);
            Controls.Add(buttonStop);
            Controls.Add(buttonPause);
            Controls.Add(textBox5);
            Controls.Add(label6);
            Controls.Add(textBox4);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBox3);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Algorytm genetyczny dla problemu minimalnego bezbłędnego dopasowania sekwencji";
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label3;
        private Label label4;
        private TextBox textBox3;
        private Button button2;
        private Button button3;
        private Label label2;
        private Label label5;
        private TextBox textBox4;
        private Label label6;
        private TextBox textBox5;
        private Button buttonPause;
        private Button buttonStop;
        private Label label7;
        private TextBox textBox6;
        private Label label8;
        private Label label9;
        private TextBox textBox7;
        private Label label10;
        private TextBox textBox8;
        private TextBox textBox9;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private TextBox textBox10;
        private TextBox textBox11;
        private TextBox textBox12;
        private TextBox textBox13;
        private TextBox textBox14;
        private TextBox textBox15;
        private Button button4;
        private Label label20;
        private Label label22;
        private Label label23;
        private TextBox textBox16;
        private Label label21;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Label label24;
        private Label label25;
        private Button button5;
        private Button button6;
        private Button button7;
        private CheckBox checkBox1;
        private TextBox textBox17;
        private TextBox textBox18;
        private Label label26;
        private Label label27;
    }
}
