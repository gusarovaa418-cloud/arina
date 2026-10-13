using System.Collections.Generic;
using System.Linq;

namespace PW1.Model
{
    public class Logic
    {
        private List<Gnome> _gnomes = new List<Gnome>();
        private int _nextId = 1;

        // CRUD: Создание
        public void AddGnome(string name, string clan, string profession, int age, int beardLength)
        {
            var gnome = new Gnome
            {
                Id = _nextId++,
                Name = name,
                Clan = clan,
                Profession = profession,
                Age = age,
                BeardLength = beardLength
            };
            _gnomes.Add(gnome);
        }

        // CRUD: Чтение
        public List<Gnome> GetAllGnomes()
        {
            return _gnomes.ToList();
        }

        // CRUD: Обновление
        public bool UpdateGnome(int id, string newName, string newClan, string newProfession, int newAge, int newBeardLength)
        {
            var gnome = _gnomes.FirstOrDefault(g => g.Id == id);
            if (gnome == null) return false;

            gnome.Name = newName;
            gnome.Clan = newClan;
            gnome.Profession = newProfession;
            gnome.Age = newAge;
            gnome.BeardLength = newBeardLength;
            return true;
        }

        // CRUD: Удаление
        public bool DeleteGnome(int id)
        {
            var gnome = _gnomes.FirstOrDefault(g => g.Id == id);
            if (gnome == null) return false;

            _gnomes.Remove(gnome);
            return true;
        }

        // Бизнес-функция 1: Группировка по кланам
        public Dictionary<string, List<Gnome>> GroupGnomesByClan()
        {
            return _gnomes.GroupBy(g => g.Clan)
                          .ToDictionary(group => group.Key, group => group.ToList());
        }

        // Бизнес-функция 2: Фильтрация по профессии + сортировка по возрасту
        public List<Gnome> GetGnomesByProfession(string profession)
        {
            return _gnomes.Where(g => g.Profession.ToLower().Contains(profession.ToLower()))
                          .OrderBy(g => g.Age)
                          .ToList();
        }
    }
}