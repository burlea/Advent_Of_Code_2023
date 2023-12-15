

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

static string GetKey(string currentSprings, List<int> numbers, int currentGroupSize) {
    return currentSprings + "|" + string.Join(",", numbers) + "|" + currentGroupSize;
}
static long recursivelyGetAllCombinations(string currentSprings, List<int> numbers, Dictionary<string,long> solutionsMap, int currentGroupSize){

if (currentSprings.Length == 0){
    if (numbers.Count == 0 && currentGroupSize == 0){
        return 1;
    } else {
        return 0;
    }
} else {
    if (solutionsMap.TryGetValue(GetKey(currentSprings, numbers, currentGroupSize), out long value)){
        return value;
    } else {
        long total = 0;

        char currentSpring = currentSprings[0];

        if (currentSpring == '?'){

            // for '#' future
            total += recursivelyGetAllCombinations(currentSprings[1..], numbers, solutionsMap, currentGroupSize+1);

            //for '.' future
             if (currentGroupSize == 0){
                total += recursivelyGetAllCombinations(currentSprings[1..], numbers, solutionsMap, 0);
            } else {
                if (numbers.Count != 0 && numbers.First() == currentGroupSize){
                    total += recursivelyGetAllCombinations(currentSprings[1..], numbers[1..], solutionsMap, 0);
                }
            }

        } else if (currentSpring == '#') {
            total += recursivelyGetAllCombinations(currentSprings[1..], numbers, solutionsMap, currentGroupSize + 1);

        } else { // a '.'
            if (currentGroupSize == 0){
                total += recursivelyGetAllCombinations(currentSprings[1..], numbers, solutionsMap, 0);
            } else {
                if (numbers.Count != 0 && numbers.First() == currentGroupSize){
                    total += recursivelyGetAllCombinations(currentSprings[1..], numbers[1..], solutionsMap, 0);
                }
            }
        }
        solutionsMap[GetKey(currentSprings, numbers, currentGroupSize)] = total;
        return total;
    }
}
}

static void Task1(string[] input) {

    Dictionary<string, long> solutionsMap = [];
    long totalCombinations = 0;

    for (int i = 0; i < input.Length; i++){
        string[] parts = input[i].Split(' ');

         if (!parts[0].Contains('?')){
            continue;
        }

        List<int> numbers = parts[1].Split(',').ToList().ConvertAll<int>(number => Int32.Parse(number));

        totalCombinations += recursivelyGetAllCombinations(parts[0] + ".", numbers, solutionsMap, 0);
    }

    Console.WriteLine(totalCombinations);
}

static void Task2(string[] input){

    long totalCombinations = 0;
    Dictionary<string, long> solutionsMap = [];

    for (int i = 0; i < input.Length; i++){
        string[] parts = input[i].Split(' ');

        string numberSprings = parts[1];
        string expandedNumberSprings = numberSprings + "," + numberSprings + "," + numberSprings + "," + numberSprings + "," + numberSprings;

        List<int> numbers = expandedNumberSprings.Split(',').ToList().ConvertAll<int>(number => Int32.Parse(number));

        string springs = parts[0];

        string expandedSprings = springs + "?" + springs + "?" + springs + "?" + springs + "?" + springs + ".";


        totalCombinations += recursivelyGetAllCombinations(expandedSprings, numbers, solutionsMap, 0);

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