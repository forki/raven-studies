using Raven.Client.UniqueConstraints;
using SomeBasicRavenApp.Core;
using SomeBasicRavenApp.Core.Entities;
using System.Collections.Generic;

namespace SomeBasicRavenApp.Tests.TestData
{
    class Customer1: IIdentifiableByNumber
    {
        public Customer1()
        {
            Orders = new List<Order>();
        }
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        [UniqueConstraint]
        public virtual string Email { get; set; }

        public virtual IList<Order> Orders { get; set; }

        public virtual int Version { get; set; }
    }
}
