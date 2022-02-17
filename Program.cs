using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace PrimeHunter
{
    public class Primes
    {
        public int prime = 0;
        public int count = 0;
    }

    class Program
    {
        public static Primes[] primes;
        static void Main(string[] args)
        {
            primes = LoadPrimes("primes.txt");
            DisplayStartingCounts(primes);
            CalculateNext(primes, 10000);
        }
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
                        newPrimes[i].prime = int.Parse(splitPrimes[0]);
                        newPrimes[i].count = int.Parse(splitPrimes[1]);
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
                        newPrimes[i].prime = int.Parse(currentPrimes[i]);   
                    }
                    Console.WriteLine("Found " + newPrimes.Length + " previous primes, Calculating starting Counts");
                    CalculateStartCount(newPrimes);
                    return newPrimes;
                }
            }
            else return null;
        }
        static void CalculateStartCount(Primes[] list)
        {
            Console.WriteLine("List length = " + list.Length);

            for (int i = 0; i < list.Length; i++)
            {
                int count = 0;
                list[i].count = 0;
                while (count <= list[list.Length - 1].prime -1)
                {
                    count++;
                    list[i].count++;
                    if (list[i].count == list[i].prime)
                    {
                        Console.WriteLine(count + " is divisible by " + list[i].prime + " Count = " + list[i].count);
                        list[i].count = 0;
                    }
                }
            }
        }
        static void CalculateNext(Primes[] list, int maxNum)
        {
            int count = list[list.Length - 1].prime;
            while (count != maxNum)
            {
                count++;
                bool foundPos = true;
                for (int i = 0; i < list.Length; i++)
                {
                    list[i].count++;
                    if (list[i].count == list[i].prime)
                    {
                        Console.WriteLine(count + " is divisible by " + list[i].prime + " Count = " + list[i].count);
                        list[i].count = 0;
                        foundPos = false;
                    }
                }
                if (foundPos)
                {
                    Primes newPrime = new Primes();
                    newPrime.prime = count;
                    newPrime.count = count;
                    Array.Resize(ref list, list.Length + 1);
                    list[list.Length - 1] = newPrime;
                    Console.WriteLine("Found possible prime at " + count);
                }
            }


        }
        static void DisplayStartingCounts(Primes[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                    Console.WriteLine(list[i].prime + " Starts at count " + list[i].count);
            }
        }
        static void DisplayAllPrimeTotals()
        {
            for (int i = 0; i < primes.Length; i++)
            {
                Console.WriteLine("Number = " + primes[i] + " Total = " + CharacterTotal(primes[i].prime));
            }
        }
        static int CharacterTotal(int check)
        {
            //Add the digits up until we are left with only one
            while (check.ToString().Length > 1)
            {
                string toSplit = check.ToString();
                check = 0;
                for (int i = 0; i < toSplit.Length; i++)
                {
                    check += toSplit[i] - '0';
                }
            }
            return check;
        }
        static bool ThreeSixNine(int test)
        {
            //Check if divisible by 3/6/9
            int check = CharacterTotal(test);
            if (check == 3 || check == 6 || check == 9) return false;
            else return true;
        }
    }
}
