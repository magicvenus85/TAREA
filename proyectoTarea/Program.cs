﻿using System;
using System.Collections.Generic;

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
        // usuarios de pruebaxd
        Users.Add(new User("admin@test.com", "admin123", UserType.Admin));
        for (int i = 1; i <= 10; i++)
        {
            Users.Add(new User($"user{i}@test.com", $"user{i}123", UserType.User));
        }
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

        if (currentUser.Type == UserType.Admin)
        {
            // Muestra el menú del administrador
        }
        else
        {
            // Muestra el menú del usuario
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
