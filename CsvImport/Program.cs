using CsvImport.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvImport
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Person> persons = new List<Person>();

            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\Persons.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                bool firstLine = true;

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (!firstLine)
                    {
                        persons.Add(new Person()
                        {
                            Id = int.Parse(fields[0]),
                            FirstName = fields[1],
                            LastName = fields[2],
                            BirthDate = DateTime.Parse(fields[3]),
                            Age = DateTime.Today.Year - DateTime.Parse(fields[3]).Year,
                            Gender = fields[4],
                            Children = fields[5].Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList()
                        });
                    }
                    else
                    {
                        firstLine = false;
                    }

                }
            }

            WriteLogs(persons);
        }

        public static void WriteLogs(List<Person> persons)
        {
            int numberOfFemales = persons.Where(p => p.Gender == "Female").Count();
            int numberOfMales = persons.Where(p => p.Gender == "Male").Count();
            List<string> males = persons.Where(p => p.Gender == "Male").OrderBy(p => p.LastName).Select(p => p.LastName).ToList();
            Person oldestFemale = persons.Where(p => p.Gender == "Female").OrderByDescending(p => p.Age).FirstOrDefault();
            Person mostChildren = persons.OrderByDescending(p => p.Children.Count).FirstOrDefault();
            List<string> allChildren = persons.SelectMany(p => p.Children).ToList();
            allChildren.Sort();

            Console.WriteLine($"Number of Females: {numberOfFemales}");
            Console.WriteLine($"Number of Males: {numberOfMales}");
            Console.WriteLine("List of males: ");
            Console.WriteLine();

            foreach (string name in males)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine();
            Console.WriteLine($"Oldest female is: {oldestFemale.FirstName} {oldestFemale.LastName}, aged: {oldestFemale.Age}");
            Console.WriteLine($"Most number of Children: {mostChildren.FirstName} {mostChildren.LastName}, count: {mostChildren.Children.Count()}");

            Console.WriteLine("Child list:");

            foreach (string child in allChildren)
            {
                Console.WriteLine(child);
            }
            Console.ReadLine();
        }
    }
}
