using System;

namespace Week1_PartB_Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Week 1 - Part B Exercises ===");
            
            Student s1 = new Student();
            s1.Name = "Asiya";
            s1.Age = 25;
            s1.Grade = 10;
            s1.StudentId = "12345";
            s1.PrintDetails();
            
            Console.WriteLine("Try creating a Person, Student, and Teacher object!");
        }
    }
}
