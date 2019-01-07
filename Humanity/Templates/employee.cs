
namespace Humanity.Templates
{
    class Employee : Human
    {
        public string Company { get; set; }

        public Employee(string name, string surname, uint age, int hp, int mana, uint id, float str, string company)
            : base(name, surname, age, hp, mana, id, str)
        {
            Company = company;
        }
    }
}
