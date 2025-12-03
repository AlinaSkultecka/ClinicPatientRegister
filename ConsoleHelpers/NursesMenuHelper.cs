using ClinicPatientRegister_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public class NursesMenuHelper
    {
        // LIST ALL NURSES
        public void ListNurses(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL NURSES ===\n");

            // Loop through all nurses in the database
            foreach (var n in db.Nurses)
            {
                Console.WriteLine($"{n.NurseId}. {n.FirstName} {n.LastName} " +
                                  $"- {n.PersonalNumber12} - {n.Email}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }


        // ADD NEW NURSE
        public void AddNurse(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ADD NURSE ===");

            // Ask for personal number until valid
            string pn;
            while (true)
            {
                Console.Write("Personal number (12 digits): ");
                pn = Console.ReadLine() ?? string.Empty;

                // Validate that PN is exactly 12 digits
                if (pn.Length != 12 || !pn.All(char.IsDigit))
                {
                    Console.WriteLine("Invalid personal number. Must be 12 digits (YYYYMMDDXXXX).");
                    Console.WriteLine("Press ENTER to try again...");
                    Console.ReadLine();

                    Console.Clear();   // <-- CLEAN SCREEN
                    continue;          // <-- ASK AGAIN
                }

                // Validate the date part
                string datePart = pn.Substring(0, 8);

                if (!DateTime.TryParseExact(datePart, "yyyyMMdd", null,
                    System.Globalization.DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Invalid date in personal number.");
                    Console.WriteLine("Press ENTER to try again...");
                    Console.ReadLine();

                    Console.Clear();   // <-- CLEAN SCREEN
                    continue;          // <-- ASK AGAIN
                }

                break; // Valid PN → exit loop
            }


            // Ask for first name
            Console.Write("First name: ");
            string fn = Console.ReadLine() ?? string.Empty;

            // Ask for last name
            Console.Write("Last name: ");
            string ln = Console.ReadLine() ?? string.Empty;

            // Ask for certification
            Console.Write("Certification: ");
            string cr = Console.ReadLine() ?? string.Empty;


            // Ask for shift until valid
            string shft;
            var allowedShifts = new[] { "Morning", "Evening" };

            while (true)
            {
                Console.Write("Shift (Morning / Evening ): ");
                shft = (Console.ReadLine() ?? string.Empty).Trim();

                // Handle completely empty or whitespace input
                if (string.IsNullOrWhiteSpace(shft))
                {
                    Console.WriteLine("Shift cannot be empty.\n");
                    continue;
                }

                // Convert safely to uppercase (prevents errors)
                shft = char.ToUpper(shft[0]) + shft.Substring(1).ToLower();

                // Check if value is allowed
                if (!allowedShifts.Contains(shft))
                {
                    Console.WriteLine("Invalid shift. Allowed: Morning or Evening.\n");
                    continue;
                }

                break;
            }

            // Ask for home address
            Console.Write("Home address: ");
            string addr = Console.ReadLine() ?? string.Empty;

           
            // Ask for phone
            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? string.Empty;

            // Ask for email
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            // Create new nurse object
            var newNurse = new Nurse
            {
                PersonalNumber12 = pn,
                FirstName = fn,
                LastName = ln,
                Certification = cr,
                Shift = shft,
                HomeAddress = addr,
                Phone = phone,
                Email = email
                // DateOfBirth is computed in DB from PersonalNumber12
            };

            // Save nurse to database
            db.Nurses.Add(newNurse);
            db.SaveChanges();

            Console.WriteLine("\nNurse added successfully!");
            Console.ReadLine();
        }


        // EDIT EXISTING NURSE
        public void EditNurse(ClinicDbContext db)
        {
            Console.Clear();

            // Ask for nurse ID
            Console.Write("Enter the ID of the nurse to edit: ");
            int id;

            // Validate ID input
            if (!int.TryParse(Console.ReadLine() ?? string.Empty, out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            // Try to find the nurse by ID
            var n = db.Nurses.Find(id);

            // If nurse does not exist
            if (n == null)
            {
                Console.WriteLine("Nurse not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nLeave field empty to keep old value.\n");

            // Edit first name
            Console.Write($"First name ({n.FirstName}): ");
            string fn = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fn)) n.FirstName = fn;

            // Edit last name
            Console.Write($"Last name ({n.LastName}): ");
            string ln = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ln)) n.LastName = ln;

            // Edit certification
            Console.Write($"Certification ({n.Certification}): ");
            string cr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(cr)) n.Certification = cr;

            // Edit shift
            Console.Write($"Shift ({n.Shift}): ");
            string shft = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(shft)) n.Shift = shft;

            // Edit home address
            Console.Write($"Address ({n.HomeAddress}): ");
            string addr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(addr)) n.HomeAddress = addr;

            // Edit phone
            Console.Write($"Phone ({n.Phone}): ");
            string phone = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(phone)) n.Phone = phone;

            // Edit email
            Console.Write($"Email ({n.Email}): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(email)) n.Email = email;

            // Save changes
            db.SaveChanges();

            Console.WriteLine("\nNurse updated!");
            Console.ReadLine();
        }


        // DELETE NURSE
        public void DeleteNurse(ClinicDbContext db)
        {
            Console.Clear();

            // Ask for nurse ID
            Console.Write("Enter the ID of the nurse to delete: ");
            int id;

            // Validate ID input
            if (!int.TryParse(Console.ReadLine() ?? string.Empty, out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            // Try to find the nurse
            var n = db.Nurses.Find(id);

            // If nurse is not found
            if (n == null)
            {
                Console.WriteLine("Nurse not found.");
                Console.ReadLine();
                return;
            }

            // Confirm delete
            Console.WriteLine($"\nAre you sure you want to delete {n.FirstName} {n.LastName}? (y/n)");
            if ((Console.ReadLine() ?? string.Empty).ToLower() != "y")
                return;

            // Delete and save
            db.Nurses.Remove(n);
            db.SaveChanges();

            Console.WriteLine("Nurse deleted.");
            Console.ReadLine();
        }
    }
}
