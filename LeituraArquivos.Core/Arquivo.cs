using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics.Metrics;

namespace LeituraArquivos.Core
{
    public class Arquivo
    {
        public static string Diretorio;

        public static string mensag_tipo1;

        public static string mensag_tipo2;

        public static string intervalo;

        public IConfigurationRoot _configuration;

        private static byte[] byteUDP;


        /// <summary>
        /// Construção do metodo da interface start
        /// </summary>
        public void Start()
        {
            Leitura();
        }

        public Arquivo(IConfigurationRoot configuration)
        {
            _configuration = configuration;

            var configDiretorio = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build().GetSection("Configuration")["Diretorio"].ToString();
            Diretorio = configDiretorio;

            var pacoteTipo1 = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build().GetSection("Configuration")["mensag_tipo1"].ToString();
            mensag_tipo1 = pacoteTipo1;

            var pacoteTipo2 = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build().GetSection("Configuration")["mensag_tipo2"].ToString();
            mensag_tipo2 = pacoteTipo2;

            var tempoInt = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build().GetSection("Configuration")["intervaloEntreEnvios"].ToString();
            intervalo = tempoInt;
        }

        public void Leitura()
        {
            int counter = 0;

            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File(@"C:\log\leituralog.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

            // ler os arquivos das pastas e das subpastas
            string[] file = Directory.GetFiles(Diretorio, "*", SearchOption.AllDirectories);
            foreach (string arquivos in file)
            {
                var text = File.ReadLines(arquivos);
                foreach (string lerLinha in text)
                {
                    var mensagem = "";
                    try
                    {
                        var teste = lerLinha.Split(' ');

                        if (teste.Length >= 1)
                        {
                            if (teste.Contains(mensag_tipo1) || teste.Contains(mensag_tipo2))
                            {
                                mensagem = String.Concat(teste[2], "\r\n");

                                counter++;

                                byteUDP = System.Text.Encoding.ASCII.GetBytes(mensagem.ToCharArray());
                                Byte[] inputToBeSent = byteUDP;

                                System.Console.WriteLine("Enviado: {0}.", mensagem);

                                var intTemp = int.Parse(intervalo);
                                Thread.Sleep(intTemp);

                                System.Console.WriteLine("There were {0} lines.", counter);
                                Log.Information(mensagem);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("O erro foi: " + ex);
                    }
                }
            }
            Log.Information("Final aqui");
        }
    }
}