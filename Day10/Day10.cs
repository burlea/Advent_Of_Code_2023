

using System.Data;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

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

    //Task1(map);
    Task2(map);
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

static void PrintPath(string message, List<Tuple<int,int>> path) {

    Console.WriteLine(message + ": ");

    for (int i = 0; i < path.Count; i++){
      
        Console.WriteLine("(" + path[i].Item1 + "," + path[i].Item2 + "), ");
    }

    Console.WriteLine("");
}

static bool SeeIfAddSpot(char [,] map, int[,] distances, int row, int col){

    if (row < 0 || row >= map.Length || col < 0 || col >= map.GetLength(0)){
        return false;
    }

    if (distances[row,col] != 0 && map[row,col] != 'S'){
        return false;
    } 

    if (map[row,col] == '.'){
        return false;
    }

    return true;
}

static void AddTop(int row, int col, char [,] map, List<Tuple<int,int>> nextSpotToLookAt, int[,] distances){

    if (SeeIfAddSpot(map,distances,row-1,col) && 
                (map[row-1,col] == '|' || map[row-1,col] == 'F' ||  map[row-1,col] == '7')) { // top mid
        nextSpotToLookAt.Add(Tuple.Create(row-1, col));
    }
}

static void AddLeft(int row, int col, char [,] map, List<Tuple<int,int>> nextSpotToLookAt, int[,] distances){

    if (SeeIfAddSpot(map,distances,row,col-1) && 
                (map[row,col-1] == '-' || map[row,col-1] == 'L' || map[row,col-1] == 'F')){ // mid left
        nextSpotToLookAt.Add(Tuple.Create(row, col-1));
    }
}

static void AddRight(int row, int col, char [,] map, List<Tuple<int,int>> nextSpotToLookAt, int[,] distances){

    if (SeeIfAddSpot(map,distances,row,col+1) && 
                (map[row,col+1] == '-' || map[row,col+1] == 'J' || map[row,col+1] == '7')){ // mid right
        nextSpotToLookAt.Add(Tuple.Create(row, col+1));
    }
}

static void AddBottom(int row, int col, char [,] map, List<Tuple<int,int>> nextSpotToLookAt, int[,] distances){

    if (SeeIfAddSpot(map,distances,row+1,col) && 
                (map[row+1,col] == '|' || map[row+1,col] == 'J' || map[row+1,col] == 'L')){ // bottom mid
        nextSpotToLookAt.Add(Tuple.Create(row+1, col));
    }
}

static void AddSpots(Tuple<int,int> spot, char [,] map, List<Tuple<int,int>> nextSpotToLookAt, int[,] distances) {
    int row = spot.Item1;
    int col = spot.Item2;

    switch (map[row,col]){
        case 'S':
            AddTop(row,col,map,nextSpotToLookAt,distances);
            AddBottom(row,col,map,nextSpotToLookAt,distances);
            AddRight(row: row,col,map,nextSpotToLookAt,distances);
            AddLeft(row: row,col,map,nextSpotToLookAt,distances);
            break;
        case '|':
            AddTop(row,col,map,nextSpotToLookAt,distances);
            AddBottom(row,col,map,nextSpotToLookAt,distances);
            break;
        case '-':
            AddLeft(row,col,map,nextSpotToLookAt,distances);
            AddRight(row,col,map,nextSpotToLookAt,distances);
            break;
        case 'L':
            AddTop(row,col,map,nextSpotToLookAt,distances);
            AddRight(row,col,map,nextSpotToLookAt,distances);
            break;
        case 'J':
            AddTop(row,col,map,nextSpotToLookAt,distances);
            AddLeft(row,col,map,nextSpotToLookAt,distances);
            break;
        case '7':
            AddLeft(row,col,map,nextSpotToLookAt,distances);
            AddBottom(row,col,map,nextSpotToLookAt,distances);
            break;
        case 'F':
            AddBottom(row,col,map,nextSpotToLookAt,distances);
            AddRight(row,col,map,nextSpotToLookAt,distances);
            break;
        case '.':
            break;
        default:
            break;
    }
}

