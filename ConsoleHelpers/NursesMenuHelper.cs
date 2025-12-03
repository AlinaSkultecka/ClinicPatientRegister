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
        public void ListNurses(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL NURSES ===\n");

            foreach (var n in db.Nurses)
            {
                Console.WriteLine($"{n.NurseId}. {n.FirstName} {n.LastName} " +
                                  $"- {n.PersonalNumber12} - {n.Email}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }

        // Fix for CS8600: Use null-coalescing operator to ensure non-null assignment from Console.ReadLine()

        public void AddNurse(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ADD NURSE ===");

            Console.Write("Personal number (12 digits): ");
            string pn = Console.ReadLine() ?? string.Empty;

            Console.Write("First name: ");
            string fn = Console.ReadLine() ?? string.Empty;

            Console.Write("Last name: ");
            string ln = Console.ReadLine() ?? string.Empty;

            Console.Write("Certification: ");
            string cr = Console.ReadLine() ?? string.Empty;

            Console.Write("Shift: ");
            string shft = Console.ReadLine() ?? string.Empty;

            Console.Write("Home address: ");
            string addr = Console.ReadLine() ?? string.Empty;

            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

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
                // DateOfBirth is computed in DB from PersonalNumber12, so we don't set it here
            };

            db.Nurses.Add(newNurse);
            db.SaveChanges();

            Console.WriteLine("\nNurse added successfully!");
            Console.ReadLine();
        }

        public void EditNurse(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the nurse to edit: ");
            int id;

            if (!int.TryParse(Console.ReadLine() ?? string.Empty, out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var n = db.Nurses.Find(id);

            if (n == null)
            {
                Console.WriteLine("Nurse not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nLeave field empty to keep old value.\n");

            Console.Write($"First name ({n.FirstName}): ");
            string fn = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fn)) n.FirstName = fn;

            Console.Write($"Last name ({n.LastName}): ");
            string ln = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ln)) n.LastName = ln;

            Console.Write($"Certification ({n.Certification}): ");
            string cr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(cr)) n.Certification = cr;

            Console.Write($"Shift ({n.Shift}): ");
            string shft = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(shft)) n.Shift = shft;

            Console.Write($"Address ({n.HomeAddress}): ");
            string addr = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(addr)) n.HomeAddress = addr;

            Console.Write($"Phone ({n.Phone}): ");
            string phone = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(phone)) n.Phone = phone;

            Console.Write($"Email ({n.Email}): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(email)) n.Email = email;

            db.SaveChanges();

            Console.WriteLine("\nNurse updated!");
            Console.ReadLine();
        }

        public void DeleteNurse(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the nurse to delete: ");

            int id;
            if (!int.TryParse(Console.ReadLine() ?? string.Empty, out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var n = db.Nurses.Find(id);

            if (n == null)
            {
                Console.WriteLine("Nurse not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nAre you sure you want to delete {n.FirstName} {n.LastName}? (y/n)");
            if ((Console.ReadLine() ?? string.Empty).ToLower() != "y")
                return;

            db.Nurses.Remove(n);
            db.SaveChanges();

            Console.WriteLine("Nurse deleted.");
            Console.ReadLine();
        }
    }
}
