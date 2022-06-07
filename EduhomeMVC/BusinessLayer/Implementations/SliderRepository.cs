﻿using BusinessLayer.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class SliderRepository : ISliderService
    {
        private readonly AppDbContext _context;

        public SliderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Slider> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            Slider slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();

            if (slider is null)
            {
                throw new NullReferenceException();
            }

            return slider;
        }

        public async Task<List<Slider>> GetAll()
        {
            List<Slider> sliders = await _context.Sliders.Where(n => !n.IsDeleted).ToListAsync();

            if (sliders is null)
            {
                throw new NullReferenceException();
            }

            return sliders;
        }

        public async Task Create(Slider entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException();
            }

            entity.CreatedDate = DateTime.Now;

            await _context.Sliders.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Slider entity)
        {
            if (entity is null)
            {
                throw new NullReferenceException();
            }

            entity.UpdatedDate = DateTime.Now;

            _context.Sliders.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            Slider slider = await Get(id);

            slider.IsDeleted = true;

            await Update(slider);
        }
    }
}