### Första felet. Programmet kraschar vid start
Programmet kraschade direkt med ett IndexOutOfRangeException. Felet pekade på rad 2 i Program.cs, så först tog jag bort den raden. Då startade programmet, men listan var alltid tom eftersom rad 2 är den som läser in filen. Jag lade tillbaka raden och läste hela anropsstacken. Då såg jag att kraschen egentligen hände i Load() i ShoppingList.cs.

Save() skriver \r\n efter varje rad, men Load() delade bara på \n. Det gav en tom rad sist som saknade namn och kraschade programmet. Dessutom blev \r kvar i varje namn, så namnen försvann i utskriften och sökningen hittade inga varor.

Jag bytte till File.ReadAllLines och hoppar över tomma rader och rader som inte går att läsa.

### Andra felet. Programmet kraschar om items.txt saknas
Load() försökte läsa filen utan att kolla om den fanns, vilket gav ett FileNotFoundException. Jag lade till en kontroll med File.Exists. Saknas filen startar programmet med en tom lista.

### Tredje felet. Bokstäver i stället för tal kraschar programmet
Menyn, priset och numret använde int.Parse, som kastar ett FormatException om texten inte är ett tal. Jag bytte till int.TryParse på alla tre ställena. Vid fel inmatning visas ett meddelande och menyn kommer tillbaka.

### Fjärde felet. Ta bort en vara som inte finns kraschar programmet
RemoveAt kollade inte om numret fanns i listan, vilket gav ett ArgumentOutOfRangeException. Nu kontrollerar metoden att numret ligger mellan 1 och antalet varor och returnerar true eller false. Program.cs skriver ut rätt meddelande.

### Femte felet. Totalsumman är fel
Programmet visade 121 kr i stället för 136 kr. Loopen i Total() började på i = 1, så den första varan räknades aldrig med. Jag ändrade till i = 0.

### Sjätte felet. Sparandet döljer fel
Det här felet hittade jag genom att läsa koden. Save() hade en tom catch och skrev alltid "Listan är sparad", även om sparandet misslyckades. Jag flyttade meddelandet in i try och ersatte den tomma catch med två som fångar IOException och UnauthorizedAccessException och skriver ut vad som gick fel.

## Så testade jag
Jag skrev bokstäver och tomma svar i menyn, priset och numret, tog bort varor som inte fanns, döpte om items.txt, sparade och startade om, sökte efter varor i listan och räknade totalsumman för hand.

### Budget tak samt småfix. 
Från början jämförde Add() Total() + pris med Budget. Nu jämför Add() i stället priset med det som är kvar av budgeten, Budget - Total(). Den uträkningen kan inte bli för stor, så kontrollen fungerar även för väldigt höga priser.
Jag testade genom att lägga till en vara för 2147483647 kr. Den stoppas nu av budgeten. Sedan lade jag till en vara som gjorde totalen precis 500 kr, och den gick igenom.
