using Application.Models.OrderDetailDto;
using Application.Models.OrderDTOs;
using Application.Models.UserDTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimplicityStoreProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepository;
        private readonly IOrderDetailRepository _ordersDetailRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IProductsRepository _productsRepository;



        public OrderDetailController(IOrderRepository ordersRepository, IOrderDetailRepository ordersDetailRepository, IUsersRepository usersRepository, IProductsRepository productsRepository )
        {
            _ordersRepository = ordersRepository;
            _ordersDetailRepository = ordersDetailRepository;
            _usersRepository = usersRepository;
            _productsRepository = productsRepository;
        }
        [HttpGet]
        [Authorize]

        public ActionResult<List<OrderDetailsDto>> GetOrders()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            var ordersDetail = _ordersDetailRepository.GetAllOrdersDetail();

            if (ordersDetail == null || !ordersDetail.Any())
            {
                return NotFound();
            }

            var ordersDetailDtos = ordersDetail.Select(order => OrderDetailsDto.Create(order)).ToList();
            return Ok(ordersDetailDtos);
        }

        [HttpGet("{id}")]
        [Authorize]

        public ActionResult<OrderDetailsDto> GetOrdersDetail(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);


            if (user.Role != "Admin")
            {
                return BadRequest("no tenes lo permisos suficientes");
            }



            var OrderDetail = _ordersDetailRepository.GetOrderDetailById(id);
            if (OrderDetail == null)
            {
                return NotFound();
            }


            var Order = _ordersRepository.GetOrderById(OrderDetail.OrderId);



            if (Order == null)
            {
                return NotFound("no existe esa orden");
            }

            if (user.Role != "Admin" && Order.UserId != userId)
            {
                return BadRequest("no es tu order");
            }


            return Ok(OrderDetailsDto.Create(OrderDetail));

        }

        [HttpDelete]
        [Authorize]

        public ActionResult DeleteOrderDetail(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);



            var OrderDetail = _ordersDetailRepository.GetOrderDetailById(id);
            if (OrderDetail == null)
            {
                return NotFound();
            }

            var Order = _ordersRepository.GetOrderById(OrderDetail.OrderId);

            if (Order == null)
            {
                return NotFound("no existe esa orden");
            }

            if (Order.UserId != userId)
            {
                return BadRequest("no es tu order");
            }

            _productsRepository.AddStock(OrderDetail.ProductId,OrderDetail.Quantity);
            _productsRepository.SaveChanges();

            _ordersDetailRepository.DeleteDetailOrder(OrderDetail);
            _ordersDetailRepository.SaveChanges();


            return Ok("Orden eliminada Correctamente");
        

        }
        [HttpPost]
        [Authorize]
        public ActionResult<OrderDetailsDto> CreateOrderDetail(int OrderId,[FromBody] OrderDetailCreateDto orderDeatailCreate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);




            var product = _productsRepository.GetProductById(orderDeatailCreate.ProductId);

            if (product == null)
            {
                return NotFound("no existe ese product con el id");

            }

            OrderDetail newOrderDetail = OrderDetailCreateDto.ToOrderDetail(orderDeatailCreate);

            var Order = _ordersRepository.GetOrderById(OrderId);

            if (Order == null)
            {
                return NotFound("no existe esa orden");
            }

            if (Order.UserId != userId)
            {
                return BadRequest("no es tu order");
            }

            if (product.Available == false) 
            {
                return BadRequest("el producto No esta disponible");
            }

            if (orderDeatailCreate.Quantity <= 0)
            {
                return BadRequest("Selecciona una cantidad");
            }

            if (product.Stock < orderDeatailCreate.Quantity)
            {
                return BadRequest("El producto no tiene suficiente Stock");
            }

            _productsRepository.ReducerStock(product.Id, orderDeatailCreate.Quantity);



            newOrderDetail.OrderId = OrderId;
            newOrderDetail.Price = product.Price * orderDeatailCreate.Quantity;


            _ordersDetailRepository.AddDetail(newOrderDetail);
            _productsRepository.SaveChanges();
            _ordersDetailRepository.SaveChanges();


            return Ok(OrderDetailsDto.Create(newOrderDetail));


        }



    }
}
