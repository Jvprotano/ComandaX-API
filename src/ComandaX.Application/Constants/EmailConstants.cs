using ComandaX.Domain.Entities;

namespace ComandaX.Application.Constants;

public static class EmailConstants
{
    public const string FROM_EMAIL = "ComandaX <onboarding@resend.dev>";
    private static string CUSTOMER_TAB_EMAIL_BODY(CustomerTab tab, decimal total) => $@"
        <div style='
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            padding: 24px;
            background-color: #fafafa;
        '>
            <h2 style='text-align:center; color:#222; margin-bottom: 16px;'>Resumo de sua Comanda em MM Sorveteria e pastelaria</h2>
            <p style='font-size: 15px; margin-bottom: 24px; text-align:center;'>
                <strong>Comanda #{tab.Code}</strong><br />
                Obrigado por consumir conosco!
            </p>

            <div style='border-top:1px solid #ddd; margin-top:8px; padding-top:8px;'>
                <ul style='list-style:none; padding:0; margin:0;'>
                    {string.Join("", tab.Orders.Select(o => $@"
                        <li style='margin-bottom: 20px; background-color:#fff; border:1px solid #ddd; border-radius:6px; padding:12px;'>
                            <div style='font-weight:bold; color:#444; margin-bottom:6px;'>
                                Pedido #{o.Code}
                            </div>
                            <ul style='list-style:none; padding-left:12px; margin:0;'>
                                {string.Join("", o.OrderProducts.Select(op => $@"
                                    <li style='font-size:14px; padding:4px 0; border-bottom:1px solid #f1f1f1;'>
                                        <span style='display:inline-block; width:55%;'>{op.Product?.Name}</span>
                                        <span style='display:inline-block; width:15%; text-align:center;'>x{op.Quantity}</span>
                                        <span style='display:inline-block; width:25%; text-align:right;'>R$ {op.TotalPrice:F2}</span>
                                    </li>
                                "))}
                            </ul>
                            <div style='text-align:right; margin-top:8px; font-weight:bold; color:#000;'>
                                Total do Pedido: R$ {o.OrderProducts.Sum(p => p.TotalPrice):F2}
                            </div>
                        </li>
                    "))}
                </ul>
            </div>

            <div style='border-top:1px solid #ddd; margin-top:20px; padding-top:16px; text-align:right;'>
                <p style='font-size:16px; font-weight:bold; color:#000;'>
                    Total Geral: R$ {total:F2}
                </p>
            </div>

            <p style='font-size:13px; color:#777; text-align:center; margin-top:24px;'>
                Este é um resumo automático enviado pela ComandaX.<br/>
                Caso não tenha solicitado, por favor ignore este e-mail.
            </p>
        </div>
        ";

    public static string GetCustomerTabEmailBody(CustomerTab tab)
    {
        var total = tab.Orders.Sum(o => o.OrderProducts.Sum(p => p.TotalPrice));
        return CUSTOMER_TAB_EMAIL_BODY(tab, total);
    }
}
