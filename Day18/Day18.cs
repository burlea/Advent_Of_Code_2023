

using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Net.Security;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Day18
{
  class Program
  {
    static void Main(string[] args)
    {
        string[] input = File.ReadAllLines("input.txt");

        // Task1(input);
        Task2(input);
    }

    // static void Task1(string[] input){

    //     List<Step> plan = [];

    //     foreach (string line in input){
    //         string [] parts = line.Split();

    //         string direction = parts[0].Trim();
    //         int amount = Int32.Parse(parts[1]);
    //         string color = parts[2];
    //         plan.Add(new Step(direction,amount,color));
    //     }

    //     List<Tuple<int,int>> verticies = [];
    //     Tuple<int,int> currentPoint = Tuple.Create(0,0);
    //     verticies.Add(currentPoint);

    //     int perimeter = 0;

    //     for (int i = 0; i < plan.Count;i++){
    //         Step step = plan[i];

    //         Tuple<int,int> newPoint;

    //         switch(step.direction){
    //             case "R":
    //                 newPoint = Tuple.Create(currentPoint.Item1, currentPoint.Item2 + step.amount);
    //                 break;
    //             case "L":
    //                 newPoint = Tuple.Create(currentPoint.Item1, currentPoint.Item2 - step.amount);
                    
    //                 break;
    //             case "U":
    //                 newPoint = Tuple.Create(currentPoint.Item1 - step.amount, currentPoint.Item2);
    //                 break;
    //             case "D":
    //                 newPoint = Tuple.Create(currentPoint.Item1 + step.amount, currentPoint.Item2);
    //                 break;
    //             default:
    //                 Console.WriteLine("UNEXPECTED DIRECTION: " + step.direction);
    //                 newPoint = Tuple.Create(currentPoint.Item1, currentPoint.Item2);
    //                 break;
    //         }

    //         perimeter += step.amount;
    //         verticies.Add(newPoint);
    //         currentPoint = newPoint;
    //     }

    //     Console.WriteLine("Perimeter: " + perimeter);


    //     int yMin = verticies.Min(vertex => vertex.Item1);
    //     int xMin = verticies.Min(vertex => vertex.Item2);
    //     int yOffset = 0;
    //     int xOffset = 0;

    //     if (yMin < 0){
    //         yOffset = 0-yMin;
    //     }

    //     if (xMin < 0){
    //         xOffset = 0 - xMin;
    //     }

    //     for (int i = 0; i < verticies.Count; i++){
    //         verticies[i] = Tuple.Create(verticies[i].Item1 + yOffset, verticies[i].Item2 + xOffset);
    //     }

    //     // for(int i = 0; i < verticies.Count; i++){
    //     //     Console.WriteLine($"Verticies: ({verticies[i].Item1},{verticies[i].Item2})");
    //     // }

    //     double area = 0;
    //     double sum1 = 0.0;
    //     double sum2 = 0.0;
    //     int j = verticies.Count-1;

    //     for (int i = 0; i < verticies.Count-1; i++){
    //         sum1 += verticies[index: i].Item1 * verticies[i+1].Item2;
    //         sum2 += verticies[index: i].Item2 * verticies[i+1].Item1;
    //     }

    //     sum1 += verticies[verticies.Count-1].Item1 * verticies[0].Item2;
    //     sum2 += verticies[index: 0].Item1 * verticies[verticies.Count-1].Item2;

    //     area = Math.Abs((sum1-sum2)/2.0);

    //     area = area + (perimeter/2) + 1;

    //     Console.WriteLine(area);
    // }


    static void Task2(string[] input){

        List<Step> plan = [];

        foreach (string line in input){
            string [] parts = line.Split();
            string color = parts[2];

            string distanceString = color[2..^2];
            string directionString = color[^2..^1];
            string direction;

            switch(directionString){
                case "0":
                    direction = "R";
                    break;
                case "1":
                    direction = "D";
                    break;
                case "2":
                    direction = "L";
                    break;
                case "3":
                    direction = "U";
                    break;
                default:
                    direction="";
                    Console.WriteLine("DEFAULTING");
                    break;
            }

            // Console.WriteLine(direction + " " + Int64.Parse(distanceString, System.Globalization.NumberStyles.HexNumber));

            plan.Add(new Step(direction,Int64.Parse(distanceString, System.Globalization.NumberStyles.HexNumber),color));
        }

        List<Tuple<long,long>> verticies = [];
        Tuple<long,long> currentPoint = Tuple.Create(0L,0L);
        verticies.Add(currentPoint);

        long perimeter = 0;

        for (int i = 0; i < plan.Count;i++){
            Step step = plan[i];

            Tuple<long,long> newPoint;

            switch(step.direction){
                case "R":
                    newPoint = Tuple.Create(currentPoint.Item1, currentPoint.Item2 + step.amount);
                    break;
                case "L":
                    newPoint = Tuple.Create(currentPoint.Item1, currentPoint.Item2 - step.amount);
                    
                    break;
                case "U":
                    newPoint = Tuple.Create(currentPoint.Item1 - step.amount, currentPoint.Item2);
                    break;
                case "D":
                    newPoint = Tuple.Create(currentPoint.Item1 + step.amount, currentPoint.Item2);
                    break;
                default:
                    Console.WriteLine("UNEXPECTED DIRECTION: " + step.direction);
                    newPoint = Tuple.Create(currentPoint.Item1, currentPoint.Item2);
                    break;
            }

            perimeter += step.amount;
            verticies.Add(newPoint);
            currentPoint = newPoint;
        }

        Console.WriteLine("Perimeter: " + perimeter);


        long yMin = verticies.Min(vertex => vertex.Item1);
        long xMin = verticies.Min(vertex => vertex.Item2);
        long yOffset = 0;
        long xOffset = 0;

        if (yMin < 0){
            yOffset = 0-yMin;
        }

        if (xMin < 0){
            xOffset = 0 - xMin;
        }

        for (int i = 0; i < verticies.Count; i++){
            verticies[i] = Tuple.Create(verticies[i].Item1 + yOffset, verticies[i].Item2 + xOffset);
        }

        // for(int i = 0; i < verticies.Count; i++){
        //     Console.WriteLine($"Verticies: ({verticies[i].Item1},{verticies[i].Item2})");
        // }

        long area = 0;
        long sum1 = 0;
        long sum2 = 0;
        int j = verticies.Count-1;

        for (int i = 0; i < verticies.Count-1; i++){
            sum1 += verticies[index: i].Item1 * verticies[i+1].Item2;
            sum2 += verticies[index: i].Item2 * verticies[i+1].Item1;
        }

        sum1 += verticies[^1].Item1 * verticies[0].Item2;
        sum2 += verticies[index: 0].Item1 * verticies[^1].Item2;

        area = (long) Math.Abs((sum1-sum2)/2.0);

        area = area + (perimeter/2) + 1;

        Console.WriteLine(area);
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
}

class Step (string direction, long amount, string color){
    public string direction = direction;
    public long amount = amount;
    public string color = color;
}

}