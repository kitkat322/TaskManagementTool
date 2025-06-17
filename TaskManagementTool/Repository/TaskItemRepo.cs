//using System.Xml.Linq;
//using TaskManagementTool.Models;

//namespace TaskManagementTool.Repository
//{
//    public class TaskItemRepo : ITaskItemRepo
//    {
//        private readonly List<TaskItem> _tasks = new();
//        private int _nextId = 1;

//        // get all tasks
//        public IEnumerable<TaskItem> GetAll()
//        {
//            return _tasks;
//        }

//        // get task by ID
//        public TaskItem? GetById(int id)
//        {
//            return _tasks.FirstOrDefault(t => t.ID == id);
//        }

//        // add new task
//        public void Add(TaskItem task)
//        {
//            task.SetId(_nextId++);
//            //task.ID = _nextId++;
//            _tasks.Add(task);
//        }

//        // update excisting task
//        public void Update(TaskItem task)
//        {
//            var existing = GetById(task.ID);
//            if (existing == null)
//            {
//                Console.WriteLine("Task is not found");
//                return;
//            }


//            existing.Titel = task.Titel;
//            existing.Beschreibumg = task.Beschreibumg;
//            existing.Priorität = task.Priorität;
//            existing.AktuellerStatus = task.AktuellerStatus;
//            Console.WriteLine("Task is updated successfully");
//        }

//        // delete task by ID
//        public void Delete(int id)
//        {
//            var task = GetById(id);
//            if (task != null)
//            {
//                _tasks.Remove(task);
//            }
//        }
//    }
//}
