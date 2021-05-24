using System;

namespace DiaryApp_MVC_.Models
{
    // ����� �������
    public class Memo : Note
    {
        public Memo() { }
        public Memo(string type, string theme, DateTime startTime)
            : base(type, theme, startTime) { }
        public void Update(Memo memo)
        {
            base.Update(memo);
        }
    }
}