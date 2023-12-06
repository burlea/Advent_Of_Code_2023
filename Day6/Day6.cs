using System.Text.RegularExpressions;

partial class Program
{
static void Main()
{
    string[] lines = File.ReadAllLines("input.txt");

    Task1(lines);
    Task2(lines);
}

static void Task1(string [] input) {
    string timesString = input[0];
    string distanceString = input[1];

    List<int> times = timesString.Split(':',' ').Skip(1).Where(temp => temp != "").ToList().ConvertAll<int>(time => Int32.Parse(time));
    List<int> dist = distanceString.Split(':', ' ').Skip(1).Where(dist => dist != "").ToList().ConvertAll<int>(dist => Int32.Parse(dist));

    int result = 1;

    for (int i = 0; i < times.Count; i++){
        result *= GetNumberWaysToBeat(times[i], dist[i]);
    }

    Console.WriteLine(result);
    
}

static int GetNumberWaysToBeat(int time, int distance) {

    int numberWaysToWin = 0;

    for (int i = 1; i < time-1; i++) {

        double speed = i;
        double remaingTime = time - i;

        double distanceTraveled = speed * remaingTime;

        if (distanceTraveled > distance) {
            numberWaysToWin++;
        }
    }

    return numberWaysToWin;
}

static long GetNumberWaysToBeat(long time, long distance) {

    long numberWaysToWin = 0;

    for (long i = 1; i < time-1; i++) {

        double speed = i;
        double remaingTime = time - i;

        double distanceTraveled = speed * remaingTime;

        if (distanceTraveled > distance) {
            numberWaysToWin++;
        }
    }

    return numberWaysToWin;
}

static void Task2(string [] input){

    string timesString = input[0];
    string distanceString = input[1];

    long time = Int64.Parse(s: MyRegex().Replace(timesString.Split(':')[1], ""));
    long dist = Int64.Parse(s: MyRegex().Replace(distanceString.Split(':')[1], ""));


    long result = GetNumberWaysToBeat(time, dist);

    Console.WriteLine(result);
}

    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();
}
