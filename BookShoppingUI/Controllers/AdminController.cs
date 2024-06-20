using BookShoppingUI.Constants;
using BookShoppingUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShoppingUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public AdminController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> AllOrders()
        {
            var orders = await _orderRepository.GetOrder(true);
            return View(orders);
        }

        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _orderRepository.TogglePaymentStatus(orderId);
            }
            catch (Exception)
            {

            }

            return RedirectToAction(nameof(AllOrders));
        }

        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId)
                        ?? throw new Exception("Order not found");

            var orderStatusList = (await _orderRepository.GetOrderStatuses())
                .Select(os =>
                {
                    return new SelectListItem
                    {
                        Text = os.StatusName,
                        Value = os.Id.ToString(),
                        Selected = os.Id == order.OrderStatusId
                    };
                });

            var model = new UpdateOrderStatusModel
            {
                OrderId = orderId,
                OrderStatusList = orderStatusList,
                OrderStatusId = order.OrderStatusId
            };

            return View(model);
        }

        [HttpPost]  
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.OrderStatusList = (await _orderRepository.GetOrderStatuses())
                    .Select(os =>
                    {
                        return new SelectListItem
                        {
                            Text = os.StatusName,
                            Value = os.Id.ToString(),
                            Selected = os.Id == model.OrderStatusId
                        };
                    });

                    return View(model);
                }

                await _orderRepository.ChangeOrderStatus(model);
                TempData["msg"] = "Order status updated successfully";
            }
            catch (Exception)
            {
                TempData["msg"] = "Something went wrong";
            }

            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = model.OrderId });
        }
    }
}
