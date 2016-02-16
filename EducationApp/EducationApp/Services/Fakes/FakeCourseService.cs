using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EducationApp.Models;

namespace EducationApp.Services.Fakes
{
    public class FakeCourseService : ICourseService
    {
        public Task<ICollection<Course>> SearchCoursesAsync(string query)
        {
            var random = new Random();
            ICollection<Course> result = new List<Course>();

            for (var i = 0; i < Constants.Internet.DefaultResultsPerPage; i++)
            {
                result.Add(GenerateFakeCourse(random));
            }

            return Task.FromResult(result);
        }

        public Task FetchSpeculativelyAsync(int courseId) => Task.Run(() => { });
        public Task FetchCoursesSpeculativelyAsync(params Category[] subcategories) => Task.Run(() => { });

        public Task<Course> GetCourseDetailsAsync(int courseId)
        {
            var course = GenerateFakeCourse();
            course.Id = courseId;
            return Task.FromResult(course);
        }

        public Task<ICollection<Course>> GetCategoryCoursesAsync(int categoryId, bool speculatively = false)
        {
            var rand = new Random();
            var result = new List<Course>();
            for (var i = 0; i < 20; i++)
            {
                result.Add(GenerateFakeCourse(rand));
            }
            return Task.FromResult((ICollection<Course>) result);
        }

        public Task UpdateCourseSessionsAsync(Course course, bool speculatively = false) => Task.FromResult(course);

        public static Course GenerateFakeCourse(Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }

            var fakeCourse = new Course
            {
                Title = $"Course {random.Next(100)} example",
                Id = random.Next(100),
                Publisher = "Example publisher",
                StartDate = DateTime.UtcNow,
                Price = 250,
                ContentUrl = "http://www.microsoft.be",
                Duration = 123,
                InCatalog = true,
                IsNew = true,
                IsActive = true,
                Instructor = GenerateFakeInstructor(random)
            };
            fakeCourse.Descriptions.Add(GenerateFakeDescription(fakeCourse.Id));
            var amountSessions = random.Next(10);
            for (var i = 0; i < amountSessions; i++)
            {
                fakeCourse.Sessions.Add(GenerateFakeSession(fakeCourse.Id, random));
            }

            return fakeCourse;
        }

        public static Instructor GenerateFakeInstructor(Random random)
        {
            if (random == null)
            {
                random = new Random();
            }

            return new Instructor
            {
                Email = "satya@microsoft.com",
                Employer = "Microsoft",
                EmployerDepartment = "Management",
                FirstName = "Satya",
                LastName = "Nadella",
                Language = "en",
                IsActive = true,
                Id = random.Next(),
                Gender = "m"
            };
        }

        public static Description GenerateFakeDescription(int courseId) => new Description
        {
            CourseId = courseId,
            Audience = "Example audience",
            CourseContent = "This course is about things",
            ExternalUrl = "http://www.microsoft.be",
            Language = "nl",
            LongDescription = "This is more info about this course which is about a lot of different things.",
            Materials = "You need stuff",
            Methods = "You learn stuff",
            Objectives = "You have to do stuff",
            Platforms = "You use stuff",
            Prerequisites = "You have to know stuff",
            ShortDescription = "Stuff"
        };

        public static Session GenerateFakeSession(int courseId, Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }

            var fakeSession = new Session
            {
                CourseId = courseId,
                Id = random.Next(),
                Location = GenerateFakeLocation(),
                Preparation = "Clean your LightSaber"
            };

            var schedulesAmount = random.Next(10);
            for (var i = 0; i < schedulesAmount; i++)
            {
                fakeSession.SessionSchedules.Add(GenerateFakeSessionSchedule(fakeSession.Id));
            }

            return fakeSession;
        }

        public static SessionSchedule GenerateFakeSessionSchedule(int sessionId, Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }

            var fakeSchedule = new SessionSchedule
            {
                Id = random.Next(),
                ClassRoom = GenerateFakeClassRoom(random),
                StartTime = DateTime.UtcNow.AddDays(1),
                EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                SessionId = sessionId,
                Instructor = GenerateFakeInstructor(random)
            };

            return fakeSchedule;
        }

        public static Location GenerateFakeLocation(Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }

            var id = random.Next();
            var fakeLocation = new Location
            {
                Id = id,
                Name = "Building " + id
            };

            var classRoomAmount = random.Next(10);
            for (var i = 0; i < classRoomAmount; i++)
            {
                fakeLocation.ClassRooms.Add(GenerateFakeClassRoom(random));
            }

            return fakeLocation;
        }

        public static ClassRoom GenerateFakeClassRoom(Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }

            var id = random.Next();
            return new ClassRoom
            {
                Id = id,
                Name = "Class " + id
            };
        }
    }
}