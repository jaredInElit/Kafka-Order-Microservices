using Confluent.Kafka;
using Kafka_Ordering_API.Models;
using Kafka_Ordering_API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Kafka_Ordering_API.Controllers
{
    [ApiController]
    public class OrderServiceApiController : ControllerBase
    {
        private readonly OrderDatabaseService _databaseService;
        private readonly KafkaProducerService _kafkaProducerService;
        public OrderServiceApiController(OrderDatabaseService databaseService, KafkaProducerService kafkaProducerService)
        {
            _databaseService = databaseService;
            _kafkaProducerService = kafkaProducerService;
          _kafkaProducerService = kafkaProducerService;
        }

        [HttpGet]
        [Route("/api/order/{id}")]
        public async Task<IActionResult> GetOrder([FromRoute][Required] string id)
        {
            try
            {
                var result = await _databaseService.GetAsync(id);
                if (result == null)
                    return NotFound(new { message = "Order not found" });

                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPatch]
        [Route("/api/order/{id}")]
        public async Task<IActionResult> PatchOrder([FromRoute][Required] string id, [FromBody] GetOrderDto body)
        {
            try
            {
                var existingOrder = await _databaseService.GetAsync(id);
                if (existingOrder == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                if (body.ProductID != null)
                    existingOrder.ProductID = body.ProductID;
                if (body.Quantity.HasValue)
                    existingOrder.Quantity = body.Quantity;

                await _databaseService.UpdateAsync(id, existingOrder);

                var idResponse = new IdResponseDto
                {
                    Id = id
                };

                return Ok(idResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("/api/order")]
        public async Task<IActionResult> PostOrder([FromBody] PostOrderDto body)
        {
            try
            {
                var id = await _databaseService.CreateAsync(body);
                var idResponse = new IdResponseDto
                {
                    Id = id
                };

                var orderJson = JsonConvert.SerializeObject(body);
                await _kafkaProducerService.ProduceMessageAsync("order-topic", orderJson);

                return Created("GetOrder", idResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("/api/order")]
        public async Task<IActionResult> QueryOrder([FromQuery] int? top, [FromQuery] int? skip)
        {
            try
            {
                var orders = await _databaseService.GetOrdersAsync(top, skip);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
    }
}
