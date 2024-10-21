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
                    new Dish { Name = "���-�����", Description = "����� ������, ���������", Calories = 200, Image = $"", Category = null, CategoryId =  3},
                    new Dish { Name = "����", Description = "����� ����, ��� �������", Calories = 330, Image = $"", Category = null, CategoryId =  3},
                    new Dish { Name = "�������� � ��������� �����", Description = "������ �������� � �������� �������", Calories = 250, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "��������� � ��������", Description = "��������� ���� � ������� ����������", Calories = 150, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "������", Description = "������������ ����� � ������� � ������ ������", Calories = 350, Image = $"", Category = null, CategoryId = 2 },
                    new Dish { Name = "��������� �����", Description = "������ ����� � ����� ���� � ��������", Calories = 220, Image = $"", Category = null, CategoryId = 2 },
                    new Dish { Name = "����� �� ��������", Description = "������ ����� ������� ��������", Calories = 700, Image = $"", Category = null, CategoryId = 4 },
                    new Dish { Name = "����� ���������", Description = "�������� ����� � ������� � ����������", Calories = 550, Image = $"", Category = null, CategoryId = 4 },
                    new Dish { Name = "������� �������� BBQ", Description = "������ ������� �������� � ����� BBQ", Calories = 280, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "�������� � �������", Description = "������������ �������� � ������� � �����", Calories = 300, Image = $"", Category = null, CategoryId = 1 }
                };
        private List<Category> categories = new List<Category>
                {
                    new Category { Id = 1, Name = "��������", NormalizedName = "starters" },
                    new Category { Id = 2, Name = "������", NormalizedName = "salads" },
                    new Category { Id = 3,Name = "����", NormalizedName = "soups" },
                    new Category { Id = 4,Name = "�������� �����", NormalizedName = "main-dishes" },
                    new Category { Id = 5,Name = "�������", NormalizedName = "drinks" },
                    new Category { Id = 6,Name = "�������", NormalizedName = "desserts" }
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
            // ������������� ��������� � �������� ������
            fakeHttpRequest.Headers.Returns(headerDictionary);

            // ������������� �������� ������ � �������� HttpContext
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

            Assert.Equal("���", result.ViewData["currentCategory"]);

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
            // Arrange: ��������� ������� ��� �������� ����������� ����������
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>()
            {
                Successful = false,
                ErrorMessage = "Failed to fetch categories."
            }));
            // Act: ����� ������ �����������
            var result = await _productController.Index(null, 1) as NotFoundObjectResult;
            // Assert: ��������, ��� ��������� - NotFound
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Failed to fetch categories.", notFoundResult.Value);
        }
        [Fact]
        public async Task Index_ReturnsNotFound_WhenDishesFetchFails()
        {
            // Arrange: ��������� ��������� ���������� ��� �����
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

            // Act: ����� ������ �����������
            var result = await _productController.Index(null, 1) as NotFoundObjectResult;

            // Assert: ��������, ��� ��������� - NotFound
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Failed to fetch dishes.", notFoundResult.Value);
        }
    }
}