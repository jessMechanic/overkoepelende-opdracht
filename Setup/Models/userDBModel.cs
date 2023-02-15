
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CardGame.Models
{
    public class userDBModel : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public string DbPath { get; }

        public userDBModel()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "blogging.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<Post> Posts { get; } = new();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int UserId { get; set; }
        public User Owner { get; set; }
    }
}
