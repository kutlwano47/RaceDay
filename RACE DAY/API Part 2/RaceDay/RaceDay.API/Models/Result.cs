
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Result
{
    [Key]
    public int ResultID { get; set; }

    public int EnrolmentID { get; set; }

    public int? Position { get; set; }

    public TimeSpan? FinishTime { get; set; }

    public decimal? Score { get; set; }

    public string? Remarks { get; set; }

    public DateTime DateRecorded { get; set; } = DateTime.Now;

    public Enrolment? Enrolment { get; set; }
}
