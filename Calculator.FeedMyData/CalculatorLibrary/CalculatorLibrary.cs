using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CalculatorLibrary;

public class Calculator
{
  public int useCount = 0;
  JsonWriter writer;
  public List<OperationLog> history = new();

  public Calculator()
  {
    StreamWriter logFile = File.CreateText("calculatorlog.json");
    logFile.AutoFlush = true;
    writer = new JsonTextWriter(logFile);
    writer.Formatting = Formatting.Indented;
  }

  public double DoOperation(double num1, double num2, string op)
  {
    double result = double.NaN;
    string operation = "";

    switch (op)
    {
      case "a":
        operation = "Add";
        result = num1 + num2;
        break;
      case "s":
        operation = "Substract";
        result = num1 - num2;
        break;
      case "m":
        operation = "Multiply";
        result = num1 * num2;
        break;
      case "d":
        if (num2 != 0)
        {
          operation = "Divide";
          result = num1 / num2;
        }
        break;
      case "p":
        operation = "Power";
        result = Math.Pow(num1, num2);
        break;
      case "sr":
        operation = "Square Root";
        result = Math.Sqrt(num1);
        break;
      case "sin":
        operation = "Sin";
        result = Math.Sin(num1);
        break;
      case "ex":
        operation = "Exponent";
        result = num1 * Math.Pow(10, num2);
        break;
      default:
        break;
    }

    if (!Regex.IsMatch(op, "^(sr|sin)$"))
    {
      OperationLog newLog = new(operation, result, num1, num2);
      history.Add(newLog);
    }

    else
    {
      OperationLog newLog = new(operation, result, num1);
      history.Add(newLog);
    }

    useCount++;
    return result;
  }

  public void Finish()
  {
    writer.WriteStartObject();
    writer.WritePropertyName("CalculatorUsage");
    writer.WriteValue(useCount);
    writer.WritePropertyName("Operations");
    writer.WriteStartArray();

    foreach (OperationLog operation in history)
    {
      writer.WriteValue(operation.Display());
      Console.WriteLine(operation.Display());
    }

    writer.WriteEndArray();
    writer.WriteEndObject();
    writer.Close();
  }
}

public class OperationLog
{
  private static int itemCount = 1;
  private int operationNumber;
  public string ID { get; }
  public double Operand1 { get; }
  public double Operand2 { get; }
  public string Operation { get; }
  public double Result { get; }
  bool singleNumberOperation;

  public OperationLog(string operation, double result, double operand1, double operand2)
  {
    singleNumberOperation = false;
    operationNumber = itemCount++;
    ID = operationNumber.ToString("D3");
    ID = $"[{ID}]";
    Operand1 = operand1;
    Operand2 = operand2;
    Operation = operation;
    Result = result;
  }

  public OperationLog(string operation, double result, double operand1)
  {
    singleNumberOperation = true;
    operationNumber = itemCount++;
    ID = operationNumber.ToString("D3");
    ID = $"[{ID}]";
    Operand1 = operand1;
    Operation = operation;
    Result = result;
  }

  public string Display()
  {
    if (!singleNumberOperation)
      return $"{ID} {Operand1} {Operation} {Operand2} = {Result}";

    else
      return $"{ID} {Operand1} {Operation} = {Result}";
  }
}