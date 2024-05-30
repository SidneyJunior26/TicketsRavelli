using System.Net.Mail;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infrastructure.EmailServices.Interfaces;

namespace TicketsRavelli.Infrastructure.EmailServices.Implementations
{
    public class EmailService : IEmailService
    {
        public void TesteEmail()
        {
            string email = "";
            string assunto = "";

            //string imagePath = Path.Combine(@"C:\inetpub\wwwroot\publish\Imagens", "Logo.png");
            string imageSrc = $"cid:{Guid.NewGuid()}";

            //LinkedResource logoResource = new LinkedResource(imagePath);
            //logoResource.ContentId = imageSrc;
            //logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            string corpo = @$"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Tickets Ravelli - Pagamento Recebido</title>
                        <style>
                        body {{
                            font-family: Arial, sans-serif;
                        }}

                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}

                        .logo {{
                            text-align: center;
                            margin-bottom: 20px;
                        }}

                        .instructions {{
                            margin-bottom: 20px;
                        }}

                        .button {{
                            display: inline-block;
                            background-color: #F8A824;
                            color: #000000;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 4px;
                        }}

                        a {{
                            color: #000000;
                        }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                        <div class=""logo"">
                        </div>
                        <div class=""instructions"">
                            Olá, isso é um teste<br><br>
                            <br><br>
                            Atenciosamente,
                            <br>
                            <b>Equipe Rav Bike Brasil</b>
                        </div>
                        </div>
                    </body>
                    </html>
                    ";

            using (SmtpClient client = new SmtpClient("smtps.uhserver.com", 465))
            {
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;

                //client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Credentials = new System.Net.NetworkCredential("", "");
                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress("");
                mensagem.To.Add(new MailAddress(email));
                mensagem.Subject = assunto;
                mensagem.Body = corpo;

                mensagem.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(corpo, null, "text/html");
                //htmlView.LinkedResources.Add(logoResource);
                mensagem.AlternateViews.Add(htmlView);

                client.Send(mensagem);
            }
        }

        public void EnviarConfirmacaoPagamento(Subscription inscricao)
        {
            string emailAtleta = inscricao.Atleta.Email;
            string assunto = "Tickets Ravelli - Boas notícias! Recebemos seu pagamento.";

            string imagePath = Path.Combine(@"C:\inetpub\wwwroot\publish\Imagens", "Logo.png");
            string imageSrc = $"cid:{Guid.NewGuid()}";

            LinkedResource logoResource = new LinkedResource(imagePath);
            logoResource.ContentId = imageSrc;
            logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            string corpo = @$"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Tickets Ravelli - Pagamento Recebido</title>
                        <style>
                        body {{
                            font-family: Arial, sans-serif;
                        }}

                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}

                        .logo {{
                            text-align: center;
                            margin-bottom: 20px;
                        }}

                        .instructions {{
                            margin-bottom: 20px;
                        }}

                        .button {{
                            display: inline-block;
                            background-color: #F8A824;
                            color: #000000;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 4px;
                        }}

                        a {{
                            color: #000000;
                        }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                        <div class=""logo"">
                            <img src=""cid:{imageSrc}"" alt=""Logo da empresa"" width=""200vh"">
                        </div>
                        <div class=""instructions"">
                            Olá, {inscricao.Atleta.Nome}<br><br>
                            {inscricao.Evento.TxtEmailBaixa}
                            <br><br>
                            Atenciosamente,
                            <br>
                            <b>Equipe Rav Bike Brasil</b>
                        </div>
                        </div>
                    </body>
                    </html>
                    ";

            using (SmtpClient client = new SmtpClient("", 587))
            {
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("", "");

                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress("");
                mensagem.To.Add(new MailAddress(emailAtleta));
                mensagem.Subject = assunto;
                mensagem.Body = corpo;

                mensagem.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(corpo, null, "text/html");
                htmlView.LinkedResources.Add(logoResource);
                mensagem.AlternateViews.Add(htmlView);

                client.Send(mensagem);
            }
        }

