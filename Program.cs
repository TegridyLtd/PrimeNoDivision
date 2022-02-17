/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2022 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free for academic use.                                  //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Numerics;
using System.IO;
namespace PrimeHunter
{
    public class Primes
    {
        public BigInteger prime = 0;
        public BigInteger count = 0;
    }

    class Program
    {
        public static Primes[] primes;
        static void Main(string[] args)
        {
            string loadPath = "Primes.txt";
            string savePath = "NewPrimes.txt";
            BigInteger maxNum = 10000;
            bool saveCounts = false;

            if (args.Length == 4)
            {
                loadPath = args[0];
                savePath = args[1];
                maxNum = BigInteger.Parse(args[2]);
                saveCounts = bool.Parse(args[3]);
            }

            primes = LoadPrimes(loadPath);
            primes = CalculateUpto(primes, maxNum);
            SavePrimeList(primes, saveCounts, savePath);
        }

        static void CalculateStartCount(Primes[] list)
        {
            Console.WriteLine("List length = " + list.Length);
            for (int i = 0; i < list.Length; i++)
            {
                BigInteger count = 0;
                list[i].count = 0;
                while (count <= list[list.Length - 1].prime -1)
                {
                    count++;
                    list[i].count++;
                    if (list[i].count == list[i].prime)
                    {
                        list[i].count = 0;
                    }
                }
            }
        }
        static Primes[] CalculateUpto(Primes[] list, BigInteger maxNum)
        {
            BigInteger count = list[list.Length - 1].prime;
            while (count != maxNum)
            {
                //Increase the counts for all known primes & check if we are at a division point
                count++;
                bool foundPos = true;
                for (int i = 0; i < list.Length; i++)
                {
                    list[i].count++;
                    if (list[i].count == list[i].prime)
                    {
                        list[i].count = 0;
                        foundPos = false;
                    }
                }

                //If we have found a possible match add it to the array so we don't miss it on the next pass
                if (foundPos)
                {
                    Primes newPrime = new Primes();
                    newPrime.prime = count;
                    newPrime.count = 0;
                    Array.Resize(ref list, list.Length + 1);
                    list[list.Length - 1] = newPrime;
                }
            }
            Console.WriteLine("Found " + (list.Length - primes.Length).ToString() + " new primes betwen 0 and " + maxNum);
            return list;
        }
        static void DisplayStartingCounts(Primes[] list)
        {
            //For debug, displays the starting count for prime in the array.
            for (int i = 0; i < list.Length; i++)
            {
                    Console.WriteLine(list[i].prime + " Starts at count " + list[i].count);
            }
        }
        static void DisplayAllPrimeTotals()
        {
            //Displays the total of all the numbers in the prime added until only one remains
            for (int i = 0; i < primes.Length; i++)
            {
                Console.WriteLine("Prime = " + primes[i] + " Character Total = " + CharacterTotal(primes[i].prime));
            }
        }
        static int CharacterTotal(BigInteger check)
        {
            //Add the digits up until we are left with only one
            int total = 0;
            while (check.ToString().Length > 1)
            {
                string toSplit = check.ToString();
                for (int i = 0; i < toSplit.Length; i++)
                {
                    total += toSplit[i] - '0';
                }
            }
            return total;
        }
        static bool ThreeSixNine(int test)
        {
            //Check if divisible by 3/6/9. if returns nine numnber will always be divisible by 9. 3 or 6 and its a multiple of 3 or 6.
            int check = CharacterTotal(test);
            if (check == 3 || check == 6 || check == 9) return false;
            else return true;
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// Save/Load Functions
        static Primes[] LoadPrimes(string filePath)
        {
            //see if we have any existing data
            if (File.Exists(filePath))
            {
                string[] currentPrimes = File.ReadAllLines(filePath);
                string[] splitPrimes = currentPrimes[0].Split(',');

                //Check if we have primes or counts
                if (splitPrimes.Length == 2)
                {
                    //load primes with counts
                    Primes[] newPrimes = new Primes[currentPrimes.Length];
                    for (int i = 0; i < currentPrimes.Length; i++)
                    {
                        splitPrimes = currentPrimes[i].Split(',');
                        newPrimes[i] = new Primes();
                        newPrimes[i].prime = BigInteger.Parse(splitPrimes[0]);
                        newPrimes[i].count = BigInteger.Parse(splitPrimes[1]);
                    }
                    Console.WriteLine("Found " + newPrimes.Length + " previous primes with counts");
                    return newPrimes;
                }
                else
                {
                    //load primes then calculate counts
                    Primes[] newPrimes = new Primes[currentPrimes.Length];
                    for (int i = 0; i < currentPrimes.Length; i++)
                    {
                        newPrimes[i] = new Primes();
                        newPrimes[i].prime = BigInteger.Parse(currentPrimes[i]);
                    }
                    Console.WriteLine("Found " + newPrimes.Length + " previous primes, Calculating starting Counts");
                    CalculateStartCount(newPrimes);
                    return newPrimes;
                }
            }
            else
            {
                Primes[] newPrimes = new Primes[2];
                newPrimes[0] = new Primes();
                newPrimes[0].prime = 2;
                newPrimes[0].count = 1;

                newPrimes[1] = new Primes();
                newPrimes[1].prime = 3;
                newPrimes[1].count = 0;
                Console.WriteLine("No data found using default starting numbers");
                return newPrimes;
            }
        }
        static void SavePrimeList(Primes[] list, bool withCounts, string path)
        {
            string[] saveData = new string[list.Length];

            if (withCounts)
            {
                //write save data including the counts
                for (int i = 0; i < list.Length; i++)
                {
                    saveData[i] = list[i].prime.ToString() + "," + list[i].count.ToString();
                }
            }
            else
            {
                //write save data without counts
                for (int i = 0; i < list.Length; i++)
                {
                    saveData[i] = list[i].prime.ToString();
                }
            }
            File.WriteAllLines(path, saveData);
        }
    }
}
