using System.Text;


namespace Day14
{
  class Program
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

    static void TiltMapNorth(char [,] map){

        List<int> placesToPutRocks = [];

        for (int j = 0; j < map.GetLength(1); j++){
            for (int i = 0; i < map.GetLength(0); i++){
                if (map[i,j] == '.'){
                    placesToPutRocks.Add(i);
                } else if (map[i,j] == '#'){
                    placesToPutRocks = [];
                } else {
                    if (placesToPutRocks.Count != 0){
                        map[i,j] = '.';
                        map[placesToPutRocks.First(), j] = 'O';
                        placesToPutRocks.RemoveAt(0);
                        placesToPutRocks.Add(i);
                    }
                }
            }

            placesToPutRocks = [];
        }
    }

    static void TiltMapSouth(char [,] map){

        List<int> placesToPutRocks = [];

        for (int j = 0; j < map.GetLength(1); j++){
            for (int i = map.GetLength(0) - 1; i >=0; i--){
                if (map[i,j] == '.'){
                    placesToPutRocks.Add(i);
                } else if (map[i,j] == '#'){
                    placesToPutRocks = [];
                } else {
                    if (placesToPutRocks.Count != 0){
                        map[i,j] = '.';
                        map[placesToPutRocks.First(), j] = 'O';
                        placesToPutRocks.RemoveAt(0);
                        placesToPutRocks.Add(i);
                    }
                }
            }

            placesToPutRocks = [];
        }
    }

    static void TiltMapWest(char [,] map){

        List<int> placesToPutRocks = [];

        for (int i = 0; i < map.GetLength(0); i++){
            for (int j = 0; j < map.GetLength(1); j++){
                if (map[i,j] == '.'){
                    placesToPutRocks.Add(j);
                } else if (map[i,j] == '#'){
                    placesToPutRocks = [];
                } else {
                    if (placesToPutRocks.Count != 0){
                        map[i,j] = '.';
                        map[i, placesToPutRocks.First()] = 'O';
                        placesToPutRocks.RemoveAt(0);
                        placesToPutRocks.Add(j);
                    }
                }
            }

            placesToPutRocks = [];
        }
    }

    static void TiltMapEast(char [,] map){

        List<int> placesToPutRocks = [];

        for (int i = 0; i < map.GetLength(0); i++){
            for (int j = map.GetLength(1) -1; j >= 0; j--){
                if (map[i,j] == '.'){
                    placesToPutRocks.Add(j);
                } else if (map[i,j] == '#'){
                    placesToPutRocks = [];
                } else {
                    if (placesToPutRocks.Count != 0){
                        map[i,j] = '.';
                        map[i, placesToPutRocks.First()] = 'O';
                        placesToPutRocks.RemoveAt(0);
                        placesToPutRocks.Add(j);
                    }
                }
            }

            placesToPutRocks = [];
        }
    }

    static int CalculateLoad(char[,] map){

        int load = 0;

        for (int i = 0; i < map.GetLength(0); i++){
            int rowLoad = map.GetLength(0) - i;

            for (int j = 0; j < map.GetLength(1); j++) {
                if (map[i,j] == 'O'){
                    load += rowLoad;
                }
            }
        }

        return load;
    }

    static void Task1(char[,] map) {
        TiltMapNorth(map);

        int load = CalculateLoad(map);

        Console.WriteLine(load);
    }

    static void DoCycle(char[,] map){
        TiltMapNorth(map);
        TiltMapWest(map);
        TiltMapSouth(map);
        TiltMapEast(map);
    }

    static string ConvertToKey(char[,] map){

        StringBuilder key = new StringBuilder("");

        for (int i = 0; i < map.GetLength(0); i++){

            int numberInGroup = 0;
            string numbersInRowString = "";

            for (int j = 0; j < map.GetLength(1); j++){
                if (map[i,j] == 'O'){
                    numberInGroup++;
                } else if (map[i,j] == '#'){
                    numbersInRowString = numbersInRowString + numberInGroup + ",";
                    numberInGroup = 0;
                }
            }
            
            numbersInRowString = numbersInRowString + numberInGroup + ",";
            key.Append(numbersInRowString + "|");
        }

        return key.ToString();
    }

    static char[,] GetMapBase(char[,] map){
        char[,] mapBase = new char[map.GetLength(0),map.GetLength(1)];

        for (int i = 0; i < map.GetLength(0); i++){
            for (int j = 0; j < map.GetLength(1); j++){
                if (map[i,j] == '#'){
                    mapBase[i,j] = '#';
                } else {
                    mapBase[i,j] = '.';
                }
            }
        }
        return mapBase;
    }

    static char[,] ConvertToMap(string mapKey, char[,] mapBase){

        string[] rowGroupings = [.. mapKey.Split("|", StringSplitOptions.RemoveEmptyEntries)];
        char[,] newMap = (char[,]) mapBase.Clone();

        for (int i = 0; i < mapBase.GetLength(0); i++){

            List<int> groupings = rowGroupings[i].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(group => Int32.Parse(group));
            int currentOsToAdd = groupings.Last();
            groupings.RemoveAt(groupings.Count-1);

            for (int j = mapBase.GetLength(1) - 1; j >=0; j--){
                
                if (j == mapBase.GetLength(1) - 1 && mapBase[i,j] == '#'){
                    currentOsToAdd = groupings.Last();
                    groupings.RemoveAt(groupings.Count-1);
                } else if (j == mapBase.GetLength(1) - 1 && currentOsToAdd > 0){
                    newMap[i,j] = 'O';
                    currentOsToAdd--;
                } else if (mapBase[i,j] == '#'){
                    currentOsToAdd = groupings.Last();
                    groupings.RemoveAt(groupings.Count-1);
                } else if (mapBase[i,j] == '.' && currentOsToAdd > 0){
                    newMap[i,j] = 'O';
                    currentOsToAdd--;
                }
            }
        }
        return newMap;
    }

    static void Task2(char[,] map){

        char[,] mapBase = GetMapBase(map);

        Dictionary<string,string> mapToCycledMap = [];

        DoCycle(map);
        string mapKey = ConvertToKey(map);

        long loopFor = 1000000000;

        for (long i = 0; i < loopFor-1; i++){
            if (mapToCycledMap.TryGetValue(mapKey, out string cycledMapKey)){
                mapKey = cycledMapKey;
            } else {
                string mapBefore = mapKey;
                map = ConvertToMap(mapKey, mapBase);
                DoCycle(map);
                mapKey = ConvertToKey(map);
                mapToCycledMap[key: mapBefore] = mapKey;
            }
        }

        int load = CalculateLoad(ConvertToMap(mapKey, mapBase));
        Console.WriteLine(load);
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
}