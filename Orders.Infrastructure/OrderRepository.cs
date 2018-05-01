﻿using Orders.DataModel;
using Orders.DataModel.Enums;
using Orders.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orders.Infrastructure
{
    public class OrderRepository
    {
        private Random _random;

        public OrderRepository()
        {
            _random = new Random();
        }

        public void PlaceOrder(string customerName, string identificationNumber, string supplierName, List<int> productIds)
        {
            using (var ctx = new OrdersDbContext())
            {
                var supplier = ctx.Suppliers.FirstOrDefault(s => s.Name == supplierName);

                if (supplier == null)
                {
                    return;
                }

                var customer = ctx.Customers
                    .FirstOrDefault(c => c.Name == customerName && c.IdentificationNumber == identificationNumber);

                if (customer == null)
                {
                    customer = new Customer
                    {
                        Name = customerName,
                        IdentificationNumber = identificationNumber
                    };
                }

                var order = new Order
                {
                    Customer = customer,
                    Supplier = supplier,
                    OrderedDate = DateTime.Now,
                    Status = OrderStatus.Opened,
                    ExpectedDate = DateTime.Now.AddDays(_random.Next(1, 10))
                };

                foreach (var productId in productIds)
                {
                    var product = ctx.Products.Find(productId);
                    order.Products.Add(product);
                }

                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Order> GetByCustomer(string customerName, string identificationNumber)
        {
            using (var ctx = new OrdersDbContext())
            {
                var customer = ctx.Customers
                    .FirstOrDefault(c => c.Name == customerName && c.IdentificationNumber == identificationNumber);

                return customer == null ? new List<Order>() : customer.Orders.ToList();
            }
        }
    }
}