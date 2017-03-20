using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;


namespace ClienteTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient tx;

        public MainWindow()
        {
            InitializeComponent();
            tx = new TcpClient();
        }

        private void JanelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BTEnvia_Click(object sender, RoutedEventArgs e)
        {


            String linha = TBEnvia.Text;
            Stream fluxoDeDados = tx.GetStream();

            ASCIIEncoding codificacao = new ASCIIEncoding();
            byte[] dados = codificacao.GetBytes(linha);

            fluxoDeDados.Write(dados, 0, dados.Length);
            TBEnvia.Text = "";
        }
         
        private void JanelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {


        }

        private void TBEnvia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                BTEnvia_Click(sender, e);
        }

        private void BTConectaAoServidor_Click(object sender, RoutedEventArgs e)
        {
            tx.Connect("127.0.0.1", 8001);
        }

        private void BTDesconectaDoServidor_Click(object sender, RoutedEventArgs e)
        {
            tx.Close();
        }
    }

}
    