using MongoDB.Bson;

namespace Crezco.CodingTest.Api.Location.Storage;

public record IpLocation(ObjectId Id, string Ip, Location Location);