        public void EnviarNovoBoleto(Subscription inscricao)
        {
            string emailAtleta = inscricao.Atleta.Email;
            string assunto = "Tickets Ravelli - Seu boleto foi gerado";

            string imagePath = Path.Combine(@"C:\inetpub\wwwroot\publish\Imagens", "Logo.png");
            string imageSrc = $"cid:{Guid.NewGuid()}";

            LinkedResource logoResource = new LinkedResource(imagePath);
            logoResource.ContentId = imageSrc;
            logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            string corpo = @$"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Tickets Ravelli - Novo Boleto</title>
                        <style>
                        body {{
                            font-family: Arial, sans-serif;
                        }}

                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}

                        .logo {{
                            text-align: center;
                            margin-bottom: 20px;
                        }}

                        .instructions {{
                            margin-bottom: 20px;
                        }}

                        .button {{
                            display: inline-block;
                            background-color: #F8A824;
                            color: #000000;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 4px;
                        }}

                        a {{
                            color: #000000;
                        }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                        <div class=""logo"">
                            <img src=""cid:{imageSrc}"" alt=""Logo da empresa"" width=""200vh"">
                        </div>
                        <div class=""instructions"">
                            Olá, {inscricao.Atleta.Nome}<br><br>
                            Esperamos que este e-mail o encontre bem. Gostaríamos de informar que o boleto para o evento '{inscricao.Evento.Nome}' foi gerado com sucesso.

                            <a href=""{inscricao.GnLink}"" target=""_blank"">Clique aqui</a> para visualizar seu boleto.

                            Por favor, efetue o pagamento até a data de vencimento especificada.

                            Agradecemos sua participação e esperamos vê-lo em nosso evento.
                            <br><br>
                            Atenciosamente,
                            <br>
                            <b>Equipe Rav Bike Brasil</b>
                        </div>
                        </div>
                    </body>
                    </html>
                    ";

            using (SmtpClient client = new SmtpClient("", 587))
            {
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("", "");

                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress("");
                mensagem.To.Add(new MailAddress(emailAtleta));
                mensagem.Subject = assunto;
                mensagem.Body = corpo;

                mensagem.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(corpo, null, "text/html");
                htmlView.LinkedResources.Add(logoResource);
                mensagem.AlternateViews.Add(htmlView);

                client.Send(mensagem);
            }
        }

        public void EnviarCodigoResetSenha(string codigo, Athlete atleta)
        {
            string emailAtleta = atleta.Email;
            string assunto = "Código de Segurança - Novo acesso Tickets Ravelli";

            string imagePath = Path.Combine(@"C:\inetpub\wwwroot\publish\Imagens", "Logo.png");
            string imageSrc = $"cid:{Guid.NewGuid()}";

            LinkedResource logoResource = new LinkedResource(imagePath);
            logoResource.ContentId = imageSrc;
            logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            string corpo = @$"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Novo acesso Rav Bike Brasil</title>
                        <style>
                        body {{
                            font-family: Arial, sans-serif;
                        }}

                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}

                        .logo {{
                            text-align: center;
                            margin-bottom: 20px;
                        }}

                        .code {{
                            font-size: 20px;
                            text-align: center;
                            margin-bottom: 20px;
                        }}

                        .instructions {{
                            margin-bottom: 20px;
                        }}

                        .button {{
                            display: inline-block;
                            background-color: #F8A824;
                            color: #000000;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 4px;
                        }}

                        a {{
                            color: #000000;
                        }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                        <div class=""logo"">
                            <img src=""cid:{imageSrc}"" alt=""Logo da empresa"" width=""200vh"">
                        </div>
                        <div class=""code"">
                            Seu código de segurança: <strong>{codigo}</strong>
                        </div>
                        <div class=""instructions"">
                            Olá, {atleta.Nome}<br><br>
                            Para redefinir sua senha no nosso novo sistema, utilize o código de segurança fornecido acima na página de redefinição de senha e digite o código e sua nova senha.
                            <br><br>
                            Certifique-se de redefinir sua senha assim que possível para garantir a segurança da sua conta.
                            <br><br>
                            Atenciosamente,
                            <br>
                            <b>Equipe Rav Bike Brasil</b>
                        </div>
                        </div>
                    </body>
                    </html>
                    ";

            using (SmtpClient client = new SmtpClient("", 587))
            {
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("", "");

                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress("");
                mensagem.To.Add(new MailAddress(emailAtleta));
                mensagem.Subject = assunto;
                mensagem.Body = corpo;

                mensagem.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(corpo, null, "text/html");
                htmlView.LinkedResources.Add(logoResource);
                mensagem.AlternateViews.Add(htmlView);

                client.Send(mensagem);
            }
        }

