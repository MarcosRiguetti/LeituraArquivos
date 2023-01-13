using LeituraArquivos.Core;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().AddJsonFile("AppSettings.Json").Build();
Arquivo arquivo = new Arquivo(configuration);
arquivo.Start();
System.Console.ReadLine();
