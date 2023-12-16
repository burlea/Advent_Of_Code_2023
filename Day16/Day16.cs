using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Day1
{
  class Program
  {
    static void Main(string[] args)
    {
        string[] input = File.ReadAllLines("input.txt");
        char [,] map = new char[input.Length,input[0].Length];

        for (int i = 0; i < input.Length; i++){
            string line = input[i];
            for (int j = 0; j < line.Length; j++) {
                char currentChar = line[j];
                map[i,j] = currentChar;
            }
        }

        Task1(map);
        Task2(map);
    }

    enum Direction {
        up,
        down,
        left,
        right
    }

    static string ConvertPointToStringWithDirection(int i, int j, Direction direction){
        return "" + i + "," + j + "|" + direction.ToString();
    }

    static string ConvertPointToString(int i, int j){
        return "" + i + "," + j;
    }

    static void AddNewPoint(int i, int j, Direction directionToGo, char[,] map, Stack<Tuple<int,int, Direction>> pointsToContinueAt){

        switch(directionToGo){
            case Direction.up:
                if (i > 0){
                    pointsToContinueAt.Push(Tuple.Create(i-1,j,directionToGo));
                }
                break;
            case Direction.down:
                if (i < map.GetLength(0) - 1){
                    pointsToContinueAt.Push(Tuple.Create(i+1,j,directionToGo));
                }
                break;

            case Direction.left:
                if (j > 0){
                    pointsToContinueAt.Push(Tuple.Create(i,j-1,directionToGo));
                }
                break;
            case Direction.right:
                if (j < map.GetLength(1) - 1){
                    pointsToContinueAt.Push(Tuple.Create(i,j+1,directionToGo));
                }
                break;
        }
    }

    static int PlotBeam(char[,] map, Tuple<int,int,Direction> starting){
        Stack<Tuple<int,int, Direction>> pointsToContinueAt = [];
        pointsToContinueAt.Push(starting);
        HashSet<string> currentEnergized = [];
        HashSet<string> currentEnergizedDirection = [];

        while (pointsToContinueAt.Count != 0){
            Tuple<int,int, Direction> currentPoint = pointsToContinueAt.Pop();

            int i = currentPoint.Item1;
            int j = currentPoint.Item2;
            Direction direction = currentPoint.Item3;

            string key = ConvertPointToString(i,j);
            currentEnergized.Add(key);

            string keyDirection = ConvertPointToStringWithDirection(i,j,direction);
            if (!currentEnergizedDirection.Add(keyDirection))
            {
                continue;
            }

            switch (map[i,j]){
                case '.':
                    AddNewPoint(i, j, direction, map, pointsToContinueAt);
                    break;
                case '/':
                    switch(direction){
                        case Direction.up:
                            AddNewPoint(i, j, Direction.right, map, pointsToContinueAt);
                            break;
                        case Direction.down:
                            AddNewPoint(i, j, Direction.left, map, pointsToContinueAt);
                            break;
                        case Direction.left:
                            AddNewPoint(i, j, Direction.down, map, pointsToContinueAt);
                            break;
                        case Direction.right:
                            AddNewPoint(i, j, Direction.up, map, pointsToContinueAt);
                            break;
                    }
                    break;
                case '\\':
                    switch(direction){
                        case Direction.up:
                            AddNewPoint(i, j, Direction.left, map, pointsToContinueAt);
                            break;
                        case Direction.down:
                            AddNewPoint(i, j, Direction.right, map, pointsToContinueAt);
                            break;
                        case Direction.left:
                            AddNewPoint(i, j, Direction.up, map, pointsToContinueAt);
                            break;
                        case Direction.right:
                            AddNewPoint(i, j, Direction.down, map, pointsToContinueAt);
                            break;
                    }
                    break;
                case '|':
                    switch(direction){
                        case Direction.up:
                            AddNewPoint(i, j, direction, map, pointsToContinueAt);
                            break;
                        case Direction.down:
                            AddNewPoint(i, j, direction, map, pointsToContinueAt);
                            break;
                        case Direction.left:
                            AddNewPoint(i, j: j, Direction.up, map, pointsToContinueAt);
                            AddNewPoint(i, j: j, Direction.down, map, pointsToContinueAt);
                            break;
                        case Direction.right:
                            AddNewPoint(i, j: j, Direction.up, map, pointsToContinueAt);
                            AddNewPoint(i, j: j, Direction.down, map, pointsToContinueAt);
                            break;
                    }
                    break;
                case '-':
                    switch(direction){
                        case Direction.left:
                            AddNewPoint(i, j, direction, map, pointsToContinueAt);
                            break;
                        case Direction.right:
                            AddNewPoint(i, j, direction, map, pointsToContinueAt);
                            break;
                        case Direction.up:
                            AddNewPoint(i, j: j, Direction.left, map, pointsToContinueAt);
                            AddNewPoint(i, j: j, Direction.right, map, pointsToContinueAt);
                            break;
                        case Direction.down:
                            AddNewPoint(i, j: j, Direction.left, map, pointsToContinueAt);
                            AddNewPoint(i, j: j, Direction.right, map, pointsToContinueAt);
                            break;
                    }
                    break;
            }
        }

        //char [,] newMap = MapOutPath(map, currentEnergizedDirection: currentEnergizedDirection)

        return currentEnergized.Count;
    }

    // static char[,] MapOutPath(char[,] map, HashSet<string> currentEnergizedDirection){

    //     char[,] newMap = new char[map.GetLength(0),map.GetLength(1)];

    //     foreach(string point in currentEnergizedDirection){
            
    //     }

    //     return map;
    // }

    static void Task1(char [,] map) {

        // PrintMap("Initial", map);

        int totalEnergized = PlotBeam(map,Tuple.Create(0,0,Direction.right));

        Console.WriteLine(totalEnergized);
        
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

    static void PrintHashSet(string message, HashSet<string> set) {

        Console.WriteLine(message + ": ");

        set.ToList<String>().ForEach(x => Console.WriteLine(x));

    }

    static void Task2(char [,] map){

        int currentMaxEnergized = 0;

        int jMax = map.GetLength(1) - 1;
        int iMax = map.GetLength(0) - 1;

        for (int i = 0; i < map.GetLength(0); i++){
            currentMaxEnergized = Math.Max(currentMaxEnergized, PlotBeam(map,Tuple.Create(i,0,Direction.right)));
            currentMaxEnergized = Math.Max(currentMaxEnergized, PlotBeam(map,Tuple.Create(i,jMax,Direction.left)));
        }

        for (int j = 0; j < map.GetLength(1); j++){
            currentMaxEnergized = Math.Max(currentMaxEnergized, PlotBeam(map,Tuple.Create(0,j,Direction.down)));
            currentMaxEnergized = Math.Max(currentMaxEnergized, PlotBeam(map,Tuple.Create(iMax,j,Direction.up)));
        }

        Console.WriteLine(currentMaxEnergized);
    }

}
}