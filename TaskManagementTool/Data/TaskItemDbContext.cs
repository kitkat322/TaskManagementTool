using Microsoft.EntityFrameworkCore;
using System;
using TaskManagementTool.Models;

namespace TaskManagementTool.Data
{
    public class TaskItemDbContext : DbContext
    {
        public TaskItemDbContext(DbContextOptions<TaskItemDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}