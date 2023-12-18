using System;
using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace Day1
{
  class Program
  {
    static void Main(string[] args)
    {
        string[] input = File.ReadAllLines("input.txt");

        int [,] map = new int[input.Length,input[0].Length];

        for (int i = 0; i < input.Length; i++){
            string line = input[i];
            for (int j = 0; j < line.Length; j++) {
                map[i,j] = Int32.Parse(line[j].ToString());
            }
        }

        Task1(map);
        Task2(map);
    }

    static int GetSmallestDistance(List<Node> nodesToCheck){
      
      int leastDistance = Int32.MaxValue;
      int index = -1;

      for (int i = 0; i < nodesToCheck.Count; i++){
        if (nodesToCheck[i].distance < leastDistance && nodesToCheck[i].hasSeenBefore == false){
          leastDistance = nodesToCheck[i].distance;
          index = i;
        }
      }

      return index;
    }

    static char GetSymbol(Direction direction){
            return direction switch
            {
                Direction.north => '^',
                Direction.east => '>',
                Direction.west => '<',
                Direction.south => 'v',
                _ => 'O',
            };
        }

    static void UpdateNeighbor(Node currentNode, Node nodeToGoTo, Direction directionMoved, List<Node> nodesToCheck){

      bool toPrint = currentNode.i == 1 && currentNode.j==4;

      if (toPrint){
        Console.WriteLine("Current Node: " + currentNode.ToString());
        Console.WriteLine("DIRECTION BEING CONSIDERED: " + directionMoved);
        Console.WriteLine(value: "nodeToGoTo Node: " + nodeToGoTo.ToString());

      }

      if (currentNode.directionMovedToGetHere != directionMoved){
        int newDistance = currentNode.distance + nodeToGoTo.cost;

        if (newDistance <= nodeToGoTo.distance){
          if (toPrint){
            Console.WriteLine("CHOSEN TO ADD");
          }
          nodeToGoTo.distance = newDistance;
          nodeToGoTo.directionMovedToGetHere = directionMoved;
          nodeToGoTo.currentTimesMovingStraight = 1;
          nodeToGoTo.previousNode = currentNode;
          nodesToCheck.Add(item: nodeToGoTo);
        }
      } else if (currentNode.directionMovedToGetHere == directionMoved && currentNode.currentTimesMovingStraight < 3){
        int newDistance = currentNode.distance + nodeToGoTo.cost;

        if (newDistance <= nodeToGoTo.distance){

          if (toPrint){
            Console.WriteLine("CHOSEN TO ADD");
          }

          nodeToGoTo.distance = newDistance;
          nodeToGoTo.directionMovedToGetHere = directionMoved;
          nodeToGoTo.currentTimesMovingStraight = currentNode.currentTimesMovingStraight+1;
          //path[currentNode.i,currentNode.j] = GetSymbol(directionMoved);
          nodeToGoTo.previousNode = currentNode;
          nodesToCheck.Add(item: nodeToGoTo);
        }
      }

      if (toPrint){
        Console.WriteLine(value: "nodeToGoTo Node After Processing: " + nodeToGoTo.ToString());
        Console.WriteLine("");
      }
    }

    static void MapTraversal(Node [,] map, List<Node> nodesToCheck){

      //char[,] path = new char[map.GetLength(0),map.GetLength(1)];

      while(nodesToCheck.Count != 0){
        
        //PrintList(message: "During Traversal Nodes To Check", nodesToCheck);

        int currentNodeIndex = GetSmallestDistance(nodesToCheck);

        if (currentNodeIndex == -1){
          break;
        }

        Node currentNode = nodesToCheck[currentNodeIndex];
        //currentNode.ToString();
        nodesToCheck[currentNodeIndex].hasSeenBefore = true;
        //nodesToCheck.RemoveAt(currentNodeIndex);

        // if (currentNode.i == map.GetLength(0)-1 && currentNode.j == map.GetLength(1)-1){
        //   break;
        // }
        // PrintList(message: "After removing", nodesToCheck);

        //Neighbors

         if (currentNode.i > 0 && currentNode.directionMovedToGetHere != Direction.south){
           Node nodeAbove = map[currentNode.i - 1, currentNode.j];
           UpdateNeighbor(currentNode, nodeAbove, Direction.north, nodesToCheck);
          }

          if (currentNode.i < map.GetLength(0)-1 && currentNode.directionMovedToGetHere != Direction.north){
           Node nodeBelow = map[currentNode.i + 1, currentNode.j];
           UpdateNeighbor(currentNode, nodeBelow, Direction.south, nodesToCheck);
          }

          if (currentNode.j > 0 && currentNode.directionMovedToGetHere != Direction.east){
           Node nodeLeft = map[currentNode.i, currentNode.j - 1];
           UpdateNeighbor(currentNode, nodeLeft, Direction.west, nodesToCheck);
          }

          if (currentNode.j < map.GetLength(1)-1 && currentNode.directionMovedToGetHere != Direction.west){
           Node nodeRight = map[currentNode.i, currentNode.j+1];
           UpdateNeighbor(currentNode, nodeRight, Direction.east, nodesToCheck);
          }

          // PrintList(message: "After Adding", nodesToCheck);
      }

      //PrintMap("Path Traversed",path);
    }

    static void Task1(int [,] map) {

      Node[,] graph = new Node[map.GetLength(0),map.GetLength(1)];
      List<Node> nodesToCheck = [];

      for (int i = 0; i < map.GetLength(0); i++){
        for (int j = 0; j < map.GetLength(1); j++) {
          graph[i,j] = new Node(i,j,map[i,j]);

          if (i==0 && j == 0){
            graph[i,j].distance = 0;
            graph[i,j].directionMovedToGetHere = Direction.none;
            graph[i,j].currentTimesMovingStraight =0;
            nodesToCheck.Add(graph[i,j]);
          }
          //nodesToCheck.Add(graph[i,j]);
        }
      }

      // PrintMap("Initial", graph);
      // PrintList("Initial Nodes To Check", nodesToCheck);

      MapTraversal(graph, nodesToCheck );

      // PrintMap("After Traversal", graph);
      // PrintPath(graph);
      // PrintList(message: "After Traversal Nodes To Check", nodesToCheck);

      int distanceToGoal = GetResult(nodesToCheck, graph);

      Console.WriteLine(distanceToGoal);
    }

    static int GetResult(List<Node> nodesToCheck, Node[,] map){

      int leastDistance = Int32.MaxValue;
      Node nodeEnd = null;

      for (int i = 0; i < nodesToCheck.Count; i++){
        if (nodesToCheck[i].distance < leastDistance && nodesToCheck[i].i == map.GetLength(0)-1 && nodesToCheck[i].j == map.GetLength(1)-1){
          leastDistance = nodesToCheck[i].distance;
          nodeEnd = nodesToCheck[i];
        }
      }

      PrintPath(map, nodeEnd);

      return leastDistance;
    }

    static void PrintPath(Node[,] map, Node currentNode){
      char[,] path = new char[map.GetLength(0), map.GetLength(1)];

      for (int i = 0; i < path.GetLength(0);i++){
        for (int j = 0; j < path.GetLength(1);j++){
          path[i,j] = Char.Parse(map[i,j].cost.ToString());
        }
      }
      
      while (currentNode.previousNode != null){
        path[currentNode.i,currentNode.j] = GetSymbol(currentNode.directionMovedToGetHere);
        currentNode = currentNode.previousNode;
      }

      PrintMap("Path", path);
    }

    static void PrintMap(string message, Node[,] graph){

      Console.WriteLine(message);

      for (int i = 0; i < graph.GetLength(0); i++){
        for (int j = 0; j < graph.GetLength(1); j++){

          Console.WriteLine(graph[i,j].ToString());
        }
      }
    }

    static void PrintList(string message, List<Node> list){

      Console.WriteLine(message);

      for (int i = 0; i < list.Count; i++){
        Console.WriteLine("Index: " + i + " Node: " + list[i].ToString());
      }
    }

     static void PrintMap(string message, char[,] map) {

      Console.WriteLine(message + ": ");

      for (int i = 0; i < map.GetLength(0); i++){
          for(int j = 0; j < map.GetLength(1); j++) {
              Console.Write(map[i,j] + " ");
          }
          Console.WriteLine("");
      }

      Console.WriteLine("");
    }

    static void Task2(int [,] map){

    }

}

class Node(int i, int j, int cost){
  public int i = i;
  public int j = j;
  public int cost = cost;
  public int distance = Int32.MaxValue;
  public Direction directionMovedToGetHere = Direction.none;
  public int currentTimesMovingStraight = 0;
  public Node previousNode;
  public bool hasSeenBefore;

  public override string ToString(){

    return $"Point: ({i},{j}) Cost: {cost} Distance: {distance} Current Times Moving Straight: {currentTimesMovingStraight} + Direction: {directionMovedToGetHere}";
  }
}

enum Direction {
  none,
  north,
  south,
  east,
  west
}
}