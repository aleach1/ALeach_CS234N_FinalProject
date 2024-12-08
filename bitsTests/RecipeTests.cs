using bitsEFClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace bitsTests
{
    [TestFixture]
    public class RecipeTests
    {

        BitsContext dbContext;
        Recipe? r;
        List<Recipe>? recipes;

        [SetUp]
        public void Setup()
        {

            dbContext = new BitsContext();
        }//Name from Recipe, ABV from recipe, IBU from

        [Test]
        public void GetAllTest()
        {
            recipes = dbContext.Recipes.OrderBy(r => r.RecipeId).ToList();
            Assert.AreEqual(4, recipes.Count);
            Assert.AreEqual(1, recipes[0].RecipeId);

        }

        [Test]
        public void GetRecipeById()
        {
            r = dbContext.Recipes.Find(1);
            Assert.IsNotNull(r);
            Assert.AreEqual(r.Name, "Fuzzy Tales Juicy IPA");

        }

        [Test]
        public void GetWithIngredientsTest()
        {
            r = dbContext.Recipes.Include("RecipeIngredients").Where(re => re.RecipeId == 1).SingleOrDefault();
            //c = dbContext.Customers.Include("Invoices").Where(c => c.CustomerId == 20).SingleOrDefault();
            TestContext.WriteLine("Ingredients: " + r.RecipeIngredients);
            Assert.IsNotNull(r);
            Assert.AreEqual("Fuzzy Tales Juicy IPA", r.Name);
            Assert.AreEqual(13, r.RecipeIngredients.Count);
        }
    }
}