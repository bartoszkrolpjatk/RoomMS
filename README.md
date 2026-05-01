# System Zarządzania Salami i Rezerwacjami - REST API (ASP.NET Core)

***
**Uwaga:** Projekt został zrealizowany w ramach zadania akademickiego (projekt na studia) na przedmiot **APBD** (Aplikacje Bazodanowe).
***

***
## 🎯 Wprowadzenie i Cel Projektu
Niniejszy projekt stanowi praktyczne **wprowadzenie do architektury REST API przy użyciu ASP.NET Core w języku C#**. 

Głównym celem ćwiczenia jest przećwiczenie kluczowych mechanizmów budowy nowoczesnych aplikacji sieciowych, w tym:
* Definiowania ścieżek dostępu (**Routing**).
* Poprawnego wykorzystywania metod HTTP (**GET, POST, PUT, DELETE**).
* Przechwytywania i wiązania danych wejściowych (**Model Binding**) z różnych źródeł: trasy (Route), zapytania (Query String) oraz ciała żądania (Body).
* Stosowania rygorystycznej walidacji i zwracania adekwatnych, standardowych **kodów statusu HTTP** (m.in. 200 OK, 201 Created, 204 No Content, 400 Bad Request, 404 Not Found, 409 Conflict).
***

***
## 💾 Przechowywanie Danych (Architektura In-Memory)
Aplikacja w swoim założeniu **nie jest podpięta do rzeczywistej bazy danych** (np. SQL czy Entity Framework). 

Zgodnie z wymaganiami projektu, dane odczytywane i zapisywane przez aplikację są przechowywane **wyłącznie w pamięci operacyjnej serwera**. Symulacją warstwy trwałej (Persistence Layer) zajmują się specjalne klasy repozytoriów. Stanowią one nakładkę na wbudowane listy statyczne w C#, które są inicjalizowane domyślnym zestawem danych (sale oraz rezerwacje) w momencie uruchamiania aplikacji. Repozytoria te wstrzykiwane są jako *Singletony*.
***

***
## 📂 Struktura Kodu
Aplikacja została zaprojektowana w sposób modułowy, oddzielając od siebie warstwy odpowiedzialności:

* **`Controllers/`** 
  Punkt wejścia aplikacji. Zawiera kontrolery API (`RoomsController`, `ReservationsController`) z odpowiednimi atrybutami, odpowiadające za przetwarzanie żądań i odpowiedź do klienta.
* **`Models/`**
  * **`Entities/`** - Modele domenowe, odzwierciedlające "bazodanowy" wygląd obiektów (m.in. `Room`, `Reservation`).
  * **`Dtos/`** - Obiekty typu Data Transfer Object. Służą do wymiany informacji z klientem oraz walidacji z użyciem adnotacji (Data Annotations).
* **`Repositories/`** 
  Klasy przechowujące listy z danymi (`RoomRepository`, `ReservationRepository`). Izolują kontrolery od bezpośredniego dostępu do struktur danych w pamięci.
* **`Profiles/`** 
  Pliki konfiguracyjne dla biblioteki **AutoMapper**, umożliwiające automatyczne mapowanie między encjami domenowymi a obiektami DTO, co znacząco zmniejsza ilość powtarzalnego kodu.
***

***
## ⚙️ Logika Biznesowa
Mimo braku bazy danych, aplikacja rygorystycznie pilnuje logiki biznesowej, na którą składają się poniższe reguły:

* **Walidacja danych:** System odrzuca ujemne pojemności sal czy puste pola tekstowe przed wejściem do algorytmów rezerwacyjnych.
* **Aktywność i istnienie sal:** Tworzenie rezerwacji dla pokoju, który został usunięty lub ma przypisaną flagę oznaczającą brak aktywności, jest niemożliwe.
* **Zapobieganie konfliktom czasowym:** System weryfikuje godziny rezerwacji (wykorzystując typy `DateOnly` oraz `TimeOnly`) zapobiegając nakładaniu się na siebie dwóch spotkań w tym samym czasie dla tej samej sali (Zwraca `409 Conflict`).
* **Integralność:** Próba usunięcia sali (DELETE), do której są już przypisane przyszłe rezerwacje, może zostać zablokowana by uniknąć porzuconych referencji.
***

***
## 📡 Główne Endpointy API

### 🏢 Sale Dydaktyczne (`RoomsController`)
* `GET /api/rooms` - Zwraca wszystkie sale (wspiera filtrowanie po pojemności, wyposażeniu i aktywności w *Query String*).
* `GET /api/rooms/{id}` - Zwraca konkretną salę.
* `GET /api/rooms/{buildingCode}` - Zwraca listę sal przefiltrowaną po kodzie budynku z trasy (*Route*).
* `POST /api/rooms` - Rejestruje nową salę w systemie.
* `PUT /api/rooms/{id}` - Nadpisuje dane istniejącej sali.
* `DELETE /api/rooms/{id}` - Usuwa salę z rejestru.

### 📅 Rezerwacje (`ReservationsController`)
* `GET /api/reservations` - Zwraca rezerwacje (z opcją filtrowania m.in. po dacie, statusie, ID sali).
* `GET /api/reservations/{id}` - Zwraca konkretną rezerwację.
* `POST /api/reservations` - Generuje nową rezerwację, dbając o walidację konfliktów w kalendarzu sali.
* `PUT /api/reservations/{id}` - Aktualizuje istniejącą rezerwację (z inteligentną walidacją konfliktów czasowych ignorującą samą siebie).
* `DELETE /api/reservations/{id}` - Usuwa rezerwację z systemu.
***
