using System.Windows;

namespace Humanity.Templates
{
    class Human
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public uint Age { get; set; }
        public float HitPoints { get; set; }
        public float Mana { get; set; }
        public uint Id { get; set; }
        public float Strength { get; set; }

        public Human(){ }
        public Human(string name, string surname, uint age, int hp, int mana, uint id, float str)
        {
            Name = name;
            Surname = surname;
            Age = age;
            HitPoints = hp;
            Mana = mana;
            Id = id;
            Strength = str;
        }
        public void ShowHuman()
        {
            MessageBox.Show("Name - "+ Name +"\n" + "Surname - " + Surname + "\n" + "Age - " + Age + "\n" + "Id - " + Id + "\n" + "HP - " + HitPoints + "\n" + "Mana - " + Mana + "\n" + "Strength - " + Strength);
        }
        public void HitHuman(float strength, Human human)
        {
            human.HitPoints = human.HitPoints - strength;
            MessageBox.Show($"{human.Name} \n Id - {human.Id} \n Your HP - {human.HitPoints}");
        }
        //public override string ToString()
        //{
        //    return $"Id - {Id}\nName - {Name}";
        //    //return ("Name - " + Name + "\n" + "Surname - " + Surname + "\n" + "Age - " + Age + "\n" + "Id - " + Id + "\n" + "HP - " + HitPoints + "\n" + "Mana - " + Mana + "\n" + "Strength - " + Strength);
        //}
        public void ShowSelectedItem(Human h)
        {
            string txtstr = "Name - " + h.Name + "\n" + "Surname - " + h.Surname + "\n" + "Age - " + h.Age + "\n" + "Id - " + h.Id + "\n" + "HP - " + h.HitPoints + "\n" + "Mana - " + h.Mana + "\n" + "Strength - " + h.Strength;
            MessageBox.Show(txtstr);
        }
    }
}
