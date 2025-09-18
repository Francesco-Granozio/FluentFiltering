namespace GameStore.Infrastructure.Seeding;

/// <summary>
/// Classi helper per generare dati di test realistici
/// </summary>
public static class SeedData
{
    // Dati per utenti
    public static readonly string[] Nomi = {
        "Mario", "Luigi", "Peach", "Bowser", "Yoshi", "Toad", "Donkey Kong", "Link",
        "Zelda", "Samus", "Pikachu", "Charizard", "Sonic", "Tails", "Knuckles", "Shadow",
        "Cloud", "Sephiroth", "Tifa", "Aerith", "Barret", "Cid", "Vincent", "Yuffie",
        "Kratos", "Atreus", "Aloy", "Nathan Drake", "Elena", "Sully", "Joel", "Ellie",
        "Master Chief", "Cortana", "Arbiter", "Sergeant Johnson", "Geralt", "Yennefer",
        "Triss", "Ciri", "Dandelion", "Zoltan", "Vesemir", "Eskel", "Lambert", "Letho"
    };

    public static readonly string[] Cognomi = {
        "Rossi", "Bianchi", "Verdi", "Neri", "Blu", "Gialli", "Rosa", "Viola",
        "Marrone", "Grigi", "Arancioni", "Turchese", "Indaco", "Magenta", "Ciano", "Beige",
        "Ferrari", "Lamborghini", "Maserati", "Bugatti", "Porsche", "BMW", "Mercedes", "Audi",
        "Volkswagen", "Fiat", "Alfa Romeo", "Lancia", "Ford", "Chevrolet", "Toyota", "Honda",
        "Nissan", "Mazda", "Subaru", "Mitsubishi", "Suzuki", "Daihatsu", "Isuzu", "Kawasaki",
        "Yamaha", "Ducati", "Harley", "Triumph", "KTM", "Aprilia", "Moto Guzzi", "MV Agusta"
    };

    public static readonly string[] DominiEmail = {
        "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "libero.it", "virgilio.it",
        "tiscali.it", "alice.it", "tin.it", "fastwebnet.it", "windtre.it", "tim.it",
        "vodafone.it", "iliad.it", "poste.it", "aruba.it", "register.it", "pec.it"
    };

    public static readonly string[] Paesi = {
        "Italia", "Francia", "Germania", "Spagna", "Regno Unito", "Stati Uniti", "Canada",
        "Brasile", "Argentina", "Messico", "Giappone", "Corea del Sud", "Cina", "India",
        "Australia", "Nuova Zelanda", "Svezia", "Norvegia", "Danimarca", "Finlandia",
        "Paesi Bassi", "Belgio", "Svizzera", "Austria", "Polonia", "Repubblica Ceca",
        "Ungheria", "Romania", "Bulgaria", "Grecia", "Turchia", "Russia", "Ucraina"
    };

    // Dati per giochi
    public static readonly string[] TitoliGiochi = {
        "The Legend of Zelda: Breath of the Wild", "Super Mario Odyssey", "Red Dead Redemption 2",
        "The Witcher 3: Wild Hunt", "God of War", "Horizon Zero Dawn", "Uncharted 4: A Thief's End",
        "The Last of Us Part II", "Cyberpunk 2077", "Assassin's Creed Valhalla",
        "Call of Duty: Modern Warfare", "FIFA 24", "NBA 2K24", "Madden NFL 24",
        "Grand Theft Auto V", "Minecraft", "Fortnite", "Apex Legends", "Valorant",
        "Counter-Strike 2", "Dota 2", "League of Legends", "World of Warcraft",
        "Final Fantasy XVI", "Elden Ring", "Dark Souls III", "Bloodborne", "Sekiro",
        "Ghost of Tsushima", "Spider-Man", "Spider-Man: Miles Morales", "Ratchet & Clank",
        "Demon's Souls", "Returnal", "Deathloop", "Hades", "Cuphead", "Ori and the Will of the Wisps",
        "Celeste", "Hollow Knight", "Dead Cells", "Enter the Gungeon", "Risk of Rain 2",
        "Monster Hunter: World", "Monster Hunter Rise", "Persona 5 Royal", "Yakuza: Like a Dragon",
        "Resident Evil 4", "Resident Evil Village", "Resident Evil 2 Remake", "Silent Hill 2",
        "Metal Gear Solid V", "Death Stranding", "Control", "Alan Wake", "Quantum Break",
        "Hellblade: Senua's Sacrifice", "Inside", "Limbo", "Little Nightmares", "Among Us",
        "Fall Guys", "It Takes Two", "Overcooked 2", "Moving Out", "Human: Fall Flat",
        "Gang Beasts", "Cuphead", "Ori and the Blind Forest", "A Hat in Time", "Yooka-Laylee",
        "Crash Bandicoot N. Sane Trilogy", "Spyro Reignited Trilogy", "Crash Team Racing Nitro-Fueled"
    };

