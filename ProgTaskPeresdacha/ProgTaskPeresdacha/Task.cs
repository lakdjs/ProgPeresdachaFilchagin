using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgTaskPeresdacha;

namespace ProgTaskPeresdacha
{
    public class Task<T>
    {
        public T Description { get; }
        public Priority TaskPriority { get; }

        public Task(T description, Priority priority)
        {
            Description = description;
            TaskPriority = priority;
        }

        public override string ToString()
        {
            return $"{Description} (Приоритет: {TaskPriority})";
        }
    }
}
