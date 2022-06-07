using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduhomeMVC.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        // GET: SliderController
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = new List<Slider>();

            try
            {
                sliders = await _sliderService.GetAll();
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });
            }

            return View(sliders);
        }

        // GET: SliderController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Slider slider = new Slider();

            try
            {
                slider = await _sliderService.Get(id);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });
            }

            return View(slider);
        }

        // GET: SliderController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            try
            {
                await _sliderService.Create(slider);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });
            }
        }

        // GET: SliderController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Slider slider = new Slider();

            try
            {
                slider = await _sliderService.Get(id);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });
            }

            return View(slider);
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Slider slider)
        {
            try
            {
                await _sliderService.Update(slider);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });     
            }
        }

        // GET: SliderController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: SliderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                await _sliderService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });
            }
        }
    }
}
