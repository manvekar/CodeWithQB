using CodeWithQB.API.Features.ShoppingCarts;
using CodeWithQB.Core.Models;
using CodeWithQB.Core.Extensions;
using CodeWithQB.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class ShoppingCartScenarios: ShoppingCartScenarioBase
    {

        [Fact]
        public async Task ShouldCreate()
        {
            using (var server = CreateServer())
            {
                IEventStore eventStore = server.Host.Services.GetService(typeof(IEventStore)) as IEventStore;

                var response = await server.CreateClient()
                    .PostAsAsync<CreateShoppingCartCommand.Request, CreateShoppingCartCommand.Response>(Post.ShoppingCarts, new CreateShoppingCartCommand.Request() {
                        ShoppingCart = new ShoppingCartDto()
                        {

                        }
                    });

                var entity = eventStore.Query<ShoppingCart>().First();

                Assert.True(entity.UserId != default(Guid));
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetShoppingCartsQuery.Response>(Get.ShoppingCarts);

                Assert.True(response.ShoppingCarts.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {

            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetShoppingCartByIdQuery.Response>(Get.ShoppingCartById(1));

                Assert.True(getByIdResponse.ShoppingCart.ShoppingCartId != default(Guid));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<UpdateShoppingCartCommand.Request, UpdateShoppingCartCommand.Response>(Post.ShoppingCarts, new UpdateShoppingCartCommand.Request()
                    {
                        ShoppingCart = getByIdResponse.ShoppingCart
                    });

                Assert.True(saveResponse.ShoppingCartId != default(Guid));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.ShoppingCart(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}