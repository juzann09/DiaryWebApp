using System;

namespace DiaryApp_MVC_.Models
{
    // класс Дело
    public class ThingToDo : Note
    {
        public DateTime EndTime { get; set; }
        public ThingToDo() { }
        public ThingToDo(string type, string theme, DateTime startTime,
            DateTime endTime)
            : base(type, theme, startTime)
        {
            EndTime = endTime;
        }
        public void Update(ThingToDo thingToDo)
        {
            base.Update(thingToDo);
            EndTime = thingToDo.EndTime;
        }
    }
}