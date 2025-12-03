using ClinicPatientRegister_v2.Models;
using System;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public class PatientsMenuHelper
    {
        public void ListPatients(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL PATIENTS ===\n");

            foreach (var p in db.Patients)
            {
                Console.WriteLine($"{p.PatientId}. {p.FirstName} {p.LastName} " +
                                  $"- {p.PersonalNumber12} - {p.Email}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }

        public void AddPatient(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ADD PATIENT ===");

            Console.Write("Personal number (12 digits): ");
            string pn = Console.ReadLine() ?? string.Empty;

            Console.Write("First name: ");
            string fn = Console.ReadLine() ?? string.Empty;

            Console.Write("Last name: ");
            string ln = Console.ReadLine() ?? string.Empty;

            Console.Write("Home address: ");
            string addr = Console.ReadLine() ?? string.Empty;

            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            var newPatient = new Patient
            {
                PersonalNumber12 = pn,
                FirstName = fn,
                LastName = ln,
                HomeAddress = addr,
                Phone = phone,
                Email = email
                // DateOfBirth is computed in DB from PersonalNumber12, so we don't set it here
            };

            db.Patients.Add(newPatient);
            db.SaveChanges();

            Console.WriteLine("\nPatient added successfully!");
            Console.ReadLine();
        }

        public void EditPatient(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the patient to edit: ");
            int id;

            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var p = db.Patients.Find(id);

            if (p == null)
            {
                Console.WriteLine("Patient not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nLeave field empty to keep old value.\n");

            Console.Write($"First name ({p.FirstName}): ");
            string fn = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fn)) p.FirstName = fn;

            Console.Write($"Last name ({p.LastName}): ");
            string ln = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ln)) p.LastName = ln;

            Console.Write($"Address ({p.HomeAddress}): ");
            string addr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(addr)) p.HomeAddress = addr;

            Console.Write($"Phone ({p.Phone}): ");
            string phone = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(phone)) p.Phone = phone;

            Console.Write($"Email ({p.Email}): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(email)) p.Email = email;

            db.SaveChanges();

            Console.WriteLine("\nPatient updated!");
            Console.ReadLine();
        }

        public void DeletePatient(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the patient to delete: ");

            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var p = db.Patients.Find(id);

            if (p == null)
            {
                Console.WriteLine("Patient not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nAre you sure you want to delete {p.FirstName} {p.LastName}? (y/n)");
            if (Console.ReadLine().ToLower() != "y")
                return;

            db.Patients.Remove(p);
            db.SaveChanges();

            Console.WriteLine("Patient deleted.");
            Console.ReadLine();
        }
    }
}

