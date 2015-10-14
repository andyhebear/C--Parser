using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DDW;

namespace testParser {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e) {
            DirectoryInfo di = new System.IO.DirectoryInfo("tests");
            if (di.Exists) {
                FileInfo[] files = di.GetFiles("*.cs", SearchOption.AllDirectories);
                foreach (var v in files) {
                    this.comboBox_csfile.Items.Add(v.FullName.Remove(0, di.Parent.FullName.Length+1));
                }
            }
        }
        private void button_open_Click(object sender, EventArgs e) {
            OpenFileDialog of = new OpenFileDialog();
            of.CheckFileExists = true;
            of.CheckPathExists = true;
            of.DefaultExt = "*.cs";
            of.Filter = "*.cs|*.cs";
            of.Multiselect = false;
            of.RestoreDirectory = true;

            if (of.ShowDialog() == DialogResult.OK) {
                this.comboBox_csfile.Text = of.FileName;
            }
        }
        //
        private void button_Execute_Click(object sender, EventArgs e) {
           // ParserRunner pr = new ParserRunner();
            List<Parser.Error>errors=new List<Parser.Error>();
            CompilationUnitNode cn=ParserRunner.ParseFile(this.comboBox_csfile.Text,out errors);
            //
            this.textBox1.Text = "";
            if (errors.Count > 0) {
                StringBuilder sb = new StringBuilder();
                foreach (var v in errors) {
                    sb.AppendLine(string.Format("({0},{1} token:{2})  {3}",v.Line,v.Column,v.Token,v.Message));
                }
                this.textBox1.Text = sb.ToString();
            }
            else {
                StringBuilder sb = new StringBuilder();
                cn.ToSource(sb);
                this.textBox1.Text = sb.ToString();
            }

        }



    }
}
