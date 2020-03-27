using JwtApplication.Helpers;
using JwtApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtApplication.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options): base(options)
        { }
      public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {


           

           
            var roles = GetRoles();
            var users = GetUsers();
     
  
           builder.Entity<Role>().HasData(roles);
           builder.Entity<User>().HasData(users);
            base.OnModelCreating(builder);

        }





        public static List<User> GetUsers()
        {
            var _passwordHasher = new PasswordHasher();

            List<User> _users = new List<User>() {
            new User {Id=1,FirstName = "haythem", LastName = "Radhoini", Email = "test@gmail.com", Password = _passwordHasher.Hash("test"),RoleId=1 },
            new User {Id=2,FirstName = "admin", LastName = "admin", Email = "test2@gmail.com", Password = _passwordHasher.Hash("test"),RoleId=2 }
             };
            return _users;
        }

        public static List<Role> GetRoles()
        {
            var _passwordHasher = new PasswordHasher();

            List<Role> _roles = new List<Role>() {
            new Role {Id= 1, Name = "Admin" },
            new Role {Id= 2, Name = "User" }
             };
            return _roles;
        }

    }
}