# CSharpProject
C# project made for the course Tecniche di sviluppo software in ambiente industriale of the Università degli Studi di Parma

DOCUMENTAZIONE DELL’APPLICAZIONE E TUTTE LE SUE FUNZIONALITA’
Nel nostro progetto abbiamo implementato un’applicazione distribuita riguardante la gestione di un             E-Commerce scritta in linguaggio C# e la comunicazione tra le varie entità è stata sviluppata tramite servizi WCF.
Come primo step, subito dopo l’avvio compare la pagina di Login in cui è richiesto di inserire e-mail e password. In questa fase è presente un controllo in cui il sistema riconosce se l’utente è Admin (in quanto mail caratterizzata da finale “@ecommerce.com“ oppure  User se presenta qualsiasi altra tipologia di mail.
Nel caso non si sia ancora in possesso delle credenziali, tramite un link SignUp che rimanda alla schermata apposita si ha la possibilità di registrarsi inserendo: nome, cognome, e-mail, password ed eventualmente poi tornare indietro alla pagina di login per effettuare l’accesso.
ADMIN: Dopo aver effettuato il login viene reindirizzato alla WelcomePage dove sono presenti tre bottoni principali: Products, Users e Orders.
•	PRODUCTS: in questa interfaccia possiamo visualizzare tutti i prodotti presenti sul sito e può decidere di aggiungerne nuovi oppure rimuoverne.
•	USERS/ORDERS: queste due pagine invece hanno solo la possibilità di visualizzare i rispettivi utenti e ordini con tutte le relative informazioni.
USER: Dopo aver effettuato il login viene reindirizzato alla HomePage dove l’utente riesce a visualizzare i prodotti dell’e-commerce. Dopo aver cliccato su uno dei prodotti comparirà la pagina dedicata a esso che presenta l’immagine, nome, descrizione, quantità e prezzo, da qui c’è la possibilità di selezionare la quantità che si desidera acquistare (impossibile selezionare una quantità maggiore a quella stabilita del prodotto) e se il prezzo è minore o uguale alla somma di denaro presente sul wallet l’utente può effettuare l’ordine. 
Tramite il “go back” si ritorna alla schermata iniziale e cliccando il bottone Profile apparirà la pagina riservata al profilo dell’utente in cui sono presenti tutte le sue informazioni riguardanti l’indirizzo (città, nazione, codice postale, via) necessarie per poter compiere un ordine. È presente anche il wallet in cui appare la somma di denaro presente sul portafoglio dell’utente con anche la possibilità di aggiungere disponibilità ad essa.
Infine, come ultima possibilità, tramite il bottone Orders, l’utente visualizza lo storico degli ordini effettuati tramite una schermata dedicata.
Per entrambi i casi, admin/user è presente nelle relative pagine iniziali il Logout per tornare alla pagina inziale di Login in modo da avere la possibilità di cambiare utente. Inoltre lo User dispone in ogni pagina del bottone go back che permette di muoversi tra le schermate.

Euclidi Filippo mtr.294517
Angeloni Matteo mtr.295645
