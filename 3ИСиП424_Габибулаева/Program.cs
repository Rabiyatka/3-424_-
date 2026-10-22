using System;
using System.Collections.Generic;

namespace UniversitySystem
{
    // Базовый класс для людей (Имя, Возраст, Email)
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public Person(string name, int age, string email)
        {
            Name = name;
            Age = age;
            Email = email;
        }

        // Метод, который будет переопределяться у Студентов и Преподавателей (Полиморфизм)
        public virtual void PrintInfo()
        {
            Console.WriteLine($"Имя: {Name}, Возраст: {Age}, Email: {Email}");
        }
    }

    // Класс Преподавателя (Наследование)
    class Teacher : Person
    {
        public string Faculty { get; set; } // Кафедра / Факультет

        public Teacher(string name, int age, string email, string faculty) : base(name, age, email)
        {
            Faculty = faculty;
        }

        // Переопределяем вывод информации для преподавателя (Полиморфизм)
        public override void PrintInfo()
        {
            Console.WriteLine($"[Преподаватель] Имя: {Name}, Возраст: {Age}, Email: {Email}, Кафедра: {Faculty}");
        }
    }

    // Класс Студента (Наследование)
    class Student : Person
    {
        public string StudentId { get; set; } // Номер студенческого билета
        public List<Course> SpisokCursov { get; set; } // Список курсов студента

        public Student(string name, int age, string email, string studentId) : base(name, age, email)
        {
            StudentId = studentId;
            SpisokCursov = new List<Course>();
        }

        // Переопределяем вывод информации для студента (Полиморфизм)
        public override void PrintInfo()
        {
            Console.WriteLine($"[Студент ID: {StudentId}] Имя: {Name}, Возраст: {Age}, Email: {Email}, Курсов: {SpisokCursov.Count}");
        }
    }

    // Класс Курса
    class Course
    {
        public string Title { get; set; } // Название курса
        public Teacher Instructor { get; set; } // Преподаватель курса
        public List<Student> SpisokStudents { get; set; } // Список студентов на курсе

        public Course(string title, Teacher instructor)
        {
            Title = title;
            Instructor = instructor;
            SpisokStudents = new List<Student>();
        }
    }

    class Program
    {
        // Списки для хранения всех данных в системе
        static List<Student> allStudents = new List<Student>();
        static List<Teacher> allTeachers = new List<Teacher>();
        static List<Course> allCourses = new List<Course>();

        static void Main(string[] args)
        {
            AddDemoData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ ===");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Показать всех студентов");
                Console.WriteLine("3. Добавить преподавателя");
                Console.WriteLine("4. Показать всех преподавателей");
                Console.WriteLine("5. Создать курс");
                Console.WriteLine("6. Показать все курсы");
                Console.WriteLine("7. Записать студента на курс");
                Console.WriteLine("8. Посмотреть курсы студента");
                Console.WriteLine("9. Посмотреть студентов на курсе");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите действие (0-9): ");

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1": AddStudent(); break;
                    case "2": ShowStudents(); break;
                    case "3": AddTeacher(); break;
                    case "4": ShowTeachers(); break;
                    case "5": AddCourse(); break;
                    case "6": ShowCourses(); break;
                    case "7": ZapisatStudentToCourse(); break;
                    case "8": ShowStudentCourses(); break;
                    case "9": ShowCourseStudents(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод. Нажмите любую клавишу..."); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
            }
        }

        // 1. Добавление студента с проверками
        static void AddStudent()
        {
            Console.WriteLine("--- Добавление нового студента ---");

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Ошибка: Имя не должно быть пустым!"); return; }

            Console.Write("Введите возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0) { Console.WriteLine("Ошибка: Возраст должен быть положительным числом!"); return; }

            Console.Write("Введите email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) { Console.WriteLine("Ошибка: Некорректный email!"); return; }

            Console.Write("Введите ID студенческого билета: ");
            string id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id)) { Console.WriteLine("Ошибка: ID не должен быть пустым!"); return; }

            // Создаем и добавляем студента
            Student newStudent = new Student(name, age, email, id);
            allStudents.Add(newStudent);
            Console.WriteLine("\nСтудент успешно добавлен!");
        }

        // 2. Показать всех студентов
        static void ShowStudents()
        {
            Console.WriteLine("--- Список всех студентов ---");
            if (allStudents.Count == 0) Console.WriteLine("Студентов пока нет.");

            foreach (var student in allStudents)
            {
                student.PrintInfo(); // Вызовется метод из класса Student
            }
        }

        // 3. Добавление преподавателя с проверками
        static void AddTeacher()
        {
            Console.WriteLine("--- Добавление нового преподавателя ---");

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Ошибка: Имя не должно быть пустым!"); return; }

            Console.Write("Введите возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0) { Console.WriteLine("Ошибка: Возраст должен быть положительным числом!"); return; }

            Console.Write("Введите email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) { Console.WriteLine("Ошибка: Некорректный email!"); return; }

            Console.Write("Введите кафедру (факультет): ");
            string faculty = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(faculty)) { Console.WriteLine("Ошибка: Кафедра не должна быть пустой!"); return; }

            Teacher newTeacher = new Teacher(name, age, email, faculty);
            allTeachers.Add(newTeacher);
            Console.WriteLine("\nПреподаватель успешно добавлен!");
        }

        // 4. Показать всех преподавателей
        static void ShowTeachers()
        {
            Console.WriteLine("--- Список всех преподавателей ---");
            if (allTeachers.Count == 0) Console.WriteLine("Преподавателей пока нет.");

            foreach (var teacher in allTeachers)
            {
                teacher.PrintInfo(); // Вызовется метод из класса Teacher
            }
        }

        // 5. Создание курса
        static void AddCourse()
        {
            Console.WriteLine("--- Создание нового курса ---");
            if (allTeachers.Count == 0)
            {
                Console.WriteLine("Ошибка: В системе нет ни одного преподавателя. Сначала добавьте преподавателя!");
                return;
            }

            Console.Write("Введите название курса: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title)) { Console.WriteLine("Ошибка: Название не должно быть пустым!"); return; }

            // Показываем список преподавателей на выбор
            Console.WriteLine("\nВыберите номер преподавателя для этого курса:");
            for (int i = 0; i < allTeachers.Count; i++)
            {
                Console.WriteLine($"{i} - {allTeachers[i].Name}");
            }

            Console.Write("Номер: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= allTeachers.Count)
            {
                Console.WriteLine("Ошибка: Неверный номер преподавателя!");
                return;
            }

            Course newCourse = new Course(title, allTeachers[index]);
            allCourses.Add(newCourse);
            Console.WriteLine($"\nКурс '{title}' успешно создан!");
        }

        // 6. Показать все курсы
        static void ShowCourses()
        {
            Console.WriteLine("--- Список доступных курсов ---");
            if (allCourses.Count == 0) Console.WriteLine("Курсов пока нет.");

            foreach (var course in allCourses)
            {
                Console.WriteLine($"Курс: '{course.Title}' | Преподаватель: {course.Instructor.Name} | Студентов записано: {course.SpisokStudents.Count}");
            }
        }

        // 7. Запись студента на курс
        static void ZapisatStudentToCourse()
        {
            Console.WriteLine("--- Запись студента на курс ---");
            Console.Write("Введите ID студенческого билета: ");
            string studentId = Console.ReadLine();
            // Ищем студента
            Student foundStudent = null;
            foreach (var s in allStudents)
            {
                if (s.StudentId.Equals(studentId, StringComparison.OrdinalIgnoreCase))
                {
                    foundStudent = s;
                    break;
                }
            }
            if (foundStudent == null) { Console.WriteLine("Ошибка: Студент с таким ID не найден!"); return; }
            Console.Write("Введите точное название курса: ");
            string courseTitle = Console.ReadLine();
            // Ищем курс
            Course foundCourse = null;
            foreach (var c in allCourses)
            {
                if (c.Title.Equals(courseTitle, StringComparison.OrdinalIgnoreCase))
                {
                    foundCourse = c;
                    break;
                }
            }
            if (foundCourse == null) { Console.WriteLine("Ошибка: Курс с таким названием не найден!"); return; }
            // Проверяем, не записан ли уже студент
            if (foundStudent.SpisokCursov.Contains(foundCourse))
            {
                Console.WriteLine("Студент уже записан на этот курс!");
                return;
            }
            // Связываем студенттов и курсы
            foundStudent.SpisokCursov.Add(foundCourse);
            foundCourse.SpisokStudents.Add(foundStudent);
            Console.WriteLine($"Успешно! Студент {foundStudent.Name} записан на курс '{foundCourse.Title}'.");
        }
        // 8. Просмотр курсов студентааа
        static void ShowStudentCourses()
        {
            Console.Write("Введите ID студента: ");
            string id = Console.ReadLine();
            foreach (var s in allStudents)
            {
                if (s.StudentId.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\nКурсы, на которые записан {s.Name}:");
                    if (s.SpisokCursov.Count == 0) Console.WriteLine("Нет активных курсов.");
                    foreach (var course in s.SpisokCursov)
                    {
                        Console.WriteLine($"- {course.Title} (Преподаватель: {course.Instructor.Name})");
                    }
                    return;
                }
            }
            Console.WriteLine("Студент с таким ID не найден.");
        }
        // 9. Просмотр студентов на курсе
        static void ShowCourseStudents()
        {
            Console.Write("Введите название курса: ");
            string title = Console.ReadLine();
            foreach (var c in allCourses)
            {
                if (c.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\nСтуденты на курсе '{c.Title}':");
                    if (c.SpisokStudents.Count == 0) Console.WriteLine("На курс еще никто не записался.");
                    foreach (var student in c.SpisokStudents)
                    {
                        Console.WriteLine($"- {student.Name} (ID: {student.StudentId})");
                    }
                    return;
                }
            }
            Console.WriteLine("Курс с таким названием не найден.");
        }
        // Автоматические демо-данные, чтобы программа не была пустой
        static void AddDemoData()
        {
            Teacher t1 = new Teacher("Крутой Препод", 45, "KrytoiPrerod@mail.ru", "Программирование");
            allTeachers.Add(t1);
            Course c1 = new Course("Основы C#", t1);
            allCourses.Add(c1);
            Student s1 = new Student("Полинка", 19, "polinka@mail.ru", "101");
            allStudents.Add(s1);
            // Сразу запишем Анну на курс "Основы C#"
            s1.SpisokCursov.Add(c1);
            c1.SpisokStudents.Add(s1);
        }
    }
}