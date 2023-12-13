

using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

partial class Program
{
    
static void Main()
{
    string[] input = File.ReadAllLines("input.txt");
    Task1(input);
    Task2(input);
}

static List<int> getBrokenSpringsCount(string springs){
    List<int> brokenSprings = [];
    int currentTotalBrokenSprings = 0;

    for (int i = 0; i <springs.Length; i++){
        if (springs[i] == '#'){
            currentTotalBrokenSprings++;
        } else {
            if (currentTotalBrokenSprings != 0){
                brokenSprings.Add(currentTotalBrokenSprings);
                currentTotalBrokenSprings = 0;
            }
        }
    }

    if (currentTotalBrokenSprings != 0){
        brokenSprings.Add(currentTotalBrokenSprings);
    }

    return brokenSprings;
}

static bool SeeIfSolutionCorrect(string springs, List<int> numbers) {

    List<int> brokenSprings = getBrokenSpringsCount(springs);

    if (brokenSprings.Count != numbers.Count){
        return false;
    }

    for (int i = 0; i < brokenSprings.Count; i++){
        if (brokenSprings[i] != numbers[i]){
            return false;
        }
    }

    return true;
}

static long recursivelyGetAllCombinations(string currentSprings, List<int> numbers, Dictionary<string,long> solutionsMap){

if (solutionsMap.TryGetValue(currentSprings, out long value)){
    return value;
}

if (!currentSprings.Contains('?')){
    if (SeeIfSolutionCorrect(currentSprings, numbers)){
        return 1;
    } else {
        return 0;
    }
} else {
    var regex = new Regex(Regex.Escape("?"));

    if (tooManyBroken(currentSprings, numbers)){
        return 0;
    }

    if (notEnoughBroken(currentSprings, numbers)){
        return 0;
    }

    if (tooManyConsecutiveBroken(currentSprings, numbers)){
        return 0;
    }
    
    string nextOneIsBroken = regex.Replace(currentSprings, "#", 1);
    string nextOneIsOperational = regex.Replace(currentSprings, ".", 1);

    long total = recursivelyGetAllCombinations(nextOneIsBroken, numbers, solutionsMap) + recursivelyGetAllCombinations(nextOneIsOperational, numbers, solutionsMap);

    solutionsMap[currentSprings] = total;
    return total;
}
    
}

static bool tooManyBroken(string springs, List<int> numbers){
    return springs.Count(spring => spring == '#') > numbers.Sum();
}

static bool notEnoughBroken(string springs, List<int> numbers){
    return springs.Count(spring => spring == '#' || spring=='?') < numbers.Sum();
}

static bool tooManyConsecutiveBroken(string springs, List<int> numbers){

    List<int> brokenSpringCount = getBrokenSpringsCount(springs);

    if (brokenSpringCount.Count == 0){
        return false;
    }

    return brokenSpringCount.Max() > numbers.Max();
    
}

static void Task1(string[] input) {

    long totalCombinations = 0;

    for (int i = 0; i < input.Length; i++){
        string[] parts = input[i].Split(' ');

         if (!parts[0].Contains('?')){
            continue;
        }

        List<int> numbers = parts[1].Split(',').ToList().ConvertAll<int>(number => Int32.Parse(number));
        Dictionary<string,long> solutionsMap = [];

        totalCombinations += recursivelyGetAllCombinations(parts[0], numbers, solutionsMap);

        // Console.WriteLine("Total So Far: " + totalCombinations);
        // Console.WriteLine("Starting: " +  parts[0]);
    }

    Console.WriteLine(totalCombinations);
}

static void Task2(string[] input){

    long totalCombinations = 0;

    for (int i = 0; i < input.Length; i++){
        string[] parts = input[i].Split(' ');

         if (!parts[0].Contains('?')){
            continue;
        }

        string numberSprings = parts[1];
        string expandedNumberSprings = numberSprings + "," + numberSprings + "," + numberSprings + "," + numberSprings + "," + numberSprings;

        List<int> numbers = expandedNumberSprings.Split(',').ToList().ConvertAll<int>(number => Int32.Parse(number));

        string springs = parts[0];

        string expandedSprings = springs + "?" + springs + "?" + springs + "?" + springs + "?" + springs;

        Dictionary<string,long> solutionsMap = [];

        totalCombinations += recursivelyGetAllCombinations(expandedSprings, numbers, solutionsMap);

        Console.WriteLine("Line: " + i+1);

        // Console.WriteLine("Total So Far: " + totalCombinations);
        // Console.WriteLine("Starting: " +  parts[0]);
    }

    Console.WriteLine(totalCombinations);

}

static void PrintMap(string message, char[,] map) {

    Console.WriteLine(message + ": ");

    for (int i = 0; i < map.GetLength(0); i++){
        for(int j = 0; j < map.GetLength(1); j++) {
            Console.Write(map[i,j]);
        }
        Console.WriteLine("");
    }

    Console.WriteLine("");
}

static void PrintList(string message, List<int> list){
    Console.Write(message + ": ");

    foreach (int num in list){
        Console.Write(num + " ");
    }

    Console.WriteLine("");
}

}