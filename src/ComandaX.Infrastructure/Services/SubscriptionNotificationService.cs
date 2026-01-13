using Microsoft.Extensions.Logging;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Resend;

namespace ComandaX.Infrastructure.Services;

/// <summary>
/// Service for sending subscription-related notifications via email.
/// </summary>
public class SubscriptionNotificationService : ISubscriptionNotificationService
{
    private readonly IResend _resend;
    private readonly ILogger<SubscriptionNotificationService> _logger;
    private const string FromEmail = "onboarding@resend.dev"; // Change to your domain

    public SubscriptionNotificationService(
        IResend resend,
        ILogger<SubscriptionNotificationService> logger)
    {
        _resend = resend;
        _logger = logger;
    }

    /// <summary>
    /// Sends a welcome email when a new trial subscription is created.
    /// </summary>
    public async Task SendTrialStartedEmailAsync(Tenant tenant, Subscription subscription, string email)
    {
        try
        {
            var expirationDate = subscription.EndDate.ToString("dd/MM/yyyy");

            var message = new EmailMessage
            {
                From = FromEmail,
                To = { email },
                Subject = "Bem-vindo ao ComandaX - Seu período de teste",
                HtmlBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body {{ font-family: Arial, sans-serif; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #4CAF50; color: white; padding: 20px; border-radius: 5px; }}
                            .content {{ padding: 20px; }}
                            .footer {{ background-color: #f5f5f5; padding: 10px; text-align: center; font-size: 12px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Bem-vindo ao ComandaX! </h1>
                            </div>
                            <div class='content'>
                                <p>Olá {tenant.Name},</p>
                                <p>Parabéns! Você iniciou seu período de teste gratuito no ComandaX.</p>
                                <p><strong>Seu período de teste expira em: {expirationDate}</strong></p>
                                <p>Durante os próximos 30 dias, você terá acesso completo a todos os recursos do ComandaX para gerenciar sua comanda eletrônica.</p>
                                <p>Quando seu período de teste expirar, você poderá continuar usando ComandaX com um plano pago de apenas R$ 49,90 por mês.</p>
                                <p>Se tiver dúvidas, não hesite em entrar em contato conosco.</p>
                                <p>Bom uso!<br/>
                                <strong>Equipe ComandaX</strong></p>
                            </div>
                            <div class='footer'>
                                <p>&copy; 2024 ComandaX. Todos os direitos reservados.</p>
                            </div>
                        </div>
                    </body>
                    </html>"
            };

            await _resend.EmailSendAsync(message);
            _logger.LogInformation("Trial started email sent to {Email} for tenant {TenantId}", email, tenant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send trial started email to {Email}", email);
            throw;
        }
    }

    /// <summary>
    /// Sends a reminder email when the subscription is expiring soon.
    /// </summary>
    public async Task SendExpiringNotificationAsync(Tenant tenant, Subscription subscription, string email)
    {
        try
        {
            var daysRemaining = (subscription.EndDate - DateTime.UtcNow).Days;
            var expirationDate = subscription.EndDate.ToString("dd/MM/yyyy");

            var message = new EmailMessage
            {
                From = FromEmail,
                To = { email },
                Subject = $"ComandaX - Sua assinatura vence em {daysRemaining} dias",
                HtmlBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body {{ font-family: Arial, sans-serif; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #FF9800; color: white; padding: 20px; border-radius: 5px; }}
                            .content {{ padding: 20px; }}
                            .footer {{ background-color: #f5f5f5; padding: 10px; text-align: center; font-size: 12px; }}
                            .warning {{ background-color: #fff3cd; border-left: 4px solid #ff9800; padding: 10px; margin: 20px 0; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Atenção: Sua assinatura está expirando! </h1>
                            </div>
                            <div class='content'>
                                <p>Olá {tenant.Name},</p>
                                <div class='warning'>
                                    <p><strong>Sua assinatura/período de teste vence em {expirationDate} ({daysRemaining} dias).</strong></p>
                                </div>
                                <p>Para continuar usando todos os recursos do ComandaX sem interrupção, renove sua assinatura agora.</p>
                                <p><strong>Plano mensal:</strong> R$ 49,90/mês</p>
                                <p>Após a expiração da sua assinatura, você terá acesso apenas a leitura dos seus dados.</p>
                                <p>Clique no link abaixo para renovar sua assinatura:</p>
                                <p><a href='https://comandax.app/subscription' style='background-color: #FF9800; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Renovar Assinatura</a></p>
                                <p>Dúvidas? Entre em contato conosco.<br/>
                                <strong>Equipe ComandaX</strong></p>
                            </div>
                            <div class='footer'>
                                <p>&copy; 2024 ComandaX. Todos os direitos reservados.</p>
                            </div>
                        </div>
                    </body>
                    </html>"
            };

            await _resend.EmailSendAsync(message);
            _logger.LogInformation("Expiring soon notification sent to {Email} for tenant {TenantId}", email, tenant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send expiring notification to {Email}", email);
            throw;
        }
    }

    /// <summary>
    /// Sends a notification when the subscription has expired.
    /// </summary>
    public async Task SendExpiredNotificationAsync(Tenant tenant, Subscription subscription, string email)
    {
        try
        {
            var message = new EmailMessage
            {
                From = FromEmail,
                To = { email },
                Subject = "ComandaX - Sua assinatura expirou",
                HtmlBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body {{ font-family: Arial, sans-serif; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #f44336; color: white; padding: 20px; border-radius: 5px; }}
                            .content {{ padding: 20px; }}
                            .footer {{ background-color: #f5f5f5; padding: 10px; text-align: center; font-size: 12px; }}
                            .warning {{ background-color: #ffebee; border-left: 4px solid #f44336; padding: 10px; margin: 20px 0; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Sua Assinatura Expirou</h1>
                            </div>
                            <div class='content'>
                                <p>Olá {tenant.Name},</p>
                                <div class='warning'>
                                    <p><strong>Sua assinatura ComandaX expirou em {subscription.EndDate:dd/MM/yyyy}.</strong></p>
                                </div>
                                <p>Infelizmente, suas funcionalidades de escrita foram desativadas. Você ainda pode visualizar seus dados, mas não conseguirá criar novas comandas, produtos ou fazer outras alterações.</p>
                                <p><strong>Para recuperar o acesso completo:</strong></p>
                                <p>Renove sua assinatura agora e continue usando ComandaX sem interrupção.</p>
                                <p><strong>Plano mensal:</strong> R$ 49,90/mês</p>
                                <p><a href='https://comandax.app/subscription' style='background-color: #f44336; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Renovar Agora</a></p>
                                <p>Alguma dúvida? Entre em contato conosco.<br/>
                                <strong>Equipe ComandaX</strong></p>
                            </div>
                            <div class='footer'>
                                <p>&copy; 2024 ComandaX. Todos os direitos reservados.</p>
                            </div>
                        </div>
                    </body>
                    </html>"
            };

            await _resend.EmailSendAsync(message);
            _logger.LogInformation("Subscription expired notification sent to {Email} for tenant {TenantId}", email, tenant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send expiration notification to {Email}", email);
            throw;
        }
    }

    /// <summary>
    /// Sends a confirmation email when a subscription is activated/renewed.
    /// </summary>
    public async Task SendSubscriptionActivatedEmailAsync(Tenant tenant, Subscription subscription, string email)
    {
        try
        {
            var expirationDate = subscription.EndDate.ToString("dd/MM/yyyy");
            var price = (subscription.PriceInCentavos ?? Subscription.MONTHLY_PRICE_CENTAVOS) / 100m;

            var message = new EmailMessage
            {
                From = FromEmail,
                To = { email },
                Subject = "ComandaX - Assinatura ativada com sucesso!",
                HtmlBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body {{ font-family: Arial, sans-serif; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #4CAF50; color: white; padding: 20px; border-radius: 5px; }}
                            .content {{ padding: 20px; }}
                            .footer {{ background-color: #f5f5f5; padding: 10px; text-align: center; font-size: 12px; }}
                            .details {{ background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Assinatura Ativada com Sucesso! </h1>
                            </div>
                            <div class='content'>
                                <p>Olá {tenant.Name},</p>
                                <p>Agradecemos seu pagamento! Sua assinatura do ComandaX foi ativada com sucesso.</p>
                                <div class='details'>
                                    <p><strong>Detalhes da Assinatura:</strong></p>
                                    <p><strong>Data de Expiração:</strong> {expirationDate}</p>
                                    <p><strong>Valor Pago:</strong> R$ {price:F2}</p>
                                    <p><strong>ID de Referência:</strong> {subscription.AbacatePayBillingId ?? "N/A"}</p>
                                </div>
                                <p>Você agora tem acesso completo a todos os recursos do ComandaX por mais 30 dias.</p>
                                <p>Sua assinatura será renovada automaticamente a cada mês. Você receberá um email de confirmação antes de cada renovação.</p>
                                <p>Divirta-se usando ComandaX!<br/>
                                <strong>Equipe ComandaX</strong></p>
                            </div>
                            <div class='footer'>
                                <p>&copy; 2024 ComandaX. Todos os direitos reservados.</p>
                            </div>
                        </div>
                    </body>
                    </html>"
            };

            await _resend.EmailSendAsync(message);
            _logger.LogInformation("Subscription activated confirmation email sent to {Email} for tenant {TenantId}", email, tenant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send subscription activated email to {Email}", email);
            throw;
        }
    }
}
