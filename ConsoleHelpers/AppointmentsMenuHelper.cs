using ClinicPatientRegister_v2.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public class AppointmentsMenuHelper
    {
        public void ListAppointments(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL APPOINTMENTS ===\n");

            var appointments = db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Nurse);

            foreach (var a in appointments)
            {
                var patientName = a.Patient != null ? $"{a.Patient.FirstName} {a.Patient.LastName}" : "Unknown patient";
                var doctorName = a.Doctor != null ? $"{a.Doctor.FirstName} {a.Doctor.LastName}" : "No doctor";
                var nurseName = a.Nurse != null ? $"{a.Nurse.FirstName} {a.Nurse.LastName}" : "No nurse";

                Console.WriteLine(
                    $"{a.AppointmentId}. {a.AppointmentDate:G} - {patientName} " +
                    $"- Doctor: {doctorName} - Nurse: {nurseName} - Status: {a.Status} " +
                    $"{(string.IsNullOrWhiteSpace(a.Reason) ? "" : $" - Reason: {a.Reason}")}"
                );
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }

        public void BookAppointment(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== BOOK NEW APPOINTMENT ===");

            Console.Write("Patient ID: ");
            if (!int.TryParse(Console.ReadLine(), out int patientId))
            {
                Console.WriteLine("Invalid Patient ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            Console.Write("Doctor ID (optional, press ENTER to skip): ");
            string doctorInput = Console.ReadLine() ?? string.Empty;
            int? doctorId = null;
            if (int.TryParse(doctorInput, out int dId))
                doctorId = dId;

            Console.Write("Nurse ID (optional, press ENTER to skip): ");
            string nurseInput = Console.ReadLine() ?? string.Empty;
            int? nurseId = null;
            if (int.TryParse(nurseInput, out int nId))
                nurseId = nId;

            Console.Write("Appointment date & time (e.g. 2025-12-01 14:30): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Invalid date/time. Press ENTER...");
                Console.ReadLine();
                return;
            }

            Console.Write("Reason (optional): ");
            string reason = Console.ReadLine() ?? string.Empty;

            var newAppointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                NurseId = nurseId,
                AppointmentDate = date,
                Reason = string.IsNullOrWhiteSpace(reason) ? null : reason,
                Status = "Scheduled"
            };

            db.Appointments.Add(newAppointment);
            db.SaveChanges();

            Console.WriteLine("\nAppointment booked successfully!");
            Console.ReadLine();
        }

        public void EditAppointment(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the appointment to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var a = db.Appointments.Find(id);

            if (a == null)
            {
                Console.WriteLine("Appointment not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nLeave field empty to keep old value.\n");

            Console.Write($"Date & time ({a.AppointmentDate:G}): ");
            string dateInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(dateInput) &&
                DateTime.TryParse(dateInput, out DateTime newDate))
            {
                a.AppointmentDate = newDate;
            }

            Console.Write($"Reason ({a.Reason}): ");
            string reason = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(reason)) a.Reason = reason;

            Console.Write($"Status ({a.Status}) [e.g. Scheduled/Completed/Cancelled]: ");
            string status = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(status)) a.Status = status;

            Console.Write($"Doctor ID ({a.DoctorId?.ToString() ?? "none"}): ");
            string doctorInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(doctorInput))
            {
                if (int.TryParse(doctorInput, out int dId))
                    a.DoctorId = dId;
            }

            Console.Write($"Nurse ID ({a.NurseId?.ToString() ?? "none"}): ");
            string nurseInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(nurseInput))
            {
                if (int.TryParse(nurseInput, out int nId))
                    a.NurseId = nId;
            }

            db.SaveChanges();

            Console.WriteLine("\nAppointment updated!");
            Console.ReadLine();
        }

        public void CancelAppointment(ClinicDbContext db)
        {
            Console.Clear();
            Console.Write("Enter the ID of the appointment to cancel: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press ENTER...");
                Console.ReadLine();
                return;
            }

            var a = db.Appointments.Find(id);

            if (a == null)
            {
                Console.WriteLine("Appointment not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nAre you sure you want to cancel appointment #{a.AppointmentId} " +
                              $"for patient {a.PatientId} at {a.AppointmentDate:G}? (y/n)");
            if (Console.ReadLine().ToLower() != "y")
                return;

            // Option 1: mark as cancelled (safer, keeps history)
            a.Status = "Cancelled";
            db.SaveChanges();

            // Option 2 (if you really want to delete):
            // db.Appointments.Remove(a);
            // db.SaveChanges();

            Console.WriteLine("Appointment cancelled.");
            Console.ReadLine();
        }
    }
}

