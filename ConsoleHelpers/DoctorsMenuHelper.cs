using ClinicPatientRegister_v2.Models;
using System;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public class DoctorsMenuHelper
    {
        // LIST ALL DOCTORS
        public void ListDoctors(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL DOCTORS ===\n");

            // Loop through all doctors in the database
            foreach (var d in db.Doctors)
            {
                Console.WriteLine($"{d.DoctorId}. {d.FirstName} {d.LastName} " +
                                  $"- {d.PersonalNumber12} - {d.Specialization} - {d.Email}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }


        // ADD NEW DOCTOR
        public void AddDoctor(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ADD DOCTOR ===");

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

            // Ask for specialization
            Console.Write("Specialization: ");
            string spec = Console.ReadLine() ?? string.Empty;

            // Ask for home address
            Console.Write("Home address: ");
            string addr = Console.ReadLine() ?? string.Empty;

            // Ask for phone
            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? string.Empty;

            // Ask for email
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            // Create new doctor object
            var newDoctor = new Doctor
            {
                PersonalNumber12 = pn,
                FirstName = fn,
                LastName = ln,
                Specialization = spec,
                HomeAddress = addr,
                Phone = phone,
                Email = email
                // DateOfBirth is computed in DB from PersonalNumber12
            };

            // Save doctor to database
            db.Doctors.Add(newDoctor);
            db.SaveChanges();

            Console.WriteLine("\nDoctor added successfully!");
            Console.ReadLine();
        }


        // EDIT EXISTING DOCTOR
        public void EditDoctor(ClinicDbContext db)
        {
            Console.Clear();

            // Ask for doctor ID
            Console.Write("Enter the ID of the doctor to edit: ");
            int id;

            // Validate ID input
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            // Try to find the doctor by ID
            var d = db.Doctors.Find(id);

            // If doctor does not exist
            if (d == null)
            {
                Console.WriteLine("Doctor not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nLeave field empty to keep old value.\n");

            // Edit first name
            Console.Write($"First name ({d.FirstName}): ");
            string fn = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fn)) d.FirstName = fn;

            // Edit last name
            Console.Write($"Last name ({d.LastName}): ");
            string ln = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ln)) d.LastName = ln;

            // Edit specialization
            Console.Write($"Specialization ({d.Specialization}): ");
            string spec = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(spec)) d.Specialization = spec;

            // Edit address
            Console.Write($"Address ({d.HomeAddress}): ");
            string addr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(addr)) d.HomeAddress = addr;

            // Edit phone
            Console.Write($"Phone ({d.Phone}): ");
            string phone = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(phone)) d.Phone = phone;

            // Edit email
            Console.Write($"Email ({d.Email}): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(email)) d.Email = email;

            // Save changes
            db.SaveChanges();

            Console.WriteLine("\nDoctor updated!");
            Console.ReadLine();
        }


        // DELETE DOCTOR
        public void DeleteDoctor(ClinicDbContext db)
        {
            Console.Clear();

            // Ask for doctor ID
            Console.Write("Enter the ID of the doctor to delete: ");
            int id;

            // Validate ID input
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            // Try to find the doctor
            var d = db.Doctors.Find(id);

            // If doctor is not found
            if (d == null)
            {
                Console.WriteLine("Doctor not found.");
                Console.ReadLine();
                return;
            }

            // Confirm delete
            Console.WriteLine($"\nAre you sure you want to delete Dr. {d.FirstName} {d.LastName}? (y/n)");
            if ((Console.ReadLine() ?? string.Empty).ToLower() != "y")
                return;

            // Delete and save
            db.Doctors.Remove(d);
            db.SaveChanges();

            Console.WriteLine("Doctor deleted.");
            Console.ReadLine();
        }
    }
}