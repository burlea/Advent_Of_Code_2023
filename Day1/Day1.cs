using System;

namespace Day1
{
  class Program
  {
    static void Main(string[] args)
    {
      Task1();
      Task2();
    }

    static void Task1() {
      string[] lines = File.ReadAllLines("task1.txt");

      int sum = 0;

      foreach (string line in lines) {

        string numberString = new string(line.Where(c => Char.IsDigit(c)).ToArray());
        string numberStringFirstLast = "" + numberString[0] + "" + numberString[numberString.Length-1];
        int number = Int32.Parse(numberStringFirstLast);
        sum = sum + number;
      }

      Console.WriteLine(sum);
    }

    static void Task2(){

      string[] lines = File.ReadAllLines("task1.txt");

      int newSum = 0;

      foreach (string line in lines) {
        List<int> numbersInString = new List<int>();
        int lineSize = line.Length;

        for (int i = 0; i < lineSize; i++) {
      
          if (Char.IsDigit(line[i])){
            numbersInString.Add(line[i] - '0');
          }

          if (line[i] == 'z') {
            if (lineSize -1 - i >= 3) {
              if (line.Substring(i, 4).Equals("zero")){
                numbersInString.Add(0);
              }
            }
          }

          if (line[i] == 'o') {
            if (lineSize -1 - i >= 2) {
              if (line.Substring(i, 3).Equals("one")){
                numbersInString.Add(1);
              }
            }
          }

          if (line[i] == 't') {
            if (lineSize -1 - i >= 2) {
              if (line.Substring(i, 3).Equals("two")){
                numbersInString.Add(2);
              }
            }
          }

          if (line[i] == 't') {
            if (lineSize -1 - i >= 4) {
              if (line.Substring(i, 5).Equals("three")){
                numbersInString.Add(3);
              }
            }
          }

          if (line[i] == 'f') {
            if (lineSize -1 - i >= 3) {
              if (line.Substring(i, 4).Equals("four")){
                numbersInString.Add(4);
              }
            }
          }

          if (line[i] == 'f') {
            if (lineSize -1 - i >= 3) {
              if (line.Substring(i, 4).Equals("five")){
                numbersInString.Add(5);
              }
            }
          }

          if (line[i] == 's') {
            if (lineSize -1 - i >= 2) {
              if (line.Substring(i, 3).Equals("six")){
                numbersInString.Add(6);
              }
            }
          }

          if (line[i] == 's') {
            if (lineSize -1 - i >= 4) {
              if (line.Substring(i, 5).Equals("seven")){
                numbersInString.Add(7);
              }
            }
          }

          if (line[i] == 'e') {
            if (lineSize -1 - i >= 4) {
              if (line.Substring(i, 5).Equals("eight")){
                numbersInString.Add(8);
              }
            }
          }

          if (line[i] == 'n') {
            if (lineSize -1 - i >= 3) {
              if (line.Substring(i, 4).Equals("nine")){
                numbersInString.Add(9);
              }
            }
          }
        }

        string numberStringFirstLast = "" + numbersInString.First() + "" + numbersInString.Last();
        int number = Int32.Parse(numberStringFirstLast);
        newSum = newSum + number;
      }
      Console.WriteLine(newSum);
    }
  }
}