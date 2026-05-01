using RoomManagementSystem.Models;
using RoomManagementSystem.Models.Entities;

namespace RoomManagementSystem.Repositories;

public class ReservationRepository
{
    private readonly List<Reservation> _reservations =//todo change to map
    [
        new()
        {
            Id = 1, RoomId = 1, OrganizerName = "Jim Halpert", Topic = "MAS Lecture", Date = new DateOnly(2026, 04, 29),
            StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(14, 30), Status = ReservationStatus.Pending,
        },
        new()
        {
            Id = 2, RoomId = 2, OrganizerName = "Michael Scott", Topic = "APBD Laboratory", Date = new DateOnly(2026, 04, 30),
            StartTime = new TimeOnly(08, 00), EndTime = new TimeOnly(10, 00), Status = ReservationStatus.Pending
        },
        new()
        {
            Id = 3, RoomId = 3, OrganizerName = "Kevin Malone", Topic = "Project Meeting", Date = new DateOnly(2026, 04, 29),
            StartTime = new TimeOnly(10, 00), EndTime = new TimeOnly(11, 30), Status = ReservationStatus.Confirmed
        },
        new()
        {
            Id = 4, RoomId = 5, OrganizerName = "Ryan Howard", Topic = "Staff Briefing", Date = new DateOnly(2026, 05, 01),
            StartTime = new TimeOnly(12, 00), EndTime = new TimeOnly(13, 00), Status = ReservationStatus.Pending
        },
        new()
        {
            Id = 5, RoomId = 1, OrganizerName = "Kelly Kapoor", Topic = "Student Consultations", Date = new DateOnly(2026, 04, 29),
            StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(17, 00), Status = ReservationStatus.Confirmed
        }
    ];

    public IReadOnlyList<Reservation> Reservations => _reservations.AsReadOnly();

    public void AddReservation(Reservation reservation) => _reservations.Add(reservation);
}