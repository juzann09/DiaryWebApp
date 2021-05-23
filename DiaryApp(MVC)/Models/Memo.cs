using DiaryApp_MVC_.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;

namespace DiaryApp_MVC_.Models
{
    public class Memo : Note
    {
        public Memo() { }
        public void Update(Memo memo)
        {
            base.Update(memo);
        }
    }
}