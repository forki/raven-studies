using System;

namespace SomeBasicRavenApp.Core.Entities
{
    public struct ProductId : IEquatable<ProductId>
    {
        public readonly string Value;

        public ProductId(string value)
        {
            this.Value = value;
        }

        public readonly static ProductId Empty = new ProductId();

        public bool Equals(ProductId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(Value, other.Value);
        }
        public override bool Equals(object obj)
        {
            if (obj is ProductId)
                return Equals((ProductId)obj);
            return false;
        }
        public override int GetHashCode()
        {
            return Value != null
                ? Value.GetHashCode()
                : 0;
        }
        public override string ToString()
        {
            return Value != null
                ? Value.ToString()
                : string.Empty;
        }

    }
}
