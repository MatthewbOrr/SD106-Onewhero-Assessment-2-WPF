using System;


namespace SD106_Onewhero_Assessment_2.Model
{
    public class BookingItem
    {
        public int BookingId { get; set; }
        public int EventTitle { get; set; }
        public int NumberOfTickets { get; set; }
        public string Status { get; set; }

        public DateTime? BookingDate { get; set; }
    }
}
