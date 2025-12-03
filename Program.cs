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
            var helper = new PatientsMenuHelper();

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
            using var db = new ClinicDbContext();
            var helper = new NursesMenuHelper();

            while (true)
            {
                var options = new List<string>
                {
                    "List all nurses",
                    "Add nurse",
                    "Edit nurse",
                    "Delete nurse",
                    "Back"
                };

                int choice = MenuSelector.Show("              NURSE MENU", options);

                switch (choice)
                {
                    case 0: helper.ListNurses(db); break;
                    case 1: helper.AddNurse(db); break;
                    case 2: helper.EditNurse(db); break;
                    case 3: helper.DeleteNurse(db); break;
                    case 4: return;
                }
            }
        }

        static void DoctorsMenu()
        {
            using var db = new ClinicDbContext();
            var helper = new DoctorsMenuHelper();

            while (true)
            {
                var options = new List<string>
                {
                    "List all doctors",
                    "Add doctor",
                    "Edit doctor",
                    "Delete doctor",
                    "Back"
                };

                int choice = MenuSelector.Show("              DOCTOR MENU", options);

                switch (choice)
                {
                    case 0: helper.ListDoctors(db); break;
                    case 1: helper.AddDoctor(db); break;
                    case 2: helper.EditDoctor(db); break;
                    case 3: helper.DeleteDoctor(db); break;
                    case 4: return;
                }
            }
        }

        static void AppointmentsMenu()
        {
            using var db = new ClinicDbContext();
            var helper = new AppointmentsMenuHelper();

            while (true)
            {
                var options = new List<string>
                {
                    "List appointments",
                    "Book new appointment",
                    "Edit appointment",
                    "Cancel appointment",
                    "Back"
                };

                int choice = MenuSelector.Show("           APPOINTMENTS MENU", options);

                switch (choice)
                {
                    case 0: helper.ListAppointments(db); break;
                    case 1: helper.BookAppointment(db); break;
                    case 2: helper.EditAppointment(db); break;
                    case 3: helper.CancelAppointment(db); break;
                    case 4: return;
                }
            }
        }
    }
}
