using DiaryApp_MVC_.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DiaryApp_MVC_.Controllers
{
    public class NotesController : Controller
    {
        ApplicationContext db;
        static List<Note> AllNotesList;
        static List<Note> FilteredNotesList;
        CultureInfo ci;
        DateTimeFormatInfo dtfi;
        Calendar greg = new GregorianCalendar();
        const int weekLength = 7;
       // static string noteTypeToFilter;
        static DateTime dateTimeToFilter;
        static bool typeFilterUsed = false;
        static bool dateFilterUsed = false;
        static string message;
        public NotesController(ApplicationContext context)
        {
            ci = new CultureInfo("ru-RU");
            dtfi = ci.DateTimeFormat;
            db = context;
            GetNotesFromDB();
        }
        // начало, перенаправляет на страницу со списком заметок
        public IActionResult Index()
        {
            return RedirectToAction("FilterNotes");
        }
        // перенаправляет на страницу с сообщением, что заметки не найдены
        public IActionResult NotesNotFound()
        {
            return View("NotesNotFound",message);
        }
        // показывает список заметок
        // (если был применен фильтр, то отфильтрованный список)
        public IActionResult FilterNotes()
        {
            // если не примененен никакой фильтр, то
            // записать в список заметок все заметки из базы
            if (!(typeFilterUsed | dateFilterUsed))
                FilteredNotesList = AllNotesList;
            if (FilteredNotesList == null) return RedirectToAction("NotesNotFound");
            else if (FilteredNotesList.Count==0)
            {
                return RedirectToAction("NotesNotFound");
            }
            return View(FilteredNotesList);
        }
        // записывает в список заметок заметки, подходящие по типу
        // и перенаправляет на страницу заметок
        public IActionResult FilterNotesByType(string noteType)
        {
            typeFilterUsed = true;
            if (typeFilterUsed)
            {
                ViewBag.ChosenType = noteType;
                FilteredNotesList = AllNotesList.FindAll(x => x.Type == noteType);
            }
            return RedirectToAction("FilterNotes");
        }
        // записывает в список заметок заметки, подходящие по дню
        // и перенаправляет на страницу заметок
        public IActionResult FilterNotesByDay(DateTime chosenDay)
        {
            dateFilterUsed = true;
            if (dateFilterUsed)
            {
                FilteredNotesList = AllNotesList.FindAll(
                    x => x.StartTime.ToShortDateString()
                    == chosenDay.ToShortDateString());
                dateTimeToFilter = chosenDay;
            }
            message = $"на {chosenDay.ToShortDateString()}";
            return RedirectToAction("FilterNotes");
        }
        // записывает в список заметок заметки, подходящие по неделе
        // и перенаправляет на страницу заметок
        public IActionResult FilterNotesByWeek(string chosenWeekStr)
        {
            dateFilterUsed = true;
            if (dateFilterUsed)
            {
                FilteredNotesList = GetWeekNotes(chosenWeekStr);
            }
            return RedirectToAction("FilterNotes");
        }
        // записывает в список заметок заметки, подходящие по месяцу
        // и перенаправляет на страницу заметок
        public IActionResult FilterNotesByMonth(DateTime chosenDate)
        {
            dateFilterUsed = true;
            if (dateFilterUsed)
            {
                FilteredNotesList = GetMonthNotes(chosenDate);
            }
            var chosenMonthName = dtfi.MonthNames[chosenDate.Month - 1].ToLower();
            message = $"на {chosenMonthName}";
            return RedirectToAction("FilterNotes");
        }
        // сбрасывает фильтры и перенаправляет к списку всех заметок
        public IActionResult CancelFilters()
        {
            DropFilters();
            return RedirectToAction("Index");
        }
        // сбрасывает фильтры по типу заметки и по дате
        // и в список заметок записывает все заметки из базы
        private void DropFilters()
        {
            typeFilterUsed = false;
            dateFilterUsed = false;
            FilteredNotesList = AllNotesList;
        }
        // перенаправляет к заполнению полей для новой заметки
        public IActionResult CreateNote(string noteType)
        {
            ViewBag.ChosenNoteType = noteType;
            ViewBag.NoteIsMeeting = (noteType == NoteType.Meeting);
            ViewBag.NoteIsMemo = (noteType == NoteType.Memo);
            ViewBag.NoteIsThingToDo = (noteType == NoteType.ThingToDo);
            return View();
        }
        // добавляет в базу встречу
        public bool AddMeeting(Meeting meeting)
        {
            if (IsDateDefault(meeting.StartTime)) return false;
            db.Meetings.Add(meeting);
            db.SaveChanges();
            GetNotesFromDB();
            return true;
        }
        // добавляет в базу напоминание
        public bool AddMemo(Memo memo)
        {
            if (IsDateDefault(memo.StartTime)) return false;
            db.Memos.Add(memo);
            db.SaveChanges();
            GetNotesFromDB();
            return true;
        }
        // добавляет в базу дело
        public bool AddThingToDo(ThingToDo thingToDo)
        {
            if (IsDateDefault(thingToDo.StartTime)) return false;
            db.ThingsToDo.Add(thingToDo);
            db.SaveChanges();
            GetNotesFromDB();
            return true;
        }
        // удаляет заметку
        public IActionResult DeleteNote(int noteID, string noteType)
        {
            if (noteType == NoteType.Meeting) db.Remove(db.Meetings.First(x => x.ID == noteID));
            else if (noteType == NoteType.ThingToDo) db.Remove(db.ThingsToDo.First(x => x.ID == noteID)); 
            else db.Remove(db.Memos.First(x => x.ID == noteID)); 
            db.SaveChanges();
            GetNotesFromDB();
            if (dateFilterUsed | typeFilterUsed)
                FilteredNotesList.Remove(FilteredNotesList.First(x => x.ID == noteID));

            return RedirectToAction("Index");
        }
        // отмечвет заметку как выполненную
        public IActionResult DeactivateNote(int noteID, string noteType)
        {
            if (noteType == NoteType.Meeting) (db.Meetings.First(x => x.ID == noteID)).Active=false;
            else if (noteType == NoteType.ThingToDo) (db.ThingsToDo.First(x => x.ID == noteID)).Active = false;
            else (db.Memos.First(x => x.ID == noteID)).Active = false;
            db.SaveChanges();
            GetNotesFromDB();
            if (dateFilterUsed | typeFilterUsed) 
                FilteredNotesList.First(x => x.ID == noteID).Active = false;
            
            return RedirectToAction("Index");
        }
        // перенаправляет к форме редактирования заметки
        public IActionResult EditNote(int noteID, string noteType)
        {
            var note = AllNotesList.Find(x => (x.ID == noteID) & (x.Type==noteType));
            ViewBag.StartDateTimeStr = note.StartTime.ToString("s");
            if (note.Type == NoteType.Meeting)
            {
                ViewBag.EndDateTimeStr = ((Meeting)note).EndTime.ToString("s");
                return View((Meeting)note);
            }
            else if (note.Type == NoteType.Memo) return View((Memo)note);
            else
            {
                ViewBag.EndDateTimeStr = ((ThingToDo)note).EndTime.ToString("s");
                return View((ThingToDo)note);
            }
        }
        // обновляет встречу в базе
        public bool UpdateMeeting(Meeting meeting)
        {
            var oldMeeting = db.Meetings.First(x => x.ID == meeting.ID);
            oldMeeting.Update(meeting);
            db.SaveChanges();
            GetNotesFromDB();
            return true;
        }
        // обновляет напоминание в базе
        public bool UpdateMemo(Memo memo)
        {
            var oldMemo = db.Memos.First(x => x.ID == memo.ID);
            oldMemo.Update(memo);
            db.SaveChanges();
            GetNotesFromDB();
            return true;
        }
        // обновляет дело в базе
        public bool UpdateThingToDo(ThingToDo thingToDo)
        {
            var oldThingToDo = db.ThingsToDo.First(x => x.ID == thingToDo.ID);
            oldThingToDo.Update(thingToDo);
            db.SaveChanges();
            GetNotesFromDB();
            return true;
        }
        // проверяет, является ли дата датой по-умолчанию
        private bool IsDateDefault(DateTime dateTime)
        {
            if (dateTime.CompareTo(DateTime.MinValue) > 0) return false;
            return true;
        }
        // выдает список заметок на выбранной неделе
        private List<Note> GetWeekNotes(string chosenWeekStr)
        {
            int year;
            int weekNum;
            if (chosenWeekStr == null)
            {
                message = $"с {DateTime.MinValue.ToShortDateString()} " +
                    $"по {DateTime.MinValue.AddDays(7).ToShortDateString()}";
                return null;
            }
            if (int.TryParse(chosenWeekStr.Split("-W")[0], out year)
                & (int.TryParse(chosenWeekStr.Split("-W")[1], out weekNum)))
            {
                // первый день года
                var yearStart = new DateTime(year, 1, 1);
                // первый денб первой недели года
                var firstWeekStart = yearStart;
                // если год начался не с понедельника
                if (yearStart.DayOfWeek != DayOfWeek.Monday)
                {
                    // разница между первым днем года и следующим понедельником
                    var dif = 7 - (yearStart.DayOfWeek - DayOfWeek.Monday);
                    firstWeekStart = yearStart.AddDays(dif);
                }
                var weekStart = firstWeekStart.AddDays((weekNum - 1) * weekLength);
                var weekEnd = weekStart.AddDays(weekLength);
                var weekNotes = AllNotesList.Where(x => (x.StartTime >= weekStart)
                    & (x.StartTime <= weekEnd));
                message = $"с {weekStart.ToShortDateString()} по {weekEnd.ToShortDateString()}";
                return weekNotes.ToList();
            }
            else return null;
        }
        // выдает список заметок в выбранном месяце
        public List<Note> GetMonthNotes(DateTime chosenDate)
        {
            if (IsDateDefault(chosenDate)) return null;
            ViewBag.ChosenMonth = chosenDate;
            var monthNotes = AllNotesList.Where(x => (x.StartTime.Month == chosenDate.Month)
                & (x.StartTime.Year == chosenDate.Year));
            return monthNotes.ToList();
        }
        // записывает в список NotesList все заметки из БД
        private void GetNotesFromDB()
        {
            var notes = new List<Note>();
            foreach (var meeting in db.Meetings.ToList())
            {
                Note noteToAppend = meeting;
                notes.Add(noteToAppend);
            }
            foreach (var thing in db.ThingsToDo.ToList())
            {
                Note noteToAppend = thing;
                notes.Add(noteToAppend);
            }
            foreach (var memo in db.Memos.ToList())
            {
                Note noteToAppend = memo;
                notes.Add(noteToAppend);
            }
            AllNotesList = notes;
        }
    }
}
