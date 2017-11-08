using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using BigRowDataEF.Entity;
using BIgRowDataEF.DB;

namespace BigRowDataEF
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string fileName = "file.csv";
            var fileReader = new FileReader<TaxCategory>(fileName);
            var convertEntity = fileReader.ConvertEntity();
            var bigDataDbContext = new BigDataDbContext();
            bigDataDbContext.Database.CreateIfNotExists();
            //bigDataDbContext.Configuration.AutoDetectChangesEnabled = false;
            //bigDataDbContext.Configuration.ValidateOnSaveEnabled = false;
            //bigDataDbContext.TaxCategories.AddRange(convertEntity);
            //bigDataDbContext.SaveChanges();



            using (TransactionScope scope = new TransactionScope())
            {
                BigDataDbContext context = null;
                try
                {
                    context = new BigDataDbContext();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var watch = Stopwatch.StartNew();
                    int count = 0;
                    foreach (var entityToInsert in convertEntity)
                    {
                        
                        ++count;
                        context = AddToContext(context, entityToInsert, count, 10000, true);
                    }
                    
                    context.SaveChanges();
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    var totalSeconds = TimeSpan.FromMilliseconds(elapsedMs).TotalSeconds;
                    Console.WriteLine(totalSeconds);
                }
                finally
                {
                    if (context != null)
                        context.Dispose();
                }

                scope.Complete();
            }
            Console.ReadKey();

        }

        

        private static BigDataDbContext AddToContext(BigDataDbContext context,
    TaxCategory entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<TaxCategory>().Add(entity);

            if (count % commitCount == 0)
            {
                Console.Title = count.ToString();
                var watch2 = Stopwatch.StartNew();

                context.SaveChanges();

                watch2.Stop();
                var elapsedMs2 = watch2.ElapsedMilliseconds;
                var totalSeconds2 = TimeSpan.FromMilliseconds(elapsedMs2).TotalSeconds;

                Console.WriteLine(totalSeconds2);
                if (recreateContext)
                {
                    context.Dispose();
                    context = new BigDataDbContext();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }
    }
}