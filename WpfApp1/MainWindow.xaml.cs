using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{

    public interface Ikare
    {
      string name { get; set; }
      TextBox tb { get; set; }
        TextBox tb2 { get; set; }
    }

    public class CustomKare : Grid, Ikare
    {
        public string name { get ; set ; }
        public TextBox tb { get; set; }
        public TextBox tb2 { get; set; }

        public CustomKare(int i,int t)
        {

            name = (i + 1) + ":" + (t + 1);
            this.AllowDrop = true;
            this.RowDefinitions.Add(new RowDefinition());
            this.RowDefinitions.Add(new RowDefinition());
            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.ColumnDefinitions.Add(new ColumnDefinition());
            tb = new TextBox();
            tb.Text = name;
            tb.Background = Brushes.Transparent;
            tb.BorderBrush = Brushes.Transparent;
            tb2 = new TextBox();
       
            tb2.Background = Brushes.Transparent;
            tb2.BorderBrush = Brushes.Transparent;
            Grid.SetColumn(tb2, 1);
            Grid.SetRow(tb2, 1);
            this.Children.Add(tb);
            this.Children.Add(tb2);
            if ((i + t) % 2 == 0)
            {
                this.Background = Brushes.DarkOrange;
            }
            else
            {
                this.Background = Brushes.LightYellow;
            }
            Grid.SetRow(this, i);
            Grid.SetColumn(this, t);


        }

    }

    public partial class MainWindow : Window, INotifyPropertyChanged
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

      
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private int[] xNokta = {2, 1, -1, -2,
                      -2, -1, 1, 2};
        private int[] yNokta = {1, 2, 2, 1,
                      -1, -2, -2, -1};


        bool _tahtaiçikontrol(int x, int y, int[,] tahta)
        {
            return (x >= 0 && x < 8 &&
                   y >= 0 && y < 8 &&
                   tahta[x, y] == -1);
        }

        void cozumBul(int k,int l)
        {
            Siralama = "";
            int[,] tahta = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int t = 0; t < 8; t++)
                {
                    tahta[i, t] = -1;
                }
            }

            tahta[k, l] = 0;

            if (!cozum(k, l, 1, tahta))
            {
                Siralama = "Çözüm yok";
          
            }
            else
                for (int i = 0; i < 8; i++)
                {
                    

                    Siralama += System.Environment.NewLine;
                    for (int y = 0; y < 8; y++)
                    {
                        //gr1.Children.OfType<Grid>().Where(x => x.Children.OfType<TextBox>().FirstOrDefault().Text == (i + 1) + ":"+(y+1));
                        gr1.Children.OfType<Ikare>().Where(x => x.name == (i + 1) + ":" + (y + 1)).FirstOrDefault().tb2.Text = tahta[i, y].ToString(); 

                        Siralama += tahta[i, y] + "\t";
                    }
                }

        
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

            Siralama = "Algoritmayı başlatmak için atı sürükleyip ve bir kareye bırakınız." + System.Environment.NewLine + "1:1, 5:3, 5:6, 3:7 ve 8:8 kareleri daha hızlı çalışıyor";
            for (int i = 0; i < 8; i++)
            {
                gr1.ColumnDefinitions.Add(new ColumnDefinition());
                gr1.RowDefinitions.Add(new RowDefinition());
              
                for (int t = 0; t < 8; t++)
                {

                   
                    CustomKare kare = new CustomKare(i,t);

                    //tb.Text = ;
                    kare.Drop += Kare_Drop;
                    gr1.Children.Add(kare);
                }
            }
           

        }

        private void Temizle()
        {
            foreach (Ikare s in gr1.Children.OfType<Grid>().Where(x => x.Background.GetType() == typeof(ImageBrush)))
            {
                int a1 = Convert.ToInt32(s.name.Split(':').FirstOrDefault()) - 1;
                int a2 = Convert.ToInt32(s.name.Split(':').LastOrDefault()) - 1;

                if ((a1 + a2) % 2 == 0)
                {
                    (s as Grid).Background = Brushes.DarkOrange;
                }
                else
                {
                    (s as Grid).Background = Brushes.LightYellow;
                }
            }
        }

        private async void Kare_Drop(object sender, DragEventArgs e)
        {
            Grid grid = e.Data.GetData(typeof(Grid)) as Grid;
            Grid kare = sender as Grid;
           
           string kareyazisi= kare.Children.OfType<TextBox>().FirstOrDefault().Text;
          int a= Convert.ToInt32( kareyazisi.Split(':').FirstOrDefault())-1;
            int b = Convert.ToInt32(kareyazisi.Split(':').LastOrDefault())-1;

            Temizle();
           
            kare.Background = grid.Background;
            cozumBul(a,b);
            
        }
  

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {            
            Grid item = e.Source as Grid;       
            if (item != null)
            {
                Grid item1 = (Grid)e.Source as Grid;
                object dataObject = sender as Grid;
                DragDrop.DoDragDrop(item1, dataObject, DragDropEffects.Copy);
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
    }
}
