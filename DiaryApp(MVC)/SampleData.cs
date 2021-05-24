using DiaryApp_MVC_.Models;
using System.Linq;

namespace DiaryApp_MVC_
{    
    // класс для создания начальной базы данных
    public static class SampleData
    {
        // создает и заполняет БД при первом запуске приложения
        public static void Initialize(ApplicationContext context)
        {
            ////delete all notes from db
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
            ////add notes to db
            if (!context.Meetings.Any())
            {
                context.Meetings.AddRange(
                    new Meeting(NoteType.Meeting,
                        "Обсудить 1-ю главу ВКР",
                       new System.DateTime(2021, 5, 19, 16, 0, 0),
                        new System.DateTime(2021, 5, 19, 17, 0, 0),
                        "ИжГту, аудитория 2-204"
                    ),
                    new Meeting(NoteType.Meeting,
                        "Обсудить презентацию ВКР",
                        new System.DateTime(2021, 5, 21, 10, 0, 0),
                        new System.DateTime(2021, 5, 21, 11, 0, 0),
                        "ИжГту, аудитория 2-220"
                    ));
            }
            if (!context.Memos.Any())
            {
                context.Memos.AddRange(
                    new Memo(NoteType.Memo,
                        "В 1-ой главе подробнее описать актуальность",
                        new System.DateTime(2021, 5, 19, 18, 0, 0)
                    ),
                    new Memo(NoteType.Memo,
                       "В презентации убрать лишние слайды, убрать картинки",
                        new System.DateTime(2021, 5, 21, 18, 0, 0)
                    ));
            }
            if (!context.ThingsToDo.Any())
            {
                context.ThingsToDo.AddRange(
                    new ThingToDo(
                        NoteType.ThingToDo,
                        "Внести изменения в 1-ю главу ВКР",
                        new System.DateTime(2021, 5, 19, 18, 0, 0),
                        new System.DateTime(2021, 5, 21, 9, 0, 0)
                    ),
                    new ThingToDo(NoteType.ThingToDo,
                        "Внести изменения в презентацию ВКР",
                        new System.DateTime(2021, 5, 21, 11, 0, 0),
                        new System.DateTime(2021, 5, 23, 15, 0, 0)
                    )
                );
            }
            context.SaveChanges();
        }
    }
}