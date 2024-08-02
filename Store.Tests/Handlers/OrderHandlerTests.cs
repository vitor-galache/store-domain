using Store.Domain.Commands;
using Store.Domain.Handlers;
using Store.Domain.Repositories;
using Store.Tests.Repositories;

namespace Store.Tests.Handlers;

[TestClass]
public class OrderHandlerTests
{
    private static readonly ICustomerRepository _customerRepository = new FakeCustomerRepository();
    private static readonly IDeliveryFeeRepository _deliveryFeeRepository = new FakeDeliveryFeeRepository();
    private static readonly IDiscountRepository _discountRepository = new FakeDiscountRepository();
    private static readonly IProductRepository _productRepository = new FakeProductRepository();
    private static readonly IOrderRepository _orderRepository = new FakeOrderRepository();

    private readonly OrderHandler _handler = new OrderHandler(_customerRepository,
        _deliveryFeeRepository,
        _discountRepository,
        _productRepository,
        _orderRepository);

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_cliente_inexistente_o_pedido_nao_deve_ser_gerado()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678910"; // Cliente inexistente
        command.ZipCode = "12345678";
        command.PromoCode = "12345678";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

        _handler.Handle(command);
        Assert.AreEqual(_handler.Invalid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_cep_invalido_o_pedido_nao_deve_ser_gerado_normalmente()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678911";
        command.ZipCode = "0";
        command.PromoCode = "12345678";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Validate();
        Assert.AreEqual(command.Invalid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_promocode_inexistente_o_pedido_deve_ser_gerado_normalmente()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678911";
        command.ZipCode = "12345678";
        command.PromoCode = "12345671"; // PromoCode inexistente
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

        _handler.Handle(command);
        Assert.AreEqual(_handler.Valid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_pedido_sem_itens_o_mesmo_nao_deve_ser_gerado()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678911";
        command.ZipCode = "12345678";
        command.PromoCode = "12345678";
        command.Validate();
        Assert.AreEqual(command.Invalid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
    {
        var command = new CreateOrderCommand();
        command.Customer = ""; // Cliente inválido
        command.ZipCode = "1234567891011"; // Cep inválido
        command.PromoCode = "12345678";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Validate();
        Assert.AreEqual(command.Invalid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_comando_valido_o_pedido_deve_ser_gerado()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678911";
        command.ZipCode = "12345678";
        command.PromoCode = "12345678";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

        _handler.Handle(command);
        Assert.AreEqual(_handler.Valid, true);
    }
}