using System;
using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Day19
{
    partial class Program
  {

    static long total = 0;
    static void Main(string[] args)
    {
        string[] input = File.ReadAllLines("input.txt");

        Task1(input);
        Task2(input);
    }

    static void Task1(string[] input){

        int cutOffPoint = 0;
        List<string> workflowStrings = [];

        for (int i = 0; i < input.Length; i++){
            if (input[i] == ""){
                cutOffPoint = i + 1;
                break;
            } else {
                string workflowString = input[i]; 
                workflowStrings.Add(workflowString);
            }
        }

        List<string> partsStrings = [];
        for (int i = cutOffPoint; i < input.Length; i++){
            string partsString = input[i]; 
            partsStrings.Add(partsString);   
        }

        List<Part> parts = GetParts(partsStrings);
        Dictionary<string, Workflow> workflows = GetWorkflows(workflowStrings);

        int totalSum = 0;

        foreach(Part part in parts){
            Workflow currentWorkflow = workflows["in"];
            bool decisionReached = false;

            while (!decisionReached){
                
                string nextWorkflowId = "";

                foreach(Step step in currentWorkflow.steps){
                    if (step.justEnd){
                        if (step.decision!=null){
                            decisionReached = true;
                            
                            if (step.decision=="A"){
                                totalSum += part.GetSum();
                            }
                            
                            break;
                        } else {
                            nextWorkflowId = step.nextWorkflow;
                            break;
                        }
                    } else {
                        int partRating = 0;

                        switch(step.element){
                            case 'x':
                                partRating = part.x;
                                break;
                            case 'm':
                                partRating = part.m;
                                break;
                            case 'a':
                                partRating = part.a;
                                break;
                            case 's':
                                partRating = part.s;
                                break;
                        }

                        bool doesMeetCondition;

                        if (step.operation == '>'){
                            doesMeetCondition = partRating > step.contraint;
                        } else {
                            doesMeetCondition = partRating < step.contraint;
                        }

                        if (doesMeetCondition){
                            if (step.decision != null){
                                decisionReached = true;
                            
                                if (step.decision=="A"){
                                    totalSum += part.GetSum();
                                }

                                break;
                            } else {
                                nextWorkflowId = step.nextWorkflow;
                                break;
                            }
                        }
                    }
                }

                if (nextWorkflowId != ""){
                    currentWorkflow = workflows[nextWorkflowId];
                }  
            }
        }

        Console.WriteLine(totalSum);
    }


    static void Task2(string[] input){

        List<string> workflowStrings = [];

        for (int i = 0; i < input.Length; i++){
            if (input[i] == ""){
                break;
            } else {
                string workflowString = input[i]; 
                workflowStrings.Add(workflowString);
            }
        }
        
        Dictionary<string, Workflow> workflows = GetWorkflows(workflowStrings);

        Tuple<int,int> [] ranges = [Tuple.Create(1,4000), Tuple.Create(1,4000), Tuple.Create(1,4000), Tuple.Create(1,4000)];

        GoThroughWorkflow(workflows, workflows["in"], ranges);

        Console.WriteLine(total);
    }

    static void GoThroughWorkflow(Dictionary<string, Workflow> workflows, Workflow currentWorkflow, Tuple<int,int>[] ranges){

        foreach(Step step in currentWorkflow.steps){
            if (step.justEnd){
                if (step.decision!=null){
                    
                    if (step.decision=="A"){
                        GetCombinations(ranges);
                    }
                    
                } else {
                    GoThroughWorkflow(workflows, workflows[step.nextWorkflow],ranges);
                }
            } else {
                char operation = step.operation;

                Tuple<int,int>[] rangesFulfillCondition;
                Tuple<int,int>[] rangesNotFulfillCondition;
                Tuple<Tuple<int,int>,Tuple<int,int>> newRanges;

                switch(step.element){
                    case 'x':
                        newRanges = GetRangesAfterCondition(ranges[0], step.operation, step.contraint);
                        rangesFulfillCondition = [newRanges.Item1, ranges[1], ranges[2], ranges[3]];
                        rangesNotFulfillCondition = [newRanges.Item2, ranges[1], ranges[2], ranges[3]];
                        break;
                    case 'm':
                        newRanges = GetRangesAfterCondition(ranges[1], step.operation, step.contraint);
                        rangesFulfillCondition = [ranges[0], newRanges.Item1, ranges[2], ranges[3]];
                        rangesNotFulfillCondition = [ranges[0], newRanges.Item2, ranges[2], ranges[3]];
                        break;
                    case 'a':
                        newRanges = GetRangesAfterCondition(ranges[2], step.operation, step.contraint);
                        rangesFulfillCondition = [ranges[0], ranges[1],newRanges.Item1, ranges[3]];
                        rangesNotFulfillCondition = [ranges[0], ranges[1], newRanges.Item2, ranges[3]];
                        break;
                    case 's':
                        newRanges = GetRangesAfterCondition(ranges[3], step.operation, step.contraint);
                        rangesFulfillCondition = [ranges[0], ranges[1], ranges[2], newRanges.Item1];
                        rangesNotFulfillCondition = [ranges[0], ranges[1], ranges[2], newRanges.Item2];
                        break;
                    default:
                        rangesFulfillCondition = [ranges[0], ranges[1], ranges[2], ranges[3]];
                        rangesNotFulfillCondition = [ranges[0], ranges[1], ranges[2], ranges[3]];
                        break;
                }

                if (step.decision != null){
                
                    if (step.decision=="A"){
                         GetCombinations(rangesFulfillCondition);
                    }

                } else {
                    GoThroughWorkflow(workflows, workflows[step.nextWorkflow],rangesFulfillCondition);
                }

                ranges = rangesNotFulfillCondition;
            }
        }
    }

    static Tuple<Tuple<int,int>,Tuple<int,int>> GetRangesAfterCondition (Tuple<int,int> range, char operation, int contraint){
        Tuple<int,int> fulfilledRange;
        Tuple<int,int> unfulfilledRange;

        if (operation == '<'){

            if (range.Item1 <= contraint - 1){
                fulfilledRange = Tuple.Create(item1: range.Item1, contraint - 1);
            } else {
                fulfilledRange = Tuple.Create(-1,-1);
            }

            if (contraint <= range.Item2){
                unfulfilledRange = Tuple.Create(contraint, range.Item2);
            } else {
                unfulfilledRange = Tuple.Create(-1,-1);
            }

        } else {
            if (contraint+1 <= range.Item2){
                fulfilledRange = Tuple.Create(contraint+1, range.Item2);
            } else {
                fulfilledRange = Tuple.Create(-1,-1);
            }

            if (range.Item1 <= contraint){
                unfulfilledRange = Tuple.Create(range.Item1, contraint);
            } else {
                unfulfilledRange = Tuple.Create(-1,-1);
            }
        }

        return Tuple.Create(fulfilledRange, unfulfilledRange);
    }

    static List<Part> GetParts(List<string> partsStrings){

        List<Part> parts = [];

        foreach(String partString in partsStrings){
             string[] numbersStrings = MyRegex().Split(partString);

             List<int> numbers = [];
             for (int i = 0; i < numbersStrings.Length; i++){
                if (!string.IsNullOrEmpty(numbersStrings[i])){
                    numbers.Add(Int32.Parse(numbersStrings[i]));
                }
             }

             parts.Add(new Part(numbers[0],numbers[1], numbers[2], numbers[3]));
        }

        return parts;
    }

    static void GetCombinations(Tuple<int,int>[] ranges){
        Tuple<int,int> xRange = ranges[0];
        Tuple<int,int> mRange = ranges[1];
        Tuple<int,int> aRange = ranges[2];
        Tuple<int,int> sRange = ranges[3];

        if (xRange.Item1 < 0 || xRange.Item2 < 0 || 
            mRange.Item1 < 0 || mRange.Item2 < 0 || 
            aRange.Item1 < 0 || aRange.Item2 < 0 || 
            sRange.Item1 < 0 || sRange.Item2 < 0){

        } else {

        long xTotal = xRange.Item2-xRange.Item1 + 1;
        long mTotal = mRange.Item2-mRange.Item1 + 1;
        long aTotal = aRange.Item2-aRange.Item1 + 1;
        long sTotal = sRange.Item2-sRange.Item1 + 1;

        long total1  = xTotal * mTotal;
        long total2 = total1 * aTotal;
        long total3 = total2 * sTotal;

        total += total3;
        }
    }

    static Dictionary<string, Workflow> GetWorkflows(List<string> workflowStrings){

        Dictionary<string, Workflow> workflows = [];

        foreach(string workflowString in workflowStrings){
            string [] splitBracket = workflowString.Split("{");
            string id = splitBracket[0].Trim();

            List<Step> steps = [];
            string[] rulesString = splitBracket[1][..^1].Split(",");

            foreach (string ruleString in rulesString){

                Step step = new();

                if (ruleString.Contains(':')){
                    step.justEnd = false;
                    step.element = ruleString[0];
                    step.operation = ruleString[1];
                    string[] consequents = ruleString.Split(':');
                    step.contraint = Int32.Parse(consequents[0][2..]);
                    string consequent = consequents[1];

                    if ("RA".Contains(consequent)){
                        step.decision = consequent;
                    } else {
                        step.nextWorkflow = consequent;
                    }
                } else {
                    step.justEnd = true;
                    if ("RA".Contains(ruleString)){
                        step.decision = ruleString;
                    } else {
                        step.nextWorkflow = ruleString;
                    }
                }

                steps.Add(step);
            }

                Workflow workflow = new(id)
                {
                    steps = steps
                };
                
                workflows[id] = workflow;
        }

        return workflows;
    }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();
        [GeneratedRegex(@"^\d+")]
        private static partial Regex MyRegex1();
    }


class Part(int x, int m, int a, int s){
    public int x = x;
    public int m = m;
    public int a = a;
    public int s = s;

    public override string ToString(){
        return $"X: {x} M: {m} A: {a} S: {s}";
    }

    public int GetSum(){
        return x + m + a + s;
    }
}

class Workflow(string id){
    public string id = id;

    public List<Step> steps = []; 

    public void PrintWorkflow(){
        Console.WriteLine("Id: " + id);

        foreach(Step steps in steps){
            Console.WriteLine(steps.ToString());
        }
    }
}

class Step(){
    public bool justEnd;
    public char element;
    public int contraint;
    public char operation;
    public string nextWorkflow;
    public string decision;

    public override string ToString(){
        return $"Element: {element}, Operation: {operation}, Constraint: {contraint}, nextWorkflow: {nextWorkflow}, Decision: {decision}, JustEnd: {justEnd}";
    }
}
}