using m_test.MongoDB.DTOs;

namespace m_test.Services;

public interface IStudentService
{
    Task CreateAsync(StudentDto student);
}
