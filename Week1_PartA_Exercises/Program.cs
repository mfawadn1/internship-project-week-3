using System;
using System.Linq;

namespace Week1_PartA_Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Week 1 - Part A Exercises ===");
                Console.WriteLine("1. Calculator (Example Implemented)");
                Console.WriteLine("2. Even or Odd Checker");
                Console.WriteLine("3. FizzBuzz");
                Console.WriteLine("4. Marks to Grade");
                Console.WriteLine("5. Array Min/Max");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option (1-6): ");
                
                string choice = Console.ReadLine();
                Console.WriteLine(); // Blank line for formatting

                switch (choice)
                {
                    case "1":
                        Calculator();
                        break;
                    case "2":
                        EvenOrOdd();
                        break;
                    case "3":
                        FizzBuzz();
                        break;
                    case "4":
                        MarksToGrade();
                        break;
                    case "5":
                        ArrayMinMax();
                        break;
                    case "6":
                        exit = true;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please enter a number from 1 to 6.");
                        break;
                }
            }
        }
        
        static void Calculator()
        {
            Console.WriteLine("--- Calculator ---");
            Console.Write("Enter first number: ");
            if (!double.TryParse(Console.ReadLine(), out double num1))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.Write("Enter second number: ");
            if (!double.TryParse(Console.ReadLine(), out double num2))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            Console.WriteLine($"Sum: {num1 + num2}");
            Console.WriteLine($"Difference: {num1 - num2}");
            Console.WriteLine($"Product: {num1 * num2}");
            
            if (num2 != 0)
                Console.WriteLine($"Quotient: {num1 / num2}");
            else
                Console.WriteLine("Quotient: Cannot divide by zero.");
        }

        static void EvenOrOdd()
        {
            Console.WriteLine("--- Even or Odd ---");
            Console.WriteLine("Enter thenumber");
            int input = int.Parse(Console.ReadLine());
            if (input % 2 == 0)
                Console.WriteLine("Even");
            else
                Console.WriteLine("Odd");
        }

        static void FizzBuzz()
        {
            Console.WriteLine("--- FizzBuzz ---");
            for (int i = 1; i <= 100; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                    Console.WriteLine("FizzBuzz");
                else if (i % 3 == 0)
                    Console.WriteLine("Fizz");
                else if (i % 5 == 0)
                    Console.WriteLine("Buzz");
                else
                    Console.WriteLine(i);
            }
        }

        static void MarksToGrade()
        {
            Console.WriteLine("--- Marks to Grade ---");
            Console.Write("Enter your grade: ");
            int grade = int.Parse(Console.ReadLine());
            if (grade >= 90 && grade <= 100){
                Console.WriteLine("A");
            }
            else if (grade >= 80 && grade < 90){
                Console.WriteLine("B");
            }
            else if (grade >= 70 && grade < 80){
                Console.WriteLine("C");
            }
            else if (grade >= 60 && grade < 70){
                Console.WriteLine("D");
            }
            else {
                Console.WriteLine("F");
            }
        }

        static void ArrayMinMax()
        {
            Console.WriteLine("--- Array Min/Max ---");
            int[] array={11,2,3,4,35,6,7,8,9,10};
            int min = array[0];
            int max = array[0];
            for (int i=0; i<10; i++){
                if (array[i]<min)
                    min=array[i];
                if (array[i]>max)
                    max=array[i];

            }
            Console.WriteLine($"Smallest value: {min}");
            Console.WriteLine($"Largest value: {max}");
        }
    }
}
