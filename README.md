# Zmiany związane z siecią

Aktualnie:
1. Oddzielenie serwera (wyświetlanie) i klienta (wysyłanie informacji)
2. Część protokołu zdefiniowana
3. Usunięte implementacje konkretnych komórek (tymczasowo, odtworzymy je z Gita, bo były zbyt mocno powiązane z logiką wyświetlania)

Do zrobienia:
1. Zrobienie serwera słuchającego na odpowiednim porcie i dodanie do niego pełnej implementacji protokołu + poprawa protokołu
2. Serwer zajmuje się głównie wyświetlaniem, więc ma mieć logikę z wyświetlaniem i wysyłaniem (taka baza danych in-memory)
3. Przywrócenie konkretnych implementacji logiki komórek w CellClient, oparte na procesach

Do protokołu należy dodać koncepcję poruszania się