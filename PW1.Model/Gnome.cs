using System;

namespace PW1.Model
{
    public class Gnome
    {
        public int Id { get; set; }
        public string Name { get; set; }         // Имя
        public string Clan { get; set; }         // Клан
        public string Profession { get; set; }   // Профессия
        public int Age { get; set; }             // Возраст
        public int BeardLength { get; set; }     // Длина бороды (см)

        public override string ToString()
        {
            return $"[{Id}] {Name} из клана {Clan} | {Profession} | Возраст: {Age} лет | Борода: {BeardLength} см";
        }
    }
}