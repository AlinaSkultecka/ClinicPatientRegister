using ClinicPatientRegister_v2.ConsoleHelpers;
using ClinicPatientRegister_v2.Models;
using System.Collections.Generic;
using System;

namespace ClinicPacientRegister_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var options = new List<string>
                {
                    "Patients",
                    "Nurses",
                    "Doctors",
                    "Appointments",
                    "Exit"
                };

                int choice = MenuSelector.Show("     CLINIC PATIENT REGISTER SYSTEM", options);

                switch (choice)
                {
                    case 0:
                        PatientsMenu();
                        break;

                    case 1:
                        NursesMenu();
                        break;

                    case 2:
                        DoctorsMenu();
                        break;

                    case 3:
                        AppointmentsMenu();
                        break;

                    case 4:
                        return; // Exit the app
                }
            }
        }

        // ============= SUBMENUS =============
        static void PatientsMenu()
        {
            using var db = new ClinicDbContext();
            var helper = new ConsoleHelpers.PatientMenuHelper();

            while (true)
            {
                var options = new List<string>
                {
                    "List all patients",
                    "Add patient",
                    "Edit patient",
                    "Delete patient",
                    "Back"
                };

                int choice = MenuSelector.Show("              PATIENTS MENU", options);

                switch (choice)
                {
                    case 0: helper.ListPatients(db); break;
                    case 1: helper.AddPatient(db); break;
                    case 2: helper.EditPatient(db); break;
                    case 3: helper.DeletePatient(db); break;
                    case 4: return;
                }
            }
        }

        static void NursesMenu()
        {
            Console.Clear();
            Console.WriteLine("=== NURSES MENU ===");
            Console.WriteLine("1. List all nurses");
            Console.WriteLine("2. Add nurse");
            Console.WriteLine("3. Edit nurse");
            Console.WriteLine("4. Delete nurse");
            Console.WriteLine("0. Back");
            Console.WriteLine("-------------------------");
            Console.Write("Enter choice: ");
            Console.ReadLine(); // Placeholder
        }

        static void DoctorsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== DOCTORS MENU ===");
            Console.WriteLine("1. List all doctors");
            Console.WriteLine("2. Add doctor");
            Console.WriteLine("3. Edit doctor");
            Console.WriteLine("4. Delete doctor");
            Console.WriteLine("0. Back");
            Console.WriteLine("-------------------------");
            Console.Write("Enter choice: ");
            Console.ReadLine(); // Placeholder
        }

        static void AppointmentsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== APPOINTMENTS MENU ===");
            Console.WriteLine("1. List appointments");
            Console.WriteLine("2. Book new appointment");
            Console.WriteLine("3. Edit appointment");
            Console.WriteLine("4. Cancel appointment");
            Console.WriteLine("0. Back");
            Console.WriteLine("-------------------------");
            Console.Write("Enter choice: ");
            Console.ReadLine(); // Placeholder
        }
    }
}
