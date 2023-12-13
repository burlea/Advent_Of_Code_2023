

using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Text;
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

static char[][] GetMap(List<string> mapStrings){
    char[][] map = new char[mapStrings.Count][];

    for (int i = 0; i < map.Length; i++){
        map[i] = mapStrings[i].ToCharArray();
    }

    return map;
}

static int GetLinesBeforeVertical(List<string> mapStrings){

    //PrintList(message: "GetLinesBeforeVertical", mapStrings);


    char [][] map = GetMap(mapStrings);
    char [,] flippedMap = new char[map[0].Length,map.Length];

    for (int i = 0; i < map.Length; i++){
        for (int j = 0; j < map[0].Length; j++){
            flippedMap[j,i] = map[i][j];
        }
    }

    List<string> flippedMapStrings = [];

    for (int i = 0; i < flippedMap.GetLength(0);i++){
        var builder = new StringBuilder();

        for (int j = 0; j < flippedMap.GetLength(1);j++){
            builder.Append(flippedMap[i,j]);
        }
        
        flippedMapStrings.Add(builder.ToString());
    }



    return GetLinesAboveHorizontal(flippedMapStrings);
}

static int GetDifferences(string string1, string string2){

    int totalDifferences = 0;

    for(int i = 0; i < string1.Length; i++){
        if (string1[i]!=string2[i]){
            totalDifferences++;
        }
    }

    return totalDifferences;
}

static bool CheckCut(List<string> list, int cut){
    int rowsBefore = cut;
    int rowsAfter = list.Count - cut;

    int range = Math.Min(rowsBefore, rowsAfter);

    int totalDifferences = 0;

    for (int i = 0; i < range; i++){

        string string1 = list[cut-1-i];
        string string2 = list[cut+i];

        totalDifferences += GetDifferences(string1, string2);

        if (totalDifferences>1){
            return false;
        }
    }

    return totalDifferences == 1;
}

static int GetLinesAboveHorizontal(List<string> mapStrings){

    //PrintList(message: "GetLinesAboveHorizontal", mapStrings);

    for (int i = 0; i< mapStrings.Count-1; i++){
        if (CheckCut(mapStrings, i+1)){
            return i+1;
        }
    }

    return 0;
}

static void Task1(string[] input) {
    
    List<int> linesBeforeVertical = [];
    List<int> linesAboveHorizontal = [];

    // Fill for each map
    List<string> mapStrings = [];

    int hor;
    int ver;

    foreach (string line in input){

        if (line.Length == 0){

            hor = GetLinesAboveHorizontal(mapStrings);
            ver = GetLinesBeforeVertical(mapStrings);

            //Console.WriteLine("Hor: " + hor + " Ver: " + ver);

            linesAboveHorizontal.Add(hor);
            linesBeforeVertical.Add(ver);

            mapStrings = [];
        } else {
            mapStrings.Add(line);
        }
    }

    hor = GetLinesAboveHorizontal(mapStrings);
    ver = GetLinesBeforeVertical(mapStrings);

    //Console.WriteLine("Hor: " + hor + " Ver: " + ver);

    linesAboveHorizontal.Add(hor);
    linesBeforeVertical.Add(ver);
    

    int summarization = linesBeforeVertical.Sum() + 100*linesAboveHorizontal.Sum();

    Console.WriteLine(summarization);
}

static void Task2(string[] input){

    List<int> linesBeforeVertical = [];
    List<int> linesAboveHorizontal = [];

    // Fill for each map
    List<string> mapStrings = [];

    int hor;
    int ver;

    foreach (string line in input){

        if (line.Length == 0){

            hor = GetLinesAboveHorizontal(mapStrings);
            ver = GetLinesBeforeVertical(mapStrings);

            //Console.WriteLine("Hor: " + hor + " Ver: " + ver);

            linesAboveHorizontal.Add(hor);
            linesBeforeVertical.Add(ver);

            mapStrings = [];
        } else {
            mapStrings.Add(line);
        }
    }

    hor = GetLinesAboveHorizontal(mapStrings);
    ver = GetLinesBeforeVertical(mapStrings);

    //Console.WriteLine("Hor: " + hor + " Ver: " + ver);

    linesAboveHorizontal.Add(hor);
    linesBeforeVertical.Add(ver);
    

    int summarization = linesBeforeVertical.Sum() + 100*linesAboveHorizontal.Sum();

    Console.WriteLine(summarization);

}

static void PrintList(string message, List<string> list){
    Console.Write(message + ": ");

    foreach (string num in list){
        Console.WriteLine(num);
    }

    Console.WriteLine("");
}

}