//CREADO POR 
//MARIO ORLANDO SALAMANCA GUZMAN U20210994
//YONATAN AGUSTIN CHICAS AMAYA U20211000
//ULISES ISMAEL QUINTANILLA LOPEZ

using System;
using System.Collections.Generic;
using Spectre.Console;


//TAREA COMPUTO 2 Integrantes Yonatan Agustín Chicas Amaya --Mario Orlando Salamanca Guzmán-- Ulises Ismael Quintanilla Gonzalez
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
        Users.Add(new User("admin", "admin", UserType.Admin, new List<Movie>()));

        // Añadiendo usuarios regulares
        Users.Add(new User("user", "123456", UserType.User, new List<Movie> { new Movie("Inception", false, "Action") }));
        Users.Add(new User("user2@test.com", "123456", UserType.User, new List<Movie> { new Movie("The Matrix", false, "Sci-Fi") }));
        Users.Add(new User("user3@test.com", "123456", UserType.User, new List<Movie> { new Movie("Interstellar", false, "Sci-Fi") }));
        Users.Add(new User("user4@test.com", "123456", UserType.User, new List<Movie> { new Movie("The Big Lebowski", false, "Comedy") }));
        Users.Add(new User("user5@test.com", "123456", UserType.User, new List<Movie> { new Movie("Pulp Fiction", false, "Crime") }));
        Users.Add(new User("user6@test.com", "123456", UserType.User, new List<Movie> { new Movie("The Dark Knight", false, "Action") }));
        Users.Add(new User("user7@test.com", "123456", UserType.User, new List<Movie> { new Movie("Forrest Gump", false, "Drama") }));
        Users.Add(new User("user8@test.com", "123456", UserType.User, new List<Movie> { new Movie("The Shawshank Redemption", false, "Drama") }));
        Users.Add(new User("user9@test.com", "123456", UserType.User, new List<Movie> { new Movie("Fight Club", false, "Drama") }));
        Users.Add(new User("user10@test.com", "123456", UserType.User, new List<Movie> { new Movie("The Godfather", false, "Crime") }));

        // Añadiendo algunas películas de prueba
        Movies.Add(new Movie("The Matrix", false, "Sci-Fi"));
        Movies.Add(new Movie("Inception", true, "Action"));
        Movies.Add(new Movie("Interstellar", false, "Sci-Fi"));
        Movies.Add(new Movie("The Big Lebowski", true, "Comedy"));
    }
    public void Run()
    {
        AnsiConsole.MarkupLine("[bold green]¡Bienvenido al sistema de gestión de películas![/]\n");

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

        AnsiConsole.MarkupLine($"[bold]¡Bienvenido {currentUser.Email}![/]\n");

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
            AnsiConsole.MarkupLine("[yellow]1.[/] Ver todas las películas");
            AnsiConsole.MarkupLine("[yellow]2.[/] Agregar película");
            AnsiConsole.MarkupLine("[yellow]3.[/] Editar película");
            AnsiConsole.MarkupLine("[yellow]4.[/] Separar películas en gratuitas y de pago");
            AnsiConsole.MarkupLine("[yellow]5.[/] Dividir películas en categorías");
            AnsiConsole.MarkupLine("[yellow]6.[/] Ver todas las compras");
            AnsiConsole.MarkupLine("[yellow]7.[/] Ver todos los usuarios");
            AnsiConsole.MarkupLine("[yellow]8.[/] Ver películas compradas por usuarios");
            AnsiConsole.MarkupLine("[yellow]9.[/] Ver películas compradas por usuario específico");
            AnsiConsole.MarkupLine("[yellow]0.[/] Salir\n");

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
                    ShowPurchasedMoviesByUsers();
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
        AnsiConsole.MarkupLine("[bold]Películas compradas por usuarios:[/]");
        foreach (var user in Users)
        {
            if (user.PurchasedMovies.Any())
            {
                AnsiConsole.MarkupLine($"Usuario: [bold]{user.Email}[/]");
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

        AnsiConsole.MarkupLine($"\n[bold]Películas compradas por {userEmail}:[/]");
        foreach (var movie in user.PurchasedMovies)
        {
            AnsiConsole.MarkupLine($"- {movie.Title}");
        }
    }

    private void AddMovie()
    {
        AnsiConsole.MarkupLine("Ingrese el título de la película:");
        string title = Console.ReadLine();
        AnsiConsole.MarkupLine("¿Es gratuita la película? ([green]sí[/]/no):");
        bool isFree = Console.ReadLine().ToLower() == "sí";
        AnsiConsole.MarkupLine("Ingrese la categoría de la película:");
        string category = Console.ReadLine();

        Movies.Add(new Movie(title, isFree, category));
        AnsiConsole.MarkupLine("[bold green]Película agregada exitosamente.[/]");
    }

    private void EditMovie()
    {
        AnsiConsole.MarkupLine("Ingrese el título de la película que desea editar:");
        string title = Console.ReadLine();

        var movie = Movies.Find(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (movie == null)
        {
            AnsiConsole.MarkupLine("[red]La película no existe.[/]");
            return;
        }

        AnsiConsole.MarkupLine("Ingrese el nuevo título de la película:");
        string newTitle = Console.ReadLine();
        AnsiConsole.MarkupLine("¿Es gratuita la película? ([green]sí[/]/no):");
        bool isFree = Console.ReadLine().ToLower() == "sí";
        AnsiConsole.MarkupLine("Ingrese la nueva categoría de la película:");
        string category = Console.ReadLine();

        movie = new Movie(newTitle, isFree, category);
        AnsiConsole.MarkupLine("[bold green]Película editada exitosamente.[/]");
    }

    private void SplitMoviesByFree()
    {
        var freeMovies = Movies.Where(m => m.IsFree).ToList();
        var paidMovies = Movies.Where(m => !m.IsFree).ToList();

        AnsiConsole.MarkupLine("[bold]Películas Gratuitas:[/]");
        foreach (var movie in freeMovies)
        {
            AnsiConsole.MarkupLine($"Título: [italic]{movie.Title}[/], Categoría: [italic]{movie.Category}[/]");
        }

        AnsiConsole.MarkupLine("\n[bold]Películas de Pago:[/]");
        foreach (var movie in paidMovies)
        {
            AnsiConsole.MarkupLine($"Título: [italic]{movie.Title}[/], Categoría: [italic]{movie.Category}[/]");
        }
    }

    private void SplitMoviesByCategory()
    {
        var categories = Movies.Select(m => m.Category).Distinct().ToList();

        AnsiConsole.MarkupLine("[bold]Películas por Categoría:[/]");
        foreach (var category in categories)
        {
            var moviesInCategory = Movies.Where(m => m.Category == category).ToList();
            AnsiConsole.MarkupLine($"\nCategoría: [italic]{category}[/]");
            foreach (var movie in moviesInCategory)
            {
                AnsiConsole.MarkupLine($"- {movie.Title}");
            }
        }
    }

    private void ShowAllMovies()
    {
        var table = new Table().Centered().Border(TableBorder.Minimal);
        table.AddColumn("Título").AddColumn("Gratis").AddColumn("Categoría");

        foreach (var movie in Movies)
        {
            var gratis = movie.IsFree ? "[green]Sí[/]" : "No";
            table.AddRow(movie.Title, gratis, movie.Category);
        }

        AnsiConsole.Render(table);
    }



    private void ShowAllUsers()
    {
        var table = new Table().Centered().Border(TableBorder.Minimal);
        table.AddColumn("Email").AddColumn("Tipo");

        foreach (var user in Users)
        {
            string userType = user.Type == UserType.Admin ? "Admin" : "User";
            table.AddRow(user.Email, userType);
        }

        AnsiConsole.Render(table);
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
        AnsiConsole.MarkupLine("[yellow]1.[/] Ver películas gratis");
        AnsiConsole.MarkupLine("[yellow]2.[/] Ver películas de pago");
        AnsiConsole.MarkupLine("[yellow]3.[/] Ver mis películas compradas");
        AnsiConsole.MarkupLine("[yellow]4.[/] Salir\n");

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

        AnsiConsole.MarkupLine("[bold]Películas disponibles:[/]");
        for (int i = 0; i < filteredMovies.Count; i++)
        {
            AnsiConsole.MarkupLine($"[yellow]{i + 1}.[/] {filteredMovies[i].Title} - {filteredMovies[i].Category}");
        }

        AnsiConsole.MarkupLine("Ingrese el número de la película que desea ver:");
        if (int.TryParse(Console.ReadLine(), out int movieSelection) && movieSelection > 0 && movieSelection <= filteredMovies.Count)
        {
            var selectedMovie = filteredMovies[movieSelection - 1];
            AnsiConsole.MarkupLine($"Disfruta viendo [bold]{selectedMovie.Title}[/]!\n");
        }
        else
        {
            AnsiConsole.MarkupLine("Selección inválida. Intente de nuevo.");
            ShowMovies(isFree);
        }
    }

    private void ShowPurchasedMovies(User currentUser)
    {
        AnsiConsole.MarkupLine("[bold]Mis películas compradas:[/]");
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
