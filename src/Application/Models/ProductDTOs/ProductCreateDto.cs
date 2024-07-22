using Application.Models.ProductCategoriesDTOs;
using Application.Models.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.ProductDTOs
{
    public class ProductCreateDto
    {

        public string? Name { get; set; }


        public string? Description { get; set; }

        public int   Stock { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }


        public static Product ToProduct(ProductCreateDto productCreate)
        {
            return new Product
            {
                Name = productCreate.Name,
                Description = productCreate.Description,
                Stock = productCreate.Stock,
                Price = productCreate.Price,
                CategoryId = productCreate.CategoryId,


            };
        }
    }
}
