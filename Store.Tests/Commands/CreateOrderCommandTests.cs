using Store.Domain.Commands;

namespace Store.Tests.Commands;

[TestClass]
public class CreateOrderCommandTests
{
    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_cliente_invalido_o_pedido_nao_deve_ser_gerado()
    {
        var command = new CreateOrderCommand();
        command.Customer = "";
        command.ZipCode = "12345678";
        command.PromoCode = "87654321";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(),1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(),1));
        command.Validate();
        Assert.AreEqual(command.Valid,false);
    }
}