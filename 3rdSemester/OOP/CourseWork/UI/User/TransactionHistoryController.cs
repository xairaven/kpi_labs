﻿using System.Text;
using Shop.Exceptions;
using Shop.Services;
using Shop.UI.Exit;
using Shop.UI.Interfaces;

namespace Shop.UI.User;

public class TransactionHistoryController : IUserInterface
{
    private readonly UserService _userService;
    
    private List<IUserInterface> UIs { get; }

    public TransactionHistoryController(UserService userService)
    {
        _userService = userService;
        
        UIs = new List<IUserInterface>();
        
        UIs.Add(new ExitController());
    }
    
    public string Message()
    {
        return "View transaction history";
    }

    public void Action()
    {
        while (true)
        {
            try
            {
                int choice = Switch();
                UIs[choice - 1].Action();
            }
            catch (ExitException) { break; }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private void Show()
    {
        Console.WriteLine("-- Account transaction history --");
        
        var history = _userService.CurrentUser.History;

        if (history.Count == 0)
        {
            Console.WriteLine("Sorry, but transaction list is empty");
            return;
        }
        
        foreach (var transaction in history)
        {
            Console.WriteLine(transaction + "\n");
        }
    }
    
    private int Switch()
    {
        int choice;
        while (true)
        {
            Show();
            Console.WriteLine(Menu());
            Console.Write("Your choice: ");

            int.TryParse(Console.ReadLine(), out choice);

            if (choice < 1 || choice > UIs.Count)
            {
                Console.WriteLine("\nWrong choice! There isn't variant like that.");
            } else break;
        }

        return choice;
    }
    
    private string Menu()
    {
        var sb = new StringBuilder();

        int menuItem = 1;
        foreach (var controller in UIs)
        {
            sb.Append($"{menuItem,2}: {controller.Message()}");
            if (menuItem != UIs.Count) sb.Append('\n'); 
            
            menuItem++;
        }

        return sb.ToString();
    }
}