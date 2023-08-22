using MongoDB.Driver;

namespace Crezco.CodingTest.Api.Location.Storage;

public class IpLocationStore
{
    private readonly IMongoCollection<IpLocation> _collection;

    public IpLocationStore(IMongoDatabase database)
    {
        _collection = database.GetCollection<IpLocation>("ipLocations");
    }

    public async Task Store(IpLocation ipLocation)
    {
        await _collection.InsertOneAsync(ipLocation);
    }

    public async Task<IpLocation> Latest(string ip)
    {
        return await _collection.Find(Builders<IpLocation>.Filter.Eq(x => x.Ip, ip))
            .Sort(Builders<IpLocation>.Sort.Descending(x => x.Id))
            .FirstOrDefaultAsync();
    }
}