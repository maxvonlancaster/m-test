using m_test.DAL.Entities;
using m_test.MongoDB;
using m_test.MongoDB.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace m_test.Services;

public class StudentService : IStudentService
{
    private readonly IMongoCollection<StudentDto> _studentsCollection;

    public StudentService(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _studentsCollection = mongoDatabase.GetCollection<StudentDto>("Students");
    }

    public async Task<List<StudentDto>> GetAsync() =>
        await _studentsCollection.Find(s => true).ToListAsync();

    public async Task<StudentDto> GetByIdAsync(string id) =>
        await _studentsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(StudentDto student) =>
        await _studentsCollection.InsertOneAsync(student);

    public async Task UpdateAsync(string id, StudentDto updatedStudent) =>
        await _studentsCollection.ReplaceOneAsync(s => s.Id == id, updatedStudent);

    public async Task RemoveAsync(string id) =>
        await _studentsCollection.DeleteOneAsync(s => s.Id == id);
}
