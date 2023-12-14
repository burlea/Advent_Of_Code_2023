

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
    //Task2(input);
}

// static List<int> getBrokenSpringsCount(string springs){
//     List<int> brokenSprings = [];
//     int currentTotalBrokenSprings = 0;

//     for (int i = 0; i <springs.Length; i++){
//         if (springs[i] == '#'){
//             currentTotalBrokenSprings++;
//         } else {
//             if (currentTotalBrokenSprings != 0){
//                 brokenSprings.Add(currentTotalBrokenSprings);
//                 currentTotalBrokenSprings = 0;
//             }
//         }
//     }

//     if (currentTotalBrokenSprings != 0){
//         brokenSprings.Add(currentTotalBrokenSprings);
//     }

//     return brokenSprings;
// }

// static bool SeeIfSolutionCorrect(string springs, List<int> numbers) {

//     List<int> brokenSprings = getBrokenSpringsCount(springs);

//     if (brokenSprings.Count != numbers.Count){
//         return false;
//     }

//     for (int i = 0; i < brokenSprings.Count; i++){
//         if (brokenSprings[i] != numbers[i]){
//             return false;
//         }
//     }

//     return true;
// }

static long recursivelyGetAllCombinations(string currentSprings, List<int> numbers, Dictionary<Tuple<string, int>,long> solutionsMap, int currentGroupSize){

Console.WriteLine("Current Springs: " + currentSprings + " Current Group Size: " + currentGroupSize);

if (currentSprings.Length == 0){
    if (numbers.Count == 0 && currentGroupSize == 0){
        return 1;
    } else {
        return 0;
    }
} else {
    if (solutionsMap.TryGetValue(Tuple.Create(currentSprings, currentGroupSize), out long value)){
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
        solutionsMap[Tuple.Create(currentSprings, currentGroupSize)] = total;
        return total;
    }
}
}

static void Task1(string[] input) {

    long totalCombinations = 0;

    for (int i = 0; i < input.Length; i++){
        string[] parts = input[i].Split(' ');

         if (!parts[0].Contains('?')){
            continue;
        }

        List<int> numbers = parts[1].Split(',').ToList().ConvertAll<int>(number => Int32.Parse(number));
        Dictionary<Tuple<string, int>, long> solutionsMap = [];

        totalCombinations += recursivelyGetAllCombinations(parts[0] + ".", numbers, solutionsMap, 0);

        // Console.WriteLine("Total So Far: " + totalCombinations);
        // Console.WriteLine("Starting: " +  parts[0]);
    }

    Console.WriteLine(totalCombinations);
}

static void Task2(string[] input){

    long totalCombinations = 0;

    for (int i = 0; i < input.Length; i++){
        string[] parts = input[i].Split(' ');

        string numberSprings = parts[1];
        string expandedNumberSprings = numberSprings + "," + numberSprings + "," + numberSprings + "," + numberSprings + "," + numberSprings;

        List<int> numbers = expandedNumberSprings.Split(',').ToList().ConvertAll<int>(number => Int32.Parse(number));

        string springs = parts[0];

        string expandedSprings = springs + "?" + springs + "?" + springs + "?" + springs + "?" + springs + ".";

        Dictionary<Tuple<string, int>,long> solutionsMap = [];

        totalCombinations += recursivelyGetAllCombinations(expandedSprings, numbers, solutionsMap, 0);

        Console.WriteLine("Line: " + (i+1));

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