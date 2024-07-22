using Application.Models.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.ProductCategoriesDTOs
{
    public class ProductCategoriesCreateDto
    {
        public string? Icon { get; set; }


        public string? Name { get; set; }

        public string? Description { get; set; }


        // nos da la entidad
        public static ProductCategory ToCategoryProduct(ProductCategoriesCreateDto productCategoriesCreate)
        {
            return new ProductCategory
            {
                Icon = productCategoriesCreate.Icon,
                Name = productCategoriesCreate.Name,
                Description = productCategoriesCreate.Description,

            };
        }
    }
}
