using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.RegularExpressions;


namespace Day14
{
    partial class Program
  {
    static void Main()
    {

    string[] input = File.ReadAllLines("input.txt");

    Task1(input);
    Task2(input);
    }

    static int GetHash(string step){
        int result = 0;

        foreach(char character in step){
            int ascii = character;
            result += ascii;
            result *= 17;
            result %= 256;
        }

        return result;
    }

    static void Task1( string[] input) {

        string inputString = input[0];
        string [] steps = inputString.Split(",");

        int sum = 0;

        for (int i = 0; i < steps.Length; i++){
            sum += GetHash(steps[i]);
        }

        Console.WriteLine(sum);
    }

    static void Task2( string[] input){

        string inputString = input[0];
        string [] steps = inputString.Split(",");

        Dictionary<int, List<Tuple<string,int>>> boxes = [];

        for (int i = 0; i < 256; i++){
            boxes[i] = [];
        }

        for (int i = 0; i < steps.Length; i++){
            AddStep(boxes, steps[i]);
        }

        long totalFocusingPower = 0;

        for (int i = 0; i < 256; i++){
            List<Tuple<string,int>> lenses = boxes[i];

            for(int j = 0; j < lenses.Count; j++){
                long focusingPower = 1;
                focusingPower *= 1 + i;
                focusingPower *= 1 + j;
                focusingPower *= lenses[j].Item2;
                totalFocusingPower += focusingPower;
            }
        }

        Console.WriteLine(totalFocusingPower);
    }


    static string GetLabel(string step){
        return new string(step.Where(Char.IsLetter).ToArray());
    }

    static void AddStep(Dictionary<int, List<Tuple<string,int>>> boxes, string step){
        string label = GetLabel(step);
        int box = GetHash(label);

        if (step.Contains('-')){
            List<Tuple<string,int>> lenses = boxes[box];

            int index = lenses.FindIndex(s => s.Item1.Equals(label));

            if (index != -1){
                lenses.RemoveAt(index);
            }

             boxes[box] = lenses;
        }

        if (step.Contains('=')){
            int focalLength = Int32.Parse(MyRegex().Match(step).Value);

            List<Tuple<string,int>> lenses = boxes[box];
            
            int index = lenses.FindIndex(s => s.Item1.Equals(label));

            if (index < 0){
                lenses.Add(Tuple.Create(label,focalLength));
            } else {
                lenses[index] = Tuple.Create(label,focalLength);
            }

            boxes[box] = lenses;
        }
    }

  static void PrintSteps(string message, string[] steps) {

    Console.WriteLine(message + ": ");

    for (int i = 0; i < steps.Length; i++){
        Console.Write(steps[i] + " ");
    }

    Console.WriteLine("");
}

  static void PrintDictionary(string message, Dictionary<int, List<Tuple<string,int>>> boxes) {

    Console.WriteLine(message + ": ");

    for (int i = 0; i < 256; i++){

        List<Tuple<string,int>> lenses = boxes[i];

        Console.WriteLine("Box: " + i);

        for (int j = 0; j < lenses.Count; j++){
            Console.Write(lenses[j].Item1 + " => " + lenses[j].Item2 + " |");
        }

        Console.WriteLine("");
    }
}

        [GeneratedRegex(@"\d+")]
        private static partial Regex MyRegex();
    }
}