using System;


namespace SD106_Onewhero_Assessment_2.Model
{
    public class BookingItem
    {
        public int BookingId { get; set; }
        public string? EventTitle { get; set; }
        public string? Description { get; set; }
        public string? EventDate { get; set; }
        public string? Location { get; set; }
        public int NumberOfTickets { get; set; }
        public string? Status { get; set; }
    }
}
