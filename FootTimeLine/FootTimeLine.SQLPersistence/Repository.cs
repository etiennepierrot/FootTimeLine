using System;

namespace FootTimeLine.SQLPersistence
{
    public class Repository
    {
        protected void UnitOfWork(Action<TimeLineContext> action)
        {
            using (var context = new TimeLineContext())
            {
                action(context);
                context.SaveChanges();
            }
        }
    }
}