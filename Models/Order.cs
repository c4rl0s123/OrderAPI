using System;
using System.Collections.Generic;

#nullable disable

namespace OrderAPI.Models
{
    public partial class Order
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public decimal? Total { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
