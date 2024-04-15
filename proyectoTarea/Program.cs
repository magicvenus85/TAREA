using System;
using System.Collections.Generic;
using System.Linq;

public enum UserType
{
    Admin,
    User
}

public record User(string Email, string Password, UserType Type);

public record Movie(string Title, bool IsFree, string Category);

public class MovieWebsite
{
    private List<User> Users = new();
    private List<Movie> Movies = new();

    public MovieWebsite()
    {
        // usuarios
        Users.Add(new User("admin@test.com", "admin123", UserType.Admin));
        for (int i = 1; i <= 10; i++)
        {
            Users.Add(new User($"user{i}@test.com", $"user{i}123", UserType.User));
        }

        // películas de prueba
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

        // Mantener el menú activo después del inicio de sesión
        bool continueRunning = true;
        while (continueRunning)
        {
            continueRunning = ShowMovieMenu();
        }
    }

    private bool ShowMovieMenu()
    {
        Console.WriteLine("Seleccione una opción:");
        Console.WriteLine("1. Ver películas gratis");
        Console.WriteLine("2. Ver películas de paga");
        Console.WriteLine("3. Salir");

        string option = Console.ReadLine();
        switch (option)
        {
            case "1":
                ShowMovies(true); // Muestra películas gratis
                return true; // Continúa en el menú
            case "2":
                ShowMovies(false); // Muestra películas de pago
                return true; // Continúa en el menú
            case "3":
                return false; // Salir del programa
            default:
                Console.WriteLine("Opción no válida. Intente de nuevo.");
                return true; // Opción inválida, repite el menú
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
            ShowMovies(isFree); // Repite si la selección es inválida
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
