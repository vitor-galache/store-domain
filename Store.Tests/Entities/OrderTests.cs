using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Tests.Entities;

[TestClass]
public class OrderTests
{
    private static readonly Customer _customer = new Customer("Cliente Teste","cliente@teste.com");
    private static readonly Product _product = new Product("Produto 1",50,true);
    private static readonly Product _product2 = new Product("Produto 2", 10, true);
    private static readonly Discount ExpireDiscount = new Discount(10,DateTime.Now);
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_novo_pedido_valido_deve_gerar_um_numero_com_8_caracteres()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
        Assert.AreEqual(8,order.Number.Length);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_novo_pedido_seu_status_deve_ser_aguardando_pagamento()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
        Assert.AreEqual(order.Status,EOrderStatus.WaitingPayment);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_pagamento_do_pedido_seu_status_deve_ser_aguardando_entrega()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
        order.AddItem(_product,1);
        order.Pay(50);
        Assert.AreEqual(order.Status,EOrderStatus.WaitingDelivery);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_pedido_cancelado_seu_status_deve_ser_cancelado()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
        order.AddItem(_product,10);
        order.Cancel();
        Assert.AreEqual(order.Status,EOrderStatus.Canceled);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_novo_item_sem_produto_o_mesmo_nao_deve_ser_adicionado()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
        order.AddItem(null,10);
        Assert.AreEqual(order.Items.Count, 0);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_novo_item_com_quantidade_zero_ou_menor_o_mesmo_nao_deve_ser_adicionado()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
       order.AddItem(_product,0);
       Assert.AreEqual(order.Items.Count,0);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_novo_pedido_valido_seu_total_deve_ser_50()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
        order.AddItem(_product,1);
        Assert.AreEqual(order.Total(),50);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_desconto_expirado_o_valor_do_pedido_deve_ser_60()
    {
        var order = new Order(_customer, 0, ExpireDiscount);
        order.AddItem(_product,1);
        order.AddItem(_product2,1);
        Assert.AreEqual(order.Total(),60);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_desconto_invalido_o_valor_do_pedido_deve_ser_60()
    {
        var order = new Order(_customer, 0, null);
        order.AddItem(_product,1);
        order.AddItem(_product2,1);
        Assert.AreEqual(order.Total(),60);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_desconto_de_10_o_valor_do_pedido_deve_ser_50()
    {
        var discount = new Discount(10,DateTime.UtcNow.AddDays(5));
        var order = new Order(_customer, 0, discount);
        order.AddItem(_product,1);
        order.AddItem(_product2,1);
        Assert.AreEqual(order.Total(),50);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_uma_taxa_de_entrega_de_10_o_valor_do_pedido_deve_ser_60()
    {
        var order = new Order(_customer,10,ExpireDiscount);
        order.AddItem(_product,1);
        Assert.AreEqual(order.Total(),60);
    }
    
    [TestMethod]
    [TestCategory("Domain")]
    public void Dado_um_pedido_sem_cliente_o_mesmo_deve_ser_invalido()
    {
        var order = new Order(null, 0, ExpireDiscount);
        Assert.IsTrue(order.Invalid);
    }
}