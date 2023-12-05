using System;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Globalization;

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
        char[] delimiterCharacters = {':', '|'};
        int sum = 0;

        foreach (String card in input) {
            string[] parts = card.Split(delimiterCharacters);
            string[] winningNumbersString = parts[1].Split(' ',StringSplitOptions.RemoveEmptyEntries);
            string[] yourNumbersString = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int[] winningNumbers = new int[winningNumbersString.Length];
            int[] yourNumbers = new int[yourNumbersString.Length];

            for(int i = 0; i < winningNumbersString.Length; i++){
                winningNumbers[i] = Int32.Parse(winningNumbersString[i].Trim());
            }

            for(int i = 0; i < yourNumbersString.Length; i++){
                yourNumbers[i] = Int32.Parse(yourNumbersString[i].Trim());

            }

            int cardPoints = 0;
            int numberMatchesInCard = -1;

            for (int i = 0; i < yourNumbers.Length; i++) {
                if (winningNumbers.Contains(yourNumbers[i])){
                    numberMatchesInCard++;
                }
            }

            if (numberMatchesInCard >= 0) {
                cardPoints = (int) Math.Pow(2,numberMatchesInCard);
            }

            sum += cardPoints;
        }

        Console.WriteLine(sum);
    }

    static void Task2(string [] input){

        char[] delimiterCharacters = {':', '|'};

        Dictionary<int, int> cardMatches = new Dictionary<int, int>();
        List<int> cardsToSee = new List<int>();

        foreach (String card in input) {
            string[] parts = card.Split(delimiterCharacters);

            int cardId = Int32.Parse(Regex.Match(parts[0], @"\d+").Value);

            string[] winningNumbersString = parts[1].Split(' ',StringSplitOptions.RemoveEmptyEntries);
            string[] yourNumbersString = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int[] winningNumbers = new int[winningNumbersString.Length];
            int[] yourNumbers = new int[yourNumbersString.Length];

            for(int i = 0; i < winningNumbersString.Length; i++){
                winningNumbers[i] = Int32.Parse(winningNumbersString[i].Trim());
            }

            for(int i = 0; i < yourNumbersString.Length; i++){
                yourNumbers[i] = Int32.Parse(yourNumbersString[i].Trim());

            }

            int numberMatchesInCard = 0;

            for (int i = 0; i < yourNumbers.Length; i++) {
                if (winningNumbers.Contains(yourNumbers[i])){
                    numberMatchesInCard++;
                }
            }

            cardMatches.Add(cardId, numberMatchesInCard);
            cardsToSee.Add(cardId);
        }

        Dictionary<int, int> cardCount = new Dictionary<int, int>();

        getCounts(cardsToSee, cardCount, cardMatches);

        int sum  = cardCount.Sum(key => key.Value);

        Console.WriteLine(sum);
    }

    static void getCounts(List<int> cardsToSee, Dictionary<int, int> cardCount ,Dictionary<int, int> cardMatches){

        List<int> nextCardsToSee = new List<int>();
        int totalCards = cardMatches.Count;

        foreach(int cardId in cardsToSee){

            if (cardCount.ContainsKey(cardId)){
                cardCount[cardId] = cardCount[cardId] + 1;
            }else {
                cardCount.Add(key: cardId, 1);
            }

            int matches = cardMatches[cardId];

            for (int i = cardId + 1; i <= Math.Min(cardId + matches, totalCards); i++){
                nextCardsToSee.Add(i);
            }
        }

        if (nextCardsToSee.Count == 0){
            return;
        } else {
            getCounts(nextCardsToSee, cardCount, cardMatches);
        }
    }
  }
}

