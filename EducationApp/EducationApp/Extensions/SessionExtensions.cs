using System;
using System.Linq;
using EducationApp.Models;
using EducationApp.Services.Fakes;

namespace EducationApp.Extensions
{
    public static class SessionExtensions
    {
        public static void AddFakeData(this Session session, Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }

            if (session.Location == null || session.Location.Name.IsNullOrWhiteSpace())
            {
                session.Location = FakeCourseService.GenerateFakeLocation(random);
            }

            if (!session.SessionSchedules.Any())
            {
                var amount = random.Next(1, 10);
                for (var i = 0; i < amount; i++)
                {
                    session.SessionSchedules.Add(FakeCourseService.GenerateFakeSessionSchedule(session.Id, random));
                }
            }
        }
    }
}