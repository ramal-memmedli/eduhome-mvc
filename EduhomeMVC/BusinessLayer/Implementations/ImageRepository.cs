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

        public async Task Update(Image entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException();
            }

            _context.Images.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            if(id is null)
            {
                throw new ArgumentNullException();
            }
            Image image = await Get(id);
            if(image is null)
            {
                throw new NullReferenceException();
            }
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}
