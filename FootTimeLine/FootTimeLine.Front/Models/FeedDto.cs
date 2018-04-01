using System.Collections.Generic;

namespace FootTimeLine.Front.Models
{
    public class FeedDto
    {
        public List<EventDto> EventDtos { get; set; }
        public GameModelPost GameDto { get; set; }
    }
}