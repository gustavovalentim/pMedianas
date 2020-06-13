using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProblemaPmedianasCapacitado
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnpMedianas_Click(object sender, EventArgs e)
        {
            ProblemaPmedianas MeuProblema = new ProblemaPmedianas();
            MeuProblema.GerarInstanciaAleatoria(150, 8);
            MeuProblema.CalcularDistancias();
            //Stopwatch Cronometro = new Stopwatch();
            //Cronometro.Start();
            MeuProblema.AlocacaoInicial();
            MeuProblema.CalcularDistanciaTotal();
            this.Text = MeuProblema.DistanciaTotal.ToString();
            pictureBox1.Image = MeuProblema.Desenhar();
            Application.DoEvents();
            System.Threading.Thread.Sleep(1500);
            MeuProblema.Metodo1();
            MeuProblema.CalcularDistanciaTotal();
            this.Text = MeuProblema.DistanciaTotal.ToString();
            pictureBox1.Image = MeuProblema.Desenhar();
            //Cronometro.Stop();
            //MessageBox.Show(Cronometro.ElapsedMilliseconds.ToString());
        }
    }
}
