using ClinicPatientRegister_v2.Models;
using System;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public class PatientsMenuHelper
    {
        // LIST ALL PATIENTS
        public void ListPatients(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL PATIENTS ===\n");

            // Loop through all patients in the database
            foreach (var p in db.Patients)
            {
                Console.WriteLine($"{p.PatientId}. {p.FirstName} {p.LastName} " +
                                  $"- {p.PersonalNumber12} - {p.Email}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }


        // ADD NEW PATIENT
        public void AddPatient(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ADD PATIENT ===");

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

            // Ask for address
            Console.Write("Home address: ");
            string addr = Console.ReadLine() ?? string.Empty;

            // Ask for phone
            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? string.Empty;

            // Ask for email
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            // Create new patient object
            var newPatient = new Patient
            {
                PersonalNumber12 = pn,
                FirstName = fn,
                LastName = ln,
                HomeAddress = addr,
                Phone = phone,
                Email = email
            };

            // Save patient to database
            db.Patients.Add(newPatient);
            db.SaveChanges();

            Console.WriteLine("\nPatient added successfully!");
            Console.ReadLine();
        }


        // EDIT EXISTING PATIENT
        public void EditPatient(ClinicDbContext db)
        {
            Console.Clear();

            // Ask for patient ID
            Console.Write("Enter the ID of the patient to edit: ");
            int id;

            // Validate ID input
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            // Try to find the patient by ID
            var p = db.Patients.Find(id);

            // If patient does not exist
            if (p == null)
            {
                Console.WriteLine("Patient not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nLeave field empty to keep old value.\n");

            // Edit first name
            Console.Write($"First name ({p.FirstName}): ");
            string fn = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fn)) p.FirstName = fn;

            // Edit last name
            Console.Write($"Last name ({p.LastName}): ");
            string ln = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ln)) p.LastName = ln;

            // Edit home address
            Console.Write($"Address ({p.HomeAddress}): ");
            string addr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(addr)) p.HomeAddress = addr;

            // Edit phone
            Console.Write($"Phone ({p.Phone}): ");
            string phone = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(phone)) p.Phone = phone;

            // Edit email
            Console.Write($"Email ({p.Email}): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(email)) p.Email = email;

            // Save changes
            db.SaveChanges();

            Console.WriteLine("\nPatient updated!");
            Console.ReadLine();
        }


        // DELETE A PATIENT
        public void DeletePatient(ClinicDbContext db)
        {
            Console.Clear();

            // Ask for patient ID
            Console.Write("Enter the ID of the patient to delete: ");
            int id;

            // Validate ID input
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            // Try to find the patient
            var p = db.Patients.Find(id);

            // If patient was not found
            if (p == null)
            {
                Console.WriteLine("Patient not found.");
                Console.ReadLine();
                return;
            }

            // Confirm delete
            Console.WriteLine($"\nAre you sure you want to delete {p.FirstName} {p.LastName}? (y/n)");
            if (Console.ReadLine().ToLower() != "y")
                return;

            // Delete and save
            db.Patients.Remove(p);
            db.SaveChanges();

            Console.WriteLine("Patient deleted.");
            Console.ReadLine();
        }
    }
}

