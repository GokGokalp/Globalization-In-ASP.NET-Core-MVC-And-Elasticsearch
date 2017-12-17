using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCoreAndESearch.Models;
using ASPNETCoreAndESearch.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace ASPNETCoreAndESearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        
        public async Task<IActionResult> Index()
        {   
            // string indexName = $"products";
            // await FeedProductsInTurkish(indexName, "tr");
            // await FeedProductsInEnglish(indexName, "en");

            return View();
        }

        public async Task<IActionResult> Products(string keyword)
        {
            var requestCultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo culture = requestCultureFeature.RequestCulture.Culture;
    
            ProductSearchResponse productSearchResponse = await _productService.SearchAsync(keyword, culture.TwoLetterISOLanguageName);

            return View(productSearchResponse);
        }

        private async Task FeedProductsInTurkish(string indexName, string lang)
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Iphone X Cep Telefonu",
                    Description = "Gümüş renk, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Iphone X Cep Telefonu",
                    Description = "Uzay grisi rengi, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Rayban Erkek Gözlük",
                    Description = "Yeşil renk"
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Rayban Kadın Gözlük",
                    Description = "Gri renk"
                }
            };

            await _productService.CreateIndexAsync($"{indexName}_{lang}");
            await _productService.IndexAsync(products, lang);
        }

        private async Task FeedProductsInEnglish(string indexName, string lang)
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Iphone X Mobile Phone",
                    Description = "Silver color, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Iphone X Mobile Phone",
                    Description = "Space gray color, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Rayban Men's Glasses",
                    Description = "Green color"
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Rayban Women's Glasses",
                    Description = "Gray color"
                }
            };

            await _productService.CreateIndexAsync($"{indexName}_{lang}");
            await _productService.IndexAsync(products, lang);
        }
    }
}