    public static readonly string[] Generi = {
        "Azione", "Avventura", "RPG", "Sparatutto", "Piattaforma", "Puzzle", "Racing",
        "Simulazione", "Strategia", "Sport", "Fighting", "Horror", "Survival", "Battle Royale",
        "MOBA", "MMORPG", "Roguelike", "Metroidvania", "Stealth", "Sandbox", "Open World",
        "Narrative", "Party Game", "Educational", "Music", "Rhythm", "Card Game", "Board Game",
        "Tower Defense", "Real-Time Strategy", "Turn-Based Strategy", "City Builder",
        "Life Simulation", "Vehicle Simulation", "Flight Simulation", "Space Simulation"
    };

    public static readonly string[] Piattaforme = {
        "PC", "PlayStation 5", "PlayStation 4", "Xbox Series X", "Xbox Series S", "Xbox One",
        "Nintendo Switch", "Nintendo 3DS", "Nintendo Wii U", "Steam Deck", "Mobile", "VR",
        "PlayStation VR", "Oculus Quest", "HTC Vive", "Valve Index", "Mac", "Linux"
    };

    public static readonly string[] Sviluppatori = {
        "Nintendo", "Sony Interactive Entertainment", "Microsoft Game Studios", "Valve Corporation",
        "CD Projekt Red", "Rockstar Games", "Ubisoft", "Electronic Arts", "Activision Blizzard",
        "Square Enix", "Capcom", "Bandai Namco", "Sega", "Konami", "FromSoftware", "Naughty Dog",
        "Santa Monica Studio", "Insomniac Games", "Guerrilla Games", "Sucker Punch Productions",
        "Polyphony Digital", "Media Molecule", "Quantic Dream", "Remedy Entertainment",
        "Ninja Theory", "Hello Games", "Team Cherry", "Supergiant Games", "Playdead",
        "Tarsier Studios", "Team17", "Devolver Digital", "Annapurna Interactive",
        "505 Games", "Focus Entertainment", "Paradox Interactive", "Larian Studios",
        "Obsidian Entertainment", "Bethesda Game Studios", "Arkane Studios", "id Software",
        "MachineGames", "Tango Gameworks", "Roundhouse Studios", "Alpha Dog Games",
        "ZeniMax Online Studios", "Bethesda Softworks", "Xbox Game Studios", "343 Industries",
        "The Coalition", "Turn 10 Studios", "Playground Games", "Rare", "Double Fine",
        "InXile Entertainment", "Compulsion Games", "Undead Labs", "Ninja Theory", "Obsidian"
    };

    public static readonly string[] MetodiPagamento = {
        "Carta di Credito", "PayPal", "Apple Pay", "Google Pay", "Amazon Pay", "Stripe",
        "Bonifico Bancario", "Contanti", "Gift Card", "Crypto", "Paysafecard", "Skrill",
        "Neteller", "Bitcoin", "Ethereum", "Litecoin", "Monero", "Ripple", "Dogecoin"
    };

    public static readonly string[] CodiciSconto = {
        "WELCOME10", "NEWUSER20", "SUMMER2024", "WINTER2024", "SPRING2024", "AUTUMN2024",
        "GAMER15", "STUDENT25", "FAMILY30", "PREMIUM50", "LOYALTY20", "BIRTHDAY15",
        "HOLIDAY25", "FLASH30", "EARLY40", "PREORDER10", "REVIEW20", "SHARE15",
        "FOLLOW25", "SUBSCRIBE30", "NEWSLETTER20", "SOCIAL15", "REFER20", "VIP35"
    };

