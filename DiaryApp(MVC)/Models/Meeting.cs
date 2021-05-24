using System;

namespace DiaryApp_MVC_.Models
{
    // класс Встреча
    public class Meeting : Note
    {
        public DateTime EndTime { get; set; }
        public string Place { get; set; }
        public Meeting() { }
        public Meeting(Note parentNote)
        {
            ID = parentNote.ID;
            Type = parentNote.Type;
            Theme = parentNote.Theme;
            StartTime = parentNote.StartTime;
            Active = parentNote.Active;

            EndTime = new DateTime();
            Place = "";
        }
        public Meeting(string type, string theme, DateTime startTime, 
            DateTime endTime, string place) 
            : base(type, theme, startTime) 
        {
            EndTime = endTime;
            Place = place;
        }
        public void Update(Meeting meeting)
        {
            base.Update(meeting);
            EndTime = meeting.EndTime;
            Place = meeting.Place;
        }
    }
}