using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendMailConsole
{
    public static class Util
    {
        private static void AddBcc(MailMessage message, string[] emailcc)
        {
            if (emailcc != null)
            {
                foreach (var destinatario in emailcc)
                {
                    if (destinatario != null)
                    {
                        string[] emaildestino = destinatario.Split(';');
                        //Destinatário
                        foreach (string vEmailP in emaildestino)
                        {
                            message.Bcc.Add(new MailAddress(vEmailP));
                        }
                    }
                }
            }
        }
        private static void AddDestinatario(MailMessage message, string[] destino)
        {
            foreach (var destinatario in destino)
            {
                if (destinatario != null)
                {
                    string[] emaildestino = destinatario.Split(';');
                    foreach (string vEmailP in emaildestino)
                    {
                        message.To.Add(new MailAddress(vEmailP));
                    }
                }
            }
        }

        public static string EnviarMensagem(EmailServerAccount conta, string[] destino, string[] emailcc, string mensagem, string titulo, string anexo)
        {
            string para = destino[0];

            if (String.IsNullOrEmpty(para)) 
            {
                return "Erro sem e-mail ! Assunto:" + titulo;
            }

            if (conta == null)
                return "Erro, conta de e-mail não existente !";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(conta.EmailOrigem, conta.NomeOrigem);
            message.ReplyToList.Add(new MailAddress(conta.Retorno));

            if (conta.EmailDebug != "")
            {
                para = conta.EmailDebug;
                message.To.Add(new MailAddress(para));
            }
            else
            {
                AddDestinatario(message, destino);
                AddBcc(message, emailcc);
            }

            //prioridade do email
            message.Priority = MailPriority.Normal;

            //utilize true pra ativar html no conteúdo do email, ou false, para somente texto
            message.IsBodyHtml = false;

            //Assunto do email
            message.Subject = titulo;

            //corpo do email a ser enviado
            message.Body = mensagem;

            // Envia a mensagem
            SmtpClient client = new SmtpClient(conta.Server, conta.Port);

            Boolean ssl = conta.Autentica;
            client.EnableSsl = ssl;

            // Insere as credenciais se o Servidor SMTP exigir
            client.Credentials = CredentialCache.DefaultNetworkCredentials;

            //endereço do servidor SMTP(para mais detalhes leia abaixo do código)
            client.Host = conta.Server;

            //para envio de email autenticado, coloque login e senha de seu servidor de email
            //para detalhes leia abaixo do código
            client.Credentials = new NetworkCredential(conta.EmailOrigem, conta.Pass);

            try
            {
                client.Send(message);
                return "";
            }
            catch (Exception ex)
            {
                return " Erro no envio de email para !  " + para + "\r\n" + " " + ex.Message + "      -       " + ex.StackTrace + System.Environment.NewLine;
            }


        }
    }
}
