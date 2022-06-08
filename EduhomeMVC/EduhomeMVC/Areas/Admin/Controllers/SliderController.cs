using BusinessLayer.Services;
using DAL.Models;
using EduhomeMVC.ViewModels;
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

            string fileName = data.ImageFile.FileName;

            if(fileName.Length > 219)
            {
                fileName.Substring(fileName.Length - 219, 219);
            }

            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(_environment.WebRootPath, "assets", "uploads", "images", "sliders", fileName);

            using(FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await data.ImageFile.CopyToAsync(fileStream);
            }

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
