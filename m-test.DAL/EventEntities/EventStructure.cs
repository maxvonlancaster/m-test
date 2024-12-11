using m_test.DAL.Enums;

namespace m_test.DAL.EventEntities;

public class EventStructure
{
    public int Id { get; set; }
    public string Component { get; set; }
    public string Asset { get; set; }
    public string Unit { get; set; }
    public string FunctionalLocation { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }
    public EventState EventState { get; set; }
    public string ModelId { get; set; }
    public int? EstimatedTimeToFailureInDays { get; set; }
    public EventClassification EventClassification { get; set; }
    public DateTime? CreatedTime { get; set; }

    public int? ParentEventId { get; set; }
    public string? TimeSeries { get; set; }

}
