using Raven.Client.Listeners;
using Raven.Json.Linq;
using SomeBasicRavenApp.Core.Entities;
using System.Linq;

namespace SomeBasicRavenApp.Core
{
    public class CustomerVersion1ToVersion2Converter : IDocumentConversionListener
    {
        public void EntityToDocument(object entity, RavenJObject document, RavenJObject metadata)
        {
        }

        public void DocumentToEntity(object entity, RavenJObject document, RavenJObject metadata)
        {
        }

        public void BeforeConversionToDocument(string key, object entity, RavenJObject metadata)
        {
        }

        public void AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
        {
            Customer c = entity as Customer;
            if (c == null)
                return;

            metadata["Customer-Schema-Version"] = 2;
            // preserve the old Name proeprty, for now.
            document["Name"] = c.Firstname + " " + c.Lastname;
        }

        public void BeforeConversionToEntity(string key, RavenJObject document, RavenJObject metadata)
        {
        }

        public void AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity)
        {
            Customer c = entity as Customer;
            if (c == null)
                return;
            if (metadata.Value<int>("Customer-Schema-Version") >= 2)
                return;

            c.Firstname = document.Value<string>("Name").Split().First();
            c.Lastname = document.Value<string>("Name").Split().Last();
        }
    }
}
