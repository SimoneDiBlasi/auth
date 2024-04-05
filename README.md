# Documentazione API di Autenticazione

## Introduzione
Questa documentazione descrive le operazioni disponibili tramite l'API di autenticazione. L'API consente agli utenti di registrarsi, accedere, recuperare la password, gestire i ruoli e ottenere informazioni personali.

## Autenticazione
Ogni richiesta all'API deve includere un header di autenticazione. Per autenticarsi, utilizzare un token valido ottenuto tramite login.

## Registrazione
Per registrare un nuovo utente, inviare una richiesta POST al seguente endpoint:
https://{url}/api/signup


### Parametri
- `username` (string): L'username dell'utente.
- `email` (string): L'email dell'utente.
- `password` (string): La password dell'utente.
- `confirmPassword` (string): La conferma della password dell'utente.
- `phoneNumber` (string): Il numero di telefono dell'utente.
- `address` (object):
  - `street` (string): Indirizzo.
  - `city` (string): Città.
  - `state` (string): Stato.
  - `postalCode` (string): Codice Postale.
  - `country` (string): Paese.
- `role` (int): Ruolo dell'utente (0: Admin, 1: SoftwareDeveloper, 2: MarketingManager, 3: UXDesigner, 4: Sales, 5: DataAnalyst).
- `claims` (dictionary<string, string>): Altre informazioni.

### Risposta
Dopo una registrazione riuscita, verrà inviata una e-mail di conferma all'indirizzo fornito. L'utente deve confermare l'e-mail per poter procedere con il login.

## Conferma E-mail
Per confermare l'e-mail dopo la registrazione, l'utente deve fare clic sul link fornito nella e-mail di conferma.

## Login
Per effettuare il login, inviare una richiesta GET al seguente endpoint:
https://{url}/api/login/validate-credential


### Parametri
- `email` (string): L'email dell'utente.
- `password` (string): La password dell'utente.

### Risposta
Se le credenziali sono corrette e l'e-mail è stata confermata, verrà restituito un token di accesso.

## Invio OTP
Dopo il login, prima di accedere alle pagine protette, verrà inviato un OTP all'utente.

## Convalida OTP
Per convalidare l'OTP ricevuto, inviare una richiesta POST al seguente endpoint:
https://{url}/api/login/{otp}?userId={$userId}


### Parametri
- `otp` (string): Il codice OTP ricevuto.
- `userId` (string): L'ID dell'utente.

### Risposta
Se l'OTP è corretto, l'utente potrà procedere con l'accesso.

## Recupero Password
Per recuperare la password, inviare una richiesta POST al seguente endpoint:
https://{url}/api/recovery/email-password?email={$email}


### Parametri
- `email` (string): L'email dell'utente.

### Risposta
Dopo il recupero della password, verrà inviata una e-mail con istruzioni per il cambio password.

## Cambio Password
Per cambiare la password, l'utente deve seguire le istruzioni ricevute via e-mail dopo il recupero password.

## Gestione Ruoli (CRUD)
Le operazioni di creazione, lettura, aggiornamento e cancellazione dei ruoli sono disponibili solo per gli utenti con ruolo di amministratore.

### Creazione Ruolo
https://{url}/api/role/{$roleName}

### Lettura Ruolo
https://{url}/api/role/{$roleName}

### Aggiornamento Ruolo
https://{url}/api/role/{$roleName}

### Cancellazione Ruolo
https://{url}/api/role/{$roleName}


## Informazioni Utente
Per ottenere le informazioni personali dell'utente, inviare una richiesta GET al seguente endpoint:
https://{url}/api/userinfo/{$Id}


### Parametri
- `id` (string): L'ID dell'utente.

### Risposta
Verranno restituite le informazioni personali dell'utente autenticato.

## Logout
Per effettuare il logout, inviare una richiesta POST al seguente endpoint:
https://{url}/api/logout


### Risposta
L'utente verrà disconnesso e il token di accesso verrà invalidato.

## Errori
In caso di errori, verranno restituiti i relativi codici di stato HTTP e messaggi di errore appropriati.

## Limitazioni
- Gli endpoint per la gestione dei ruoli sono disponibili solo per gli amministratori.
- Le richieste di cambio password e recupero password richiedono l'invio di una e-mail di conferma per confermare l'identità dell'utente.

Questa documentazione è soggetta a modifiche e aggiornamenti. Controllare regolarmente per eventuali aggiornamenti o modifiche.


