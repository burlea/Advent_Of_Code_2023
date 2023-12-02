using System;
using System.Collections.Generic;

namespace Day2
{
  class Program
  {
    static void Main()
    {
      Task1();
      Task2();
    }

    static void Task1() {
      string[] lines = File.ReadAllLines("input.txt");

      int sum = 0;

      foreach (String line in lines) {
        Game game = new Game(line);

        if (!game.isOverLimit()){
          sum = sum + game.id;
        }
      }

      Console.WriteLine(sum);
    }

    static void Task2(){
      string[] lines = File.ReadAllLines("input.txt");

      int sum = 0;

      foreach (String line in lines) {
        Game game = new Game(line);
        sum = sum + game.getPower();
      }

      Console.WriteLine(sum);
    }
  }

  class Game {

    public int id;
    public List<Rounds> rounds = new List<Rounds>();
    private char[] delimiterCharacters = { ':', ';' };
    private int maxRed = 12;
    private int maxGreen = 13;
    private int maxBlue = 14;


    public Game(string input) {
      this.parseInput(input);
    }

    private void parseInput(String input){

      string[] words = input.Split(delimiterCharacters);
      string gameIdString = new String(words[0].Where(Char.IsDigit).ToArray());
      this.id = Int32.Parse(gameIdString);

      for (int i = 1; i < words.Length; i++) {
        rounds.Add(new Rounds(words[i]));
      }
    }

    public Boolean isOverLimit(){
      foreach (Rounds round in rounds) {
        if (round.blueBalls > maxBlue | round.redBalls > maxRed | round.greenBalls > maxGreen) {
          return true;
        }
      }
      return false;
    }

    public int getPower() {

      int maxBlue = 0;
      int maxRed = 0;
      int maxGreen = 0;

      foreach (Rounds round in rounds) {
        if (round.blueBalls > maxBlue) {
          maxBlue = round.blueBalls;
        }

        if (round.redBalls > maxRed) {
          maxRed = round.redBalls;
        }

        if (round.greenBalls > maxGreen) {
          maxGreen = round.greenBalls;
        }
      }

      return maxBlue * maxGreen * maxRed;
    }
  }

  class Rounds {
    public int redBalls;
    public int blueBalls;
    public int greenBalls;

    public Rounds(string input) {

      string[] ballStrings = input.Split(',');

      foreach(String ballString in ballStrings){
        switch(new String(ballString.Where(Char.IsLetter).ToArray())){
          case "blue": this.blueBalls = this.getNumberBalls(ballString); break;
          case "red": this.redBalls = this.getNumberBalls(ballString); break;
          case "green": this.greenBalls = this.getNumberBalls(ballString); break;
        }
      }
    }

    private int getNumberBalls(string ballString) {
      return Int32.Parse(new String(ballString.Where(Char.IsDigit).ToArray()));
    }
  }
}