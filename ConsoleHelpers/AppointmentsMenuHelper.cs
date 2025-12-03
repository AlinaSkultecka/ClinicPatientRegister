using ClinicPatientRegister_v2.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public class AppointmentsMenuHelper
    {
        // LIST ALL APPOINTMENTS
        public void ListAppointments(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== ALL APPOINTMENTS ===");
            Console.WriteLine();

            // Get all appointments + related tables (Patient, Doctor, Nurse)
            var appointments = db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Nurse)
                .ToList();

            // Go through each appointment
            foreach (var appointment in appointments)
            {
                // Patient
                string patientName = "Unknown patient";
                if (appointment.Patient != null)
                {
                    patientName = appointment.Patient.FirstName + " " + appointment.Patient.LastName;
                }

                // Doctor
                string doctorName = "No doctor";
                if (appointment.Doctor != null)
                {
                    doctorName = appointment.Doctor.FirstName + " " + appointment.Doctor.LastName;
                }

                // Nurse
                string nurseName = "No nurse";
                if (appointment.Nurse != null)
                {
                    nurseName = appointment.Nurse.FirstName + " " + appointment.Nurse.LastName;
                }

                // Reason (may be empty)
                string reasonText = "";
                if (!string.IsNullOrWhiteSpace(appointment.Reason))
                {
                    reasonText = " - Reason: " + appointment.Reason;
                }

                // Print one line to the console for this appointment
                Console.WriteLine(
                    appointment.AppointmentId + ". " +
                    appointment.AppointmentDate.ToString("yyyy-MM-dd HH:mm") + " - " +
                    patientName +
                    " - Doctor: " + doctorName +
                    " - Nurse: " + nurseName +
                    " - Status: " + appointment.Status +
                    reasonText
                );
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }

        // BOOK NEW APPOINTMENT
        public void BookAppointment(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== BOOK NEW APPOINTMENT ===");
            Console.WriteLine();

            // Ask for Patient Personal number
            Console.Write("Enter Patient Personal Number (12 digits): ");
            string patientPn = Console.ReadLine()?.Trim() ?? "";

            var patient = db.Patients.FirstOrDefault(p => p.PersonalNumber12 == patientPn);

            if (patient == null)
            {
                Console.WriteLine("Patient not found. Press ENTER to go back...");
                Console.ReadLine();
                return;
            }

            int patientId = patient.PatientId;

            // Ask for Doctor ID (optional)
            Console.Write("Enter Doctor Personal Number (optional, ENTER to skip): ");
            string doctorPn = Console.ReadLine()?.Trim() ?? "";

            int? doctorId = null;
            if (!string.IsNullOrWhiteSpace(doctorPn))
            {
                var doctor = db.Doctors.FirstOrDefault(d => d.PersonalNumber12 == doctorPn);

                if (doctor == null)
                {
                    Console.WriteLine("Doctor not found. Press ENTER to go back...");
                    Console.ReadLine();
                    return;
                }

                doctorId = doctor.DoctorId;
            }

            // Ask for Nurse ID (optional)
            Console.Write("Enter Nurse Personal Number (optional, ENTER to skip): ");
            string nursePn = Console.ReadLine()?.Trim() ?? "";

            int? nurseId = null;
            if (!string.IsNullOrWhiteSpace(nursePn))
            {
                var nurse = db.Nurses.FirstOrDefault(n => n.PersonalNumber12 == nursePn);

                if (nurse == null)
                {
                    Console.WriteLine("Nurse not found. Press ENTER to go back...");
                    Console.ReadLine();
                    return;
                }

                nurseId = nurse.NurseId;
            }

            // Ask for appointment date and time
            Console.Write("Appointment date & time (e.g. 2025-12-01 14:30): ");
            string dateInput = Console.ReadLine();

            DateTime appointmentDate;
            if (!DateTime.TryParse(dateInput, out appointmentDate))
            {
                Console.WriteLine("Invalid date/time. Press ENTER to go back...");
                Console.ReadLine();
                return;
            }

            // Ask for reason (optional)
            Console.Write("Reason (optional): ");
            string reasonInput = Console.ReadLine();

            string? reason = null;
            if (!string.IsNullOrWhiteSpace(reasonInput))
            {
                reason = reasonInput;
            }

            // Create a new Appointment object
            Appointment newAppointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                NurseId = nurseId,
                AppointmentDate = appointmentDate,
                Reason = reason,
                Status = "Scheduled"
            };

            // Save to database
            db.Appointments.Add(newAppointment);
            db.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Appointment booked successfully! Press ENTER to continue...");
            Console.ReadLine();
        }

        // EDIT EXISTING APPOINTMENT
        public void EditAppointment(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== EDIT APPOINTMENT ===");
            Console.WriteLine();

            // Ask for appointment ID
            Console.Write("Enter the ID of the appointment to edit: ");
            string idInput = Console.ReadLine();

            int id;
            if (!int.TryParse(idInput, out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER to go back...");
                Console.ReadLine();
                return;
            }

            // Find the appointment in the database
            Appointment appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                Console.WriteLine("Appointment not found. Press ENTER to go back...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Leave field empty to keep the old value.");
            Console.WriteLine();

            // Edit date & time
            Console.Write("Date & time (" + appointment.AppointmentDate.ToString("yyyy-MM-dd HH:mm") + "): ");
            string dateInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                DateTime newDate;
                if (DateTime.TryParse(dateInput, out newDate))
                {
                    appointment.AppointmentDate = newDate;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Keeping old date and time.");
                }
            }

            // Edit reason
            string currentReason = appointment.Reason ?? "none";
            Console.Write("Reason (" + currentReason + "): ");
            string reasonInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(reasonInput))
            {
                appointment.Reason = reasonInput;
            }

            // Edit status
            Console.Write("Status (" + appointment.Status + ") [e.g. Scheduled/Completed/Cancelled]: ");
            string statusInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(statusInput))
            {
                appointment.Status = statusInput;
            }

            // Edit doctor ID
            // Show current doctor personal number or "none"
            string currentDoctorPn = appointment.Doctor?.PersonalNumber12 ?? "none";

            Console.Write("Doctor Personal Number (" + currentDoctorPn + "): ");
            string doctorPnInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(doctorPnInput))
            {
                // Lookup doctor by PN
                var newDoctor = db.Doctors.FirstOrDefault(d => d.PersonalNumber12 == doctorPnInput);

                if (newDoctor != null)
                {
                    appointment.DoctorId = newDoctor.DoctorId;
                }
                else
                {
                    Console.WriteLine("❌ Doctor not found. Keeping old value.");
                }
            }

            // Edit nurse ID
            // Show current nurse personal number or "none"
            string currentNursePn = appointment.Nurse?.PersonalNumber12 ?? "none";

            Console.Write("Nurse Personal Number (" + currentNursePn + "): ");
            string nursePnInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(nursePnInput))
            {
                // Lookup nurse by PN
                var newNurse = db.Nurses.FirstOrDefault(n => n.PersonalNumber12 == nursePnInput);

                if (newNurse != null)
                {
                    appointment.NurseId = newNurse.NurseId;
                }
                else
                {
                    Console.WriteLine("❌ Nurse not found. Keeping old value.");
                }
            }

            // Save changes
            db.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Appointment updated! Press ENTER to continue...");
            Console.ReadLine();
        }

        // CANCEL APPOINTMENT
        public void CancelAppointment(ClinicDbContext db)
        {
            Console.Clear();
            Console.WriteLine("=== CANCEL APPOINTMENT ===");
            Console.WriteLine();

            // Ask for appointment ID
            Console.Write("Enter the ID of the appointment to cancel: ");
            string idInput = Console.ReadLine();

            int id;
            if (!int.TryParse(idInput, out id))
            {
                Console.WriteLine("Invalid ID. Press ENTER to go back...");
                Console.ReadLine();
                return;
            }

            // Try to find the appointment
            Appointment appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                Console.WriteLine("Appointment not found. Press ENTER to go back...");
                Console.ReadLine();
                return;
            }

            // Ask for confirmation
            Console.WriteLine();
            Console.WriteLine(
                "Are you sure you want to cancel appointment #" + appointment.AppointmentId +
                " for patient " + appointment.PatientId +
                " at " + appointment.AppointmentDate.ToString("yyyy-MM-dd HH:mm") + "? (y/n)"
            );

            string confirmation = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(confirmation) || confirmation.ToLower() != "y")
            {
                Console.WriteLine("Cancellation aborted. Press ENTER to continue...");
                Console.ReadLine();
                return;
            }

            // Mark as cancelled (safer, keeps history)
            appointment.Status = "Cancelled";
            db.SaveChanges();

            // Done
            Console.WriteLine("Appointment cancelled. Press ENTER to continue...");
            Console.ReadLine();
        }
    }
}

