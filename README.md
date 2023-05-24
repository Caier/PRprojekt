## Zmiany związane z siecią edition brzeks version alpha -0.0001

przesyłam żeby zachować jakiś progres i żebyście mogli zobaczyć co na razie zrobiłem

- to co obecnie istnieje rzeczywiście można skompilować i uruchomić
- logika serwera jest w nowym projekcie "OrganismServer" korzysta z asp.net
	- w OrganismService.cs trzeba napisać implementację metod serwisu grpc tak jak jest w Protos/protocol.proto (na razie można dodać tylko bakterię jak widać)
	- OrganismLogic.cs przechowuje tak w sumie informacje o stanie i ewentualnie przenieść do niego metody z CellFactory.cs
- klient to obecnie helloworld, który łączy się z serwerem i spawni bakterię. (let's call it a proof-of-concept)
	- w [branchu michała](https://github.com/Caier/PRprojekt/commit/54b8db64dbf36bb452ddc0e9b3fb0a61711a8b98) jest pełniejsza implementacja klienta, i deklaracji protokołu

czy damy radę to zrobić na jutro? wątpięęę