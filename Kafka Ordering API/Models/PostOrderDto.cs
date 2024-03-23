using System.Runtime.Serialization;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kafka_Ordering_API.Models
{
    [DataContract]
    public partial class PostOrderDto : IEquatable<PostOrderDto>
    {
        // MongoDB BsonId for the Order
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        // Name of the Order
        [BsonElement("Name")]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        // Address of the Order
        [BsonElement("Address")]
        [DataMember(Name = "address")]
        public string Address { get; set; }

        // Email of the Order
        [BsonElement("Email")]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        // ProductID of the Order
        [BsonElement("ProductID")]
        [DataMember(Name = "productID")]
        public int? ProductID { get; set; }

        // Quantity of the Order
        [BsonElement("Quantity")]
        [DataMember(Name = "quantity")]
        public int? Quantity { get; set; }

        // Convert the Order to JSON
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        // Override Equals for Order comparison
        public override bool Equals(object obj)
        {
            return Equals(obj as PostOrderDto);
        }

        // Implementation of IEquatable Equals
        public bool Equals(PostOrderDto other)
        {
            return other != null &&
                   Name == other.Name &&
                   Address == other.Address &&
                   Email == other.Email &&
                   EqualityComparer<int?>.Default.Equals(ProductID, other.ProductID) &&
                   EqualityComparer<int?>.Default.Equals(Quantity, other.Quantity);
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Name);
            hashCode.Add(Address);
            hashCode.Add(Email);
            hashCode.Add(ProductID);
            hashCode.Add(Quantity);
            return hashCode.ToHashCode();
        }

        // Overloading operators for Order comparison
        public static bool operator ==(PostOrderDto left, PostOrderDto right)
        {
            return EqualityComparer<PostOrderDto>.Default.Equals(left, right);
        }

        public static bool operator !=(PostOrderDto left, PostOrderDto right)
        {
            return !(left == right);
        }
    }
}
