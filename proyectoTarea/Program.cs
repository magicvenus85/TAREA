using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

public enum UserType
{
    Admin,
    User
}

public record User(string Email, string Password, UserType Type, List<Movie> PurchasedMovies);

public record Movie(string Title, bool IsFree, string Category);

public class MovieWebsite
{
    private List<User> Users = new();
    private List<Movie> Movies = new();

    public MovieWebsite()
    {
        // Añadiendo un usuario administrador especial
        Users.Add(new User("admin@gmail.com", "123456", UserType.Admin, new List<Movie>()));

        // Añadiendo usuarios regulares
        for (int i = 1; i <= 20; i++) // Agregando hasta 20 para incluir los 10 adicionales
        {
            Users.Add(new User($"user{i}@test.com", "123456", UserType.User, new List<Movie> { new Movie("Megamente", false, "Animation") }));
        }

        // Añadiendo algunas películas de prueba
        Movies.Add(new Movie("The Matrix", false, "Sci-Fi"));
        Movies.Add(new Movie("Inception", true, "Action"));
        Movies.Add(new Movie("Interstellar", false, "Sci-Fi"));
        Movies.Add(new Movie("The Big Lebowski", true, "Comedy"));
    }

    public void Run()
    {
        AnsiConsole.MarkupLine("Ingrese su correo electrónico:");
        string email = Console.ReadLine();
        AnsiConsole.MarkupLine("Ingrese su contraseña:");
        string password = Console.ReadLine();

        User currentUser = Users.Find(user => user.Email == email && user.Password == password);

        if (currentUser == null)
        {
            AnsiConsole.MarkupLine("[red]Usuario o contraseña incorrectos.[/]");
            return;
        }

        AnsiConsole.MarkupLine($"Bienvenido [bold]{currentUser.Email}[/]!");

        if (currentUser.Type == UserType.Admin)
        {
            AdminMenu();
        }
        else
        {
            UserMenu(currentUser);
        }
    }

