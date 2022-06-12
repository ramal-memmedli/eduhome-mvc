using BusinessLayer.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class ParallaxRepository : IParallaxService
    {
        private readonly AppDbContext _context;
        public ParallaxRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Parallax> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            Parallax parallax = await _context.Parallaxes.Where(n => n.Id == id)
                                                         .Include(n => n.ParallaxImages)
                                                         .ThenInclude(n => n.Image)
                                                         .FirstOrDefaultAsync();

            if (parallax is null)
            {
                throw new NullReferenceException();
            }

            return parallax;
        }

        public async Task<List<Parallax>> GetAll()
        {
            List<Parallax> parallaxes = await _context.Parallaxes
                                                      .Include(n => n.ParallaxImages)
                                                      .ThenInclude(n => n.Image)
                                                      .ToListAsync();

            if (parallaxes is null)
            {
                throw new NullReferenceException();
            }

            return parallaxes;
        }

        public async Task Update(Parallax entity)
        {
            if (entity is null)
            {
                throw new NullReferenceException();
            }

            _context.Parallaxes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public Task Create(Parallax entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
