﻿using Orders.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.DataModel.Models
{
    public class Order
    {
        public Order()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTime OrderedDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}
