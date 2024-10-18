
using m_test.DAL;
using m_test.MongoDB.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace m_test.Services
{
    public class UpdateService : IHostedService
    {
        //private readonly LabDbContext _dbContext;
        private readonly IStudentService _studentService;
        private IServiceScopeFactory _serviceScopeFactory;

        public UpdateService(IStudentService studentService, 
            IServiceScopeFactory serviceScopeFactory)
        {
            //_dbContext = dbContext;
            _studentService = studentService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //var students = _dbContext.Students;
            await using (var scope = _serviceScopeFactory.CreateAsyncScope())
            using (
                var dbContext =
            scope.ServiceProvider.GetRequiredService<LabDbContext>()
            ) 
            {
                var students = dbContext.Students;
            }
                await UpdateMongoDb();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task UpdateMongoDb() 
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) 
            {
                try 
                {
                    var student = new StudentDto() 
                    {
                        Name = "John Doe"
                    };
                    await _studentService.CreateAsync(student);

                    transaction.Complete();
                }
                catch (Exception ex) 
                {
                    throw;
                }
            }
        }
    }
}
