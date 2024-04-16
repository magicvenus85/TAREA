using System;
using System.Collections.Generic;
using System.Linq;

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
        Console.WriteLine("Ingrese su correo electrónico:");
        string email = Console.ReadLine();
        Console.WriteLine("Ingrese su contraseña:");
        string password = Console.ReadLine();

        User currentUser = Users.Find(user => user.Email == email && user.Password == password);

        if (currentUser == null)
        {
            Console.WriteLine("Usuario o contraseña incorrectos.");
            return;
        }

        Console.WriteLine($"Bienvenido {currentUser.Email}!");

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
            Console.WriteLine("Menú Administrador:");
            Console.WriteLine("1. Ver todas las películas");
            Console.WriteLine("2. Agregar película");
            Console.WriteLine("3. Editar película");
            Console.WriteLine("4. Separar películas en gratuitas y de pago");
            Console.WriteLine("5. Dividir películas en categorías");
            Console.WriteLine("6. Ver todas las compras");
            Console.WriteLine("7. Ver todos los usuarios");
            Console.WriteLine("8. Ver películas compradas por usuarios");
            Console.WriteLine("9. Ver películas compradas por usuario específico");
            Console.WriteLine("0. Salir");

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
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    break;
            }
        }
    }

    private void ShowPurchasedMoviesByUsers()
    {
        Console.WriteLine("Películas compradas por usuarios:");
        foreach (var user in Users)
        {
            if (user.PurchasedMovies.Any())
            {
                Console.WriteLine($"Usuario: {user.Email}");
                foreach (var movie in user.PurchasedMovies)
                {
                    Console.WriteLine($"- {movie.Title}");
                }
            }
        }
    }

    private void ShowPurchasedMoviesByUser()
    {
        Console.WriteLine("Ingrese el correo electrónico del usuario:");
        string userEmail = Console.ReadLine();

        var user = Users.Find(u => u.Email == userEmail);
        if (user == null)
        {
            Console.WriteLine("Usuario no encontrado.");
            return;
        }

        Console.WriteLine($"Películas compradas por {userEmail}:");
        foreach (var movie in user.PurchasedMovies)
        {
            Console.WriteLine($"- {movie.Title}");
        }
    }

    private void AddMovie()
    {
        Console.WriteLine("Ingrese el título de la película:");
        string title = Console.ReadLine();
        Console.WriteLine("¿Es gratuita la película? (sí/no):");
        bool isFree = Console.ReadLine().ToLower() == "sí";
        Console.WriteLine("Ingrese la categoría de la película:");
        string category = Console.ReadLine();

        Movies.Add(new Movie(title, isFree, category));
        Console.WriteLine("Película agregada exitosamente.");
    }

    private void EditMovie()
    {
        Console.WriteLine("Ingrese el título de la película que desea editar:");
        string title = Console.ReadLine();

        var movie = Movies.Find(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (movie == null)
        {
            Console.WriteLine("La película no existe.");
            return;
        }

        Console.WriteLine("Ingrese el nuevo título de la película:");
        string newTitle = Console.ReadLine();
        Console.WriteLine("¿Es gratuita la película? (sí/no):");
        bool isFree = Console.ReadLine().ToLower() == "sí";
        Console.WriteLine("Ingrese la nueva categoría de la película:");
        string category = Console.ReadLine();

        movie = new Movie(newTitle, isFree, category);
        Console.WriteLine("Película editada exitosamente.");
    }

    private void SplitMoviesByFree()
    {
        var freeMovies = Movies.Where(m => m.IsFree).ToList();
        var paidMovies = Movies.Where(m => !m.IsFree).ToList();

        Console.WriteLine("Películas Gratuitas:");
        foreach (var movie in freeMovies)
        {
            Console.WriteLine($"Título: {movie.Title}, Categoría: {movie.Category}");
        }

        Console.WriteLine("\nPelículas de Pago:");
        foreach (var movie in paidMovies)
        {
            Console.WriteLine($"Título: {movie.Title}, Categoría: {movie.Category}");
        }
    }

    private void SplitMoviesByCategory()
    {
        var categories = Movies.Select(m => m.Category).Distinct().ToList();

        Console.WriteLine("Películas por Categoría:");
        foreach (var category in categories)
        {
            var moviesInCategory = Movies.Where(m => m.Category == category).ToList();
            Console.WriteLine($"\nCategoría: {category}");
            foreach (var movie in moviesInCategory)
            {
                Console.WriteLine($"Título: {movie.Title}");
            }
        }
    }

    private void ShowAllMovies()
    {
        foreach (var movie in Movies)
        {
            Console.WriteLine($"Título: {movie.Title}, Gratis: {movie.IsFree}, Categoría: {movie.Category}");
        }
    }

    private void ShowAllUsers()
    {
        foreach (var user in Users)
        {
            Console.WriteLine($"Email: {user.Email}, Tipo: {user.Type}");
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
        Console.WriteLine("Seleccione una opción:");
        Console.WriteLine("1. Ver películas gratis");
        Console.WriteLine("2. Ver películas de paga");
        Console.WriteLine("3. Ver mis películas compradas");
        Console.WriteLine("4. Salir");

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
                Console.WriteLine("Opción no válida. Intente de nuevo.");
                return true;
        }
    }

    private void ShowMovies(bool isFree)
    {
        var filteredMovies = Movies.Where(movie => movie.IsFree == isFree).ToList();
        if (!filteredMovies.Any())
        {
            Console.WriteLine("No hay películas disponibles en esta categoría.");
            return;
        }

        Console.WriteLine("Películas disponibles:");
        for (int i = 0; i < filteredMovies.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {filteredMovies[i].Title} - {filteredMovies[i].Category}");
        }

        Console.WriteLine("Ingrese el número de la película que desea ver:");
        if (int.TryParse(Console.ReadLine(), out int movieSelection) && movieSelection > 0 && movieSelection <= filteredMovies.Count)
        {
            var selectedMovie = filteredMovies[movieSelection - 1];
            Console.WriteLine($"Disfruta viendo {selectedMovie.Title}!");
        }
        else
        {
            Console.WriteLine("Selección inválida. Intente de nuevo.");
            ShowMovies(isFree);
        }
    }

    private void ShowPurchasedMovies(User currentUser)
    {
        Console.WriteLine("Mis películas compradas:");
        foreach (var movie in currentUser.PurchasedMovies)
        {
            Console.WriteLine($"- {movie.Title}");
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