        public void EnviarCupomCortesiaEvento(string cupom, Evento evento, string nomeAtleta, string emailAtleta)
        {
            string assunto = $"Cupom Cortesia - {evento.Nome}";

            string imagePath = Path.Combine(@"C:\inetpub\wwwroot\publish\Imagens", "Logo.png");
            string imageSrc = $"cid:{Guid.NewGuid()}";

            LinkedResource logoResource = new LinkedResource(imagePath);
            logoResource.ContentId = imageSrc;
            logoResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            string corpo = @$"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>$""Cupom Cortesia - {evento.Nome}""</title>
                        <style>
                        body {{
                            font-family: Arial, sans-serif;
                        }}

                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}

                        .logo {{
                            text-align: center;
                            margin-bottom: 20px;
                        }}

                        .code {{
                            font-size: 20px;
                            text-align: center;
                            margin-bottom: 20px;
                        }}

                        .instructions {{
                            margin-bottom: 20px;
                        }}

                        .button {{
                            display: inline-block;
                            background-color: #F8A824;
                            color: #000000;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 4px;
                        }}

                        a {{
                            color: #000000;
                        }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                        <div class=""logo"">
                            <img src=""cid:{imageSrc}"" alt=""Logo da empresa"" width=""200vh"">
                        </div>
                        <div class=""code"">
                            Seu cupom cortesia: <strong>{cupom}</strong>
                        </div>
                        <div class=""instructions"">
                            Prezado(a) <b>{nomeAtleta}</b>.
                            <br><br>
                            Em nome da equipe <b>Rav Bike Brasil</b>, temos o prazer de lhe oferecer um cupom cortesia para participar do evento <b>{evento.Nome}</b>, sem a necessidade de efetuar o pagamento.
                            <br><br>
                            Valorizamos o seu interesse e entusiasmo em participar do evento, e reconhecemos que sua presença é fundamental para enriquecer as discussões e contribuir para o nosso sucesso.
                            <br><br>
                            Por favor, considere este cupom como um gesto de nossa gratidão pela sua dedicação e apoio ao evento.
                            <br><br>
                            Para resgatar o cupom, basta informá-lo no momento de finalizar a inscrição do evento pelo nosso site https://ticketsravelli.com.br/.
                            <br><br>
                            Agradecemos novamente pelo seu envolvimento e estamos ansiosos para recebê-lo(a) no <b>{evento.Nome}</b>.
                            <br><br>
                            Esperamos que você aproveite ao máximo essa oportunidade de aprendizado, networking e compartilhamento de conhecimentos.
                            <br><br>
                            Caso tenha alguma dúvida ou precise de mais informações, não hesite em entrar em contato conosco. Estamos à disposição para ajudá-lo(a) da melhor forma possível.
                            <br><br>
                            Atenciosamente,
                            <br>
                            <b>Equipe Rav Bike Brasil</b>
                        </div>
                        </div>
                    </body>
                    </html>
                    ";

            using (SmtpClient client = new SmtpClient("", 587))
            {
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("", "");

                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress("");
                mensagem.To.Add(new MailAddress(emailAtleta));
                mensagem.Subject = assunto;
                mensagem.Body = corpo;

                mensagem.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(corpo, null, "text/html");
                htmlView.LinkedResources.Add(logoResource);
                mensagem.AlternateViews.Add(htmlView);

                client.Send(mensagem);
            }
        }
    }
}