    // Descrizioni di esempio per i giochi
    public static readonly string[] DescrizioniGiochi = {
        "Un'esperienza di gioco rivoluzionaria che ridefinisce il genere open world con meccaniche innovative e un mondo vasto da esplorare.",
        "Un capolavoro dell'arte videoludica che combina gameplay avvincente con una narrativa profonda e personaggi memorabili.",
        "Un'avventura epica che ti porterà in mondi fantastici pieni di misteri da scoprire e sfide da superare.",
        "Un gioco di ruolo immersivo con un sistema di combattimento strategico e una storia coinvolgente che ti terrà incollato allo schermo per ore.",
        "Un'esperienza di gioco unica che mescola elementi di azione, esplorazione e puzzle solving in un mondo ricco di dettagli.",
        "Un capolavoro tecnico che dimostra le potenzialità delle console di nuova generazione con grafica mozzafiato e gameplay fluido.",
        "Un gioco che rappresenta l'evoluzione del genere, offrendo centinaia di ore di contenuto e un'esperienza personalizzabile.",
        "Un'avventura indimenticabile che ti farà vivere emozioni intense attraverso una storia toccante e personaggi ben sviluppati.",
        "Un gioco innovativo che introduce nuove meccaniche di gameplay e offre un'esperienza fresca e coinvolgente.",
        "Un titolo che onora la tradizione della serie mentre introduce elementi moderni che lo rendono accessibile a nuovi giocatori."
    };

    // Titoli di recensioni
    public static readonly string[] TitoliRecensioni = {
        "Un capolavoro assoluto!", "Esperienza incredibile", "Gioco dell'anno", "Da non perdere",
        "Molto divertente", "Grafica spettacolare", "Storia coinvolgente", "Gameplay perfetto",
        "Vale ogni centesimo", "Consigliatissimo", "Supera le aspettative", "Geniale",
        "Emozionante dall'inizio alla fine", "Un must have", "Impossibile smettere di giocare",
        "Meraviglioso", "Fantastico", "Eccellente", "Perfetto", "Straordinario", "Incredibile",
        "Bellissimo", "Magnifico", "Splendido", "Eccelso", "Superbo", "Phenomenal", "Outstanding"
    };

    // Corpi di recensioni
    public static readonly string[] CorpiRecensioni = {
        "Questo gioco è semplicemente fantastico! La grafica è mozzafiato, il gameplay è fluido e la storia è coinvolgente. Lo consiglio vivamente a tutti gli appassionati del genere.",
        "Un'esperienza di gioco incredibile che mi ha tenuto incollato allo schermo per ore. I personaggi sono ben sviluppati e il mondo di gioco è ricco di dettagli.",
        "Dopo aver giocato a centinaia di giochi, questo si distingue per la sua originalità e qualità. Vale assolutamente il prezzo richiesto.",
        "La meccanica di gioco è innovativa e divertente. Ho trascorso ore a esplorare il mondo e scoprire tutti i suoi segreti nascosti.",
        "Grafica spettacolare e gameplay coinvolgente. Questo gioco rappresenta il meglio di quello che il gaming moderno può offrire.",
        "Una storia emozionante che ti farà ridere, piangere e riflettere. I personaggi sono memorabili e il mondo è bellissimo da esplorare.",
        "Il sistema di combattimento è perfetto e offre molte possibilità di personalizzazione. Ogni battaglia è unica e divertente.",
        "Questo gioco ha superato tutte le mie aspettative. La qualità dell'audio e della grafica è eccezionale.",
        "Un capolavoro che dimostra come i videogiochi possano essere una forma d'arte. Ogni dettaglio è curato alla perfezione.",
        "Gameplay fluido e controlli responsivi. Questo gioco ti farà dimenticare il tempo che passa mentre giochi.",
        "La varietà delle missioni e delle attività è impressionante. C'è sempre qualcosa di nuovo da scoprire.",
        "Un gioco che ti farà vivere emozioni intense. La colonna sonora è fantastica e si integra perfettamente con l'azione.",
        "Questo titolo rappresenta l'evoluzione del genere. Le nuove meccaniche aggiungono profondità senza complicare eccessivamente il gameplay.",
        "Un'esperienza immersiva che ti farà sentire parte del mondo di gioco. La qualità della produzione è eccezionale.",
        "Dopo aver completato il gioco, ho subito ricominciato. È così divertente che non riesco a smettere di giocarci!"
    };
}
