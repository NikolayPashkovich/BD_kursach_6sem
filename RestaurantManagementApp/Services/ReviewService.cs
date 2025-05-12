using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Services
{
    public static class ReviewService
    {
        public static async Task<int?> GetReviewRatingAsync(int orderId)
        {
            using var context = new RestaurantPlazaKursachContext();
            var review = await context.Reviews.FirstOrDefaultAsync(r => r.OrderId == orderId);
            return review?.Rating;
        }

        public static async Task AddOrUpdateReviewAsync(int orderId, int rating)
        {
            using var context = new RestaurantPlazaKursachContext();
            var review = await context.Reviews.FirstOrDefaultAsync(r => r.OrderId == orderId);

            if (review == null)
            {
                context.Reviews.Add(new Review { OrderId = orderId, Rating = rating });
            }
            else
            {
                review.Rating = rating;
            }

            await context.SaveChangesAsync();
        }

        public static async Task DeleteReviewAsync(int orderId)
        {
            using var context = new RestaurantPlazaKursachContext();
            var review = await context.Reviews.FirstOrDefaultAsync(r => r.OrderId == orderId);
            if (review != null)
            {
                context.Reviews.Remove(review);
                await context.SaveChangesAsync();
            }
        }
    }

}
