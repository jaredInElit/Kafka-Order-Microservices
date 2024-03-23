using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Kafka_Ordering_API.Models
{
    [DataContract]
    public partial class GetOrderDto : IEquatable<GetOrderDto>
    {
        // MongoDB BsonId for the Order
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [BsonElement("Name")]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [BsonElement("Address")]
        [DataMember(Name = "address")]
        public string Address { get; set; }

        [BsonElement("Email")]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [BsonElement("ProductID")]
        [DataMember(Name = "productID")]
        public int? ProductID { get; set; }

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
            return Equals(obj as GetOrderDto);
        }

        // Implementation of IEquatable Equals
        public bool Equals(GetOrderDto other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Address == other.Address &&
                   Email == other.Email &&
                   ProductID == other.ProductID &&
                   Quantity == other.Quantity;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Address, Email, ProductID, Quantity);
        }

        // Overloading operators for Order comparison
        public static bool operator ==(GetOrderDto left, GetOrderDto right)
        {
            return EqualityComparer<GetOrderDto>.Default.Equals(left, right);
        }

        public static bool operator !=(GetOrderDto left, GetOrderDto right)
        {
            return !(left == right);
        }
    }

}
