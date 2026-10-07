### Första felet. Programmet kraschar vid start
Programmet kraschade direkt med ett IndexOutOfRangeException. Felet pekade på rad 2 i Program.cs, så först tog jag bort den raden. Då startade programmet, men listan var alltid tom eftersom rad 2 är den som läser in filen. Jag lade tillbaka raden och läste hela anropsstacken. Då såg jag att kraschen egentligen hände i Load() i ShoppingList.cs.

Save() skriver \r\n efter varje rad, men Load() delade bara på \n. Det gav en tom rad sist som saknade namn och kraschade programmet. Dessutom blev \r kvar i varje namn, så namnen försvann i utskriften och sökningen hittade inga varor.

Jag bytte till File.ReadAllLines och hoppar över tomma rader och rader som inte går att läsa.

### Andra felet. Programmet kraschar om items.txt saknas
Load() försökte läsa filen utan att kolla om den fanns, vilket gav ett FileNotFoundException. Jag lade till en kontroll med File.Exists. Saknas filen startar programmet med en tom lista.

### Tredje felet. Bokstäver i stället för tal kraschar programmet
Menyn, priset och numret använde int.Parse, som kastar ett FormatException om texten inte är ett tal. Jag bytte till int.TryParse på alla tre ställena. Vid fel inmatning visas ett meddelande och menyn kommer tillbaka.