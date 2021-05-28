using Microsoft.Extensions.Configuration;
using System;

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

            
            var sendResponse =  Util.EnviarMensagem(conta, new string[] { "marcio.nizzola@etec.sp.gov.br" } , null, "TESTE DE EMAIL " + DateTime.Now, "teste de email etec ", null);

            Console.WriteLine(sendResponse);
        }
    }
}
