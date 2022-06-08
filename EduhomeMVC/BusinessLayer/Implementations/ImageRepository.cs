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
    public class ImageRepository : IImageService
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Image> Get(int? id)
        {
            if(id is null)
            {
                throw new ArgumentNullException();
            }

            Image image = await _context.Images.Where(n => n.Id == id).FirstOrDefaultAsync();

            if(image is null)
            {
                throw new NullReferenceException();
            }

            return image;
        }

        public Task<List<Image>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task Create(Image entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException();
            }

            await _context.Images.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task Update(Image entity)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int? id)
        {
            throw new System.NotImplementedException();
        }
    }
}
