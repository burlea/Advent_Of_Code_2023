using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO.Pipes;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Channels;

partial class Program
{
    
static void Main()
{
    string[] lines = File.ReadAllLines("input.txt");

    Task1(lines);
    Task2(lines);
}

static void Task1(string [] input) {
    string instructions = input[0];

    Dictionary<string,Tuple<string,string>> map = [];

    for(int i = 2; i < input.Length; i++) {
        string [] parts = input[i].Split('=');

        Regex alphabetValues = MyRegex();

        string key = alphabetValues.Replace(parts[0], "").Trim();

        // Console.WriteLine("Key: " + key + "Value: " + "AAA" + " Equals? " + string.Equals(key,"AAA"));
        string [] values = parts[1].Split(",");

        string leftValue = alphabetValues.Replace(values[0], "").Trim();
        string rightValue = alphabetValues.Replace(values[1], "").Trim();

        map.Add(key, Tuple.Create(leftValue, rightValue));
    }

    //     Console.WriteLine("Instructions: " + instructions);
    // foreach (KeyValuePair<string,Tuple<string,string>> item in map)
    // {
    //     //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
    //     Console.WriteLine("Key = {0}, Left Value = {1}, Right Value = {2}", item.Key, item.Value.Item1, item.Value.Item2);
    // }

    string currentPlace = "AAA";
    int numberSteps = 0;

    while(currentPlace != "ZZZ") {

        for(int i = 0; i < instructions.Length; i++){
            char instruction = instructions[i];

            if (instruction == 'L'){
                currentPlace = map[currentPlace].Item1;
            } else {
                currentPlace = map[currentPlace].Item2;
            }

            numberSteps++;

            if (currentPlace == "ZZZ"){
                break;
            }
        }
    }

    Console.WriteLine(numberSteps);
}

static void Task2(string [] input){

    string instructions = input[0];
    Dictionary<string,Tuple<string,string>> map = [];
    List<string> startingNodes = [];


    for(int i = 2; i < input.Length; i++) {
        string [] parts = input[i].Split('=');

        Regex alphabetValues = MyRegex();

        string key = alphabetValues.Replace(parts[0], "").Trim();

        if (key[2] == 'A'){
            startingNodes.Add(key);
        }

        string [] values = parts[1].Split(",");

        string leftValue = alphabetValues.Replace(values[0], "").Trim();
        string rightValue = alphabetValues.Replace(values[1], "").Trim();

        map.Add(key, Tuple.Create(leftValue, rightValue));
    }

    string[] currentNodes = [.. startingNodes];

    long[] numberStepsTillZ = new long[currentNodes.Length];
    
    long numberSteps = 0;

    while(!isAllAtEnd(numberStepsTillZ)) {

        for(int i = 0; i < instructions.Length; i++){
            char instruction = instructions[i];

            numberSteps++;

            for(int j = 0; j < currentNodes.Length; j++){

                if (instruction == 'L'){
                    currentNodes[j] = map[currentNodes[j]].Item1;
                } else {
                    currentNodes[j] = map[currentNodes[j]].Item2;
                }

                if (currentNodes[j][2]=='Z'){
                    if (numberStepsTillZ[j] == 0){
                        numberStepsTillZ[j] = numberSteps;
                    }
                }
            }

            if (isAllAtEnd(numberStepsTillZ)){
                break;
            }
        }
    }

    long lcm = numberStepsTillZ.Aggregate(Lcm);

    Console.WriteLine(lcm);
}

static bool isAllMapped(string[] currentNodes) {
    foreach(string node in currentNodes) {
        if (node[2]!='Z'){
            return false;
        }
    }

    return true;
}

static bool isAllAtEnd(long[] numberStepsTillZ) {

    foreach(long steps in numberStepsTillZ) {
        if (steps==0){
            return false;
        }
    }
    return true;
}

static long Lcm(long a, long b) {
    return Math.Abs(a * b) / Gcd(a, b);
}
static long Gcd(long a, long b)
{
   return b == 0 ? a : Gcd(b, a % b);
}

    [GeneratedRegex("[^a-zA-Z0-9 -]")]
    private static partial Regex MyRegex();
}