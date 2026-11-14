
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Enrolment
{
    [Key]
    public int EnrolmentID { get; set; }

    public int ParticipantID { get; set; }
    public int EventID { get; set; }
    public int CategoryID { get; set; }

    public DateTime EnrolmentDate { get; set; } = DateTime.Now;

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Enrolled";

    [Required, MaxLength(20)]
    public string PaymentStatus { get; set; } = "Pending";

    public decimal PaymentAmount { get; set; }

    public string? Notes { get; set; }

    public User? Participant { get; set; }
    public Event? Event { get; set; }
    public Category? Category { get; set; }
    public Result? Result { get; set; }
}
