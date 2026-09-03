using System;
using System.Collections.Generic;
using System.Linq;

namespace Week1_PartC_Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Week 1 - Part C Exercises ===");

            // Create a List and add five students
            List<Student> students = new List<Student>
            {
                new Student { Id = 1, Name = "Asiya", Department = "AI", Marks = 85.5 },
                new Student { Id = 2, Name = "Minahil", Department = "Math", Marks = 72.0 },
                new Student { Id = 3, Name = "Aliha", Department = "CS", Marks = 92.5 },
                new Student { Id = 4, Name = "Anoosha", Department = "Physics", Marks = 88.0 },
                new Student { Id = 5, Name = "Ayema", Department = "CS", Marks = 79.0 }
            };

            // Find students from a particular department using LINQ
            var csStudents = students.Where(s => s.Department == "CS").ToList();
            Console.WriteLine("\n-- CS Department Students --");
            foreach (var s in csStudents)
            {
                Console.WriteLine($"{s.Name} ({s.Marks})");
            }

            // Sort students by marks
            var sortedStudents = students.OrderByDescending(s => s.Marks).ToList();
            Console.WriteLine("\n-- Students Sorted by Marks (Highest to Lowest) --");
            foreach (var s in sortedStudents)
            {
                Console.WriteLine($"{s.Name}: {s.Marks}");
            }

            // Find a student by ID
            Console.WriteLine("\n-- Find Student by ID --");
            Console.Write("Enter Student ID to search: ");
            string inputId = Console.ReadLine();

            // Handle invalid numeric input without crashing
            try
            {
                int id = int.Parse(inputId);
                var student = students.FirstOrDefault(s => s.Id == id);
                
                if (student != null)
                {
                    Console.WriteLine($"Found Student: {student.Name}, Dept: {student.Department}");
                }
                else
                {
                    Console.WriteLine($"No student found with ID {id}.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter a valid numeric ID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
