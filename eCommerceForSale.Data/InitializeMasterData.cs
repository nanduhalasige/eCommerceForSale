using eCommerceForSale.Data.Data;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerceForSale.Data
{
    public class InitializeMasterData
    {
        public static void Initialize(ApplicationDbContext context, IOptions<MasterDataOptions> masterDataOptions)
        {
            context.Database.EnsureCreated();
            InsertSelectedWeights(context, masterDataOptions.Value);
            context.SaveChanges();
        }

        private static void InsertSelectedWeights(ApplicationDbContext context, MasterDataOptions masterDataOptions)
        {
            if (!context.ProductWeights.Any())
            {
                var weights = masterDataOptions.Weights;
                var weightSplit = weights.Split(',');
                var productWeights = new List<ProductWeight>();
                foreach (var weight in weightSplit)
                {
                    productWeights.Add(
                        new ProductWeight
                        {
                            Weight = Convert.ToInt32(weight.Split('-')[0]),
                            WeightUnit = weight.Split('-')[1]
                        });
                }

                foreach (ProductWeight weight in productWeights)
                {
                    context.ProductWeights.Add(weight);
                }
            }
            return;
        }
    }
}