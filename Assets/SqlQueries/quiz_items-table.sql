CREATE DATABASE IF NOT EXISTS quizgame_db;
USE quizgame_db;

CREATE TABLE IF NOT EXISTS quiz_items (
	quiz_item_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    category_id INT NOT NULL,
	question_text VARCHAR(256) NOT NULL,
    correct_answer VARCHAR(100) NOT NULL,
    answer_2 VARCHAR(100) NOT NULL,
    answer_3 VARCHAR(100) NOT NULL,
    explanation TEXT NOT NULL
);

INSERT INTO quiz_items (category_id, question_text, correct_answer, answer_2, answer_3, explanation) VALUES
(1, 'Cosa rappresenta l\'acronimo CRUD?', 'Create, Read, Update, Delete', 'Copy, Replace, Undo, Delete', 'Check, Run, Update, Deploy', 'CRUD indica le quattro operazioni fondamentali nei database: Create (creare), Read (leggere), Update (aggiornare) e Delete (eliminare).'),
(1, 'Quale comando SQL serve per creare un nuovo record?', 'INSERT', 'CREATE', 'ADD', 'Il comando INSERT INTO serve per creare (aggiungere) un nuovo record in una tabella.'),
(1, 'Quale operazione CRUD corrisponde al comando SELECT?', 'Read', 'Create', 'Update', 'SELECT serve per leggere i dati, quindi corrisponde all\'operazione Read del modello CRUD.'),
(1, 'Quale comando SQL serve per modificare un record esistente?', 'UPDATE', 'CHANGE', 'ALTER', 'UPDATE permette di modificare i valori dei campi di un record già presente nella tabella.'),
(1, 'Quale comando SQL elimina un record da una tabella?', 'DELETE', 'DROP', 'REMOVE', 'DELETE rimuove uno o più record da una tabella, a differenza di DROP che elimina l\'intera tabella.'),
(1, 'Quale operazione CRUD viene eseguita con il comando INSERT?', 'Create', 'Read', 'Update', 'INSERT aggiunge un nuovo record, quindi rappresenta l\'operazione Create.'),
(1, 'Cosa fa l\'operazione DELETE senza una clausola WHERE?', 'Elimina tutti i record', 'Elimina solo un record', 'Non elimina nulla', 'DELETE senza WHERE elimina tutti i record della tabella, lasciandola però intatta.'),
(1, 'Quale comando SQL viene usato per leggere dati da più tabelle?', 'JOIN', 'MERGE', 'APPEND', 'Le JOIN permettono di leggere e combinare dati provenienti da più tabelle relazionate.'),
(1, 'Quale clausola serve per aggiornare solo determinati record?', 'WHERE', 'WHEN', 'IF', 'La clausola WHERE filtra i record su cui eseguire UPDATE, DELETE o SELECT.'),
(1, 'Quale comando serve per creare una nuova tabella nel database?', 'CREATE TABLE', 'INSERT', 'ADD TABLE', 'CREATE TABLE viene usato per definire la struttura di una nuova tabella nel database.'),
(2, 'Quale parte della tavola da surf sta in acqua?', 'Il bottom', 'Il nose', 'Il deck', 'Il bottom è la parte inferiore della tavola che sta a contatto con l''acqua. La sua forma (concavo, convesso, piatto) influenza la manovrabilità e la velocità.'),
(2, 'Come si chiama la manovra di girarsi sull''onda?', 'Cut back', 'Floater', 'Aerial', 'Il cut back è una manovra fondamentale dove il surfista inverte la direzione tornando verso la parte più ripida dell''onda per mantenere velocità e potenza.'),
(2, 'Qual è il nome della schiuma bianca dell''onda?', 'White water', 'Foam', 'Lip', 'Il white water (o whitewater) è la schiuma bianca che si forma quando l''onda si rompe. È ideale per i principianti per imparare a prendere le prime onde.'),
(2, 'Cosa significa "dropper" nel surf?', 'Rubare un''onda a qualcuno', 'Cadere dalla tavola', 'Un tipo di tavola', 'Dropper significa prendere un''onda quando qualcuno ha già la priorità. È una grave violazione dell''etichetta del surf e può essere pericoloso.'),
(2, 'Quale tipo di onda è più adatta ai principianti?', 'Beach break', 'Reef break', 'Point break', 'I beach break (onde che si rompono su fondale sabbioso) sono più sicuri per i principianti: il fondale è morbido, le onde sono più prevedibili e meno potenti.'),
(2, 'Come si chiama la corda che collega la tavola alla caviglia?', 'Leash', 'Rope', 'Line', 'Il leash è la corda elastica che collega la tavola alla caviglia del surfista. È essenziale per la sicurezza: impedisce alla tavola di allontanarsi e colpire altri surfisti.'),
(2, 'Qual è la posizione corretta dei piedi in take-off?', 'Uno avanti e uno dietro', 'Paralleli', 'Entrambi al centro', 'La stance corretta ha un piede avanti (lead foot) e uno dietro (back foot) perpendicolari alla direzione della tavola. Questa posizione garantisce equilibrio e controllo.'),
(2, 'Cosa indica la "priority" nel surf?', 'Il surfista più vicino al picco ha priorità', 'Il surfista più esperto', 'Chi è arrivato prima in acqua', 'La priority va al surfista più vicino al punto dove l''onda inizia a rompersi (il picco). Questa regola previene collisioni e garantisce un surf sicuro e rispettoso.'),
(2, "Che cos'è un 'barrel'?", 'Il tubo formato dall''onda', 'Un tipo di tavola', 'Una manovra aerea', "Il barrel (o tubo) è quando l'onda si chiude creando un cilindro cavo. Surfare dentro al tubo è considerato il momento più emozionante e tecnico del surf."),
(2, 'Qual è la parte anteriore della tavola?', 'Nose', 'Tail', 'Rail', 'Il nose è la parte anteriore (punta) della tavola. Le tavole longboard hanno un nose più largo e arrotondato, mentre le shortboard hanno un nose più stretto e appuntito.'),
(3, 'In quale videogioco compare il personaggio “Master Chief”?', 'Halo', 'Gears of War', 'Call of Duty', 'Master Chief è il protagonista della saga Halo.'),
(3, 'Quale azienda ha creato la console Switch?', 'Nintendo', 'Sony', 'Microsoft', 'La console Switch è prodotta da Nintendo.'),
(3, 'In “The Legend of Zelda”, quale è il nome del protagonista?', 'Link', 'Zelda', 'Ganon', 'Zelda è la principessa, il protagonista è Link.'),
(3, 'In quale gioco si raccolgono anelli dorati come oggetti principali?', 'Sonic the Hedgehog', 'Super Mario', 'Crash Bandicoot', 'Gli anelli dorati sono iconici nella serie Sonic.'),
(3, 'Qual è la principale arma di Cloud in Final Fantasy VII?', 'Buster Sword', 'Ascia da battaglia', 'Doppie lame', 'Cloud combatte con la famosa spada gigante “Buster Sword”.'),
(3, 'Quale gioco popolare ha introdotto la modalità “Battle Royale”?', 'PUBG', 'Halo 3', 'Overwatch', 'PUBG ha reso popolare il formato Battle Royale moderno.'),
(3, 'Chi è la mascotte storica di Nintendo?', 'Mario', 'Sonic', 'Pikachu', 'Mario è la mascotte principale di Nintendo.'),
(3, 'Quale gioco ha il motto “Rip and Tear”?', 'DOOM', 'Fortnite', 'Warframe', '“Rip and Tear” è il motto iconico della saga DOOM.'),
(3, 'In Minecraft, quale materiale serve per costruire un piccone di diamante?', 'Diamanti e bastoni', 'Ferro e bastoni', 'Oro e bastoni', 'Il piccone di diamante richiede due bastoni e tre diamanti.'),
(3, 'Qual è il nome del protagonista di God of War?', 'Kratos', 'Ares', 'Zeus', 'Kratos è il protagonista della serie God of War.');

SELECT * FROM quiz_items;