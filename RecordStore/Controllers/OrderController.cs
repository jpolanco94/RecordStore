﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace RecordStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;
        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }
        [Authorize]
        public ViewResult List() => View(repository.Orders.Where(o => !o.Shipped));
        [HttpPost]
        public IActionResult MarkShipped(int orderId)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }
        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }

    }
}