using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Candles.Domain.Entities;
using Candles.Domain.Repositories;
using Candles.Repositories.Context;
using Candles.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Candles.Repositories
{
    public class CandlesRepository : ICandlesRepository
    {
        private const int PageSize = 1000;

        private readonly ConnectionFactory _connectionFactory;
        private readonly IMapper _mapper;

        public CandlesRepository(ConnectionFactory connectionFactory, IMapper mapper)
        {
            _connectionFactory = connectionFactory;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Candle>> GetAllAsync()
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var candles = new List<Candle>();

                var pageIndex = 0;

                List<CandleEntity> entities;

                do
                {
                    entities = await context.Candles
                        .AsNoTracking()
                        .OrderBy(o => o.Time)
                        .Skip(PageSize * pageIndex)
                        .Take(PageSize)
                        .ToListAsync();

                    pageIndex++;

                    candles.AddRange(_mapper.Map<List<Candle>>(entities));
                } while (entities.Count == PageSize);

                return candles;
            }
        }

        public async Task InsertAsync(IReadOnlyList<Candle> candles)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entities = _mapper.Map<List<CandleEntity>>(candles);

                context.Candles.AddRange(entities);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(IReadOnlyList<Candle> candles)
        {
            using (var context = _connectionFactory.CreateDataContext())
            {
                var entities = _mapper.Map<List<CandleEntity>>(candles);

                foreach (var entity in entities)
                    context.Entry(entity).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}
