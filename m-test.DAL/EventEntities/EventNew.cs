using m_test.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m_test.DAL.EventEntities;

public class EventNew
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
    public string? TimeSeries { get; set; }
    public virtual ICollection<SubEvent> SubEvents { get; set; }
}
