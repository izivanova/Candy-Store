﻿using Orders.DataModel;
using Orders.DataModel.Enums;
using Orders.DataModel.Models;
using Orders.Infrastructure.DTO;
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

        public void PlaceOrder(string customerName, string identificationNumber, string supplierName, List<ProductDto> productDtos)
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

                foreach (var productDto in productDtos)
                {
                    var product = ctx.Products.Find(productDto.Id);
                    var orderDetails = new OrderDetails
                    {
                        Order = order,
                        Product = product,
                        ProductQuantity = productDto.Quantity
                    };

                    order.OrderDetails.Add(orderDetails);
                }

                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<OrderDto> GetByCustomer(string customerName, string identificationNumber)
        {
            using (var ctx = new OrdersDbContext())
            {
                var customer = ctx.Customers
                    .FirstOrDefault(c => c.Name == customerName && c.IdentificationNumber == identificationNumber);

                if(customer == null)
                {
                    return Enumerable.Empty<OrderDto>();
                }

                var result = new List<OrderDto>();
                foreach (var order in customer.Orders)
                {
                    var dto = new OrderDto
                    {
                        Supplier = order.Supplier.Name,
                        ExpectedDate = order.ExpectedDate,
                        OrderedDate = order.OrderedDate,
                        Status = order.Status.ToString()
                    };

                    dto.Products = order.OrderDetails
                        .Select(od => new ProductDto
                        {
                            Id = od.ProductId,
                            Name = od.Product.Name,
                            Price = od.Product.Price,
                            Quantity = od.ProductQuantity
                        }).ToList();

                    result.Add(dto);
                }

                return result;
            }
        }
    }
}
