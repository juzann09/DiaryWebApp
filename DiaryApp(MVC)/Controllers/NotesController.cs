using DiaryApp_MVC_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        static NoteType noteTypeToFilter;
        static DateTime dateTimeToFilter;
        static bool typeFilterUsed = false;
        static bool dateFilterUsed = false;
        public NotesController(ApplicationContext context)
        {
            ci = new CultureInfo("ru-RU");
            dtfi = ci.DateTimeFormat;
            db = context;
            GetNotesFromDB();
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
        // начало
        public IActionResult Index()
        {
            return RedirectToAction("FilterNotes");
        }
        // показывает список заметок
        // (если был применен фильтр, то отфильтрованный список)
        public IActionResult FilterNotes()
        {
            // если не примененен никакой фильтр, то
            // записать в список заметок все заметки из базы
            if (!(typeFilterUsed | dateFilterUsed))
                FilteredNotesList = AllNotesList;
            return View(FilteredNotesList);
        }
        // записывает в список заметок заметки, подходящие по типу
        // и перенаправляет на страницу заметок
        public IActionResult FilterNotesByType(NoteType noteType, bool typeFilter)
        {
            typeFilterUsed = true;
            if (typeFilterUsed)
            {
                ViewBag.ChosenType = noteTypeToFilter;
                FilteredNotesList = AllNotesList.FindAll(x => x.Type == noteTypeToFilter);
                noteTypeToFilter = noteType;
            }
            return RedirectToAction("FilterNotes");
        }
        // записывает в список заметок заметки, подходящие по дню
        // и перенаправляет на страницу заметок
        public IActionResult FilterNotesByDay(DateTime chosenDay, bool dateFilter)
        {
            dateFilterUsed = true;
            if (dateFilterUsed)
            {
                FilteredNotesList = AllNotesList.FindAll(
                    x => x.StartTime.ToShortDateString()
                    == chosenDay.ToShortDateString());
                dateTimeToFilter = chosenDay;
            }
            return RedirectToAction("FilterNotes");
        }
        // выдает список заметок на выбранной неделе
        public List<Note> GetWeekNotes(string chosenWeekStr)
        {
            int year;
            int weekNum;
            if (chosenWeekStr == null) return null;
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
                //ViewBag.WeekStartDate = weekStart;
                //ViewBag.WeekEndDate = ViewBag.WeekStartDate.AddDays(weekLength);
                var weekNotes = AllNotesList.Where(x => (x.StartTime >= weekStart)
                    & (x.StartTime <= weekEnd));
                return weekNotes.ToList();
            }
            else return null;
        }
        // записывает в список заметок заметки, подходящие по неделе
        // и перенаправляет на страницу заметок
        public IActionResult FilterNotesByWeek(string chosenWeekStr, bool dateFilter)
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
        public IActionResult FilterNotesByMonth(DateTime chosenDate, bool dateFilter)
        {
            dateFilterUsed = true;
            if (dateFilterUsed)
            {
                FilteredNotesList = GetMonthNotes(chosenDate);
            }
            ViewBag.ChosenMonthName = dtfi.MonthNames[chosenDate.Month - 1].ToLower();
            return RedirectToAction("FilterNotes");
        }
        // сбрасывает фильтры и перенаправляет к списку всех заметок
        public IActionResult CancelFilter()
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
        // выдает список заметок в выбранном месяце
        public List<Note> GetMonthNotes(DateTime chosenDate)
        {
            if (IsDateDefault(chosenDate)) return null;
            ViewBag.ChosenMonth = chosenDate;
            var monthNotes = AllNotesList.Where(x => (x.StartTime.Month == chosenDate.Month)
                & (x.StartTime.Year == chosenDate.Year));
            return monthNotes.ToList();
        }
        // перенаправляет к заполнению полей для новой заметки
        public IActionResult CreateNote(NoteType noteType)
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
        public IActionResult DeleteNote(int noteID, NoteType noteType)
        {
            if (noteType == NoteType.Meeting) db.Remove(db.Meetings.First(x => x.ID == noteID));
            else if (noteType == NoteType.ThingToDo) db.Remove(db.ThingsToDo.First(x => x.ID == noteID)); 
            else db.Remove(db.Memos.First(x => x.ID == noteID)); 
            db.SaveChanges();
            GetNotesFromDB();
            return RedirectToAction("Index");
        }
        // отмечвет заметку как выполненную
        public IActionResult DeactivateNote(int noteID, NoteType noteType)
        {
            if (noteType == NoteType.Meeting) (db.Meetings.First(x => x.ID == noteID)).Active=false;
            else if (noteType == NoteType.ThingToDo) (db.ThingsToDo.First(x => x.ID == noteID)).Active = false;
            else (db.Memos.First(x => x.ID == noteID)).Active = false;
            db.SaveChanges();
            GetNotesFromDB();
            return RedirectToAction("Index");
        }
        // перенаправляет к форме редактирования заметки
        public IActionResult EditNote(int noteID, NoteType noteType)
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
    }
}
