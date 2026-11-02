using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

internal class NotificationService
{
    private readonly IConfiguration _configuration;

    public NotificationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendTransactionEmailAsync(string recipientEmail, decimal amount, string description)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            "NET26 BANKEN",
            _configuration["SMTP_FROM"]
        ));

        message.To.Add(MailboxAddress.Parse(recipientEmail));

        message.Subject = "Transaktionsbekräftelse";

        string htmlBody = $@"
    <!DOCTYPE html>
    <html lang=""sv"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    </head>

    <body style=""
        margin: 0;
        padding: 30px 10px;
        background-color: #f1f5f9;
        font-family: Arial, sans-serif;
    "">

        <div style=""
            max-width: 500px;
            margin: auto;
            background-color: white;
            border-radius: 12px;
            overflow: hidden;
        "">

            <div style=""
                background-color: #142b49;
                padding: 25px;
                color: white;
            "">
                <h1 style=""margin: 0; font-size: 24px;"">
                    NET26 BANKEN
                </h1>

                <p style=""margin-bottom: 0; color: #d5e2f0;"">
                    TRANSAKTIONSBEKRÄFTELSE
                </p>
            </div>

            <div style=""padding: 30px;"">

                <h2 style=""color: #16865b; font-size: 20px;"">
                    ✓ Överföringen är genomförd
                </h2>

                <p style=""color: #64748b; font-size: 12px;"">
                    ÖVERFÖRT BELOPP
                </p>

                <h1 style=""color: #142b49; font-size: 32px;"">
                    {amount:N2} SEK
                </h1>

                <hr style=""border: 0; border-top: 1px solid #e2e8f0;"">

                <h3 style=""color: #142b49;"">
                    Transaktionsinformation
                </h3>

                <p style=""color: #475569;"">
                    <strong>Typ:</strong> Banköverföring
                </p>

                <p style=""color: #475569;"">
                    <strong>Beskrivning:</strong> {System.Net.WebUtility.HtmlEncode(description)}
                </p>

                <p style=""color: #475569;"">
                    <strong>Datum:</strong> {DateTime.Now:yyyy-MM-dd HH:mm}
                </p>

                <hr style=""border: 0; border-top: 1px solid #e2e8f0;"">

                <p style=""color: #94a3b8; font-size: 12px;"">
                    Detta är en automatisk bekräftelse från NET26 BANKEN
                </p>

            </div>
        </div>

    </body>
    </html>";

        message.Body = new TextPart("html")
        {
            Text = htmlBody
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _configuration["SMTP_HOST"],
            587,
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            _configuration["SMTP_USERNAME"],
            _configuration["SMTP_PASSWORD"]
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}