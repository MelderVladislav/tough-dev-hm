namespace UberPopug.Domains.Auth.API.Configuration;

public class EmailSettings
{
    public bool ShouldConfirmEmail { get; set; }

    public string From { get; set; }

    public string Password { get; set; }

    public string HtmlBody { get; set; }

    public string Host { get; set; }

    public int? Port { get; set; }

    public int ExpirationInHours { get; set; }

    public string DefaultHtmlTemplate => @"<h2>You have been registered on our website.</h2>
         <div>
             <p>"
                                         + "Please, <a href=\"activationLink\">click here</a> to confirm you account."
                                         + @"</p>
         </div>";
}