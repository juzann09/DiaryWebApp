using DiaryApp_MVC_.Models;
using System.Linq;

namespace DiaryApp_MVC_
{
    public static class SampleData
    {
        public static void Initialize(ApplicationContext context)
        {
            //delete all notes from db
            //if (context.Meetings.Any())
            //{
            //    var count = context.Meetings.Count();
            //    for (int i = 0; i < count; i++)
            //    {
            //        var noteToRemove = context.Meetings.ToList()[count - i - 1];
            //        context.Meetings.Remove(noteToRemove);
            //        context.SaveChanges();
            //    }
            //}
            //if (context.Memos.Any())
            //{
            //    var count = context.Memos.Count();
            //    for (int i = 0; i < count; i++)
            //    {
            //        var noteToRemove = context.Memos.ToList()[count - i - 1];
            //        context.Memos.Remove(noteToRemove);
            //        context.SaveChanges();
            //    }
            //}
            //if (context.ThingsToDo.Any())
            //{
            //    var count = context.ThingsToDo.Count();
            //    for (int i = 0; i < count; i++)
            //    {
            //        var noteToRemove = context.ThingsToDo.ToList()[count - i - 1];
            //        context.ThingsToDo.Remove(noteToRemove);
            //        context.SaveChanges();
            //    }
            //}
            //add notes to db
            if (!context.Meetings.Any())
            {
                context.Meetings.AddRange(
                    new Meeting
                    {
                        Type = NoteType.Meeting,
                        Theme = "Обсудить 1-ю главу ВКР",
                        StartTime = new System.DateTime(2021, 5, 19, 16, 0, 0),
                        EndTime = new System.DateTime(2021, 5, 19, 17, 0, 0),
                        Place = "ИжГту, аудитория 2-204",
                        Active= true
                    },
                    new Meeting
                    {
                        Type = NoteType.Meeting,
                        Theme = "Обсудить презентацию ВКР",
                        StartTime = new System.DateTime(2021, 5, 21, 10, 0, 0),
                        EndTime = new System.DateTime(2021, 5, 21, 11, 0, 0),
                        Place = "ИжГту, аудитория 2-220",
                        Active = true
                    });
            }
            if (!context.Memos.Any())
            {
                context.Memos.AddRange(
                    new Memo
                    {
                        Type = NoteType.Memo,
                        Theme = "В 1-ой главе подробнее описать актуальность",
                        StartTime = new System.DateTime(2021, 5, 19, 18, 0, 0),
                        Active = true
                    },
                    new Memo
                    {
                        Type = NoteType.Memo,
                        Theme = "В презентации убрать лишние слайды, убрать картинки",
                        StartTime = new System.DateTime(2021, 5, 21, 18, 0, 0),
                        Active = true
                    });
            }
            if (!context.ThingsToDo.Any())
            {
                context.ThingsToDo.AddRange(
                    new ThingToDo
                    {
                        Type = NoteType.ThingToDo,
                        Theme = "Внести изменения в 1-ю главу ВКР",
                        StartTime = new System.DateTime(2021, 5, 19, 18, 0, 0),
                        EndTime = new System.DateTime(2021, 5, 21, 9, 0, 0),
                        Active = true
                    },
                    new ThingToDo
                    {
                        Type = NoteType.ThingToDo,
                        Theme = "Внести изменения в презентацию ВКР",
                        StartTime = new System.DateTime(2021, 5, 21, 11, 0, 0),
                        EndTime = new System.DateTime(2021, 5, 23, 15, 0, 0),
                        Active = true
                    }
                );
            }
            context.SaveChanges();
        }
    }
}