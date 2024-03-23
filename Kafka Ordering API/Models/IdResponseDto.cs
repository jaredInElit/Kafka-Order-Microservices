using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Kafka_Ordering_API.Models
{
    [DataContract]
    public partial class IdResponseDto : IEquatable<IdResponseDto>
    {
        // MongoDB BsonId for the IdResponse
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        // Convert the IdResponse to JSON
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        // Override Equals for IdResponse comparison
        public override bool Equals(object obj)
        {
            return Equals(obj as IdResponseDto);
        }

        // Implementation of IEquatable Equals
        public bool Equals(IdResponseDto other)
        {
            return other != null && Id == other.Id;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        // Overloading operators for IdResponse comparison
        public static bool operator ==(IdResponseDto left, IdResponseDto right)
        {
            return EqualityComparer<IdResponseDto>.Default.Equals(left, right);
        }

        public static bool operator !=(IdResponseDto left, IdResponseDto right)
        {
            return !(left == right);
        }
    }
}
