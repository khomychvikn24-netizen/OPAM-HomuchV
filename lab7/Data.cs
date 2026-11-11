class Student { public string Name; public string Group; public int Score; public Student(string n,string g,int s){Name=n;Group=g;Score=s;} }
static class Data {
    public static List<Student> Students()=>new(){new("Коваленко","КН-3/1",88),new("Мельник","КН-3/2",64),new("Іщенко","КН-3/1",93),new("Бондар","КН-3/1",71),new("Савченко","КН-3/2",55),new("Ткачук","КН-3/2",90),new("Кравець","КН-3/1",76),new("Іванів","КН-3/1",48),new("Мороз","КН-3/2",82),new("Шевчук","КН-3/1",67),new("Бойко","КН-3/2",95),new("Кушнір","КН-3/2",73)};
}
