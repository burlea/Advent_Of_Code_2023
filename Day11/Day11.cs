

using Microsoft.VisualBasic;

partial class Program
{
    
static void Main()
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

static Dictionary<int,Tuple<int,int>> GetGalaxyMappings(char [,] map) {

    Dictionary<int,Tuple<int,int>> galaxyMappings = [];

    int currentGalaxy = 0;

    for (int i = 0; i < map.GetLength(0); i++){
        for (int j = 0; j < map.GetLength(1); j++){
            if (map[i,j] == '#'){
                galaxyMappings[currentGalaxy] = Tuple.Create(i,j);
                currentGalaxy++;
            }
        }
    }

    return galaxyMappings;
}

static char[,] ExpandMap(char [,] map){

    List<int> rowsToAdd = [];
    List<int> colToAdd = [];

    
    for (int i = 0; i < map.GetLength(0); i++){
        bool noGalaxyInRow = true;
        for (int j = 0; j < map.GetLength(1); j++){
             if (map[i,j] == '#'){
                noGalaxyInRow = false;
             }
        }
        if (noGalaxyInRow){
            rowsToAdd.Add(i);
        }
    }

    for (int j = 0; j < map.GetLength(1); j++){
        bool noGalaxyInCol = true;
        for (int i = 0; i < map.GetLength(0); i++){
             if (map[i,j] == '#'){
                noGalaxyInCol = false;
             }
        }
        if (noGalaxyInCol){
            colToAdd.Add(j);
        }
    }

    char[,] expandedRowsMap = AddRows(map, rowsToAdd);
    char[,] expandedColumnsMap = AddColumns(expandedRowsMap, colToAdd);

    return expandedColumnsMap;
}

static char[,] AddRows(char[,] map, List<int> rowsToAdd){

    char[,] expandedMap = new char[map.GetLength(0) + rowsToAdd.Count, map.GetLength(1)];

    int currentMapRow = 0;

    for (int i = 0; i < expandedMap.GetLength(0); i++){
        for (int j = 0; j < expandedMap.GetLength(1); j++){
            expandedMap[i,j] = map[currentMapRow,j];
        }

        if (rowsToAdd.Contains(currentMapRow)){
            rowsToAdd.Remove(currentMapRow);
        } else {
            currentMapRow++;
        }
    }

    return expandedMap;

}

static char[,] AddColumns(char[,] map, List<int> colsToAdd){

    char[,] expandedMap = new char[map.GetLength(0), map.GetLength(1) + colsToAdd.Count];

    int currentMapCol = 0;

    for (int j = 0; j < expandedMap.GetLength(1); j++){
        for (int i = 0; i < expandedMap.GetLength(0); i++){
            expandedMap[i,j] = map[i,currentMapCol];
        }

        if (colsToAdd.Contains(currentMapCol)){
            colsToAdd.Remove(currentMapCol);
        } else {
            currentMapCol++;
        }
    }

    return expandedMap;

}

static int GetTotalDistances(Dictionary<int,Tuple<int,int>> galaxyMappings) {
    List<int> galaxies = [];

    foreach (KeyValuePair<int, Tuple<int,int>> galaxy in galaxyMappings){
        galaxies.Add(galaxy.Key);
    }

    int total = 0;

    for (int i = 0; i < galaxies.Count; i++){
        for (int j = i+1; j < galaxies.Count; j++){
            Tuple<int,int> galaxy1 = galaxyMappings[i];
            Tuple<int,int> galaxy2 = galaxyMappings[j];

            int distance = Math.Abs(galaxy1.Item1 - galaxy2.Item1) + Math.Abs(galaxy1.Item2 - galaxy2.Item2);

            total += distance;
        }
    }

    return total;
}

static List<int> GetExpandedRows(char [,] map){

    List<int> expandedRows = [];

    for (int i = 0; i < map.GetLength(0); i++){
        bool noGalaxyInRow = true;
        for (int j = 0; j < map.GetLength(1); j++){
             if (map[i,j] == '#'){
                noGalaxyInRow = false;
             }
        }
        if (noGalaxyInRow){
            expandedRows.Add(i);
        }
    }

    return expandedRows;
}
static List<int> GetExpandedColumns(char [,] map){

    List<int> expandedColumns = [];

    for (int j = 0; j < map.GetLength(1); j++){
        bool noGalaxyInCol = true;
        for (int i = 0; i < map.GetLength(0); i++){
             if (map[i,j] == '#'){
                noGalaxyInCol = false;
             }
        }
        if (noGalaxyInCol){
            expandedColumns.Add(j);
        }
    }

    return expandedColumns;
}

static long GetTotalExpandedDistances(Dictionary<int,Tuple<int,int>> galaxyMappings, List<int> expandedRows, List<int> expandedColumns) {
    List<int> galaxies = [];

    foreach (KeyValuePair<int, Tuple<int,int>> galaxy in galaxyMappings){
        galaxies.Add(galaxy.Key);
    }

    long total = 0;

    long distanceAcrossExpansion = 1000000;

    for (int i = 0; i < galaxies.Count; i++){
        for (int j = i+1; j < galaxies.Count; j++){
            Tuple<int,int> galaxy1 = galaxyMappings[i];
            Tuple<int,int> galaxy2 = galaxyMappings[j];

            long startRowVal = Math.Min(galaxy1.Item1, val2: galaxy2.Item1);
            long endRowVal = Math.Max(galaxy1.Item1, val2: galaxy2.Item1);
            long numberExpandedRowsCrossed = expandedRows.Count(row => row > startRowVal && row < endRowVal);
            long horizontalDistance = (endRowVal - startRowVal) + (numberExpandedRowsCrossed * (distanceAcrossExpansion - 1));

            //Console.WriteLine(value: "Rows: " + startRowVal + " to " + endRowVal + " NumberExpandedCrossed: " + numberExpandedRowsCrossed + " Distance: " + horizontalDistance);


            long startColVal = Math.Min(galaxy1.Item2, val2: galaxy2.Item2);
            long endColVal = Math.Max(galaxy1.Item2, val2: galaxy2.Item2);
            long numberExpandedColsCrossed = expandedColumns.Count(col => col > startColVal && col < endColVal);
            long verticalDistance = (endColVal - startColVal) + (numberExpandedColsCrossed * (distanceAcrossExpansion - 1));

            //Console.WriteLine(value: "Cols: " + startColVal + " to " + endColVal + " NumberExpandedCrossed: " + numberExpandedColsCrossed + " Distance: " + verticalDistance);

            total += (horizontalDistance + verticalDistance);

            //Console.WriteLine("Current Total: " + total);
        }
    }

    return total;
}
static void Task1(char [,] map) {

    char[,] expandedMap = ExpandMap(map);

    Dictionary<int,Tuple<int,int>> galaxyMappings = GetGalaxyMappings(expandedMap);

    int totalDistances = GetTotalDistances(galaxyMappings);

    Console.WriteLine(totalDistances);
}

static void Task2(char [,] map){

    Dictionary<int,Tuple<int,int>> galaxyMappings = GetGalaxyMappings(map);

    List<int> expandedRows = GetExpandedRows(map);
    List<int> expandedColumns = GetExpandedColumns(map);

    long totalDistances = GetTotalExpandedDistances(galaxyMappings, expandedRows, expandedColumns);

    Console.WriteLine(totalDistances);
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