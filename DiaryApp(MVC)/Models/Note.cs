using DiaryApp_MVC_.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;

namespace DiaryApp_MVC_.Models
{
    public enum NoteType
    { 
        Meeting,
        ThingToDo,
        Memo
    }

    public class Note
    {
        public int ID { get; set; }
        public NoteType Type { get; set; }
        public string Theme { get; set; }
        public DateTime StartTime { get; set; }
        public bool Active { get; set; }
        public void Update(Note note)
        {
            Type = note.Type;
            Theme = note.Theme;
            StartTime = note.StartTime;
            Active = note.Active;
        }
        //public void Deactivate()
        //{
        //    Active = false;
        //}
    }
}