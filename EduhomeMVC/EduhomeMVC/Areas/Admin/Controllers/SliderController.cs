using BusinessLayer.Services;
using Common.Helpers;
using DAL.Models;
using EduhomeMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EduhomeMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _environment;

        public SliderController(ISliderService sliderService, IWebHostEnvironment environment, IImageService imageService)
        {
            _sliderService = sliderService;
            _environment = environment;
            _imageService = imageService;
        }

        // GET: SliderController
        public async Task<IActionResult> Index()
        {
            List<SliderDetails> slidersDetails = new List<SliderDetails>();

            try
            {
                List<Slider> sliders = await _sliderService.GetAll();
                foreach (Slider slider in sliders)
                {
                    slidersDetails.Add(MapSliderDetails(slider));
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });
            }

            return View(slidersDetails);
        }

        // GET: SliderController/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            SliderDetails sliderDetails;
            try
            {
                Slider slider = await _sliderService.Get(id);
                sliderDetails = MapSliderDetails(slider);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 404,
                    message = ex.Message
                });
            }

            return View(sliderDetails);
        }

        // GET: SliderController/Create
        public async Task<IActionResult> Create()
        {
            List<Slider> currentSliders = await _sliderService.GetAll();

            if (currentSliders.Count >= 10)
            {
                return Json(new
                {
                    status = 405,
                    message = "Allowed slider count is 10."
                });
            }
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreate data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            if(data.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "File cannot be null!");
                return View(data);
            }

            if(!data.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be only an image!");
                return View(data);
            }

            float fileSize = ((float)data.ImageFile.Length) / 1024 / 1024;

            float allowedFileSize = 3;

            if(fileSize > allowedFileSize)
            {
                ModelState.AddModelError("ImageFile", $"Size of image must be under {allowedFileSize}MB!");
                return View(data);
            }

            string fileName = await data.ImageFile.CreateImage(_environment.WebRootPath);

            Image image = new Image();
            image.Name = fileName;

            try
            {
                await _imageService.Create(image);
            }
            catch
            {
                ModelState.AddModelError("ImageFile", "Something went wrong");
                return View(data);
            }

            Slider slider = new Slider()
            {
                Title = data.Title,
                Body = data.Body,
                ImageId = image.Id
            };

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
            Slider slider = await _sliderService.Get(id);
            SliderCreate sliderVM = MapSliderCreate(slider);
            return View(sliderVM);
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderCreate data)
        {
            try
            {
                Slider slider = await _sliderService.Get(id);
                slider.Title = data.Title;
                slider.Body = data.Body;

                if(data.ImageFile != null)
                {
                    string fileName = await data.ImageFile.CreateImage(_environment.WebRootPath);

                    slider.Image.Name = fileName;

                    await _imageService.Update(slider.Image);
                }

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
        [HttpGet]
        //[ValidateAntiForgeryToken]
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

        private SliderDetails MapSliderDetails(Slider slider)
        {
            SliderDetails sliderDetails = new SliderDetails();

            sliderDetails.Id = slider.Id;
            sliderDetails.Title = slider.Title;
            sliderDetails.Body = slider.Body;
            sliderDetails.ImageUrl = slider.Image.Name;
            sliderDetails.IsActive = slider.IsActive;

            return sliderDetails;
        }

        private SliderCreate MapSliderCreate(Slider slider)
        {
            SliderCreate sliderCreate = new SliderCreate();
            sliderCreate.Title = slider.Title;
            sliderCreate.Body = slider.Body;
            return sliderCreate;
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            try
            {
                Slider slider = await _sliderService.Get(id);
                slider.IsActive = !slider.IsActive;
                await _sliderService.Update(slider);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction(actionName: nameof(Details), new {id});
        }
    }
}
