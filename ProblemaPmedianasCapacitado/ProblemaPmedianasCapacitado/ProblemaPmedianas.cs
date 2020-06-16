using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace ProblemaPmedianasCapacitado
{
    class ProblemaPmedianas
    {
        public int NumeroItens;
        public int NumeroMedianas;
        public double DistanciaTotal;
        public Item[] Itens;
        public Mediana[] Medianas;
        public double[,] MatrizDistancias;
        public void AlocacaoInicial()
        {
            List<int> ListaItensNaoAlocados = new List<int>();
            for(int i=0;i<NumeroMedianas;i++)
            {
                Medianas[i].ItensAlocados = new List<int>();
                Medianas[i].ItensAlocados.Add(i);
                Medianas[i].ItemMediana = i;
                Itens[i].MedianaAlocada = i;
            }
            for(int i=NumeroMedianas;i<NumeroItens;i++)
            {
                ListaItensNaoAlocados.Add(i);
            }
            while (ListaItensNaoAlocados.Count > 0)
            {
                double MenorCustoInsercao = double.MaxValue;
                int ItemMenorCustoInsercao=-1;
                int MedianaMenorCustoInsercao=-1;
                for(int i=0;i<ListaItensNaoAlocados.Count;i++)
                {
                    for(int j=0;j<NumeroMedianas;j++)
                    {
                        double DeltaAtual = DeltaInserirItemNaMediana(ListaItensNaoAlocados[i], j);
                        if (DeltaAtual<MenorCustoInsercao)
                        {
                            MenorCustoInsercao = DeltaAtual;
                            ItemMenorCustoInsercao = ListaItensNaoAlocados[i];
                            MedianaMenorCustoInsercao = j;
                        }
                    }
                }
                InserirItemNaMediana(ItemMenorCustoInsercao, MedianaMenorCustoInsercao);
                ListaItensNaoAlocados.Remove(ItemMenorCustoInsercao);
            }
            TrocarTodasAsMedianasDosGruposParaAsMelhores();
        }
        public void Metodo1()
        {
            int Vizinhanca = 1;
            while (Vizinhanca<=2)
            {
                Vizinhanca = 2;
                for(int i=0;i<NumeroItens;i++)
                {
                    for(int p=0;p<NumeroMedianas;p++)
                    {
                        if (DeltaShiftItem(i, p)<0)
                        {
                            ShiftItem(i, p);
                            Vizinhanca = 1;
                            TrocarMedianaDoGrupoParaMelhor(p);
                        }
                    }
                }
                if (Vizinhanca == 2)
                {
                    Vizinhanca = 3;
                    for (int i1 = 0; i1 < NumeroItens; i1++)
                    {
                        for (int i2 = 0; i2 < NumeroItens; i2++)
                        {
                            if (DeltaExchangeItens(i1, i2) < 0)
                            {
                                ExchangeItens(i1, i2);
                                Vizinhanca = 1;
                            }
                        }
                    }
                }
            }
        }
        public void Metodo1Alternativo()
        {
            int Vizinhanca = 1;
            while (Vizinhanca <= 2)
            {
                Vizinhanca = 2;
                if (VizinhancaShift())
                    Vizinhanca = 1;
                if (Vizinhanca == 2)
                {
                    Vizinhanca = 3;
                    if (VizinhancaDelta())
                        Vizinhanca = 1;
                }
            }
        }
        public bool VizinhancaShift()
        {
            bool TrocouSolucao = false;
            for (int i = 0; i < NumeroItens; i++)
            {
                for (int p = 0; p < NumeroMedianas; p++)
                {
                    if (DeltaShiftItem(i, p) < 0)
                    {
                        ShiftItem(i, p);
                        TrocouSolucao = true;
                        TrocarMedianaDoGrupoParaMelhor(p);
                    }
                }
            }
            return TrocouSolucao;
        }
        public bool VizinhancaDelta()
        {
            bool TrocouSolucao = false;
            for (int i1 = 0; i1 < NumeroItens; i1++)
            {
                for (int i2 = 0; i2 < NumeroItens; i2++)
                {
                    if (DeltaExchangeItens(i1, i2) < 0)
                    {
                        ExchangeItens(i1, i2);
                        TrocouSolucao = true;
                    }
                }
            }
            return TrocouSolucao;
        }
        public double DeltaInserirItemNaMediana(int _item, int _mediana)
        {
            return MatrizDistancias[_item, Medianas[_mediana].ItemMediana];
        }
        public void InserirItemNaMediana(int _item, int _mediana)
        {
            Medianas[_mediana].ItensAlocados.Add(_item);
            Itens[_item].MedianaAlocada = _mediana;
            Medianas[_mediana].Distancia += MatrizDistancias[_item, Medianas[_mediana].ItemMediana];
        }
        public double DeltaRetirarItemNaMediana(int _item, int _mediana)
        {
            return -MatrizDistancias[_item, Medianas[_mediana].ItemMediana];
        }
        public void RetirarItemNaMediana(int _item, int _mediana)
        {
            Medianas[_mediana].ItensAlocados.Remove(_item);
            Itens[_item].MedianaAlocada = -1;
            Medianas[_mediana].Distancia -= MatrizDistancias[_item, Medianas[_mediana].ItemMediana];
        }
        public double DeltaTrocarMedianaDoGrupo(int _mediana, int _item)
        {
            double Delta = -Medianas[_mediana].Distancia;
            for (int i = 0; i < Medianas[_mediana].ItensAlocados.Count; i++)
            {
                Delta += MatrizDistancias[Medianas[_mediana].ItensAlocados[i], _item];
                //obs: a distância de _item para _item é zero. Ou seja, não precisa se preocupar com o caso em que Medianas[_mediana].ItensAlocados[i]=_item 
            }
            return Delta;
        }
        public void TrocarMedianaDoGrupo(int _mediana, int _item)
        {
            Medianas[_mediana].ItemMediana = _item;
            Medianas[_mediana].Distancia = 0;
            for (int i = 0; i < Medianas[_mediana].ItensAlocados.Count; i++)
            {
                Medianas[_mediana].Distancia += MatrizDistancias[Medianas[_mediana].ItensAlocados[i], _item];
            }
        }
        public void TrocarMedianaDoGrupoParaMelhor(int _mediana)
        {
            int MelhorItemParaMediana = -1;
            double MelhorDelta = 0;
            for(int i=0;i<Medianas[_mediana].ItensAlocados.Count;i++)
            {
                int ItemAvaliado = Medianas[_mediana].ItensAlocados[i];
                double DeltaTroca = DeltaTrocarMedianaDoGrupo(_mediana, ItemAvaliado);
                if (DeltaTroca<MelhorDelta)
                {
                    MelhorItemParaMediana = ItemAvaliado;
                    MelhorDelta = DeltaTroca;
                }
            }
            if(MelhorItemParaMediana > -1)
            {
                TrocarMedianaDoGrupo(_mediana, MelhorItemParaMediana);
            }
        }
        public void TrocarTodasAsMedianasDosGruposParaAsMelhores()
        {
            for(int j=0;j<NumeroMedianas;j++)
            {
                TrocarMedianaDoGrupoParaMelhor(j);
            }
        }
        public double DeltaShiftItem(int _item, int _novaMediana)
        {
            double Delta = -MatrizDistancias[_item, Medianas[Itens[_item].MedianaAlocada].ItemMediana];
            Delta += MatrizDistancias[_item, Medianas[_novaMediana].ItemMediana];
            return Delta;
        }
        public void ShiftItem(int _item, int _novaMediana)
        {
            Medianas[Itens[_item].MedianaAlocada].ItensAlocados.Remove(_item);
            Medianas[Itens[_item].MedianaAlocada].Distancia -= MatrizDistancias[_item, Medianas[Itens[_item].MedianaAlocada].ItemMediana];
            Medianas[_novaMediana].ItensAlocados.Add(_item);
            Medianas[_novaMediana].Distancia += MatrizDistancias[_item, Medianas[_novaMediana].ItemMediana];
            Itens[_item].MedianaAlocada = _novaMediana;
        }
        public double DeltaExchangeItens(int _item1, int _item2)
        {
            double Delta = 0;
            Delta -= MatrizDistancias[_item1, Itens[_item1].MedianaAlocada];
            Delta -= MatrizDistancias[_item1, Itens[_item1].MedianaAlocada];
            Delta += MatrizDistancias[_item1, Itens[_item2].MedianaAlocada];
            Delta += MatrizDistancias[_item2, Itens[_item1].MedianaAlocada];
            return 0;
        }
        public void ExchangeItens(int _item1, int _item2)
        {
            int MedianaAtualItem1 = Itens[_item1].MedianaAlocada;
            int MedianaAtualItem2 = Itens[_item2].MedianaAlocada;
            Medianas[MedianaAtualItem1].ItensAlocados.Add(_item2);
            Medianas[MedianaAtualItem1].ItensAlocados.Remove(_item1);
            Medianas[MedianaAtualItem1].Distancia += MatrizDistancias[_item2, MedianaAtualItem1];
            Medianas[MedianaAtualItem1].Distancia -= MatrizDistancias[_item1, MedianaAtualItem1];
            Itens[_item1].MedianaAlocada = MedianaAtualItem2;
            Medianas[MedianaAtualItem2].ItensAlocados.Add(_item1);
            Medianas[MedianaAtualItem2].ItensAlocados.Remove(_item2);
            Medianas[MedianaAtualItem2].Distancia += MatrizDistancias[_item1, MedianaAtualItem2];
            Medianas[MedianaAtualItem2].Distancia -= MatrizDistancias[_item2, MedianaAtualItem2];
            Itens[_item2].MedianaAlocada = MedianaAtualItem1;
        }
        public void LerItens(string Arquivo)
        {

        }
        public void GerarInstanciaAleatoria(int _nPontos, int _nMedianas)
        {
            Itens = new Item[_nPontos];
            Medianas = new Mediana[_nMedianas];
            Random Aleatorio = new Random(1);
            for(int i=0;i<_nPontos;i++)
            {
                Itens[i] = new Item();
                Itens[i].X = Aleatorio.Next(30, 570);
                Itens[i].Y = Aleatorio.Next(30, 570);
            }
            for(int j=0;j<_nMedianas;j++)
            {
                Medianas[j] = new Mediana();
                Medianas[j].Cor = Color.FromArgb(200, Aleatorio.Next(0, 256), Aleatorio.Next(0, 256), Aleatorio.Next(0, 256));
            }
            NumeroItens = _nPontos;
            NumeroMedianas = _nMedianas;
        }
        public void CalcularDistancias()
        {
            MatrizDistancias = new double[NumeroItens, NumeroItens];
            for(int i = 0; i < NumeroItens; i++)
            {
                for(int j=0;j<NumeroItens;j++)
                {
                    MatrizDistancias[i, j] = Distancia2Pontos(Itens[i].X, Itens[j].X, Itens[i].Y, Itens[j].Y);
                }
            }
        }
        public double Distancia2Pontos(int x1, int x2, int y1, int y2)
        {
            double DeltaX = (double)x2 - (double)x1;
            double DeltaY = (double)y2 - (double)y1;
            return Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY);
        }
        public Bitmap Desenhar()
        {
            Bitmap Desenho = new Bitmap(600, 600);
            Graphics g = Graphics.FromImage(Desenho);
            g.FillRectangle(Brushes.White, 0, 0, 600, 600);
            int RaioMediana = 8;
            int RaioItem = 5;
            Font drawFont = new Font("Arial", 7);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            for (int j=0;j<NumeroMedianas;j++)
            {
                int MedianaDesenhar = Medianas[j].ItemMediana;
                Brush Pincel = new SolidBrush(Medianas[j].Cor);
                g.FillEllipse(Pincel, Itens[MedianaDesenhar].X - RaioMediana, Itens[MedianaDesenhar].Y - RaioMediana, 2*RaioMediana, 2*RaioMediana);
                for (int i = 0; i < Medianas[j].ItensAlocados.Count; i++)
                {
                    int ItemDesenhar = Medianas[j].ItensAlocados[i];
                    g.FillEllipse(Pincel, Itens[ItemDesenhar].X - RaioItem, Itens[ItemDesenhar].Y - RaioItem, 2 * RaioItem, 2 * RaioItem);
                    string st = ItemDesenhar + ": (" + Itens[ItemDesenhar].X + "," + Itens[ItemDesenhar].Y + ")";
                    g.DrawString(st, drawFont, drawBrush, Itens[ItemDesenhar].X - 6 * RaioItem, Itens[ItemDesenhar].Y - 3 * RaioItem);
                }
            }
            return Desenho;
        }
        public void CalcularDistanciaTotal()
        {
            DistanciaTotal = 0;
            for(int j=0;j<NumeroMedianas;j++)
            {
                DistanciaTotal += Medianas[j].Distancia;
            }
        }
    }
    class Mediana
    {
        public double Distancia;
        public int ItemMediana;
        public Color Cor;
        public List<int> ItensAlocados;
    }
    class Item
    {
        public int X;
        public int Y;
        public int MedianaAlocada;
    }
}

