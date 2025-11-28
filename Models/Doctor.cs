using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicPatientRegister_v2.Models;

[Index("PersonalNumber12", Name = "UQ__Doctors__61E481F4707273C9", IsUnique = true)]
public partial class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string PersonalNumber12 { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(100)]
    public string? Specialization { get; set; }

    [StringLength(100)]
    public string? HomeAddress { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [InverseProperty("Doctor")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
