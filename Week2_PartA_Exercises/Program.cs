using System;
using System.Threading.Tasks;

namespace Week2_PartA_Exercises
{
    // 2. Create an interface with two implementations
    public interface IAnimal
    {
        string Speak();
    }

    public class Dog : IAnimal
    {
        public string Speak() => "Woof!";
    }

    public class Cat : IAnimal
    {
        public string Speak() => "Meow!";
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Week 2 Part A Exercises ===");

            // 1. Generic Helper Method
            Console.WriteLine("\n--- 1. Generic Helper Method ---");
            int a = 5, b = 10;
            Console.WriteLine($"Before Swap: a={a}, b={b}");
            Swap(ref a, ref b);
            Console.WriteLine($"After Swap: a={a}, b={b}");

            string s1 = "Hello", s2 = "World";
            Swap(ref s1, ref s2);
            Console.WriteLine($"Swapped strings: s1={s1}, s2={s2}");

            // 2. Interface with two implementations
            Console.WriteLine("\n--- 2. Interface Implementations ---");
            IAnimal myDog = new Dog();
            IAnimal myCat = new Cat();
            Console.WriteLine($"Dog says: {myDog.Speak()}");
            Console.WriteLine($"Cat says: {myCat.Speak()}");

            // 3. Async method
            Console.WriteLine("\n--- 3. Async Method ---");
            Console.WriteLine("Fetching data...");
            string data = await FetchDataAsync();
            Console.WriteLine(data);

            // 4. Refactoring example (running clean methods instead of messy main block)
            Console.WriteLine("\n--- 4. Refactored Console Logic ---");
            RunRefactoredLogic();
        }

        // 1. Create a generic helper method
        public static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }

        // 3. Write an async method that waits and then returns a result
        public static async Task<string> FetchDataAsync()
        {
            await Task.Delay(2000); // Wait for 2 seconds
            return "Data fetched successfully!";
        }

        // 4. Refactor one messy console program into smaller methods/classes.
        // Instead of having a big block of logic in Main, we extract it.
        public static void RunRefactoredLogic()
        {
            string userInput = GetUserInput();
            ProcessInput(userInput);
            DisplayResult();
        }

        private static string GetUserInput()
        {
            return "Sample Input"; // Mocking input for demo purposes
        }

        private static void ProcessInput(string input)
        {
            Console.WriteLine($"Processing: {input}");
        }

        private static void DisplayResult()
        {
            Console.WriteLine("Processing Complete.");
        }
    }
}
