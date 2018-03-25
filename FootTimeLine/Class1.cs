using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FootTimeLine.EventCollectorTest
{
    public class SportdeerEventCollectorTest
    {
        [Test]
        public void METHOD()
        {
            SportdeerEventCollector collector = new SportdeerEventCollector();
            List<MatchEvent> matchEvents = collector.CollectEvent("OM-OL");
        }
    }

    public class MatchEvent
    {
    }

    public class SportdeerEventCollector
    {
        public List<MatchEvent> CollectEvent(string match)
        {
            throw new NotImplementedException();
        }
    }
}
