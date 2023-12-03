using System;
using System.ComponentModel;
using MathNet.Numerics;

namespace Day3
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
      char[][]? input = new char [lines.Length][];

      for (int i = 0; i < lines.Length; i++){
        char [] lineArray = new char[lines[i].Length];
        for (int j = 0; j < lines[i].Length; j++) {
          lineArray[j] = lines[i][j];
        }
        input[i] = lineArray;
      }
      bool toAdd = false;
      String currentNumber = "";
      int sum = 0;

      for (int i = 0; i < lines.Length; i++){
        for (int j = 0; j < lines[i].Length; j++) {

          if (Char.IsDigit(input[i][j])) {
            currentNumber = currentNumber + "" + input[i][j];

            if (i > 0 && j > 0) {
              if (IsSymbol(input[i-1][j-1])) { // top left
                toAdd = true;
              }
            }

          if (i > 0) {
            if (IsSymbol(input[i-1][j])) { // top mid
              toAdd = true;
            }
          }

          if (i > 0 && j < lines[i].Length - 1) {
            if (IsSymbol(input[i-1][j+1])) { // top right
              toAdd = true;
            }
          }

          if (j > 0) {
            if (IsSymbol(input[i][j-1])) { // mid left
              toAdd = true;
            }
          }

          if (j <  lines[i].Length - 1) {
            if (IsSymbol(input[i][j+1])) { // mid right
              toAdd = true;
            }
          }

          if (i < lines.Length - 1 && j > 0) {
            if (IsSymbol(input[i+1][j-1])) { // bottom left
              toAdd = true;
            }
          }

          if (i < lines.Length - 1) {
            if (IsSymbol(input[i+1][j])) { // bottom mid
              toAdd = true;
            }
          }

          if (i < lines.Length - 1 && j <  lines[i].Length - 1) {
            if (IsSymbol(input[i+1][j+1])) { // bottom right
              toAdd = true;
            }
          }
        } else {
          if (toAdd){
            sum += Int32.Parse(currentNumber);
            toAdd = false;
          }
          currentNumber = "";
        }
      }
    }

    Console.WriteLine(sum);
    }

    static bool IsSymbol(char input){
      return (!Char.IsDigit(input) && input != '.');
    }

    static bool IsNumber(char input){
      return Char.IsDigit(input);
    }

    static int GetWholeNumber(string stringToSearch, int indexOfFoundNumber){

      string currentNumber = "" + stringToSearch[indexOfFoundNumber];

      if (indexOfFoundNumber < stringToSearch.Length - 1){
        for (int i = indexOfFoundNumber + 1; i < stringToSearch.Length; i++) {
          if (Char.IsDigit(stringToSearch[i])){
            currentNumber = currentNumber + "" + stringToSearch[i];
          } else {
            break;
          }
        }
      }

      if (indexOfFoundNumber > 0) {
        for (int i = indexOfFoundNumber - 1; i >= 0 ; i--) {
          if (Char.IsDigit(stringToSearch[i])){
            currentNumber = stringToSearch[i] + "" + currentNumber;
          } else {
            break;
          }
        }
      }
      return Int32.Parse(currentNumber);
    }

    static void Task2(){

      string[] lines = File.ReadAllLines("input.txt");
      char[][]? input = new char [lines.Length][];

      for (int i = 0; i < lines.Length; i++){
        char [] lineArray = new char[lines[i].Length];

        for (int j = 0; j < lines[i].Length; j++) {
          lineArray[j] = lines[i][j];
        }
        input[i] = lineArray;
      }
      int numberAdjacent = 0;
      int currentGearRatio = 0;
      int sum = 0;

      bool isCurrentlyLookingAtSameNumber = false;

      for (int i = 0; i < lines.Length; i++){
        for (int j = 0; j < lines[i].Length; j++) {

          if (IsSymbol(input[i][j])) {

            if (i > 0 && j > 0) {
              if (IsNumber(input[i-1][j-1])) { // top left
                numberAdjacent++;
                int number = GetWholeNumber(lines[i-1], j-1);
                currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
                isCurrentlyLookingAtSameNumber = true;
              } else {
                isCurrentlyLookingAtSameNumber = false;
              }
            }

          if (i > 0) {
            if (IsNumber(input[i-1][j])) { // top mid
                if (!isCurrentlyLookingAtSameNumber){
                  numberAdjacent++;
                  int number = GetWholeNumber(lines[i-1], j);
                  currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
                  isCurrentlyLookingAtSameNumber = true;
                }
            } else {
              isCurrentlyLookingAtSameNumber = false;
            }
          }

          if (i > 0 && j < lines[i].Length - 1) {
            if (IsNumber(input[i-1][j+1])) { // top right
              if (!isCurrentlyLookingAtSameNumber){
                numberAdjacent++;
                int number = GetWholeNumber(lines[i-1], j+1);
                currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
                isCurrentlyLookingAtSameNumber = true;
              }
            } else {
              isCurrentlyLookingAtSameNumber = false;
            }
          }

          isCurrentlyLookingAtSameNumber = false;

          if (j > 0) {
            if (IsNumber(input[i][j-1])) { // mid left
                numberAdjacent++;
                int number = GetWholeNumber(lines[i], j-1);
                currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
            }
          }

          if (j <  lines[i].Length - 1) {
            if (IsNumber(input[i][j+1])) { // mid right
                numberAdjacent++;
                int number = GetWholeNumber(lines[i], j+1);
                currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
            }
          }

          if (i < lines.Length - 1 && j > 0) {
            if (IsNumber(input[i+1][j-1])) { // bottom left
                numberAdjacent++;
                int number = GetWholeNumber(lines[i+1], j-1);
                currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
                isCurrentlyLookingAtSameNumber = true;
            } else {
                isCurrentlyLookingAtSameNumber = false;
            }
          }

          if (i < lines.Length - 1) {
            if (IsNumber(input[i+1][j])) { // bottom mid
             if (!isCurrentlyLookingAtSameNumber){
                numberAdjacent++;
                int number = GetWholeNumber(lines[i+1], j);
                currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
                isCurrentlyLookingAtSameNumber = true;
             }
            } else {
                isCurrentlyLookingAtSameNumber = false;
            }
          }

          if (i < lines.Length - 1 && j <  lines[i].Length - 1) {
            if (IsNumber(input[i+1][j+1])) { // bottom right
             if (!isCurrentlyLookingAtSameNumber){
                numberAdjacent++;
                int number = GetWholeNumber(lines[i+1], j+1);
                currentGearRatio = (currentGearRatio == 0) ? number : currentGearRatio * number; 
                isCurrentlyLookingAtSameNumber = true;
             }
            } else {
                isCurrentlyLookingAtSameNumber = false;
            }
          }
        } else {
          if (numberAdjacent == 2){
            sum += currentGearRatio;
          }
          numberAdjacent = 0;
          currentGearRatio = 0;
          isCurrentlyLookingAtSameNumber = false;
        }
      }
    }

    Console.WriteLine(sum);
    }
  }
}
