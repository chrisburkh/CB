using CBAdmin.Data;
using CBAdmin.Models;
using Enums;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Service
{
    public class DatabaseService : IDatabaseService
    {
        public async Task ResetDatabase()
        {

            var store = DocumentStoreHolder.Store;

            Clear(store);

            await Fill(store);

            await ResetSettings(store);

        }

        private async Task ResetSettings(DocumentStore store)
        {
            var session = store.OpenAsyncSession();

            var settings = new SystemSetting("CBAdmin");

            settings.CreateTime = DateTime.Now;
            settings.ResetDatabase = false;

            await session.StoreAsync(settings);
            await session.SaveChangesAsync();
        }

        private async Task Fill(IDocumentStore store)
        {
            Console.WriteLine("Writing Default Data into Database");
            var session = store.OpenAsyncSession();

            var student = new Student[]
            {
                new Student{LastName="Müller", FirstMidName="Franz", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1989-1-11")},
                new Student{LastName="Müller", FirstMidName="Heinz", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1990-1-11")},
                new Student{LastName="Musterman", FirstMidName="Veronika", Id = string.Empty, Gender = GenderType.Female, DateOfBirth = DateTime.Parse("1991-2-12")}
            };

            foreach (Student s in student)
            {
                await session.StoreAsync(s, null);
                await session.SaveChangesAsync();

            }

            var teacher = new Teacher[]
            {
                new Teacher{LastName="Bingen", FirstMidName="Hildegard", Id = string.Empty, Gender = GenderType.Female, DateOfBirth = DateTime.Parse("1958-03-31"), Salary = SalaryType.A6, Employment_Commence = DateTime.Parse("2018-01-01")},
                new Teacher{LastName="Diesel", FirstMidName="Rudolf", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1900-01-31"), Salary = SalaryType.A8, Employment_Commence = DateTime.Parse("2017-01-01")},
                new Teacher{LastName="Fischer", FirstMidName="Helene", Id = string.Empty, Gender = GenderType.Female, DateOfBirth = DateTime.Parse("1980-01-31"), Salary = SalaryType.A8, Employment_Commence = DateTime.Parse("2016-01-01")},
                new Teacher{LastName="Humboldt", FirstMidName="Alexander", Id = string.Empty, Gender = GenderType.Male, DateOfBirth = DateTime.Parse("1905-01-31"), Salary = SalaryType.A8, Employment_Commence = DateTime.Parse("2015-01-01")},
            };

            foreach (Teacher t in teacher)
            {
                await session.StoreAsync(t, null);
                await session.SaveChangesAsync();
            }

            var course = new Course[]
            {
                new Course{Id  = string.Empty, Subject ="Deutsch", Abbreviation ="De"},
                new Course{Id  = string.Empty, Subject ="Englisch", Abbreviation ="En"},
                new Course{Id  = string.Empty, Subject ="Physik", Abbreviation ="Phy"},
            };

            foreach (Course c in course)
            {
                await session.StoreAsync(c, null);
                await session.SaveChangesAsync();
            }

            var coursesDatabase = await (session.Query<Course>().ToListAsync());
            var teachersDataBase = await (session.Query<Teacher>().ToListAsync());

            var classes = new Class[]
            {
                new Class{Id = string.Empty, Name ="8a", CourseID = coursesDatabase[0].Id, TeacherID = teachersDataBase[0].Id, Students = student},
                new Class{Id = string.Empty, Name ="8b", CourseID = coursesDatabase[1].Id, TeacherID = teachersDataBase[1].Id},
                new Class{Id = string.Empty, Name ="8c", CourseID = coursesDatabase[2].Id, TeacherID = teachersDataBase[2].Id},
            };

            foreach (Class cl in classes)
            {
                await session.StoreAsync(cl, null);
                await session.SaveChangesAsync();
            }
        }

        private void Clear(IDocumentStore store)
        {
            var session = store.OpenSession();
            session.Advanced.MaxNumberOfRequestsPerSession = 1000;


            var students = session.Query<Student>().ToList();

            foreach (Student s in students)
            {
                session.Delete(s);
                session.SaveChanges();
            }

            var teacher = session.Query<Teacher>().ToList();

            foreach (Teacher t in teacher)
            {
                session.Delete(t);
                session.SaveChanges();
            }

            var claz = session.Query<Class>().ToList();

            foreach (Class c in claz)
            {
                session.Delete(c);
                session.SaveChanges();
            }

            var course = session.Query<Course>().ToList();

            foreach (Course co in course)
            {
                session.Delete(co);
                session.SaveChanges();
            }
        }
    }
}
