using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class catalogController : BaseController
    {
        private readonly IProductManager productManager;

        public catalogController(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 30)]
        public IActionResult GetProducts()
        {
            try
            {
                var products = productManager.GetAll();
                return CustomResult("All products ar Loaded", products);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message.ToString(), HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = productManager.Add(product);
                if (isSaved)
                {
                    return CustomResult("Product Added Successfully", product, HttpStatusCode.Created);
                }
                else
                {
                    return CustomResult(product, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message.ToString(), HttpStatusCode.BadRequest);
            }

        }


        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("Id not found. Update product contain Error", product, HttpStatusCode.NotFound);
                }

                bool isUpdated = productManager.Update(product.Id, product);
                if (isUpdated)
                {
                    return CustomResult("Product Update Successfully", product, HttpStatusCode.OK);
                }
                else
                {
                    return CustomResult(product, HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message.ToString(), HttpStatusCode.BadRequest);
            }
        }


        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult DeleteProduct(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return CustomResult("Id not found. Product Delete Error", HttpStatusCode.NotFound);
                }

                bool isDelete = productManager.Delete(Id);
                if (isDelete)
                {
                    return CustomResult("Product Delete Successfully", HttpStatusCode.OK);
                }
                else
                {
                    return CustomResult("Delete Product Error!" ,HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message.ToString(), HttpStatusCode.BadRequest);

            }
        }
    }
}