using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProgTaskPeresdacha
{
    internal class Program
    {
        static List<Task<string>> tasks = new List<Task<string>>();
        static string filePath = "tasks.txt";
        static void Main(string[] args)
        {
            LoadTasks();

            while (true)
            {
                Console.WriteLine("\n--- ToDo List ---");
                Console.WriteLine("1. Добавить задачу");
                Console.WriteLine("2. Удалить задачу");
                Console.WriteLine("3. Просмотреть задачи");
                Console.WriteLine("4. Фильтровать задачи по приоритету");
                Console.WriteLine("5. Поиск задач");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        RemoveTask();
                        break;
                    case "3":
                        ViewTasks();
                        break;
                    case "4":
                        FilterTasks();
                        break;
                    case "5":
                        SearchTasks();
                        break;
                    case "6":
                        SaveTasks();
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                        break;
                }
            }
        }
        static void AddTask()
        {
            Console.Write("Введите описание задачи: ");
            
            string description = Console.ReadLine();

            Console.WriteLine("Укажите приоритет (1 - Высокий, 2 - Средний, 3 - Низкий): ");
            int priorityId;
            while (!int.TryParse(Console.ReadLine(), out priorityId) || priorityId < 1 || priorityId > 3)
            {
                Console.WriteLine("Пожалуйста, введите корректный номер приоритета.");
            }

            Priority priority = (Priority)priorityId;

            var task = new Task<string>(description, priority);
            tasks.Add(task);

            Console.WriteLine("Задача добавлена успешно!");
        }

        static void RemoveTask()
        {
            ViewTasks();
            Console.Write("Введите номер задачи для удаления: ");
            int taskId;
            if (int.TryParse(Console.ReadLine(), out taskId) && taskId > 0 && taskId <= tasks.Count)
            {
                tasks.RemoveAt(taskId - 1);
                Console.WriteLine("Задача удалена успешно!");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи.");
            }
        }

        static void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("Нет задач для отображения.");
                return;
            }

            Console.WriteLine("Список задач:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. [{tasks[i].TaskPriority}] {tasks[i].Description}");
            }
        }

        static void FilterTasks()
        {
            Console.WriteLine("Введите приоритет для фильтрации (1 - Высокий, 2 - Средний, 3 - Низкий): ");
            int priorityId;
            while (!int.TryParse(Console.ReadLine(), out priorityId) || priorityId < 1 || priorityId > 3)
            {
                Console.WriteLine("Пожалуйста, введите корректный номер приоритета.");
            }

            var filteredTasks = tasks.Where(t => (int)t.TaskPriority == priorityId).ToList();

            if (filteredTasks.Count == 0)
            {
                Console.WriteLine("Нет задач с данным приоритетом.");
            }
            else
            {
                Console.WriteLine($"Задачи с приоритетом {priorityId}:");
                for (int i = 0; i < filteredTasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. [{filteredTasks[i].TaskPriority}] {filteredTasks[i].Description}");
                }
            }
        }
        static void SearchTasks()
        {
            Console.Write("Введите ключевое слово для поиска: ");
            string keyword = Console.ReadLine().ToLower();
            var foundTasks = tasks.Where(t => t.Description.ToLower().Contains(keyword)).ToList();

            if (foundTasks.Any())
            {
                Console.WriteLine("Найденные задачи:");
                for (int i = 0; i < foundTasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. [{foundTasks[i].TaskPriority}] {foundTasks[i].Description}");
                }
            }
            else
            {
                Console.WriteLine("Задачи не найдены.");
            }
        }
        static void SaveTasks()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Description}|{(int)task.TaskPriority}");
                }
            }
            Console.WriteLine("Задачи сохранены!");
        }

        static void LoadTasks()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        string description = parts[0];
                        Priority priority = (Priority)Enum.Parse(typeof(Priority), parts[1]);
                        var task = new Task<string>(description, priority);
                        tasks.Add(task);
                    }
                }
            }

        }
    }
}
