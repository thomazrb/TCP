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
using System.Windows.Threading;
using System.Net.NetworkInformation;
namespace ClienteTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient tx = null;
        private Thread receptor = null; 
        private DispatcherTimer TimerJanela;
        private bool ServidorON = false;  
        private List<string> linhas = new List<string>();
        private int linhaAtual = 0;

        public MainWindow()
        {
            InitializeComponent();
            TimerJanela = new DispatcherTimer();
        }


        private void JanelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            TimerJanela.Tick += new System.EventHandler(AtualizaJanela);
            TimerJanela.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            TimerJanela.Start();         
        }
       
        
        void AtualizaJanela(object sender, EventArgs e)
        {
            if(ServidorON == true)
            {
                BTConectaAoServidor.IsEnabled = false;
                BTDesconectaDoServidor.IsEnabled = true;
                TBEnvia.IsEnabled = true;
                BTEnvia.IsEnabled = true;
            }
            else
            {
                BTConectaAoServidor.IsEnabled = true;
                BTDesconectaDoServidor.IsEnabled = false;
                TBEnvia.IsEnabled = false;
                BTEnvia.IsEnabled = false;

                if (tx != null && receptor != null)
                {
                    tx.Close();
                    receptor.Interrupt();
                }
            }

            if (linhaAtual < linhas.Count())
            {
                for (int i = linhaAtual; i < linhas.Count(); i++)
                {
                    TBLog.AppendText(linhas[i] + '\n');
                    linhaAtual++;
                }
                TBLog.ScrollToEnd();
            }
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
            ServidorON = false;         
            if (tx != null)
                tx.Close();
            else if(receptor != null)
                receptor.Abort();
        }

        private void TBEnvia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                BTEnvia_Click(sender, e);
        }

        private void BTConectaAoServidor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tx = null;
                tx = new TcpClient();
                string IPServidor = TBIPServidor.Text;
                tx.Connect(IPServidor, 8001);
                ServidorON = true;
                receptor = new Thread(Captura);
                receptor.Start();
                linhas.Add("Cliente Conectado ao Servidor!");

            }
            catch(Exception exceptionServidoroff)
            {
                linhas.Add("Servidor off!");
            }

        }

        private void BTDesconectaDoServidor_Click(object sender, RoutedEventArgs e)
        {
            linhas.Add("Cliente desconectado do Servidor!");
            ServidorON = false;
            BTConectaAoServidor.IsEnabled = true;
            BTDesconectaDoServidor.IsEnabled = false;
            TBEnvia.IsEnabled = false;
            BTEnvia.IsEnabled = false;
            tx.Close();
            receptor.Abort();
        }

        public void Captura()
        {
            while (ServidorON)
            {
                Stream stream = tx.GetStream();
                stream.ReadTimeout = 100;
                var builder = new StringBuilder();
                byte[] data = new byte[100];            
                int bytes_received = 0;
                int Connected = 1;

                while (Connected == 1)
                {
                    try
                    {
                        bytes_received = stream.Read(data, 0, data.Length);
                    }
                    catch (IOException ex)
                    {
                        Connected = 0;
                    }

                    if ((bytes_received > 0) && (Connected == 1))
                    {
                        for (int i = 0; i < bytes_received; i++)
                            builder.Append(Convert.ToChar(data[i]));

                        if (builder.ToString() == "a0")
                        {
                            linhas.Add("Servidor desligado! Desconectando do servidor...");
                            ServidorON = false;
                            break;
                        }
                        else
                            linhas.Add(builder.ToString());
                    }
                }           
            } 
        }
    }
}


    