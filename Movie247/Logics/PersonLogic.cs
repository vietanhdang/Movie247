using Microsoft.EntityFrameworkCore;
using Movie247.Data;
using Movie247.Helpers;
using Movie247.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie247.Logics
{
    public class PersonLogic
    {
        private readonly Movie247Context _context;

        public PersonLogic()
        {
            _context = new Movie247Context();
        }


        // Get top 10 people who have the most imdb ratings
        public async Task<List<Person>> GetTop10People()
        {
            return await _context.People.OrderByDescending(p => p.Popularity).Take(10).ToListAsync();
        }
        public Person FindPersonById(int id)
        {
            var person = _context.People.Include(x => x.MovieCasts).ThenInclude(y => y.Movie)
            .Include(x => x.MovieCrews).ThenInclude(y => y.Movie)
            .FirstOrDefault(m => m.Id == id);
            return person;
        }
        public (int, int) GetLatestAndOldestYearOfPerson()
        {
            int latestYear = _context.People.Max(m => m.Birthday.Value.Year);
            int oldestYear = _context.People.Min(m => m.Birthday.Value.Year);
            return (latestYear, oldestYear);
        }
        public async Task<List<Person>> FindPersonByFilter(FilterModel filter)
        {
            var people = from p in _context.People
                         select p;
            if (filter.Keyword != null)
            {
                people = people.Where(m => m.Name.ToLower().Contains(filter.Keyword.ToLower()));
            }
            if (filter.Category != null)
            {
                if (filter.Category.Equals("actor"))
                {
                    people = from p in people
                             join c in _context.MovieCasts on p.Id equals c.PersonId
                             select p;
                }
                if (filter.Category.Equals("director"))
                {
                    people = from p in people
                             join c in _context.MovieCrews on p.Id equals c.PersonId
                             select p;
                }
            }
            people = people.Distinct();
            filter.TotalCount = people.Count();
            if (filter.TotalCount <= 0)
            {
                return await people.ToListAsync();
            }
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalCount / filter.PageSize);
            if (filter.Page < 1)
            {
                filter.Page = 1;
            }
            if (filter.Page > filter.TotalPages)
            {
                filter.Page = filter.TotalPages;
            }
            if (!(filter.PageSize == 12 || filter.PageSize == 24 || filter.PageSize == 36))
            {
                filter.PageSize = 12;
            }
            if (filter.FromYear != 0 || filter.ToYear != 0)
            {
                var FromYearAndToYear = GetLatestAndOldestYearOfPerson();
                if (filter.FromYear != 0 && filter.FromYear < FromYearAndToYear.Item2)
                {
                    filter.FromYear = FromYearAndToYear.Item2;
                }
                if (filter.ToYear != 0 && filter.ToYear > FromYearAndToYear.Item1)
                {
                    filter.ToYear = FromYearAndToYear.Item1;
                }
                if (filter.FromYear != 0 && filter.ToYear != 0 && filter.FromYear < filter.ToYear)
                {
                    people = people.Where(p => p.Birthday.Value.Year >= filter.FromYear && p.Birthday.Value.Year <= filter.ToYear);
                }
            }
            int from = (filter.Page - 1) * filter.PageSize + 1;
            if (filter.OrderBy.Equals("asc"))
            {
                if (filter.SortBy.Equals("date"))
                {
                    people = people.OrderBy(p => p.Birthday);
                }
                if (filter.SortBy.Equals("popularity"))
                {
                    people = people.OrderBy(p => p.Popularity);
                }
            }
            else
            {
                if (filter.SortBy.Equals("date"))
                {
                    people = people.OrderByDescending(p => p.Birthday);
                }
                if (filter.SortBy.Equals("popularity"))
                {
                    people = people.OrderByDescending(p => p.Popularity);
                }
            }
            return await people.Skip(from - 1).Take(filter.PageSize).ToListAsync();
        }
    }
}
