using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {


        private string siralama;
        public string Siralama
        {
            get { return siralama; }
            set
            {
                siralama = value;
             
                OnPropertyChanged("Siralama");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
       
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int[] xNokta = {2, 1, -1, -2,
                      -2, -1, 1, 2};
      private  int[] yNokta = {1, 2, 2, 1,
                      -1, -2, -2, -1};


        bool _tahtaiçikontrol(int x,int y, int[,] tahta)
        {
            return (x >= 0 && x < 8 &&
                   y >= 0 && y < 8 &&
                   tahta[x, y] == -1);
        }

        bool cozumVarmi()
        {
            int[,] tahta = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int t = 0; t < 8; t++)
                {
                    tahta[i, t] = -1;
                }
            }

            tahta[0, 0] = 0;

            if (!cozum(0, 0, 1, tahta))
            {
               
                return false;
            }
            else
                for (int i = 0; i < 8; i++)
                {
                    Siralama +=System.Environment.NewLine;
                    for (int y = 0; y < 8; y++)
                    {
                        Siralama += tahta[i, y]+ "\t";
                    }
                }
             
             return true;
        }

       bool cozum(int x, int y, int adim,
                                int[,] tahta)
        {
            int k, xyonu, yyonu;
            if (adim == 64)
                return true;

           
            for (k = 0; k < 8; k++)
            {
                xyonu = x + xNokta[k];
                yyonu = y + yNokta[k];
                if (_tahtaiçikontrol(xyonu, yyonu, tahta))
                {
                    tahta[xyonu, yyonu] = adim;
                    if (cozum(xyonu, yyonu, adim + 1, tahta))
                        return true;
                    else
                   
                        tahta[xyonu, yyonu] = -1;
                }
            }

            return false;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            var a = cozumVarmi();
        }
    }
}