static void Task1(char [,] map) {
    //PrintMap("Starting Map", map);

    List<Tuple<int,int>> currentSpotsToLookAt = [];
    int [,] distances = new int[map.GetLength(0),map.GetLength(1)];

    // initialize with starting location

    for (int i = 0; i < map.GetLength(0); i++){
        for(int j = 0; j < map.GetLength(1); j++) {
           if(map[i,j]=='S'){
            currentSpotsToLookAt.Add(Tuple.Create(i,j));
           }
        }
    }

    int distanceFromStart = -1;

    while(currentSpotsToLookAt.Count != 0){

        List<Tuple<int,int>> nextSpotsToLookAt = [];
        distanceFromStart++;

        foreach(Tuple<int,int> spotToLookAt in currentSpotsToLookAt){
            distances[spotToLookAt.Item1,spotToLookAt.Item2] = distanceFromStart;
            AddSpots(spotToLookAt,map,nextSpotsToLookAt,distances);
        }

        //PrintMap("Distances At iteration: " + distanceFromStart, distances);

        currentSpotsToLookAt = [.. nextSpotsToLookAt];
    }

    Console.WriteLine(distanceFromStart);
    
}

static void Task2(char [,] map){

List<Tuple<int,int>> path = GetPath(map);

PrintPath("Path", path);

char[,] pathMap = GetPathMap(path, map);

PrintMap("Path Map", pathMap);

int totalInner = GetTotalInner(pathMap);

Console.WriteLine(totalInner);
}

static List<Tuple<int,int>> GetPath(char[,] map){
    List<Tuple<int,int>> path = [];
    HashSet<Tuple<int,int>> explored = [];

    Tuple<int,int> start = Tuple.Create(0,0);

    for (int i = 0; i < map.GetLength(0); i++){
        for (int j = 0; j < map.GetLength(1); j++){
            if (map[i,j] == 'S'){
                start = Tuple.Create(i,j);
                break;
            }
        }
    }

    return DFS(map, start, explored, path, start);
}

static void AddTop2(List<Tuple<int,int>> possiblePlacesToExplore, Tuple<int,int> currentPoint, char[,] map){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    if (i>0 && "|F7".Contains(map[i-1,j])){
        possiblePlacesToExplore.Add(Tuple.Create(i-1, j));
    }
}

static void AddRight2(List<Tuple<int,int>> possiblePlacesToExplore, Tuple<int,int> currentPoint, char[,] map){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    if (j < map.GetLength(1) - 1 && "-J7".Contains(map[i,j+1])){
        possiblePlacesToExplore.Add(Tuple.Create(i, j+1));
    }
}

static void AddBottom2(List<Tuple<int,int>> possiblePlacesToExplore, Tuple<int,int> currentPoint, char[,] map){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    if (i < map.GetLength(0) - 1 && "|LJ".Contains(map[i+1,j])){
        possiblePlacesToExplore.Add(Tuple.Create(i+1, j));
    }
}

static void AddLeft2(List<Tuple<int,int>> possiblePlacesToExplore, Tuple<int,int> currentPoint, char[,] map){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    if (j>0 && "-LF".Contains(map[i,j-1])){
        possiblePlacesToExplore.Add(Tuple.Create(i, j-1));
    }
}

