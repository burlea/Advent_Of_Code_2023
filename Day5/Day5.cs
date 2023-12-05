

using System.ComponentModel.DataAnnotations;

namespace Day3
{
  class Program
  {
    static void Main()
    {
        string[] lines = File.ReadAllLines("input.txt");

        Task1(lines);
        Task2(lines);
    }

    static void Task1(string [] input) {

        // Get initial seeds
        List<long> initialSeeds = input[0].Split().Skip(1).ToList().ConvertAll<long>(seed => Int64.Parse(seed));

        long currentLine = 3; 

        // seed-to-soil
        Dictionary<Tuple<long,long>,long> seedToSoil = [];
        currentLine = FillDictionary(input, currentLine, seedToSoil);

        // soil-to-fertilizer
        Dictionary<Tuple<long,long>,long> soilToFertilizer = [];
        currentLine = FillDictionary(input, currentLine, soilToFertilizer);

        //fertilizer-to-water
        Dictionary<Tuple<long,long>,long> fertilizerToWater = [];
        currentLine = FillDictionary(input, currentLine, fertilizerToWater);

        // water-to-light
        Dictionary<Tuple<long,long>,long> waterToLight = [];
        currentLine = FillDictionary(input, currentLine, waterToLight);

        // light-to-temperature
        Dictionary<Tuple<long,long>,long> lightToTemperature = [];
        currentLine = FillDictionary(input, currentLine, lightToTemperature);

        // temperature-to-humidity
        Dictionary<Tuple<long,long>,long> temperatureToHumidity = [];
        currentLine = FillDictionary(input, currentLine, temperatureToHumidity);

        // humidity-to-location
        Dictionary<Tuple<long,long>,long> humidityToLocation = [];
        currentLine = FillDictionary(input, currentLine, humidityToLocation);

        long currentLowestLocationNumber = Int64.MaxValue;

        foreach(long seed in initialSeeds){

            long soil = GetValueFromMapping(seedToSoil, seed);
            long fertilizer = GetValueFromMapping(soilToFertilizer, soil);
            long water = GetValueFromMapping(fertilizerToWater, fertilizer);
            long light = GetValueFromMapping(waterToLight, water);
            long temperature = GetValueFromMapping(lightToTemperature, light);
            long humidity = GetValueFromMapping(temperatureToHumidity, temperature);
            long location = GetValueFromMapping(humidityToLocation, humidity);

            if (location < currentLowestLocationNumber) {
                currentLowestLocationNumber = location;
            }
        }

        Console.WriteLine(currentLowestLocationNumber);
    }

    static long FillDictionary(string[] input, long currentLine, Dictionary<Tuple<long,long>,long> newMapping){

        for (long i = currentLine; i < input.Length; i++){
            if(input[i] == ""){
                currentLine = i + 2;
                break;
            } else {
                AddMapping(input[i], newMapping);
            }
        }

        return currentLine;
    }

    static void AddMapping(String mappingString, Dictionary<Tuple<long,long>,long> sourceToDestinationMap){

        List<long> mapping = mappingString.Split().ToList().ConvertAll<long>(item => Int64.Parse(item));

        long destinationStart = mapping[0];
        long sourceStart = mapping[1];
        long rangeLength = mapping[2];

        long sourceEnd = sourceStart + rangeLength - 1;
        long sourceToDestinationOffset = destinationStart - sourceStart;

        sourceToDestinationMap.Add(Tuple.Create(sourceStart, sourceEnd), sourceToDestinationOffset);
    }

    static long GetValueFromMapping(Dictionary<Tuple<long,long>,long> sourceToDestinationMap, long desiredElementToGetValue) {

        foreach(KeyValuePair<Tuple<long,long>,long> mapping in sourceToDestinationMap) {
            if (desiredElementToGetValue >= mapping.Key.Item1 && desiredElementToGetValue <= mapping.Key.Item2) {
                return desiredElementToGetValue + mapping.Value;
            }
        }

        return desiredElementToGetValue;
    }

