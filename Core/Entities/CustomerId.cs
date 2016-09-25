﻿using Raven.Imports.Newtonsoft.Json;
using SomeBasicRavenApp.Core.Internal;
using System;
using System.ComponentModel;

namespace SomeBasicRavenApp.Core.Entities
{
    [TypeConverter(typeof(ValueTypeConverter<CustomerId>))]
    [JsonConverter(typeof(ValueTypeJsonConverter<CustomerId>))]
    public struct CustomerId : IEquatable<CustomerId>
    {
        public readonly string Value;

        public CustomerId(string value)
        {
            this.Value = value;
        }

        public readonly static CustomerId Empty = new CustomerId();

        public bool Equals(CustomerId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(Value, other.Value);
        }
        public override bool Equals(object obj)
        {
            if (obj is CustomerId)
                return Equals((CustomerId)obj);
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
        public static implicit operator String(CustomerId customerId)
        {
            return customerId.Value;
        }
    }
}