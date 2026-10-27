using System.Globalization;
using System.Text;
using BankAccountNS;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
Console.Title = "PocketBank";
Console.WriteLine("POCKET BANK · Личный счёт");
Console.Write("Как к вам обращаться? ");
var name = Console.ReadLine();
if (name is null) return;
var account = new BankAccount(string.IsNullOrWhiteSpace(name) ? "Пользователь" : name.Trim(), 0);
var operations = new List<string>();

while (true)
{
    Console.WriteLine($"\n{account.CustomerName} · Баланс: {account.Balance:F2} ₽");
    Console.WriteLine("1 — Пополнить  |  2 — Снять  |  3 — История  |  0 — Выход");
    Console.Write("Ваш выбор: ");
    var choice = Console.ReadLine();
    if (choice is null or "0") break;
    if (choice == "3")
    {
        Console.WriteLine(operations.Count == 0 ? "Операций пока нет." : string.Join(Environment.NewLine, operations));
        continue;
    }
    if (choice is not ("1" or "2"))
    {
        Console.WriteLine("Выберите пункт от 0 до 3.");
        continue;
    }

    Console.Write("Сумма (например, 150,50): ");
    var input = Console.ReadLine();
    if (input is null) break;
    if (!double.TryParse(input.Trim().Replace(',', '.'), NumberStyles.AllowDecimalPoint,
        CultureInfo.InvariantCulture, out var amount) || !double.IsFinite(amount) || amount <= 0)
    {
        Console.WriteLine("Введите положительное число без пробелов и знаков валюты.");
        continue;
    }
    amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    if (amount == 0)
    {
        Console.WriteLine("Минимальная сумма — 0,01 ₽.");
        continue;
    }
    if (choice == "2" && amount > account.Balance)
    {
        Console.WriteLine("На счёте недостаточно средств.");
        continue;
    }
    if (choice == "1" && !double.IsFinite(account.Balance + amount))
    {
        Console.WriteLine("Сумма слишком велика.");
        continue;
    }

    if (choice == "1") account.Credit(amount);
    else account.Debit(amount);
    var action = choice == "1" ? "Пополнение" : "Снятие";
    operations.Add($"{DateTime.Now:dd.MM HH:mm} · {action}: {amount:F2} ₽ · Остаток: {account.Balance:F2} ₽");
    Console.WriteLine("Готово.");
}

Console.WriteLine("До встречи!");