    static List<Tuple<long,long>> GetValueListFromMapping(Dictionary<Tuple<long,long>,long> sourceToDestinationMap, List<Tuple<long,long>> sourceList) {

        sourceToDestinationMap = sourceToDestinationMap.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        List<Tuple<long,long>> destinationList = [];

        foreach(Tuple<long,long> sourceRange in sourceList){
            Tuple<long,long> currentSourceRange = sourceRange; 
            bool stillRemaining = true;

            foreach(KeyValuePair<Tuple<long,long>,long> mapping in sourceToDestinationMap) {

                if (currentSourceRange.Item1 >= mapping.Key.Item1 && currentSourceRange.Item2 <= mapping.Key.Item2) {

                    destinationList.Add(Tuple.Create(currentSourceRange.Item1 + mapping.Value, currentSourceRange.Item2 + mapping.Value));
                    stillRemaining = false;
                    break;

                } else if (currentSourceRange.Item1 >= mapping.Key.Item1 && currentSourceRange.Item1 <= mapping.Key.Item2 &&
                            currentSourceRange.Item2 >= mapping.Key.Item2){

                    destinationList.Add(Tuple.Create(currentSourceRange.Item1 + mapping.Value, mapping.Key.Item2 + mapping.Value));

                    if (mapping.Key.Item2 + 1 > currentSourceRange.Item2) {
                        stillRemaining = false;
                        break;
                    } else {
                        currentSourceRange = Tuple.Create(mapping.Key.Item2 + 1, currentSourceRange.Item2);
                    }
                } else if (currentSourceRange.Item2 >= mapping.Key.Item1 && currentSourceRange.Item2 <= mapping.Key.Item2 &&
                            currentSourceRange.Item1 <= mapping.Key.Item1){

                    destinationList.Add(Tuple.Create(mapping.Key.Item1 + mapping.Value, currentSourceRange.Item2 + mapping.Value));

                    if (mapping.Key.Item1 - 1 < currentSourceRange.Item1) {
                        stillRemaining = false;
                        break;

                    } else {
                        destinationList.Add(Tuple.Create(currentSourceRange.Item1, mapping.Key.Item1 - 1));
                        stillRemaining = false;
                        break;
                    }

                } else if (currentSourceRange.Item1 < mapping.Key.Item1 && currentSourceRange.Item2 > mapping.Key.Item2) {
                    destinationList.Add(Tuple.Create(mapping.Key.Item1 + mapping.Value, mapping.Key.Item2 + mapping.Value));
                    destinationList.Add(Tuple.Create(currentSourceRange.Item1, mapping.Key.Item1 - 1));
                    currentSourceRange = Tuple.Create(mapping.Key.Item2 + 1, currentSourceRange.Item2);
                }
            }

            if (stillRemaining) {
                destinationList.Add(currentSourceRange);
            }
        }

        return destinationList;
    }

    static void Task2(string [] input){

         // Get initial seeds
        List<long> inputInitialSeeds = input[0].Split().Skip(1).ToList().ConvertAll<long>(seed => Int64.Parse(seed));

        List<Tuple<long,long>> initialSeeds = [];

        for (int i = 0; i < inputInitialSeeds.Count; i += 2){
            long start = inputInitialSeeds[i];
            long length = inputInitialSeeds[i+1];

            initialSeeds.Add(Tuple.Create(start, start+length-1));
        }

        long currentLine = 3; 

        // seed-to-soil
        Dictionary<Tuple<long,long>,long> seedToSoil = [];
        currentLine = FillDictionary(input, currentLine, seedToSoil);

        // soil-to-fertilizer
        Dictionary<Tuple<long,long>,long> soilToFertilizer = [];
        currentLine = FillDictionary(input, currentLine, soilToFertilizer);

        //fertilizer-to-water
        Dictionary<Tuple<long,long>,long> fertilizerToWater = [];
        currentLine = FillDictionary(input, currentLine, fertilizerToWater);

        // water-to-light
        Dictionary<Tuple<long,long>,long> waterToLight = [];
        currentLine = FillDictionary(input, currentLine, waterToLight);

        // light-to-temperature
        Dictionary<Tuple<long,long>,long> lightToTemperature = [];
        currentLine = FillDictionary(input, currentLine, lightToTemperature);

        // temperature-to-humidity
        Dictionary<Tuple<long,long>,long> temperatureToHumidity = [];
        currentLine = FillDictionary(input, currentLine, temperatureToHumidity);

        // humidity-to-location
        Dictionary<Tuple<long,long>,long> humidityToLocation = [];
        currentLine = FillDictionary(input, currentLine, humidityToLocation);

        List<Tuple<long,long>> soil = GetValueListFromMapping(seedToSoil, initialSeeds);
        List<Tuple<long,long>> fertilizer = GetValueListFromMapping(soilToFertilizer, soil);
        List<Tuple<long,long>> water = GetValueListFromMapping(fertilizerToWater, fertilizer);
        List<Tuple<long,long>> light = GetValueListFromMapping(waterToLight, water);
        List<Tuple<long,long>> temperature = GetValueListFromMapping(lightToTemperature, light);
        List<Tuple<long,long>> humidity = GetValueListFromMapping(temperatureToHumidity, temperature);
        List<Tuple<long,long>> location = GetValueListFromMapping(humidityToLocation, humidity);

        location.Sort((x,y) => x.Item1.CompareTo(y.Item1));

        long lowestLocation = location[0].Item1;

        Console.WriteLine(lowestLocation);
    }
  }
}