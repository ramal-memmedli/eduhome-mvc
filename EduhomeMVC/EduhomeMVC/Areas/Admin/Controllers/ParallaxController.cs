using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;
using BusinessLayer.Services;
using EduhomeMVC.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace EduhomeMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ParallaxController : Controller
    {
        private readonly IParallaxService _parallaxService;
        private readonly IWebHostEnvironment _environment;

        public ParallaxController(IParallaxService parallaxService, IWebHostEnvironment environment)
        {
            _parallaxService = parallaxService;
            _environment = environment;
        }

        // GET: Admin/Parallax
        public async Task<IActionResult> Index()
        {
            Parallax parallax = (await _parallaxService.GetAll()).FirstOrDefault();

            ParallaxGetVM parallaxGetVM = new ParallaxGetVM()
            {
                Id = parallax.Id,
                Title = parallax.Title,
                Body = parallax.Body,
                ImageUrl = parallax.ParallaxImages.Where(n => n.IsMain).FirstOrDefault().Image.Name,
                BackgroundUrl = parallax.ParallaxImages.Where(n => !n.IsMain).FirstOrDefault().Image.Name
            };

            return View(parallaxGetVM);
        }

        // GET: Admin/Parallax/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ParallaxUpdateVM parallaxUpdateVM = new ParallaxUpdateVM();

            try
            {
                Parallax parallax = await _parallaxService.Get(id);

                parallaxUpdateVM.Title = parallax.Title;
                parallaxUpdateVM.Body = parallax.Body;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(parallaxUpdateVM);
        }

        // POST: Admin/Parallax/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, ParallaxUpdateVM updateVM)
        //{
        //    try
        //    {
        //        Parallax parallax = await _parallaxService.Get(id);
        //        updateVM.Title = parallax.Title;
        //        updateVM.Body = parallax.Body;

        //        if(!updateVM.MainImage)

        //        _context.Update(parallax);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ParallaxExists(parallax.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
    }
}