    private void AdminMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            AnsiConsole.MarkupLine("[bold]Menú Administrador:[/]");
            AnsiConsole.MarkupLine("1. Ver todas las películas");
            AnsiConsole.MarkupLine("2. Agregar película");
            AnsiConsole.MarkupLine("3. Editar película");
            AnsiConsole.MarkupLine("4. Separar películas en gratuitas y de pago");
            AnsiConsole.MarkupLine("5. Dividir películas en categorías");
            AnsiConsole.MarkupLine("6. Ver todas las compras");
            AnsiConsole.MarkupLine("7. Ver todos los usuarios");
            AnsiConsole.MarkupLine("8. Ver películas compradas por usuarios");
            AnsiConsole.MarkupLine("9. Ver películas compradas por usuario específico");
            AnsiConsole.MarkupLine("0. Salir");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ShowAllMovies();
                    break;
                case "2":
                    AddMovie();
                    break;
                case "3":
                    EditMovie();
                    break;
                case "4":
                    SplitMoviesByFree();
                    break;
                case "5":
                    SplitMoviesByCategory();
                    break;
                case "6":
                    // Lógica para ver todas las compras
                    break;
                case "7":
                    ShowAllUsers();
                    break;
                case "8":
                    ShowPurchasedMoviesByUsers();
                    break;
                case "9":
                    ShowPurchasedMoviesByUser();
                    break;
                case "0":
                    continueRunning = false;
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Opción no válida. Intente de nuevo.[/]");
                    break;
            }
        }
    }

    private void ShowPurchasedMoviesByUsers()
    {
        AnsiConsole.MarkupLine("Películas compradas por usuarios:");
        foreach (var user in Users)
        {
            if (user.PurchasedMovies.Any())
            {
                AnsiConsole.MarkupLine($"Usuario: {user.Email}");
                foreach (var movie in user.PurchasedMovies)
                {
                    AnsiConsole.MarkupLine($"- {movie.Title}");
                }
            }
        }
    }

    private void ShowPurchasedMoviesByUser()
    {
        AnsiConsole.MarkupLine("Ingrese el correo electrónico del usuario:");
        string userEmail = Console.ReadLine();

        var user = Users.Find(u => u.Email == userEmail);
        if (user == null)
        {
            AnsiConsole.MarkupLine("Usuario no encontrado.");
            return;
        }

        AnsiConsole.MarkupLine($"Películas compradas por {userEmail}:");
        foreach (var movie in user.PurchasedMovies)
        {
            AnsiConsole.MarkupLine($"- {movie.Title}");
        }
    }

    private void AddMovie()
    {
        AnsiConsole.MarkupLine("Ingrese el título de la película:");
        string title = Console.ReadLine();
        AnsiConsole.MarkupLine("¿Es gratuita la película? (sí/no):");
        bool isFree = Console.ReadLine().ToLower() == "sí";
        AnsiConsole.MarkupLine("Ingrese la categoría de la película:");
        string category = Console.ReadLine();

        Movies.Add(new Movie(title, isFree, category));
        AnsiConsole.MarkupLine("Película agregada exitosamente.");
    }

    private void EditMovie()
    {
        AnsiConsole.MarkupLine("Ingrese el título de la película que desea editar:");
        string title = Console.ReadLine();

        var movie = Movies.Find(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (movie == null)
        {
            AnsiConsole.MarkupLine("La película no existe.");
            return;
        }

        AnsiConsole.MarkupLine("Ingrese el nuevo título de la película:");
        string newTitle = Console.ReadLine();
        AnsiConsole.MarkupLine("¿Es gratuita la película? (sí/no):");
        bool isFree = Console.ReadLine().ToLower() == "sí";
        AnsiConsole.MarkupLine("Ingrese la nueva categoría de la película:");
        string category = Console.ReadLine();

        movie = new Movie(newTitle, isFree, category);
        AnsiConsole.MarkupLine("Película editada exitosamente.");
    }

    private void SplitMoviesByFree()
    {
        var freeMovies = Movies.Where(m => m.IsFree).ToList();
        var paidMovies = Movies.Where(m => !m.IsFree).ToList();

        AnsiConsole.MarkupLine("Películas Gratuitas:");
        foreach (var movie in freeMovies)
        {
            AnsiConsole.MarkupLine($"Título: {movie.Title}, Categoría: {movie.Category}");
        }

        AnsiConsole.MarkupLine("\nPelículas de Pago:");
        foreach (var movie in paidMovies)
        {
            AnsiConsole.MarkupLine($"Título: {movie.Title}, Categoría: {movie.Category}");
        }
    }

    private void SplitMoviesByCategory()
    {
        var categories = Movies.Select(m => m.Category).Distinct().ToList();

        AnsiConsole.MarkupLine("Películas por Categoría:");
        foreach (var category in categories)
        {
            var moviesInCategory = Movies.Where(m => m.Category == category).ToList();
            AnsiConsole.MarkupLine($"\nCategoría: {category}");
            foreach (var movie in moviesInCategory)
            {
                AnsiConsole.MarkupLine($"Título: {movie.Title}");
            }
        }
    }

    private void ShowAllMovies()
    {
        var table = new Table().Centered().Border(TableBorder.Minimal);
        table.AddColumn("Título").AddColumn("Gratis").AddColumn("Categoría");

        foreach (var movie in Movies)
        {
            table.AddRow(movie.Title, movie.IsFree.ToString(), movie.Category);
        }

        AnsiConsole.Render(table);
    }

    private void ShowAllUsers()
    {
        foreach (var user in Users)
        {
            AnsiConsole.MarkupLine($"Email: {user.Email}, Tipo: {user.Type}");
        }
    }

    private void UserMenu(User currentUser)
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            continueRunning = ShowMovieMenu(currentUser);
        }
    }

    private bool ShowMovieMenu(User currentUser)
    {
        AnsiConsole.MarkupLine("Seleccione una opción:");
        AnsiConsole.MarkupLine("1. Ver películas gratis");
        AnsiConsole.MarkupLine("2. Ver películas de paga");
        AnsiConsole.MarkupLine("3. Ver mis películas compradas");
        AnsiConsole.MarkupLine("4. Salir");

        string option = Console.ReadLine();
        switch (option)
        {
            case "1":
                ShowMovies(true); // Muestra películas gratis
                return true;
            case "2":
                ShowMovies(false); // Muestra películas de pago
                return true;
            case "3":
                ShowPurchasedMovies(currentUser); // Muestra películas compradas por el usuario
                return true;
            case "4":
                return false; // Salir del programa
            default:
                AnsiConsole.MarkupLine("[red]Opción no válida. Intente de nuevo.[/]");
                return true;
        }
    }

    private void ShowMovies(bool isFree)
    {
        var filteredMovies = Movies.Where(movie => movie.IsFree == isFree).ToList();
        if (!filteredMovies.Any())
        {
            AnsiConsole.MarkupLine("No hay películas disponibles en esta categoría.");
            return;
        }

        AnsiConsole.MarkupLine("Películas disponibles:");
        for (int i = 0; i < filteredMovies.Count; i++)
        {
            AnsiConsole.MarkupLine($"{i + 1}. {filteredMovies[i].Title} - {filteredMovies[i].Category}");
        }

        AnsiConsole.MarkupLine("Ingrese el número de la película que desea ver:");
        if (int.TryParse(Console.ReadLine(), out int movieSelection) && movieSelection > 0 && movieSelection <= filteredMovies.Count)
        {
            var selectedMovie = filteredMovies[movieSelection - 1];
            AnsiConsole.MarkupLine($"Disfruta viendo {selectedMovie.Title}!");
        }
        else
        {
            AnsiConsole.MarkupLine("Selección inválida. Intente de nuevo.");
            ShowMovies(isFree);
        }
    }

    private void ShowPurchasedMovies(User currentUser)
    {
        AnsiConsole.MarkupLine("Mis películas compradas:");
        foreach (var movie in currentUser.PurchasedMovies)
        {
            AnsiConsole.MarkupLine($"- {movie.Title}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        MovieWebsite movieWebsite = new MovieWebsite();
        movieWebsite.Run();
    }
}
