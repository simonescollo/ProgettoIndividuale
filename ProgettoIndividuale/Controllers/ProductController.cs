﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgettoIndividuale.Domain;
using ProgettoIndividuale.Models;
using ProgettoIndividuale.Services;
using ProgettoIndividuale.Services.Request;
using ProgettoIndividuale.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ProgettoIndividuale.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsServices _prodotti;
        private readonly ICategoryServices _categorie;
        public ProductController(IProductsServices prodotti, ICategoryServices categorie)
        {
            _prodotti = prodotti;
            _categorie = categorie;
        }
        public IActionResult Index(int page, int perPage)
        {
            var paginatedQuery = _prodotti.GetAll().ProjectToViewModel().ToPaginated(pageIndex: page, pageSize: perPage);

            var products = paginatedQuery.ToList();

            ProductListViewModel model = new ProductListViewModel()
            {
                Pagination = new PaginationViewModel(actualPage: paginatedQuery.PageIndex, maxPage: paginatedQuery.TotalPages - 1, perPage: paginatedQuery.PageSize)
            };

            foreach (var prodotto in products)
            {
                prodotto.CategoryName = _categorie.GetById(prodotto.CategoryId).CategoryName;
                model.ProductList.Add(prodotto);
            }
            return View(model);
        }

        public IActionResult RisultatoRicerca(ProductListViewModel model)
        {
            return View(model);
        }

        [HttpGet]
        public IActionResult Search()
        {
            ProductViewModel model = new();
            var categories = _categorie.GetAll();
            List<SelectListItem> c = new();
            foreach (Category cat in categories)
            {
                var selected = model.CategoryId == 0 ? false : model.CategoryId == cat.CategoryId;
                c.Add(new SelectListItem
                {
                    Selected = selected,
                    Text = cat.CategoryName,
                    Value = cat.CategoryId.ToString()
                });
            }
            ViewBag.Categories = c;
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(ProductViewModel model)
        {
            ProductSearchRequest request = new();
            request.ProductName = model.ProductName;
            request.CategoryId = model.CategoryId;
            var q = _prodotti.Search(request).ProjectToViewModel().ToPaginated(pageIndex: 1, pageSize: 10);
            ProductListViewModel risultato = new() {
                Pagination = new PaginationViewModel(actualPage: q.PageIndex, maxPage: q.TotalPages - 1, perPage: q.PageSize),
                };
            foreach (var prodotto in q.ToList())
            {
                prodotto.CategoryName = _categorie.GetById(prodotto.CategoryId).CategoryName;
                risultato.ProductList.Add(prodotto);
            }
            return View("RisultatoRicerca",risultato);

        }

        public IActionResult Delete(int id) 
        {
            _prodotti.Delete(_prodotti.Get(id));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Save(int id)
        {
            Product elemento;

            if (id != 0)
            {
                elemento = _prodotti.Get(id);
            }
            else
            {
                elemento = new();
                elemento.ProductId = id;
            }

            var categories = _categorie.GetAll();
            List<SelectListItem> c = new();
            foreach (Category cat in categories)
            {
                var selected = id == 0 ? false : elemento.CategoryId == cat.CategoryId;
                c.Add(new SelectListItem
                {
                    Selected = selected,
                    Text = cat.CategoryName,
                    Value = cat.CategoryId.ToString()
                });
            }
            ViewBag.Categories = c;
            return View("Save", elemento.ProjectToViewModel());
        }
        [HttpPost]
        public IActionResult Save(ProductViewModel elemento)
        {
            int id = elemento.ProductId;

            var categories = _categorie.GetAll();
            List<SelectListItem> c = new();
            foreach (Category cat in categories)
            {
                var selected = id == 0 ? false : elemento.CategoryId == cat.CategoryId;
                c.Add(new SelectListItem
                {
                    Selected = selected,
                    Text = cat.CategoryName,
                    Value = cat.CategoryId.ToString()
                });
            }
            ViewBag.Categories = c;

            if (ModelState.IsValid)
            {
                if (elemento.ProductId != 0)
                {
                    _prodotti.Update(elemento.ProjectFromViewModel());
                }
                else
                {
                    elemento.ProductId = _prodotti.GetAll().ToList().Count() + 1;
                    _prodotti.Insert(elemento.ProjectFromViewModel());
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Save", elemento);
            }
        }
    }
}
