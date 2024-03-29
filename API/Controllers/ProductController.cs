﻿using System;
using System.Collections.Generic;
using API.Configurations.Filter;
using Application.Interfaces;
using Application.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authentication]
    public class ProductController : BaseController
    {
        private IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        // [PermissionRequired("Permission.Product.Create")]
        [Route("product")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm]CreateProductModel model)
        {
            return await _productService.CreateProduct(model);
        }
        
        // [PermissionRequired("Permission.Product.Read")]
        [Route("product/search")]
        [HttpPost]
        public IActionResult SearchProducts(SearchProductsModel model)
        {
            return _productService.SearchProducts(model);
        }

        // [PermissionRequired("Permission.Product.Read")]
        [Route("product/fetch")]
        [HttpPost]
        public IActionResult FetchProducts(FetchModel model)
        {
            return _productService.FetchProducts(model);
        }

        // [PermissionRequired("Permission.Product.Update")]
        [Route("product")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductModel model)
        {
            return await _productService.UpdateProduct(model);
        }

        // [PermissionRequired("Permission.Product.Delete")]
        [Route("product")]
        [HttpDelete]
        public async Task<IActionResult> RemoveMulProduct(List<Guid> ids)
        {
            return await _productService.RemoveMulProduct(ids);
        }

        // [PermissionRequired("Permission.Product.Read")]
        [Route("product")]
        [HttpGet]
        public IActionResult GetProduct(Guid id)
        {
            return _productService.GetProduct(id);
        }
        
        // [PermissionRequired("Permission.Product.Read")]
        [Route("product/all")]
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return _productService.GetAllProducts();
        }
        
        // [PermissionRequired("Permission.Product.Read")]
        [Route("product/all-warehouse")]
        [HttpPost]
        public IActionResult SearchSumProductAllWarehouse(SearchSumProductModel model)
        {
            return _productService.SearchSumProduct(model, false);
        } 
        
        // [PermissionRequired("Permission.Product.Read")]
        [Route("product/in-warehouse")]
        [HttpPost]
        public IActionResult SearchSumProductInWarehouse(SearchSumProductModel model)
        {
            return _productService.SearchSumProduct(model, true);
        }
    }
}
