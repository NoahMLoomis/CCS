using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCS.Models {
    public partial class DataType {
        private readonly CCSContext _context;
        public DataType() {
            Parameter = new HashSet<Parameter>();
            _context = new CCSContext();
        }

        public decimal DataTypeId { get; set; }
        public string DataTypeName { get; set; }
        public decimal LanguageId { get; set; }

        public virtual Language Language { get; set; }
        public virtual ICollection<Parameter> Parameter { get; set; }

        public async Task<IEnumerable<DataType>> GetAllDataTypes() {
            return await _context.DataType.ToListAsync();
        }
        public async Task<IEnumerable<DataType>> GetDataTypesByLanguage(decimal languageId) {

            var cl = await _context.ChallengeLanguage.Where(x => x.ChallengeId == languageId).FirstOrDefaultAsync();

            var dataTypes = await _context.DataType
                                    .Where(l => l.LanguageId == cl.LanguageId)
                                    .ToListAsync();

            return dataTypes;
        }

        public virtual ICollection<Challenge> Challenge { get; set; }

        public async Task<IEnumerable<DataType>> GetAllDataTypesByLanguageId(decimal languageId) {
            return await _context.DataType
                .Where(d => d.LanguageId == languageId)
                .ToListAsync();
        }
    }
}
