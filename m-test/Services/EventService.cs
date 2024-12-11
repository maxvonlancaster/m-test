
using m_test.DAL;
using m_test.DAL.Entities;
using m_test.DAL.EventEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace m_test.Services;

public class EventService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private Random random = new Random();

    public EventService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await TestEvents();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task TestEvents() 
    {
        await using (var scope = _serviceScopeFactory.CreateAsyncScope())
        using (var demoContext = scope.ServiceProvider.GetRequiredService<LabDbContext>()) 
        {
            //demoContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [SubEvents]");
            //demoContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [EventStructures]");
            //demoContext.Database.ExecuteSqlRaw("DELETE FROM [EventNews]");
            //demoContext.Database
            //    .ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0; TRUNCATE TABLE [EventNews]");

            var metrics = new TimeSpan[] { TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero };

            Stopwatch sw = new Stopwatch();
            sw.Start();
            await InsertEventsStructure(demoContext);
            sw.Stop();
            metrics[0] = sw.Elapsed;

            sw = new Stopwatch();
            sw.Start();
            await GetEventsStructure(demoContext);
            sw.Stop();
            metrics[1] = sw.Elapsed;

            //sw = new Stopwatch();
            //sw.Start();
            //await InsertEventsNew(demoContext);
            //sw.Stop();
            //metrics[2] = sw.Elapsed;

            //sw = new Stopwatch();
            //sw.Start();
            //await GetEventsNew(demoContext);
            //sw.Stop();
            //metrics[3] = sw.Elapsed;
        }
    }

    private string[] units = new string[] { "A", "B", "C", "D" };
    private string[] assets = new string[] { "SHS", "DOW" };
    private string[] models = new string[] { "A", "B", "C", "D" };
    private string[] functionalLocation = new string[] { "A", "B", "C", "D" };

    private string[] values = new string[] { "A", "B", "C", "D"};

    public async Task InsertEventsStructure(LabDbContext demoContext)
    {
        for (var i = 0; i < 1000; i++) 
        {
            var eventStructure = new EventStructure() 
            {
                Component = values[random.Next(0, 4)],
                Asset = values[random.Next(0, 4)],
                Unit = values[random.Next(0, 4)],
                Description = "",
                FunctionalLocation = values[random.Next(0, 4)],
                Source = "",
                ModelId = values[random.Next(0, 4)],
                TimeSeries = "1,0,1"
            };
            var parentEvent = demoContext.EventStructures
                .FirstOrDefault(e => e.ModelId == eventStructure.ModelId
                && e.FunctionalLocation == eventStructure.FunctionalLocation);

            if (parentEvent != null) 
            {
                eventStructure.ParentEventId = parentEvent.Id;
            }
            await demoContext.EventStructures.AddAsync(eventStructure);
            await demoContext.SaveChangesAsync();
        }
    }

    public async Task GetEventsStructure(LabDbContext demoContext)
    {
        //var events = await demoContext.EventStructures.ToListAsync();
        var eventsGrouped = await demoContext.EventStructures
            .OrderBy(e => e.Id)
            .GroupBy(e => new { e.ModelId, e.FunctionalLocation })
            .Select(e => new EventStructure() 
            {
                CreatedTime = e.Select(c => c.CreatedTime).First(),
                TimeSeries = e.OrderBy(e => e.Id).Select(c => c.TimeSeries).Last(),
                Unit = e.Select(c => c.Unit).First(),
                Asset = e.Select(c => c.Asset).First(),
                ModelId = e.Select(c => c.ModelId).First(),
                FunctionalLocation = e.Select(c => c.FunctionalLocation).First(),
                Id = e.Select(c => c.Id).First()
            }).ToListAsync();
    }

    public async Task InsertEventsNew(LabDbContext demoContext)
    {
        for (var i = 0; i < 1000; i++) 
        {
            var asset = values[random.Next(0, 4)];
            var unit = values[random.Next(0, 4)];
            var eventNew = demoContext.EventNews
                .FirstOrDefault(e => e.Unit == unit
                && e.Asset == asset);
            if (eventNew != null)
            {
                var subEvent = new SubEvent()
                {
                    TimeSeries = "0,1,0",
                    EventNewId = eventNew.Id,
                };
                eventNew.TimeSeries = "0,1,0";
                demoContext.Update(eventNew);
                await demoContext.SubEvents.AddAsync(subEvent);
            }
            else 
            {
                await demoContext.EventNews.AddAsync(
                    new EventNew() 
                    {
                        Component = values[random.Next(0, 4)],
                        Asset = asset,
                        Unit = unit,
                        Description = "",
                        FunctionalLocation = "",
                        Source = "",
                        ModelId = ""
                    });
            }
            await demoContext.SaveChangesAsync();
        }
    }

    public async Task GetEventsNew(LabDbContext demoContext)
    {
        var events = await demoContext.EventNews
            .Include(e => e.SubEvents)
            .ToListAsync();
    }
}
