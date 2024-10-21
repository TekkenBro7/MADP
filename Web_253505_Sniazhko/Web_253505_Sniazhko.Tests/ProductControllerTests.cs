using Web_253505_Sniazhko.UI.Services.ProductService;
using Web_253505_Sniazhko.UI.Services.CategoryService;
using Web_253505_Sniazhko.UI.Controllers;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Web_253505_Sniazhko.Tests.Serializer;



namespace Web_253505_Sniazhko.Tests
{
    public class ProductControllerTests
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ProductController _productController;
        private List<Dish> dishes = new List<Dish>
                {
                    new Dish { Name = "Суп-харчо", Description = "Очень острый, невкусный", Calories = 200, Image = $"", Category = null, CategoryId =  3},
                    new Dish { Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = $"", Category = null, CategoryId =  3},
                    new Dish { Name = "Креветки в чесночном соусе", Description = "Сочные креветки с ароматом чеснока", Calories = 250, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "Брускетта с томатами", Description = "Хрустящий хлеб с свежими помидорами", Calories = 150, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "Цезарь", Description = "Классический салат с курицей и соусом Цезарь", Calories = 350, Image = $"", Category = null, CategoryId = 2 },
                    new Dish { Name = "Греческий салат", Description = "Свежие овощи с сыром фета и оливками", Calories = 220, Image = $"", Category = null, CategoryId = 2 },
                    new Dish { Name = "Стейк из говядины", Description = "Сочный стейк средней прожарки", Calories = 700, Image = $"", Category = null, CategoryId = 4 },
                    new Dish { Name = "Паста Карбонара", Description = "Кремовая паста с беконом и пармезаном", Calories = 550, Image = $"", Category = null, CategoryId = 4 },
                    new Dish { Name = "Куриные крылышки BBQ", Description = "Пряные куриные крылышки в соусе BBQ", Calories = 280, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "Фалафель с хумусом", Description = "Классический фалафель с хумусом и питой", Calories = 300, Image = $"", Category = null, CategoryId = 1 }
                };
        private List<Category> categories = new List<Category>
                {
                    new Category { Id = 1, Name = "Стартеры", NormalizedName = "starters" },
                    new Category { Id = 2, Name = "Салаты", NormalizedName = "salads" },
                    new Category { Id = 3,Name = "Супы", NormalizedName = "soups" },
                    new Category { Id = 4,Name = "Основные блюда", NormalizedName = "main-dishes" },
                    new Category { Id = 5,Name = "Напитки", NormalizedName = "drinks" },
                    new Category { Id = 6,Name = "Десерты", NormalizedName = "desserts" }
                };

        public ProductControllerTests()
        {
            _productService = Substitute.For<IProductService>();
            _categoryService = Substitute.For<ICategoryService>();
            var controllerContext = new ControllerContext();
            var fakeHttpContext = Substitute.For<HttpContext>();
            var fakeHttpRequest = Substitute.For<HttpRequest>();

            var fakeServices = new ServiceCollection();
            fakeServices.AddSingleton<TempDataSerializer, TempSerializer>();
            fakeServices.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();
            fakeServices.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
            var fakeServiceProvider = fakeServices.BuildServiceProvider();
            fakeHttpContext.RequestServices = fakeServiceProvider;

            var headerDictionary = new HeaderDictionary(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { /* { "x-requested-with", "XMLHttpRequest" } */ });
            // Устанавливаем заголовки в фейковый запрос
            fakeHttpRequest.Headers.Returns(headerDictionary);

            // Устанавливаем фейковый запрос в фейковый HttpContext
            fakeHttpContext.Request.Returns(fakeHttpRequest);
            controllerContext.HttpContext = fakeHttpContext;

            _productController = new ProductController(_productService, _categoryService) { ControllerContext = controllerContext };
        }
        [Fact]
        public async Task Index_ReturnsViewResults_WithCategoryService()
        {
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>()
            {
                Data = categories
            }));
            _productService.GetProductListAsync(null).Returns(Task.FromResult(new ResponseData<ListModel<Dish>>()
            {
                Data = new ListModel<Dish>
                {
                    Items = dishes,
                    CurrentPage = 1,
                    TotalPages = 5,
                }
            }));

            var result = await _productController.Index(null, 1) as ViewResult;
            Assert.NotNull(result);
            Assert.NotEqual(404, result.StatusCode);

            Assert.Equal("Все", result.ViewData["currentCategory"]);

            var viewDataCategories = result.ViewData["categories"] as List<Category>;
            Assert.NotNull(viewDataCategories);
            Assert.Equal(categories.Count, viewDataCategories.Count);
            Assert.Equal(categories[0].Name, viewDataCategories[0].Name);

            var model = result.Model as ListModel<Dish>;
            Assert.NotNull(model);
            Assert.Equal(dishes.Count, model.Items.Count);
            Assert.Equal(dishes[0].Name, model.Items[0].Name);
        }
        [Fact]
        public async Task Index_ReturnsNotFound_WhenCategoriesFetchFails()
        {
            // Arrange: Настройка сервиса для возврата неуспешного результата
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>()
            {
                Successful = false,
                ErrorMessage = "Failed to fetch categories."
            }));
            // Act: Вызов метода контроллера
            var result = await _productController.Index(null, 1) as NotFoundObjectResult;
            // Assert: Проверка, что результат - NotFound
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Failed to fetch categories.", notFoundResult.Value);
        }
        [Fact]
        public async Task Index_ReturnsNotFound_WhenDishesFetchFails()
        {
            // Arrange: Настройка успешного результата для марок
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>()
            {
                Successful = true,
                Data = categories
            }));
            _productService.GetProductListAsync(null).Returns(Task.FromResult(new ResponseData<ListModel<Dish>>()
            {
                Successful = false,
                ErrorMessage = "Failed to fetch dishes."
            }));

            // Act: Вызов метода контроллера
            var result = await _productController.Index(null, 1) as NotFoundObjectResult;

            // Assert: Проверка, что результат - NotFound
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Failed to fetch dishes.", notFoundResult.Value);
        }
    }
}