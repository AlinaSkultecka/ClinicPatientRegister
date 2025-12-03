using ClinicPatientRegister_v2.Models;
using System;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public class DoctorsMenuHelper
    {
        public void ListDoctors(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL DOCTORS ===\n");

            foreach (var d in db.Doctors)
            {
                Console.WriteLine($"{d.DoctorId}. {d.FirstName} {d.LastName} " +
                                  $"- {d.PersonalNumber12} - {d.Specialization} - {d.Email}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }

        public void AddDoctor(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ADD DOCTOR ===");

            Console.Write("Personal number (12 digits): ");
            string pn = Console.ReadLine() ?? string.Empty;

            Console.Write("First name: ");
            string fn = Console.ReadLine() ?? string.Empty;

            Console.Write("Last name: ");
            string ln = Console.ReadLine() ?? string.Empty;

            Console.Write("Specialization: ");
            string spec = Console.ReadLine() ?? string.Empty;

            Console.Write("Home address: ");
            string addr = Console.ReadLine() ?? string.Empty;

            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            var newDoctor = new Doctor
            {
                PersonalNumber12 = pn,
                FirstName = fn,
                LastName = ln,
                Specialization = spec,
                HomeAddress = addr,
                Phone = phone,
                Email = email
                // DateOfBirth is computed in DB from PersonalNumber12, so we don't set it here
            };

            db.Doctors.Add(newDoctor);
            db.SaveChanges();

            Console.WriteLine("\nDoctor added successfully!");
            Console.ReadLine();
        }

        public void EditDoctor(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the doctor to edit: ");
            int id;

            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var d = db.Doctors.Find(id);

            if (d == null)
            {
                Console.WriteLine("Doctor not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nLeave field empty to keep old value.\n");

            Console.Write($"First name ({d.FirstName}): ");
            string fn = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fn)) d.FirstName = fn;

            Console.Write($"Last name ({d.LastName}): ");
            string ln = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ln)) d.LastName = ln;

            Console.Write($"Specialization ({d.Specialization}): ");
            string spec = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(spec)) d.Specialization = spec;

            Console.Write($"Address ({d.HomeAddress}): ");
            string addr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(addr)) d.HomeAddress = addr;

            Console.Write($"Phone ({d.Phone}): ");
            string phone = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(phone)) d.Phone = phone;

            Console.Write($"Email ({d.Email}): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(email)) d.Email = email;

            db.SaveChanges();

            Console.WriteLine("\nDoctor updated!");
            Console.ReadLine();
        }

        public void DeleteDoctor(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the doctor to delete: ");

            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var d = db.Doctors.Find(id);

            if (d == null)
            {
                Console.WriteLine("Doctor not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nAre you sure you want to delete Dr. {d.FirstName} {d.LastName}? (y/n)");
            if (Console.ReadLine().ToLower() != "y")
                return;

            db.Doctors.Remove(d);
            db.SaveChanges();

            Console.WriteLine("Doctor deleted.");
            Console.ReadLine();
        }
    }
}