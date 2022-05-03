using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCS.Models {
    public partial class Teacher {
        private readonly CCSContext _context;

        public Teacher() {
            Challenge = new HashSet<Challenge>();
            _context = new CCSContext();
        }

        public Teacher(CCSContext context) {
            Challenge = new HashSet<Challenge>();
            _context = context;
        }

        public decimal TeacherId { get; set; }

        public virtual ICollection<Challenge> Challenge { get; set; }

        public async Task<Teacher> GetTeacherById(decimal? id) {
            return await _context.Teacher
                        .Include(t => t.Challenge)
                        .ThenInclude(tc => tc.ChallengeLanguage)
                        .ThenInclude(l => l.Language)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(t => t.TeacherId == id); ;
        }

        public async Task AddTeacher(Teacher teacher) {
            await _context.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Challenge>> FilterChallengeTitle(int id, string searchChallengeTitle) {
            var challenges = await _context.Challenge.Where(c => c.TeacherId == id).OrderBy(c => c.Title).ToListAsync();
            if (!string.IsNullOrEmpty(searchChallengeTitle)) {
                challenges = challenges.Where(c => c.Title.ToLower().Contains(searchChallengeTitle.ToLower())).ToList();
            }
            return challenges;
        }
    }
}