static List<Tuple<int,int>> DFS(char[,] map, Tuple<int,int> currentPoint, HashSet<Tuple<int,int>> explored, List<Tuple<int,int>> currentPath, Tuple<int,int> goal){

    Console.WriteLine("Current Point: " + currentPoint);

    Console.WriteLine("Explored: " + explored.Count);
    PrintPath("current Path", currentPath);
    explored.Add(currentPoint);

    List<Tuple<int,int>> possiblePlacesToExplore = [];

    switch(map[currentPoint.Item1, currentPoint.Item2]){
        case 'S':
            bool addedTop = false;
            if (currentPoint.Item1 > 0 && "|F7".Contains(map[currentPoint.Item1 - 1, currentPoint.Item2])){
                addedTop = true;
                AddTop2( possiblePlacesToExplore, currentPoint, map);
            }
            bool addedBottom = false;
            if (currentPoint.Item1 < map.GetLength(dimension: 0)-1 && "|LJ".Contains(map[currentPoint.Item1 + 1, currentPoint.Item2])){
                addedBottom = true;
                AddBottom2( possiblePlacesToExplore, currentPoint, map);
            }
            bool addedRight = false;
            if (currentPoint.Item2 < map.GetLength(dimension: 1)-1 && "-J7".Contains(map[currentPoint.Item1, currentPoint.Item2 + 1])){
                addedRight = true;
                AddRight2( possiblePlacesToExplore, currentPoint, map);
            }
            bool addedLeft = false;
            if (currentPoint.Item2 > 0 && "-LF".Contains(map[currentPoint.Item1, currentPoint.Item2 - 1])) {
                addedLeft = true;
                AddLeft2( possiblePlacesToExplore, currentPoint, map);
            }

            if (addedTop && addedBottom){
                map[currentPoint.Item1, currentPoint.Item2] = '|';
            } else if (addedTop && addedLeft){
                 map[currentPoint.Item1, currentPoint.Item2] = 'J';
            } else if (addedTop && addedRight){
                map[currentPoint.Item1, currentPoint.Item2] = 'L';
            } else if (addedBottom && addedLeft){
                map[currentPoint.Item1, currentPoint.Item2] = '7';
            } else if (addedBottom && addedRight){
                map[currentPoint.Item1, currentPoint.Item2] = 'F';
            } else if (addedLeft && addedRight){
                map[currentPoint.Item1, currentPoint.Item2] = '-';
            }
            
            break;
        case '|':
            AddTop2(possiblePlacesToExplore,currentPoint, map);
            AddBottom2(possiblePlacesToExplore,currentPoint, map);
            break;
        case '-':
            AddRight2(possiblePlacesToExplore,currentPoint, map);
            AddLeft2(possiblePlacesToExplore,currentPoint, map);
            break;
        case 'L':
            AddTop2(possiblePlacesToExplore,currentPoint, map);
            AddRight2(possiblePlacesToExplore,currentPoint, map);
            break;
        case 'J':
            AddTop2(possiblePlacesToExplore,currentPoint, map);
            AddLeft2(possiblePlacesToExplore,currentPoint, map);
            break;
        case '7':
            AddLeft2(possiblePlacesToExplore,currentPoint, map);
            AddBottom2(possiblePlacesToExplore,currentPoint, map);
            break;
        case 'F':
            AddBottom2(possiblePlacesToExplore,currentPoint, map);
            AddRight2(possiblePlacesToExplore,currentPoint, map);
            break;
        default:
            break;
    }

    foreach (Tuple<int,int> place in possiblePlacesToExplore){

        if (possiblePlacesToExplore.Contains(goal) && possiblePlacesToExplore.Count == 1){
            Console.WriteLine("DONE");
            return currentPath;
        }

        if (!explored.Contains(place)){

            currentPath.Add(currentPoint);
            List<Tuple<int,int>> path = DFS(map, place, explored, currentPath, goal);

            if (path.Count != 0){
                return path;
            }
        }
    }

    return [];
}

static char[,] GetPathMap(List<Tuple<int,int>> path, char[,] map){
    char[,] pathMap = new char[map.GetLength(0),map.GetLength(1)];

    foreach(Tuple<int,int> point in path){

        if (map[point.Item1,point.Item2] == '-'){
            pathMap[point.Item1,point.Item2] = '-';
        } else if ("|LF7J".Contains(map[point.Item1,point.Item2])){
            pathMap[point.Item1,point.Item2] = '|';
        }
    }

    for(int i = 0; i < pathMap.GetLength(0);i++){
        for (int j = 0; j < pathMap.GetLength(1); j++){
            if (pathMap[i,j] != '-' && pathMap[i,j] != '|'){
                pathMap[i,j] = '.';
            }
        }
    }

    return pathMap;
}

static int GetTotalInner(char[,] pathMap){

    int totalInner = 0;

    for (int i = 0; i < pathMap.GetLength(0); i++){
        int intersections = 0;
        for (int j = 0; j < pathMap.GetLength(1); j++){
            if (pathMap[i,j] == '.'){
                if (intersections % 2 == 1){
                    totalInner++;
                }
            } else if (pathMap[i,j] == '|'){
                intersections++;
            }
        }
    }

    return totalInner;
}
}