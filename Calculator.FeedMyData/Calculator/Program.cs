// Challenge instructions
// DONE - Create a functionality that will count the amount of times the calculator was used.
// DONE - Store a list with the latest calculations. And give the users the ability to delete that list.
// DONE - Allow the users to use the results in the list above to perform new calculations.
// DONE - Add extra calculations: Square Root, Taking the Power, 10x, Trigonometry functions.

using System.Text.RegularExpressions;
using CalculatorLibrary;

class Program
{
  static Calculator calculator = new();

  static void Main(string[] args)
  {
    bool endApp = false;
    Console.WriteLine("Console Calculator in C#\r");
    Console.WriteLine("------------------------\n");

    while (!endApp)
    {
      Console.WriteLine("Choose an operation to perform from the following list:");
      Console.WriteLine("\ta - Add");
      Console.WriteLine("\ts - Subtract");
      Console.WriteLine("\tm - Multiply");
      Console.WriteLine("\td - Divide");
      Console.WriteLine("\tp - Power");
      Console.WriteLine("\tex - Exponent");
      Console.WriteLine("\tsr - Square Root");
      Console.WriteLine("\tsin - Sin");

      if (calculator.History.Count() >= 1)
      {
        Console.WriteLine("\t-------");
        Console.WriteLine("\th - Display calculations History");
        Console.WriteLine("\thdel - Delete calculations History");
      }

      string op = "";
      while (op == "" || !Regex.IsMatch(op, "^(a|s|m|d|p|ex|sr|sin|h|hdel)$"))
      {
        Console.WriteLine("Your choice?");
        op = Console.ReadLine().Trim().ToLower();
      }

      if (Regex.IsMatch(op, "^(h)$"))
        HistoryMenu();

      else if (Regex.IsMatch(op, "^(hdel)$"))
      {
        calculator.ClearHistory();
        Console.WriteLine("All previous calculations were successfully deleted.");
      }

      else
        GetNumbers(op);

      Console.WriteLine("------------------------\n");
      Console.Write("Enter 'exit' to close the app, or press any other key and Enter to continue: ");
      if (Console.ReadLine() == "exit")
        endApp = true;

      Console.WriteLine("\n");
    }

    calculator.Finish();
    return;
  }

  static void HistoryMenu()
  {
    Console.WriteLine("------------------------");
    Console.WriteLine("-------- History -------");
    Console.WriteLine("------------------------");
    Console.WriteLine("Type an '[ID]' with brackets instead of a number to use a result in a new calculation.");
    foreach (OperationLog operation in calculator.History)
    {
      Console.WriteLine(operation.Display());
    }
  }

  static void GetNumbers(string op)
  {
    double cleanNum1 = 0;
    double cleanNum2 = 0;
    double result = 0;

    Console.Write("Type a number or [ID], and then press Enter: ");
    cleanNum1 = ParseInput(Console.ReadLine());

    if (!Regex.IsMatch(op, "^(sr|sin)$"))
    {
      Console.Write("Type another number or [ID], and then press Enter: ");
      cleanNum2 = ParseInput(Console.ReadLine());
    }
    try
    {
      result = calculator.DoOperation(cleanNum1, cleanNum2, op);
      if (double.IsNaN(result))
      {
        Console.WriteLine("This operation will result in a mathematical error.\n");
      }
      else Console.WriteLine("Your result: {0:0.##}\n", result);
    }
    catch (Exception e)
    {
      Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
    }
  }

  static double ParseInput(string input)
  {
    double cleanNumber;

    while (!double.TryParse(input, out cleanNumber))
    {
      if (input.StartsWith('[') && input.Trim().EndsWith(']') && input.Trim().Length == 5)
        foreach (OperationLog operation in calculator.History)
          if (input.Trim() == operation.ID)
            return operation.Result;

      Console.Write("This is not valid input. Please enter a numeric value: ");
      input = Console.ReadLine();
    }
    return cleanNumber;
  }
}

