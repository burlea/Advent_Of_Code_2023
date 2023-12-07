using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO.Pipes;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Channels;

partial class Program
{
    
static void Main()
{
    string[] lines = File.ReadAllLines("input.txt");

    Task1(lines);
    Task2(lines);
}

static void Task1(string [] input) {

    Dictionary<char, int> cardValues = new Dictionary<char, int>{
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'J', 10},
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1}
    };

    List<Tuple<string,int,int>> cardInformation = []; // hand, bid, score

    foreach (string line in input) {
        string[] pieces = line.Split();      

        string hand = pieces[0];

        int bid = Int32.Parse(pieces[1]);

        int score = GetSpecialScore(hand); 

        cardInformation.Add(Tuple.Create(hand, bid, score));
    }

    cardInformation.Sort((cardInfo1, cardInfo2) => 
        {
            if (cardInfo1.Item3 != cardInfo2.Item3) {
                return cardInfo1.Item3.CompareTo( cardInfo2.Item3);
            } else {
                for(int i = 0; i<5; i++){
                    int cardScore1 = cardValues[cardInfo1.Item1[i]];
                    int cardScore2 = cardValues[cardInfo2.Item1[i]];

                    if (cardScore1 != cardScore2){
                        return cardScore1.CompareTo(cardScore2);
                    }
                }
                return 0;
            }
        });

    int sum = 0;
    int currentRank = 1;

    foreach(Tuple<string, int, int> entry in cardInformation) {
        sum += entry.Item2 * currentRank;
        currentRank++;
    }
    Console.WriteLine(sum);
}

static void Task2(string [] input){

    Dictionary<char, int> cardValues = new Dictionary<char, int>{
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1},
        {'J', 0}
    };

    List<Tuple<string,int,int>> cardInformation = []; // hand, bid, score

    foreach (string line in input) {
        string[] pieces = line.Split();      
        string hand = pieces[0];
        int bid = Int32.Parse(pieces[1]);
        int score = GetSpecialScoreJack(hand);

        cardInformation.Add(Tuple.Create(hand, bid, score));
    }

    cardInformation.Sort((cardInfo1, cardInfo2) => 
        {
            if (cardInfo1.Item3 != cardInfo2.Item3) {
                return cardInfo1.Item3.CompareTo( cardInfo2.Item3);
            } else {
                for(int i = 0; i<5; i++){
                    int cardScore1 = cardValues[cardInfo1.Item1[i]];
                    int cardScore2 = cardValues[cardInfo2.Item1[i]];

                    if (cardScore1 != cardScore2){
                        return cardScore1.CompareTo(cardScore2);
                    }
                }
                return 0;
            }
        });

    int sum = 0;
    int currentRank = 1;

    foreach(Tuple<string, int, int> entry in cardInformation) {
        sum += entry.Item2 * currentRank;
        currentRank++;
    }

    Console.WriteLine(sum);

}

static int GetSpecialScore(string hand) {

    Dictionary<char,int> cardCounts = [];

    foreach(char card in hand){
        if (cardCounts.TryGetValue(card, out int value)){
            cardCounts[card] = value + 1;
        } else {
            cardCounts[card] = 1;
        }
    }

    int score = 0;

    Dictionary<int,int> countCounts = []; // How many times a '5' count was hit;

    foreach(KeyValuePair<char, int> card in cardCounts) {
        if (countCounts.TryGetValue(card.Value, out int value)){
            countCounts[card.Value] = value + 1;
        } else {
            countCounts[card.Value] = 1;
        }
    }

    if (countCounts.ContainsKey(5)){ // five of a kind
        score += 700;
    } else if (countCounts.ContainsKey(4)){ // four of a kind 
        score += 600;
    } else if (countCounts.ContainsKey(3) && countCounts.ContainsKey(2)) { // full house
        score += 500;
    } else if (countCounts.ContainsKey(3) && !countCounts.ContainsKey(2)){ // three of a kind 
        score += 400;
    } else if (countCounts.ContainsKey(2) && countCounts.ContainsKey(1) && countCounts[2] == 2 && countCounts[1] == 1) { // two pair
        score += 300;
    } else if (countCounts.ContainsKey(2) && countCounts.ContainsKey(1) && countCounts[2] == 1 && countCounts[1] == 3) { // One pair
        score += 200;
    } else if (countCounts.ContainsKey(1) && countCounts[1] == 5) {
        score += 100;
    }    
    return score;
}

static int GetSpecialScoreJack(string hand) {

    Dictionary<char,int> cardCounts = [];

    if (hand == "JJJJJ"){
        return 700;
    }

    foreach(char card in hand){
        if (cardCounts.TryGetValue(card, out int value)){
            cardCounts[card] = value + 1;
        } else {
            cardCounts[card] = 1;
        }
    }

    List<Tuple<char, int>> sortedCardCounts = [];

    if (cardCounts.TryGetValue('J', out int totalJacks)){
        cardCounts.Remove('J');

        foreach(KeyValuePair < char, int > pair in cardCounts){
            sortedCardCounts.Add(Tuple.Create(pair.Key, pair.Value));
        }

        sortedCardCounts = [.. sortedCardCounts.OrderByDescending(pair => pair.Item2)];
        sortedCardCounts[0] = Tuple.Create(sortedCardCounts[0].Item1, sortedCardCounts[0].Item2 + totalJacks);

    } else {

        foreach(KeyValuePair < char, int > pair in cardCounts){
            sortedCardCounts.Add(Tuple.Create(pair.Key, pair.Value));
        }
        sortedCardCounts = [.. sortedCardCounts.OrderByDescending(pair => pair.Item2)];
    }

    Dictionary<int,int> countCounts = []; // How many times a '5' count was hit;

    foreach(Tuple<char, int> card in sortedCardCounts) {
        if (countCounts.TryGetValue(card.Item2, out int value)){
            countCounts[card.Item2] = value + 1;
        } else {
            countCounts[card.Item2] = 1;
        }
    }

    if (countCounts.ContainsKey(5)){ // five of a kind
        return 700;
    } else if (countCounts.ContainsKey(4)){ // four of a kind 
        return 600;
    } else if (countCounts.ContainsKey(3) && countCounts.ContainsKey(2)) { // full house
        return 500;
    } else if (countCounts.ContainsKey(3) && !countCounts.ContainsKey(2)){ // three of a kind 
        return 400;
    } else if (countCounts.ContainsKey(2) && countCounts.ContainsKey(1) && countCounts[2] == 2 && countCounts[1] == 1) { // two pair
        return 300;
    } else if (countCounts.ContainsKey(2) && countCounts.ContainsKey(1) && countCounts[2] == 1 && countCounts[1] == 3) { // One pair
        return 200;
    } else if (countCounts.ContainsKey(1) && countCounts[1] == 5) {
        return 100;
    } else {
        return 0;
    }
}

}