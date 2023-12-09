

using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;

partial class Program
{
    
static void Main()
{
    string[] lines = File.ReadAllLines("input.txt");

    Task1(lines);
    Task2(lines);
}

static void PrintList(string message, List<int> list) {

    Console.Write(message + ": ");

    foreach(int num in list){
        Console.Write(num+ " ");
    }

    Console.WriteLine("");
}

static bool IsCompleted (List<int> sequence) {
    foreach(int number in sequence){
        if (number !=0){
            return false;
        }
    }

    return true;
}

static List<int> GetDifferences(List<int> sequence) {
    List<int> sequenceDifferences = [];

    for (int i = 0; i < sequence.Count-1; i++) {
        int diff = sequence[i+1] - sequence[i];
        sequenceDifferences.Add(diff);
    }

    return sequenceDifferences;
}

static List<int> ExpandedSequence (List<int> sequence) {

    if (IsCompleted(sequence)){
        sequence.Add(sequence.Last() + 0);
        return sequence;
    } else {
        List<int> sequenceDifferences = GetDifferences(sequence);
        sequence.Add(sequence.Last() + ExpandedSequence(sequenceDifferences).Last());
        return sequence;
    } 
}

static List<int> ExpandedSequence2 (List<int> sequence) {

    if (IsCompleted(sequence)){
        sequence = sequence.Prepend(0).ToList();
        return sequence;
    } else {
        List<int> sequenceDifferences = GetDifferences(sequence);
        sequence = sequence.Prepend(sequence.First() - ExpandedSequence2(sequenceDifferences).First()).ToList();
        return sequence;
    } 
}

static void Task1(string [] input) {

    int sum = 0;

    foreach (string line in input) {
        List<int> sequence = line.Split().Where(num => num != "").ToList().ConvertAll<int>(num => Int32.Parse(num));
        List<int> expandedSequence = ExpandedSequence(sequence);
        sum += expandedSequence.Last(); 
    }

    Console.WriteLine(sum);
}

static void Task2(string [] input){
    int sum = 0;

    foreach (string line in input) {
        List<int> sequence = line.Split().Where(num => num != "").ToList().ConvertAll<int>(num => Int32.Parse(num));
        List<int> expandedSequence = ExpandedSequence2(sequence);
        sum += expandedSequence.First();  
    }

    Console.WriteLine(sum);
}
}