﻿using ProgettoIndividuale.Domain;
using ProgettoIndividuale.EF.Data;
using ProgettoIndividuale.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgettoIndividuale.Services
{
    public class ProductsServices : IProductsServices
    {
        private NORTHWINDContext _context;

        public ProductsServices(NORTHWINDContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.AsQueryable().ProjectToDomain();
        }

        public Product Get(int id) 
        {
            return _context.Products.FirstOrDefault(x => x.ProductId == id).ProjectToDomain();
        }
        public void Delete(Product element)
        {
            _context.Products.Remove(_context.Products.FirstOrDefault(x => x.ProductId == element.ProductId));
            _context.SaveChanges();
        }

        public Product Insert(Product element)
        {
            _context.Products.Add(element.ProjectToDbModel());
            _context.SaveChanges();
            return _context.Products.FirstOrDefault(x => x.ProductId == element.ProductId).ProjectToDomain();
        }

        public Product Update(Product element)
        {
            _context.Products.Remove(_context.Products.FirstOrDefault(x => x.ProductId == element.ProductId));
            _context.Products.Add(element.ProjectToDbModel());
            _context.SaveChanges();
            return element;
        }
    }
}