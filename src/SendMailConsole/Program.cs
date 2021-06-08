using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace SendMailConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var config = builder.Build();

            var conta = config.GetSection("EmailServerAccount").Get<EmailServerAccount>();


            // Busca automaticamente todos os arquivos em todos os subdiretórios
            string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Files";
            DirectoryInfo Dir = new DirectoryInfo(path);
            FileInfo[] files = Dir.GetFiles("*", SearchOption.AllDirectories);
            var fileNames = files.Select(f => f.FullName).ToArray();

            // chamada da função de disparo do e-mail
            var sendResponse = Util.EnviarMensagem(conta, new string[] { "marcio.nizzola@etec.sp.gov.br" }, null, 
                                                    "TESTE DE EMAIL " + DateTime.Now, "teste de email etec ", fileNames);

            Console.WriteLine(sendResponse);
        }
    }
}
