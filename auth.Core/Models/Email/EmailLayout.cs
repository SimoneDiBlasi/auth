namespace auth.Core.Models.Email
{
    public class EmailLayout
    {

        public EmailLayout()
        {
            // Inizializzazioni nel costruttore se necessario
        }

        // Costruttore con un argomento
        public static string ConfirmationLayoutEmail(string confirmLink)
        {
            return $@"
            <body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f9f9f9; margin: 0; padding: 0; display: flex; justify-content: center; align-items: center; height: 100vh;"">
                <div style=""text-align: center; background-color: #ffffff; border-radius: 10px; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1); padding: 40px; max-width: 400px; width: 100%;"">
                    <h2 style=""color: #2c3e50; font-size: 24px; margin-bottom: 20px; text-transform: uppercase; letter-spacing: 1px;"">Gentile utente,</h2>
                    <p style=""margin-bottom: 20px;"">Per completare la registrazione, clicca sul pulsante sottostante:</p>
                    <a href=""{confirmLink}"" style=""text-decoration: none;"">
                        <button style=""padding: 12px 24px; background-color: #3498db; color: #fff; border: none; cursor: pointer; border-radius: 25px; transition: background-color 0.3s ease; font-size: 16px; text-transform: uppercase; letter-spacing: 1px; outline: none;"">Conferma il tuo account</button>
                    </a>
                    <p style=""margin-top: 20px;"">Se non hai richiesto questa registrazione, ignora questa email.</p>
                </div>
            </body>
         ";
        }

        public static string OTPLayoutEmail(string otp)
        {
            return $@"
            <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; display: flex; justify-content: center; align-items: center; height: 100vh;"">
                <div style=""text-align: center; background-color: #fff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); padding: 40px; max-width: 400px; width: 100%;"">
                    <h2 style=""color: #2c3e50; margin-bottom: 20px;"">Ecco il tuo codice di sicurezza:</h2>
                    <p style=""font-size: 24px; color: #007bff;"">{otp}</p>
                </div>
            </body>";
        }
    }
}

