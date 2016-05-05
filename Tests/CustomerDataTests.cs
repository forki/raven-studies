﻿using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using Order = SomeBasicRavenApp.Core.Entities.Order;
using System.Linq;
using System;
using SomeBasicRavenApp.Core.Entities;
using Raven.Client;
using System.Collections.Generic;

namespace SomeBasicRavenApp.Tests
{
    [TestFixture]
    public class CustomerDataTests
    {

        private IDocumentSession _session;


        [Test]
        public void CanGetCustomerById()
        {
            var customer = _session.Load<Customer>(1);

            Assert.IsNotNull(customer);
        }

        [Test]
        public void CustomerHasOrders()
        {
            var customer = _session.Load<Customer>(1);

            Assert.True(customer.Orders.Any());
        }

        [Test]
        public void CanGetCustomerByFirstname()
        {
            var customers = _session.Query<Customer>()
                .Where(c => c.Firstname == "Steve")
                .ToList();
            Assert.AreEqual(3, customers.Count);
        }

        [Test]
        public void CanGetProductById()
        {
            var product = _session.Load<Product>(1);

            Assert.IsNotNull(product);
        }
        [Test]
        public void OrderContainsProduct()
        {
            Assert.True(_session.Load<Customer>(1).Orders.First().Products.Any(p => p.Id == 1));
        }

        [SetUp]
        public void Setup()
        {
            _session = DbContextSetUpFixture.store.OpenSession();
        }


        [TearDown]
        public void TearDown()
        {
            _session.Dispose();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var doc = XDocument.Load(Path.Combine("TestData", "TestData.xml"));
            var import = new XmlImport(doc, "http://tempuri.org/Database.xsd");
            using (var session = DbContextSetUpFixture.store.OpenSession())
            {
                import.Parse<Core.IIdentifiableByNumber>(new[] { typeof(Customer), typeof(Product) },
                                (type, obj) => session.Store(obj), onIgnore: (type, property) =>
                                {
                                    Console.WriteLine("ignoring property {1} on {0}", type.Name, property.PropertyType.Name);
                                });
                var orders = new Dictionary<int, Order>();
                import.Parse<Order>(new[] { typeof(Order) },
                                (type, obj) => orders.Add(obj.Id,obj), onIgnore: (type, property) =>
                                {
                                    Console.WriteLine("ignoring property {1} on {0}", type.Name, property.PropertyType.Name);
                                });

                import.ParseConnections("OrderProduct", "Product", "Order", (productId, orderId) =>
                {
                    var product = session.Load<Product>(productId);
                    var order = orders[orderId];
                    order.Products.Add(product);
                });

                import.ParseIntProperty("Order", "Customer", (orderId, customerId) =>
                {
                    session.Load<Customer>(customerId).Orders.Add(orders[orderId]);
                });
                session.SaveChanges();
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            using (var session = DbContextSetUpFixture.store.OpenSession())
            {
                //session.Advanced.
            }
        }
    }
}