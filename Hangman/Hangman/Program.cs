using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Player!");
            Console.WriteLine("We drew the word to gues for you");

            List<EuropeanCapital> list = GetEuropeanCapitals();
          
            char restartGame = ' ';
            Random random = new Random();
            Stopwatch stopwatch = new Stopwatch();

            do
            {  
                List<char> errorList = new List<char>();
                int lettercounter = 0;
                if (restartGame == 'n')
                {
                    break;
                }
                restartGame = ' ';
                int elementOflist = random.Next(0, list.Count - 1);
                stopwatch.Reset();
                stopwatch.Start();
                EuropeanCapital europeanCapital = list[elementOflist];
                string capital = europeanCapital.Capital;
                int length = capital.Length;
                bool hangman = false;
                int lives = 5;
                char[] errors = new char[lives]; 
                char[] guessWord = new char[length];
                bool theEnd = false;


                for (int i = 0; i < length; i++)
                {
                    guessWord[i] = '_';
                }
                do
                {
                    bool foundletter = false;
                    bool victory = false;
                    string entireWord = "";
                    Console.WriteLine();
                    Console.WriteLine();
                    for (int i = 0; i < length; i++)
                    {
                        Console.Write(guessWord[i]);
                    }
                    Console.WriteLine();
                    Console.WriteLine("Not in the word ");
                    for (int i = 0; i < errorList.Count; i++)
                    {
                        Console.Write(" " + errorList[i]);
                    }

                    Console.WriteLine();
                    Console.WriteLine("You have got " + lives + " lives");
                    Console.WriteLine("Press '0' if you want to guess the word or press '1' if you want to guess letter");
                    string question;
                    question = Console.ReadLine();
                    if (question == "0" && hangman == false)
                    {
                        Console.WriteLine("Write word and enter");
                        entireWord = Console.ReadLine();
                        if (entireWord != europeanCapital.Capital)
                        {
                            Console.WriteLine("It is wrong answer");
                            lives = lives - 2;
                        }

                    }
                    if (question == "1")
                    {
                        lettercounter++;
                        Console.WriteLine("Write letter and enter");
                        char letter;
                        Console.WriteLine();

                        letter = Console.ReadKey().KeyChar;
                        for (int i = 0; i < length; i++)
                        {
                            if (letter == capital.ElementAt(i)) 
                            {
                                guessWord[i] = letter; 
                                foundletter = true;
                            }

                        }

                        if (foundletter == false)
                        {
                            errorList.Add(letter);
                            lives--;
                        }
                    } 
                    if (lives <= 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Hint : It is the capital of " + europeanCapital.Country);
                        theEnd = true;
                    }
                    if (new string(guessWord) == capital || entireWord == capital)
                    {
                        Console.WriteLine();
                        Console.WriteLine("You are the winner");
                        stopwatch.Stop();
                        Console.WriteLine("You guessed the capital after " + lettercounter + " letters.It took you " + stopwatch.Elapsed.Seconds.ToString()
                            + " seconds");
                        Console.WriteLine("Entre your name");
                        string user = Console.ReadLine();
                        AddHighScore(user, DateTime.Now.ToString(), stopwatch.Elapsed.Seconds.ToString(), capital);
                        theEnd = true;
                    }
                    if (theEnd == true)
                    {
                        Console.WriteLine("Do you want to restart the game yes - y, no - n");
                        restartGame = Console.ReadKey().KeyChar;
                        if (restartGame == 'y')
                        {
                            Console.Clear();


                        }
                        if (restartGame == 'n')
                        {
                            break;
                        }

                    }
                } while (restartGame != 'y');

            } while (true);
        }


        static List<EuropeanCapital> GetEuropeanCapitals()
        {
            List<EuropeanCapital> list = new List<EuropeanCapital>();
            string[] tab = File.ReadAllLines("countries_and_capitals.txt");
            for (int i = 0; i < tab.Length; i++)
            {
                string[] tempTab = tab[i].Split("|");
                EuropeanCapital europeanCapital = new EuropeanCapital();
                europeanCapital.Country = tempTab[0].Trim();
                europeanCapital.Capital = tempTab[1].Trim();
                list.Add(europeanCapital);
            }
            return list;           
        }

        static void AddHighScore(string name, string date, string guessingTime, string guesingTries )
        {
            using(StreamWriter streamWriter = File.AppendText("high_scores.txt"))
            {
                streamWriter.WriteLine(name + " | " + date + " | " + guessingTime + " | " + guesingTries);
            }
        }

    }
}
