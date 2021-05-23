using DiaryApp_MVC_.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;

namespace DiaryApp_MVC_.Models
{
    public class ThingToDo : Note
    {
        public DateTime EndTime { get; set; }
        public ThingToDo() { }
        public void Update(ThingToDo thingToDo)
        {
            base.Update(thingToDo);
            EndTime = thingToDo.EndTime;
        }
    }
}