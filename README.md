# DiaryWebApp
 Веб-приложение "Ежедневник"

Возможности:
- Отображение записей в режимах «День», «Неделя», «Месяц», «Список»;
- Фильтрация по типу записей;
- Фильтрация по дате;
- Добавление, удаление, редактирование записи;
- Отметка заметки как выполненнной;

База данных создается в файле "SampleData.cs". База данных состоит из 3-х таблиц: "Meetings" (Встречи), "ThingsToDo" (Дела) и "Memos" (Памятки). Для подключения к БД используется строка по умолчанию, прописанная в файле "appsettings.json" ("DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DiaryAppDB;Trusted_Connection=True;MultipleActiveResultSets=true")

Используемые технологии:
- Microsoft.AspNetCore.App (v.5.0.0)
- Microsoft.EntityFrameworkCore.SqlServer (v5.0.6.)
