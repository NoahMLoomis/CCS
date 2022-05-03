using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCS.Models {
    public partial class Parameter {

        private readonly CCSContext _context;
        public Parameter() {
            _context = new CCSContext();
            TestCaseParameter = new HashSet<TestCaseParameter>();
        }

        public Parameter(decimal dataTypeId, decimal challengeLanguageId, decimal position, string defaultValue, string parameterName) {
            DataTypeId = dataTypeId;
            ChallengeLanguageId = challengeLanguageId;
            Position = position;
            DefaultValue = defaultValue;
            ParameterName = parameterName;
        }

        public decimal ParameterId { get; set; }
        public decimal DataTypeId { get; set; }
        public decimal ChallengeLanguageId { get; set; }
        public decimal Position { get; set; }
        public string DefaultValue { get; set; }
        public string ParameterName { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual ChallengeLanguage ChallengeLanguage { get; set; }
        public virtual ICollection<TestCaseParameter> TestCaseParameter { get; set; }

        public async Task<List<Parameter>> GetParametersForChallenge(decimal challenge_id) {
            try {
                var cl_id = await new ChallengeLanguage().GetChallengeLanguageIdFromChallenge(challenge_id);
                return await _context.Parameter
                .Where(p => p.ChallengeLanguageId == cl_id)
                .Include(d => d.DataType)
                .ToListAsync();
            } catch (Exception e) {
                throw e;
            }
            
            
        }

        public async Task<bool> AddParameter() {
            try {
                await _context.Parameter.AddAsync(this);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception e) {
                throw e;
            }
        }

        public async Task EditParameter(Parameter p) {
            _context.Update(p);
            await _context.SaveChangesAsync();
        }
    }
}
