using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SD106_Onewhero_Assessment_2.Model
{
    public class Booking
    {
        [Key]
        [Column("booking_id")]
        public int BookingId { get; set; }

        [Required]
        [Column("event_id")]
        public int EventId { get; set; }

        [Required]
        [Column("visitor_id")]
        public int VisitorId { get; set; }

        [ForeignKey("VisitorId")]
        public User? Visitor { get; set; }

        [Column("booking_datetime")]
        public DateTime BookingDateTime { get; set; } = DateTime.Now;

        [Column("number_of_tickets")]
        public int NumberOfTickets { get; set; } = 1;

        [Column("status")]
        public string Status { get; set; } = "pending"; // pending, confirmed, cancelled = Default setup with pending

    }
}
