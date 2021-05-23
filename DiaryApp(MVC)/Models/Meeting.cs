using DiaryApp_MVC_.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;

namespace DiaryApp_MVC_.Models
{
    public class Meeting : Note
    {
        public DateTime EndTime { get; set; }
        public string Place { get; set; }
        public Meeting (Note parentNote)
        {
            ID = parentNote.ID;
            Type = parentNote.Type;
            Theme = parentNote.Theme;
            StartTime = parentNote.StartTime;
            Active = parentNote.Active;

            EndTime = new DateTime();
            Place = "";
        }
        public Meeting() { }
        public void Update(Meeting meeting)
        {
            base.Update(meeting);
            EndTime = meeting.EndTime;
            Place = meeting.Place;
        }
    }
}