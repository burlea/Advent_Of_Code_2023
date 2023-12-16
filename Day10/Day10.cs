
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

HashSet<Tuple<int,int>> explored = GetPath(map);

char[,] pathmap = GetPathMap(explored, map);


List<Tuple<int,int>> notExplored = [];

 for (int i = 0; i < map.GetLength(0);i++){
    for (int j = 0; j < map.GetLength(1); j++){
        Tuple<int,int> point = Tuple.Create(i,j);

        if (!explored.Contains(point)){
            notExplored.Add(point);
        }
    }
 }

int totalInner = GetTotalInner(notExplored, explored, map);

Console.WriteLine(totalInner);
}

static char[,] GetPathMap(HashSet<Tuple<int,int>> explored, char[,] map){
    char[,] newMap = new char[map.GetLength(0),map.GetLength(1)];

    for (int i = 0; i < newMap.GetLength(0);i++){
        for (int j = 0; j < newMap.GetLength(1); j++){
            newMap[i,j] = '.';

            if (explored.Contains(Tuple.Create(i,j))){
                newMap[i,j] = '*';
            }
        }
    }

    return newMap;
}

static HashSet<Tuple<int,int>> GetPath(char[,] map){

    Tuple<int,int> start = Tuple.Create(0,0);

    for (int i = 0; i < map.GetLength(0); i++){
        for (int j = 0; j < map.GetLength(1); j++){
            if (map[i,j] == 'S'){
                start = Tuple.Create(i,j);
                break;
            }
        }
    }

    List<Tuple<int,int>> pointsToCheck = [start];
    HashSet<Tuple<int,int>> explored = [];

    while (pointsToCheck.Count != 0){

        List<Tuple<int,int>> connectingPoints = [];

        foreach(Tuple<int,int> currentPoint in pointsToCheck){

            explored.Add(currentPoint);

            switch(map[currentPoint.Item1, currentPoint.Item2]){
                case 'S':
                    if (currentPoint.Item1 > 0 && "|F7".Contains(map[currentPoint.Item1 - 1, currentPoint.Item2])){
                        AddTop2( connectingPoints, currentPoint, map, explored);
                    }
                    if (currentPoint.Item1 < map.GetLength(dimension: 0)-1 && "|LJ".Contains(map[currentPoint.Item1 + 1, currentPoint.Item2])){
                        AddBottom2( connectingPoints, currentPoint, map, explored);
                    }
                    if (currentPoint.Item2 < map.GetLength(dimension: 1)-1 && "-J7".Contains(map[currentPoint.Item1, currentPoint.Item2 + 1])){
                        AddRight2( connectingPoints, currentPoint, map, explored);
                    }
                    if (currentPoint.Item2 > 0 && "-LF".Contains(map[currentPoint.Item1, currentPoint.Item2 - 1])) {
                        AddLeft2( connectingPoints, currentPoint, map, explored);
                    }
                    break;
                case '|':
                    AddTop2(connectingPoints,currentPoint, map, explored);
                    AddBottom2(connectingPoints,currentPoint, map, explored);
                    break;
                case '-':
                    AddRight2(connectingPoints,currentPoint, map, explored);
                    AddLeft2(connectingPoints,currentPoint, map, explored);
                    break;
                case 'L':
                    AddTop2(connectingPoints,currentPoint, map, explored);
                    AddRight2(connectingPoints,currentPoint, map, explored);
                    break;
                case 'J':
                    AddTop2(connectingPoints,currentPoint, map, explored);
                    AddLeft2(connectingPoints,currentPoint, map, explored);
                    break;
                case '7':
                    AddLeft2(connectingPoints,currentPoint, map, explored);
                    AddBottom2(connectingPoints,currentPoint, map, explored);
                    break;
                case 'F':
                    AddBottom2(connectingPoints,currentPoint, map, explored);
                    AddRight2(connectingPoints,currentPoint, map, explored);
                    break;
                default:
                    break;
            }
        }

        pointsToCheck = connectingPoints;
    }

    return explored;
}


static void AddTop2(List<Tuple<int,int>> placesToSee, Tuple<int,int> currentPoint, char[,] map, HashSet<Tuple<int,int>> explored){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    Tuple<int,int> nextPoint = Tuple.Create(i-1,j);

    if (i>0 && "|F7".Contains(map[i-1,j]) && !explored.Contains(nextPoint) && !placesToSee.Contains(nextPoint)){ 
        placesToSee.Add(nextPoint);
    }
}

static void AddRight2(List<Tuple<int,int>> placesToSee, Tuple<int,int> currentPoint, char[,] map, HashSet<Tuple<int,int>> explored){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    Tuple<int,int> nextPoint = Tuple.Create(i,j+1);

    if (j < map.GetLength(1) - 1 && "-J7".Contains(map[i,j+1]) && !explored.Contains(nextPoint) && !placesToSee.Contains(nextPoint)){ 
        placesToSee.Add(nextPoint);
    }
}

static void AddBottom2(List<Tuple<int,int>> placesToSee, Tuple<int,int> currentPoint, char[,] map, HashSet<Tuple<int,int>> explored){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    Tuple<int,int> nextPoint = Tuple.Create(i+1,j);

    if (i < map.GetLength(0) - 1 && "|LJ".Contains(map[i+1,j]) && !explored.Contains(nextPoint) && !placesToSee.Contains(nextPoint)){ 
        placesToSee.Add(nextPoint);
    }
}

static void AddLeft2(List<Tuple<int,int>> placesToSee, Tuple<int,int> currentPoint, char[,] map, HashSet<Tuple<int,int>> explored){
    int i = currentPoint.Item1;
    int j = currentPoint.Item2;

    Tuple<int,int> nextPoint = Tuple.Create(i,j-1);

    if (j>0 && "-LF".Contains(map[i,j-1]) && !explored.Contains(nextPoint) && !placesToSee.Contains(nextPoint)){ 
            placesToSee.Add(nextPoint);
    }
}

static int GetTotalInner(List<Tuple<int,int>> notExplored, HashSet<Tuple<int,int>> explored, char[,] map){

    HashSet<Tuple<int,int>> innerEdge = [];

    foreach(Tuple<int,int> point in notExplored){
        int intersections = 0;
        int jCorodinate = point.Item2+1;

        while (jCorodinate < map.GetLength(1)){
            Tuple<int,int> nextNode = Tuple.Create(point.Item1, jCorodinate);

            if (explored.Contains(nextNode) && !"-JL".Contains(map[nextNode.Item1,nextNode.Item2])){
                intersections++;
            }

            jCorodinate++;
        }

        if (intersections % 2 != 0){
            innerEdge.Add(point);
        }
    }

    return innerEdge.Count;
}
}