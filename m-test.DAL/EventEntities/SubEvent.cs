namespace m_test.DAL.EventEntities;

public class SubEvent
{
    public int Id { get; set; }
    public int EventNewId { get; set; }
    public string? TimeSeries { get; set; }
    public string? Flag { get; set; }


    public virtual EventNew EventNew { get; set; }
}
