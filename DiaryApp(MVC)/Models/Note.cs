using System;

namespace DiaryApp_MVC_.Models
{
    // ����� �������
    public class Note
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Theme { get; set; }
        public DateTime StartTime { get; set; }
        // Active = true, ���� ������� ��� �� �������� ��� �����������,
        // Active = false, ���� ������� �������� ��� �����������
        public bool Active { get; set; }
        public Note() { }
        public Note(string type, string theme, DateTime startTime)
        {
            Type = type;
            Theme = theme;
            StartTime = startTime;
            Active = true;
        }
        // ��������� ���������� � ������� ����������� �� ����� �������
        public void Update(Note note)
        {
            Type = note.Type;
            Theme = note.Theme;
            StartTime = note.StartTime;
            Active = note.Active;
        }
    }
}