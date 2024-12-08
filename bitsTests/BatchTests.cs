using bitsEFClasses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitsTests
{
    [TestFixture]
    public class BatchTests
    {

        BitsContext dbContext;
        Batch? b;
        List<Batch>? batches;

        [SetUp]
        public void Setup()
        {

            dbContext = new BitsContext();
        }//Name from Recipe, ABV from recipe, IBU from

        

        [Test]
        public void CreateTest()
        {
            b = new Batch();
            b.BatchId = 1;
            b.RecipeId = 1;
            b.EquipmentId = 1;
            b.Volume = 2;
            b.ScheduledStartDate = DateTime.Parse("12/04/24");
            dbContext.Batches.Add(b);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Batches.Find(1));
        }

        [Test]
        public void GetAllTest()
        {
            batches = dbContext.Batches.OrderBy(r => r.BatchId).ToList();
            Assert.AreEqual(1, batches.Count);
            Assert.AreEqual(1, batches[0].RecipeId);

        }

        [Test]
        public void DeleteTest()
        {
            b = dbContext.Batches.Find(1);
            dbContext.Batches.Remove(b);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Batches.Find(1));
        }

        
    }
}
